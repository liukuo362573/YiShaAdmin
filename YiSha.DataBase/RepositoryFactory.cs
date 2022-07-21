using YiSha.DataBase.Enum;

namespace YiSha.DataBase
{
    public class RepositoryFactory
    {
        public Repository BaseRepository()
        {
            return new Repository();
        }
    }
}
