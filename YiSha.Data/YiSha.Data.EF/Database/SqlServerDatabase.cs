using YiSha.Data.EF.Database;

namespace YiSha.Data.EF
{
    public sealed class SqlServerDatabase : AbstractDatabase
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        public SqlServerDatabase(string connString)
        {
            dbContext = new SqlServerDbContext(connString);
        }
    }
}