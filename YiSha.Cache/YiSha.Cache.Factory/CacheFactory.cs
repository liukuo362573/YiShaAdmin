using YiSha.Util;
using YiSha.Cache.Interface;
using YiSha.MemoryCache;
using YiSha.RedisCache;

namespace YiSha.Cache.Factory
{
    public class CacheFactory
    {
        public static ICache Cache()
        {
            switch (GlobalContext.SystemConfig.CacheProvider)
            {
                case "Redis":
                    return new RedisCacheImp();

                default:
                case "Memory":
                    return new MemoryCacheImp();
            }
        }
    }
}
