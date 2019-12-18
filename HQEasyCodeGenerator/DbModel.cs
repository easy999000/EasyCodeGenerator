
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.IO;
using Newtonsoft.Json.Linq;

namespace EasyCodeGenerator
{
    /// <summary>
    /// 数据库模型
    /// </summary>
    [Serializable]
    public class DbModel
    {
        /// <summary>
        /// 连接字符串
        /// </summary>
        public string connString { get; set; }

        /// <summary>
        /// 数据库名字
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 表集合,需要先调用加载方法
        /// </summary>
        public List<TableModel> Tables { get; set; } = new List<TableModel>();

        /// <summary>
        /// 加载数据库结构
        /// </summary>
        /// <param name="DBName">数据库名字</param>
        /// <param name="ConnStr">连接字符串</param>
        public void LoadDBSchema(string DBName, string ConnStr)
        {
            this.Name = DBName;
            this.connString = ConnStr;

            DataTable Dt = GetDBSchema(ConnStr);

            foreach (DataRow item in Dt.Rows)
            {
                string tableDescribe = item["表描述"].ToString();
                string tableName = item["TableName"].ToString();
                string num = item["序号"].ToString();
                string columnName = item["列名"].ToString();
                string columnDescribe = item["列说明"].ToString();
                string dateType = item["数据类型"].ToString();
                string typeLength = item["长度"].ToString();
                string DecimalDigit = item["小数位数"].ToString();
                string isIdentification = item["标识"].ToString();
                string isPK = item["主键"].ToString();
                string canNull = item["允许空"].ToString();
                string defaultValue = item["默认值"].ToString();
                ///创建列
                ColumnModel column = new ColumnModel()
                {
                    DBType = dateType,
                    DBTypeLength = typeLength,
                    DefaultValue = defaultValue,
                    IsDBAutoIncrement = isIdentification == "1",
                    Describe = columnDescribe,
                    IsDBTypeNull = canNull == "1",
                    IsPK = isPK == "1",
                    Name = columnName
                };

                TableModel table = null;
                var tList = Tables.Where(w => w.Name == tableName).ToList();
                if (tList.Count > 0)
                {
                    table = tList[0];
                }
                else
                {
                    ////创建表
                    table = new TableModel() { Name = tableName, Describe = tableDescribe };
                    Tables.Add(table);
                }

                table.Columns.Add(column);


            }


        }



        //string connStr = "Integrated Security=False;server=172.168.16.41;database=PartInChina_erp;User ID=admin;Password=admin4manage;Connect Timeout=30";

        /// <summary>
        /// 获取数据库表结构
        /// </summary>
        /// <param name="connStr"></param>
        /// <returns></returns>
        DataTable GetDBSchema(string connStr)
        {

            SqlConnection sqlConn = new SqlConnection(connStr);

            string sqlStr = @"SELECT   CASE WHEN epTwo.[value] is null  THEN '' else epTwo.[value] END as 表描述
		,obj.name   TableName,
        col.colorder AS 序号 ,  
        col.name AS 列名 ,  
        ISNULL(ep.[value], '') AS 列说明 ,  
        t.name AS 数据类型 ,  
        col.length AS 长度 ,  
        ISNULL(COLUMNPROPERTY(col.id, col.name, 'Scale'), 0) AS 小数位数 ,  

        CASE WHEN COLUMNPROPERTY(col.id, col.name, 'IsIdentity') = 1 THEN 1  
             ELSE 0  
        END AS 标识 ,  

        CASE WHEN EXISTS ( SELECT   1  
                           FROM     dbo.sysindexes si  
                                    INNER JOIN dbo.sysindexkeys sik ON si.id = sik.id  
                                                              AND si.indid = sik.indid  
                                    INNER JOIN dbo.syscolumns sc ON sc.id = sik.id  
                                                              AND sc.colid = sik.colid  
                                    INNER JOIN dbo.sysobjects so ON so.name = si.name  
                                                              AND so.xtype = 'PK'  
                           WHERE    sc.id = col.id  
                                    AND sc.colid = col.colid ) THEN 1  
             ELSE 0  
        END AS 主键 ,  

        CASE WHEN col.isnullable = 1 THEN 1 
             ELSE 0  
        END AS 允许空 ,  
        ISNULL(comm.text, '') AS 默认值  
FROM    dbo.syscolumns col  
        LEFT  JOIN dbo.systypes t ON col.xtype = t.xusertype  
        inner JOIN dbo.sysobjects obj ON col.id = obj.id  
                                         AND obj.xtype = 'U'  
                                         AND obj.status >= 0  
        LEFT  JOIN dbo.syscomments comm ON col.cdefault = comm.id  
        LEFT  JOIN sys.extended_properties ep ON col.id = ep.major_id  
                                                      AND col.colid = ep.minor_id  
                                                      AND ep.name = 'MS_Description'  
        LEFT  JOIN sys.extended_properties epTwo ON obj.id = epTwo.major_id  
                                                         AND epTwo.minor_id = 0  
                                                         AND epTwo.name = 'MS_Description'  

ORDER BY TableName,col.colorder  ";

            SqlDataAdapter sqlAdapter = new SqlDataAdapter(sqlStr, sqlConn);

            DataSet ds = new DataSet();

            sqlAdapter.Fill(ds);

            return ds.Tables[0];

        }

        /// <summary>
        /// 根路径
        /// </summary>
        private static string RootPath = AppDomain.CurrentDomain.BaseDirectory;
        /// <summary>
        /// 模板路径
        /// </summary>
        private static string DBConfigPath
        {
            get
            {
                return Path.Combine(RootPath, "DBConfig.json");
            }
        }
        /// <summary>
        /// 读取数据库配置文件
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> GetDBConnString()
        {
            JObject Jobject = null;
            using (var config = File.OpenText(DBConfigPath))
            {

                Newtonsoft.Json.JsonTextReader Reader
                    = new Newtonsoft.Json.JsonTextReader(config);

                Jobject = JObject.Load(Reader);
            }
            Dictionary<string, string> configDic = new Dictionary<string, string>();
            //  var builder = new ConfigurationBuilder();
            ////  builder.Add(("DBConfig.json");

            //  var configuration = builder.Build();

            var v4 = Jobject["config"];

            var v8 = v4.Children().ToList();

            foreach (var item in v8)
            {
                var v1 = item["name"];
                var v2 = item["connString"];

                configDic.Add(v1.ToString(), v2.ToString());
            }

            return configDic;
        }
    }
}
