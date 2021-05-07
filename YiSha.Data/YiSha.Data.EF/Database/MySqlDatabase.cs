using YiSha.Data.EF.DbContext;

namespace YiSha.Data.EF.Database
{
    public sealed class MySqlDatabase : AbstractDatabase
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        public MySqlDatabase(string connString)
        {
            DbContext = new MySqlDbContext(connString);
        }
    }
}