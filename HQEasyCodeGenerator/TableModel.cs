using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasyCodeGenerator
{
    /// <summary>
    /// 表模型
    /// </summary>
    [Serializable]
    public class TableModel
    {
        string __Describe;
        /// <summary>
        /// 描述
        /// </summary>
        public string Describe {
            get {
                return __Describe;
            }
            set { __Describe = value.Replace("\r", ".").Replace("\n", " "); }
        }

        /// <summary>
        /// 名字
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 生成的命名空间,编译的时候传入
        /// </summary>
        public string Namespace { get; set; }

        /// <summary>
        /// 主键列
        /// </summary>
        public ColumnModel PrimaryKey
        {
            get
            {
                var c1 = Columns.Where(w => w.IsPK  ).ToList();
                if (c1.Count > 0)
                {
                    return c1[0];
                }
                else
                {
                    return new ColumnModel() { Name = "" };
                }
            }
        }

        /// <summary>
        /// 列集合
        /// </summary>
        public List<ColumnModel> Columns { get; set; } = new List<ColumnModel>();


    }
}
