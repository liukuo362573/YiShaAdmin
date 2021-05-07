using YiSha.Cache.Interface;
using YiSha.MemoryCache;
using YiSha.RedisCache;
using YiSha.Util.Model;

namespace YiSha.Cache.Factory
{
    public static class CacheFactory
    {
        private static ICache _cache;

        private static readonly object _lockHelper = new();

        public static ICache Cache
        {
            get
            {
                if (_cache != null)
                {
                    return _cache;
                }
                lock (_lockHelper)
                {
                    _cache ??= GlobalContext.SystemConfig.CacheProvider switch
                    {
                        "Redis" => new RedisCacheImp(),
                        "Memory" => new MemoryCacheImp(),
                        _ => _cache
                    };
                }
                return _cache;
            }
        }
    }
}