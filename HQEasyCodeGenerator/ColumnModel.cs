using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasyCodeGenerator
{
    /// <summary>
    /// 列模型
    /// </summary>
    [Serializable]
    public class ColumnModel
    {
        static ColumnModel()
        {
            ColumnModel.SetDataTypeSqlToDMSFrameColumnTypesDic();
            ColumnModel.SetDataTypeSqlToCshapeDic();
        }

        /// <summary>
        /// 列描述
        /// </summary>
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
            set
            {
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
        /// 数据库数据类型
        /// </summary>
        public string DBType { get; set; }

        /// <summary>
        /// 数据库数据类型长度
        /// </summary>
        public string DBTypeLength { get; set; }

        /// <summary>
        /// 数据库字段可否可空 1可以
        /// </summary>
        public bool IsDBTypeNull { get; set; }

        /// <summary>
        /// 是否主键 1是
        /// </summary>
        public bool IsPK { get; set; }

        /// <summary>
        /// 是否是标识列 自增
        /// </summary>
        public bool IsDBAutoIncrement { get; set; }

        /// <summary>
        /// 默认值
        /// </summary>
        public string DefaultValue { get; set; }

        /// <summary>
        /// 代码数据类型
        /// </summary>
        public string CSharpType
        {
            get
            {
                string type = GetCSharpType(DBType);
                return type + (HasQuestionMark ? "?" : "");
            }
        }

        /// <summary>
        /// 类型是否有 问号 ?
        /// </summary>
        /// <returns></returns>
        public bool HasQuestionMark
        {
            get
            {
                if (this.IsDBTypeNull && CShapValueType.Contains(this.DBType))
                {
                    return true;
                }
                return false;
            }
        }


        /// <summary>
        /// c#原类型,没有问号 
        /// </summary>
        public string CsharpNoQuestionMark
        {
            get
            {
                return GetCSharpType(DBType);
            }
        }


        /// <summary>
        /// 是否是值类型
        /// </summary>
        public bool IsValueType
        {
            get
            {
                if (CShapValueType.Contains(this.DBType))
                {
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// DMS 数据类型
        /// </summary>
        public string DMSType
        {
            get
            {
                string type = GetDMSType(DBType);
                return "ColumnTypes." + type;
            }
        }


        /// <summary>
        /// 获取DMS数据类型,只是类型,不包含null符号
        /// </summary>
        /// <param name="SqlType"></param>
        /// <returns></returns>
        public static string GetDMSType(string SqlType)
        {
            if (!DataTypeSqlToDMSFrameColumnTypes.ContainsKey(SqlType))
            {
                return "null";
            }

            return DataTypeSqlToDMSFrameColumnTypes[SqlType].ToString();
        }

        /// <summary>
        /// 获取c# 数据类型,只是类型,不包含null符号
        /// </summary>
        /// <param name="SqlType"></param>
        /// <returns></returns>
        public static string GetCSharpType(string SqlType)
        {
            if (!DataTypeSqlToCshape.ContainsKey(SqlType))
            {
                return "null";
            }

            return DataTypeSqlToCshape[SqlType].ToString();
        }

        /// <summary>
        /// 数据类型c#
        /// </summary>
        public static Dictionary<string, string> DataTypeSqlToCshape = new Dictionary<string, string>();

        /// <summary>
        /// 数据类型DMS
        /// </summary>
        public static Dictionary<string, DMSFrameColumnTypes> DataTypeSqlToDMSFrameColumnTypes = new Dictionary<string, DMSFrameColumnTypes>();

        /// <summary>
        /// c#值类型类型
        /// </summary>
        public static string[] CShapValueType = new string[] {

            "int" ,
            "bigint" ,
            "binary" ,
            "bit" ,
            "datetime"  ,
            "decimal" ,
            "float" ,
            "money" ,
            "numeric" ,
            "real"  ,
            "smalldatetime" ,
            "smallint" ,
            "smallmoney" ,
            "timestamp" ,
            "tinyint" ,
            "unique identifier"
        };

        private static void SetDataTypeSqlToCshapeDic()
        {
            DataTypeSqlToCshape.Add("int", "int");
            DataTypeSqlToCshape.Add("text", "string");
            DataTypeSqlToCshape.Add("bigint", "long");
            DataTypeSqlToCshape.Add("binary", "Byte");
            DataTypeSqlToCshape.Add("bit", "bool");
            DataTypeSqlToCshape.Add("char", "string");
            DataTypeSqlToCshape.Add("datetime", "DateTime");
            DataTypeSqlToCshape.Add("decimal", "decimal");
            DataTypeSqlToCshape.Add("float", "float");
            DataTypeSqlToCshape.Add("money", "decimal");
            DataTypeSqlToCshape.Add("nchar", "string");
            DataTypeSqlToCshape.Add("ntext", "string");
            DataTypeSqlToCshape.Add("numeric", "decimal");
            DataTypeSqlToCshape.Add("nvarchar", "string");
            DataTypeSqlToCshape.Add("real", "Single");
            DataTypeSqlToCshape.Add("smalldatetime", "DateTime");
            DataTypeSqlToCshape.Add("smallint", "Int16");
            DataTypeSqlToCshape.Add("smallmoney", "decimal");
            DataTypeSqlToCshape.Add("timestamp", "DateTime");
            DataTypeSqlToCshape.Add("tinyint", "Byte");
            DataTypeSqlToCshape.Add("varchar", "string");
            DataTypeSqlToCshape.Add("variant", "Object");
            DataTypeSqlToCshape.Add("uniqueidentifier", "Guid");

        }
        private static void SetDataTypeSqlToDMSFrameColumnTypesDic()
        {
            DataTypeSqlToDMSFrameColumnTypes.Add("int", DMSFrameColumnTypes.Integer);
            DataTypeSqlToDMSFrameColumnTypes.Add("text", DMSFrameColumnTypes.String);
            DataTypeSqlToDMSFrameColumnTypes.Add("bigint", DMSFrameColumnTypes.Long);
            DataTypeSqlToDMSFrameColumnTypes.Add("binary", DMSFrameColumnTypes.Byte);
            DataTypeSqlToDMSFrameColumnTypes.Add("bit", DMSFrameColumnTypes.Bool);
            DataTypeSqlToDMSFrameColumnTypes.Add("char", DMSFrameColumnTypes.String);
            DataTypeSqlToDMSFrameColumnTypes.Add("datetime", DMSFrameColumnTypes.DateTime);
            DataTypeSqlToDMSFrameColumnTypes.Add("decimal", DMSFrameColumnTypes.Decimal);
            DataTypeSqlToDMSFrameColumnTypes.Add("float", DMSFrameColumnTypes.Float);
            DataTypeSqlToDMSFrameColumnTypes.Add("money", DMSFrameColumnTypes.Decimal);
            DataTypeSqlToDMSFrameColumnTypes.Add("nchar", DMSFrameColumnTypes.String);
            DataTypeSqlToDMSFrameColumnTypes.Add("ntext", DMSFrameColumnTypes.String);
            DataTypeSqlToDMSFrameColumnTypes.Add("numeric", DMSFrameColumnTypes.Decimal);
            DataTypeSqlToDMSFrameColumnTypes.Add("nvarchar", DMSFrameColumnTypes.String);
            DataTypeSqlToDMSFrameColumnTypes.Add("real", DMSFrameColumnTypes.Double);
            DataTypeSqlToDMSFrameColumnTypes.Add("smalldatetime", DMSFrameColumnTypes.SmallDateTime);
            DataTypeSqlToDMSFrameColumnTypes.Add("smallint", DMSFrameColumnTypes.Integer);
            DataTypeSqlToDMSFrameColumnTypes.Add("smallmoney", DMSFrameColumnTypes.Double);
            DataTypeSqlToDMSFrameColumnTypes.Add("timestamp", DMSFrameColumnTypes.DateTime);
            DataTypeSqlToDMSFrameColumnTypes.Add("tinyint", DMSFrameColumnTypes.Integer);
            DataTypeSqlToDMSFrameColumnTypes.Add("varchar", DMSFrameColumnTypes.String);
            DataTypeSqlToDMSFrameColumnTypes.Add("variant", DMSFrameColumnTypes.String);
            DataTypeSqlToDMSFrameColumnTypes.Add("uniqueidentifier", DMSFrameColumnTypes.Guid);
        }

        /// <summary>
        /// dms数据库型
        /// </summary>
        public enum DMSFrameColumnTypes
        {
            /// <summary>
            /// 
            /// </summary>
            Bool = 0,
            Byte = 1,
            Short = 2,
            Integer = 3,
            Long = 4,
            SByte = 5,
            UShort = 6,
            UInteger = 7,
            ULong = 8,
            Decimal = 9,
            Float = 10,
            Double = 11,
            String = 12,
            DateTime = 13,
            SmallDateTime = 14,
            Guid = 15
        }

    }
}
