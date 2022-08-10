using YiSha.Cache.CacheImp;
using YiSha.Cache.Interface;
using YiSha.Util;

namespace YiSha.Cache
{
    /// <summary>
    /// 缓存工厂
    /// </summary>
    public class CacheFactory
    {
        /// <summary>
        /// ICache
        /// </summary>
        private static ICache cache = null;

        /// <summary>
        /// 锁
        /// </summary>
        private static readonly object lockHelper = new();

        /// <summary>
        /// ICache
        /// </summary>
        public static ICache Cache
        {
            get
            {
                if (cache != null) return cache;
                //
                lock (lockHelper)
                {
                    if (cache != null) return cache;
                    //
                    var cacheProvider = GlobalContext.SystemConfig.CacheProvider;
                    return cache = cacheProvider.ToLower() switch
                    {
                        "redis" => new RedisCacheImp(),
                        _ => new MemoryCacheImp(),
                    };
                }
            }
        }
    }
}
