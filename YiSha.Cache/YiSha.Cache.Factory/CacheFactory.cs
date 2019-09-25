using YiSha.Cache.Interface;
using YiSha.MemoryCache;

namespace YiSha.Cache.Factory
{
    public class CacheFactory
    {
        public static ICache Cache()
        {
            return new MemoryCacheImp();
        }
    }
}
