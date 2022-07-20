using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YiSha.DataBase;
using YiSha.DataBase.Interface;
using YiSha.Util;

namespace YiSha.Data.Repository
{
    public class RepositoryFactory
    {
        public Repository BaseRepository()
        {
            IDataBase database = DbFactory.Type switch
            {
                DbType.SqlServer => new SqlServerDatabase(),
                DbType.MySql => new MySqlDatabase(),
                DbType.Oracle => new OracleDatabase(),
                _ => throw new Exception("未找到数据库配置"),
            };
            return new Repository(database);
        }
    }
}
