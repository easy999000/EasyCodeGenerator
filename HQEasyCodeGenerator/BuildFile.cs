using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using RazorEngine;
using RazorEngine.Templating; // For extension methods.

namespace EasyCodeGenerator
{
    /// <summary>
    /// 建造文件
    /// </summary>
    public class BuildFile
    {
        /// <summary>
        /// 根路径
        /// </summary>
        private string RootPath = AppDomain.CurrentDomain.BaseDirectory;
        /// <summary>
        /// 模板路径
        /// </summary>
        public string TemplatePath
        {
            get
            {
                return Path.Combine(RootPath, "Template");
            }
        }
        /// <summary>
        /// 代码路径
        /// </summary>
        public string CodePath
        {
            get
            {
                return Path.Combine(RootPath, "Code");
            }
        }

        /// <summary>
        /// 读取模板
        /// </summary>
        /// <param name="TemplateFileName"></param>
        /// <param name="CodeFileNameFormat">参数会以  TableName, TemplateName,的顺序传入</param>
        /// <returns></returns>
        private string GetTemplate(string TemplateFileName, out string CodeFileNameFormat)
        {
            CodeFileNameFormat = "{0}{1}";
            string filePath = Path.Combine(TemplatePath, TemplateFileName);
            if (!File.Exists(filePath))
            {
                return "";
            }
            var template = File.ReadAllText(filePath);

            /// @model 指令在写模板的时候,具体提示辅助开发的作用,但是不能被 正常编译,需要在模板里面去除
            string modelPattern = @"^\s*@model\s*?\S*\s*?$";

            // var v3 = System.Text.RegularExpressions.Regex.Matches(template, modelPattern, System.Text.RegularExpressions.RegexOptions.Multiline);

            template = System.Text.RegularExpressions.Regex.Replace(template, modelPattern, "", System.Text.RegularExpressions.RegexOptions.Multiline);
            ////////获取文件名格式配置
            string CodeFileNameFormatPattern = @"^\s*@@CodeFileNameFormat\s*?\S*\s*?$";

            var v4 = System.Text.RegularExpressions.Regex.Matches(template, CodeFileNameFormatPattern, System.Text.RegularExpressions.RegexOptions.Multiline);

            template = System.Text.RegularExpressions.Regex.Replace(template, CodeFileNameFormatPattern, "", System.Text.RegularExpressions.RegexOptions.Multiline);
            if (v4.Count > 0)
            {
                CodeFileNameFormat = v4[0].Value;
                CodeFileNameFormat = CodeFileNameFormat.Replace("@@CodeFileNameFormat", "").Trim();
            }

            return template;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="TemplateName"></param>
        private void ClearCodeDir(string TemplateName)
        {
            string codeDirPath = Path.Combine(CodePath, TemplateName);
            if (Directory.Exists(codeDirPath))
            {
                var v1 = System.IO.Directory.GetFiles(codeDirPath);
                foreach (var item in v1)
                {
                    System.IO.File.Delete(item);
                }
            }
        }

        /// <summary>
        /// 写入代码
        /// </summary>
        /// <param name="TemplateName">模板名字</param>
        /// <param name="TableName">表明</param>
        /// <param name="FileNameFormat">文件名的格式</param>
        /// <param name="CodeText"></param>
        private void WriteCode(string TemplateName, string TableName, string FileNameFormat, string CodeText)
        {
            string CodeFileName = string.Format(FileNameFormat, TableName, TemplateName);

            var CodePath = AppDomain.CurrentDomain.BaseDirectory + string.Format("Code\\{1}\\{2}.cs", TableName, TemplateName, CodeFileName);

            string dir = Path.GetDirectoryName(CodePath);
            Directory.CreateDirectory(dir);


            File.WriteAllText(CodePath, CodeText, UTF8Encoding.Unicode);



        }

        /// <summary>
        /// 模板生成的文件名字格式
        /// </summary>
        private Dictionary<string, string> TemplateToCodeFielNameFormatDic = new Dictionary<string, string>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="TemplateName">模板名字,不要文件扩展名</param>
        /// <param name="Tables"></param>
        /// <returns></returns>
        public string Build(string TemplateFileName, string TemplateNamespace, List<TableModel> Tables)
        {
            string TemplateName = Path.GetFileNameWithoutExtension(TemplateFileName);
            lock (Engine.Razor)
            {
                if (!Engine.Razor.IsTemplateCached(TemplateName, typeof(TableModel)))
                {
                    string CodeFielNameFormat = "";
                    var template = GetTemplate(TemplateFileName, out CodeFielNameFormat);
                    Engine.Razor.AddTemplate(TemplateName, template);
                    TemplateToCodeFielNameFormatDic.Add(TemplateName, CodeFielNameFormat);
                }
            }

            ClearCodeDir(TemplateName);

            foreach (var item in Tables)
            {
                var Table = item;
                Table.Namespace = TemplateNamespace;
                var result =
                    Engine.Razor.RunCompile(TemplateName, typeof(TableModel), Table);

                string CodeFielNameFormat = TemplateToCodeFielNameFormatDic[TemplateName];
                WriteCode(TemplateName, Table.Name, CodeFielNameFormat, result);


            }

            return "ok";
        }


        /// <summary>
        /// 获取所有的模板信息
        /// </summary>
        /// <returns></returns>
        public string[] GetAllTemplate()
        {
            return Directory.GetFiles(TemplatePath);

        }


    }
}
