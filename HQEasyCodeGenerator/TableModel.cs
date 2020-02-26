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
        public string Describe
        {
            get
            {
                return __Describe;
            }
            set {

                if (value != null)
                {

                    __Describe = value.Replace("\r", ".").Replace("\n", " ");
                }
                else
                {
                    __Describe = null;
                }
                 
            
            }
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
        /// 需要引用的相关的命名空间,生成的时候传入
        /// </summary>
        public List<string> RelatedNamespace
        {
            get { return __RelatedNamespace; }
            set { __RelatedNamespace = value; }
        }

        /// <summary>
        /// 需要引用的相关的命名空间,生成的时候传入
        /// </summary>
        private List<string> __RelatedNamespace = new List<string>();

        /// <summary>
        /// 主键列
        /// </summary>
        public ColumnModel PrimaryKeyColumn
        {
            get
            {
                var c1 = Columns.Where(w => w.IsPK).ToList();
                if (c1.Count > 0)
                {
                    return c1[0];
                }
                else
                {
                    return null;
                }
            }
        }
        /// <summary>
        /// 自增列
        /// </summary>
        public ColumnModel IncrementColumn
        {
            get
            {
                var c1 = Columns.Where(w => w.IsDBAutoIncrement).ToList();
                if (c1.Count > 0)
                {
                    return c1[0];
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// 列集合
        /// </summary>
        public List<ColumnModel> Columns { get; set; } = new List<ColumnModel>();


    }
}
