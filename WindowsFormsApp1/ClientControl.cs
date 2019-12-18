using EasyCodeGenerator;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace EasyCodeGeneratorForm
{
    public class ClientControl
    {
        /// <summary>
        /// 数据库配置信息集合
        /// </summary>
        public List<DBShow> DBList = new List<DBShow>();

        /// <summary>
        /// 数据库模型集合
        /// </summary>
        public Dictionary<string, EasyCodeGenerator.DbModel> DBRazorDic = new Dictionary<string, EasyCodeGenerator.DbModel>();

        /// <summary>
        /// 当前操作的数据库
        /// </summary>
        public EasyCodeGenerator.DbModel CurrentDBModel = null;

        /// <summary>
        /// 先前操作的表集合
        /// </summary>
        public List<TableShow> CurrentTableList = null;
        public List<TemplateShow> TemplateShowList = null;
        /// <summary>
        /// 文件生成器
        /// </summary>
        public EasyCodeGenerator.BuildFile CodeBuild = new EasyCodeGenerator.BuildFile();

        /// <summary>
        /// 获取所有的db
        /// </summary>
        /// <returns></returns>
        public List<DBShow> GetAllDB()
        {
            List<DBShow> L1 = new List<DBShow>();
            var v1 = EasyCodeGenerator.DbModel.GetDBConnString();

            foreach (var item in v1)
            {
                DBShow d = new DBShow() { DBName = item.Key, connString = item.Value };
                L1.Add(d);
            }
            DBList = L1;
            return L1;
        }
        /// <summary>
        ///选择数据库
        /// </summary>
        /// <param name="db"></param>
        public void DBListSelectChange(DBShow db)
        {
            if (db==null)
            {
                return;
            }
            if (!DBRazorDic.ContainsKey(db.DBName))
            {
                EasyCodeGenerator.DbModel dbRazor = new EasyCodeGenerator.DbModel();
                dbRazor.LoadDBSchema(db.DBName, db.connString);
                DBRazorDic.Add(db.DBName, dbRazor);
            }
            CurrentDBModel = DBRazorDic[db.DBName];

        }
        /// <summary>
        ///查询指定名字的表信息
        /// </summary>
        /// <param name="db"></param>
        public List<TableShow> SelectTable(string input)
        {
            List<TableShow> TableShowList = new List<TableShow>();
            if (CurrentDBModel != null)
            {
                var tables = CurrentDBModel.Tables.Where(w => w.Name.ToLower().Contains(input.ToLower())).ToList();
                foreach (var item in tables)
                {
                    TableShow t = new TableShow();
                    t.Check = false;
                    t.TableName = item.Name;
                    t.table = item;
                    TableShowList.Add(t);

                }
                CurrentTableList = TableShowList;
            }
            else
            {
                CurrentTableList = null;
            }

            return CurrentTableList;
        }


        /// <summary>
        /// 获取所有的模板信息
        /// </summary>
        /// <returns></returns>
        public List<TemplateShow> GetAllTemplate()
        {
            GetTemplateNamespace();
            var v1 = CodeBuild.GetAllTemplate();
            List<TemplateShow> TemplateShowList = new List<TemplateShow>();
            foreach (var item in v1)
            {
                TemplateShow t = new TemplateShow();
                t.Check = true;
                t.FileName = System.IO.Path.GetFileName(item);
                if (this.TemplateNamespaceLIst.Count(c => c.Name == t.FileName) > 0)
                {
                    t.Namespace = this.TemplateNamespaceLIst.Where(w => w.Name == t.FileName).First().Namespace;
                }
                else
                {
                    t.Namespace = "";
                }
                TemplateShowList.Add(t);
                this.TemplateShowList = TemplateShowList;
            }

            return this.TemplateShowList;
        }

        /// <summary>
        /// 模板命名空间路径
        /// </summary>
        private static string TemplateNamespacePath
        {
            get
            {
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TemplateNamespace.json");
            }
        }
        /// <summary>
        /// 命名空间配置
        /// </summary>
     //   Dictionary<string, string> NamespaceCconfigDic = new Dictionary<string, string>();

        /// <summary>
        /// 命名空间配置
        /// </summary>
        List<TemplateNamespace> TemplateNamespaceLIst = new List<TemplateNamespace>();




        /// <summary>
        /// 读取命名空间配置
        /// </summary>
        /// <returns></returns>
        public List<TemplateNamespace> GetTemplateNamespace()
        {

            string fileText = File.ReadAllText(TemplateNamespacePath);

            var config = Newtonsoft.Json.JsonConvert.DeserializeObject<List<TemplateNamespace>>(fileText);

            this.TemplateNamespaceLIst = config;

            return TemplateNamespaceLIst;
        }

        /// <summary>
        /// 保存命名空间配置
        /// </summary>
        public void SaveTemplateNamespace()
        {
            foreach (var item in this.TemplateShowList)
            {
                if (this.TemplateNamespaceLIst.Count(c => c.Name == item.FileName) > 0)
                {
                    this.TemplateNamespaceLIst.Where(w => w.Name == item.FileName).First().Namespace = item.Namespace;
                }
                else
                {
                    this.TemplateNamespaceLIst.Add(new TemplateNamespace() { Name = item.FileName, Namespace = item.Namespace });
                }
            }

            string fileText = Newtonsoft.Json.JsonConvert.SerializeObject(this.TemplateNamespaceLIst);


            File.WriteAllText(TemplateNamespacePath, fileText);

        }

        public string Build()
        {
            SaveTemplateNamespace();
            List<TableModel> buildTables = this.CurrentTableList
                .Where(w => w.Check == true).Select(p => p.table).ToList();
            List<TemplateShow> buildTemplate = this.TemplateShowList.Where(w => w.Check == true).ToList();

            foreach (var item in buildTemplate)
            {
                if (string.IsNullOrWhiteSpace(item.Namespace))
                {
                    return "命名空间不能为空";
                }
            }
            foreach (var item in buildTemplate)
            {
                this.CodeBuild.Build(item.FileName, item.Namespace, buildTables);
            }



            return "ok";
        }


    }

    /// <summary>
    /// 模板展示模型
    /// </summary>
    public class TemplateShow
    {
        public bool Check { get; set; } = false;
        public string FileName { get; set; }

        public string Namespace { get; set; }
    }
    /// <summary>
    /// 表展示模型
    /// </summary>
    public class TableShow
    {
        public bool Check { get; set; } = false;
        public string TableName { get; set; }
        public EasyCodeGenerator.TableModel table { get; set; }
    }
    /// <summary>
    /// 数据库显示模型
    /// </summary>
    public class DBShow
    {
        public string DBName { get; set; }
        public string connString { get; set; }
    }
    /// <summary>
    /// 模板命名空间
    /// </summary>
    public class TemplateNamespace
    {
        /// <summary>
        /// 名字
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 命名空间
        /// </summary>
        public string Namespace { get; set; }
    }



}
