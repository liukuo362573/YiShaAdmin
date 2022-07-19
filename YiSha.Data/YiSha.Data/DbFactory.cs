using System;
using YiSha.Util;

namespace YiSha.Data
{
    /// <summary>
    /// 数据库工厂
    /// </summary>
    public class DbFactory
    {
        /// <summary>
        /// 数据库类型
        /// </summary>
        public static DbType Type
        {
            get
            {
                var dbTypeStr = GlobalContext.SystemConfig.DBProvider ?? "sqlserver";
                var dbType = dbTypeStr.ToLower() switch
                {
                    "sqlserver" => DbType.SqlServer,
                    "mysql" => DbType.MySql,
                    "oracle" => DbType.Oracle,
                    _ => throw new Exception("未找到数据库配置"),
                };
                return dbType;
            }
        }

        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public static string Connect
        {
            get
            {
                var dbConnect = GlobalContext.SystemConfig.DBConnectionString;
                return dbConnect;
            }
        }

        /// <summary>
        /// 数据库连接命令超时
        /// </summary>
        public static int Timeout
        {
            get
            {
                var dbTimeoutStr = GlobalContext.SystemConfig.DBCommandTimeout;
                return dbTimeoutStr <= 0 ? 10 : Convert.ToInt32(dbTimeoutStr);
            }
        }
    }
}
