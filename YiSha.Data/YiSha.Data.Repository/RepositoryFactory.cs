using System;
using YiSha.Data.EF.Database;
using YiSha.Util.Model;

namespace YiSha.Data.Repository
{
    public class RepositoryFactory
    {
        public Repository BaseRepository()
        {
            IDatabase database = null;
            string dbType = GlobalContext.SystemConfig.DbProvider;
            string dbConnectionString = GlobalContext.SystemConfig.DbConnectionString;
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
                    // 支持Oracle或是更多数据库请参考上面SqlServer或是MySql的写法
                    break;
                default: throw new Exception("未找到数据库配置");
            }
            return new Repository(database);
        }
    }
}