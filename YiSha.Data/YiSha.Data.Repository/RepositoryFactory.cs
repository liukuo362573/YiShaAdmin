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
            switch (DbFactory.Type)
            {
                case DbType.SqlServer:
                    database = new SqlServerDatabase();
                    break;
                case DbType.MySql:
                    database = new MySqlDatabase();
                    break;
                case DbType.Oracle:
                    database = new OracleDatabase();
                    break;
                default:
                    throw new Exception("未找到数据库配置");
            }
            return new Repository(database);
        }
    }
}
