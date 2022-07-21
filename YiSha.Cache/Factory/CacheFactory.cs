using YiSha.Cache.Interface;
using YiSha.Util;

namespace YiSha.Cache.Factory
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
        private static readonly object lockHelper = new object();

        /// <summary>
        /// ICache
        /// </summary>
        public static ICache Cache
        {
            get
            {
                if (cache == null)
                {
                    lock (lockHelper)
                    {
                        if (cache == null)
                        {
                            var cacheProvider = GlobalContext.SystemConfig.CacheProvider;
                            switch (cacheProvider.ToLower())
                            {
                                case "redis":
                                    cache = new RedisCacheImp();
                                    break;

                                default:
                                case "memory":
                                    cache = new MemoryCacheImp();
                                    break;
                            }
                        }
                    }
                }
                return cache;
            }
        }
    }
}
