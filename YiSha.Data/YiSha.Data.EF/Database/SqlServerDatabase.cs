using YiSha.Data.EF.DbContext;

namespace YiSha.Data.EF.Database
{
    public sealed class SqlServerDatabase : AbstractDatabase
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        public SqlServerDatabase(string connString)
        {
            DbContext = new SqlServerDbContext(connString);
        }
    }
}