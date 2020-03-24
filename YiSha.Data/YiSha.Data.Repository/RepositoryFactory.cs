using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YiSha.Data.EF;
using YiSha.Util;

namespace YiSha.Data.Repository
{
    public class RepositoryFactory
    {
        public Repository BaseRepository()
        {
            IDatabase database = null;
            string dbType = GlobalContext.SystemConfig.DBProvider;
            string dbConnectionString = GlobalContext.SystemConfig.DBConnectionString;
            switch (dbType)
            {
                case "SqlServer":
                    DbHelper.DbType = DatabaseType.SqlServer;
                    database = new SqlServerDatabase(dbConnectionString);
                    break;
                case "MySql":
                    DbHelper.DbType = DatabaseType.MySql;
                    database = new MySqlDatabase(dbConnectionString);
                    break;
                case "Oracle":
                    DbHelper.DbType = DatabaseType.Oracle;
                    //database = new OracleDatabase(sConnectString);
                    break;
                default:
                    throw new Exception("未找到数据库配置");
            }
            return new Repository(database);
        }
    }
}
