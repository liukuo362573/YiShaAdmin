using YiSha.Data.EF.Database;

namespace YiSha.Data.EF
{
    public sealed class MySqlDatabase : AbstractDatabase
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        public MySqlDatabase(string connString)
        {
            dbContext = new MySqlDbContext(connString);
        }
    }
}