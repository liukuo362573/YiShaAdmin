using System.Security.AccessControl;
using YiSha.Cache.CacheImp;
using YiSha.Cache.Enum;
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
        /// 缓存类型
        /// </summary>
        public static CacheType Type
        {
            get
            {
                string dbTypeStr = GlobalContext.SystemConfig.CacheProvider;
                var cacheType = dbTypeStr.ToLower() switch
                {
                    "redis" => CacheType.Redis,
                    "memory" => CacheType.Memory,
                    _ => throw new Exception("未找到缓存配置"),
                };
                return cacheType;
            }
        }

        /// <summary>
        /// 缓存连接字符串
        /// </summary>
        public static string Connect
        {
            get
            {
                var connectionString = GlobalContext.SystemConfig.RedisConnectionString;
                return connectionString;
            }
        }

        /// <summary>
        /// ICache
        /// </summary>
        private static ICache cache = null;

        /// <summary>
        /// 锁
        /// </summary>
        private static readonly object lockHelper = new();

        /// <summary>
        /// 缓存对象
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
                    return cache = Type switch
                    {
                        CacheType.Redis => new RedisCacheImp(),
                        CacheType.Memory => new MemoryCacheImp(),
                        _ => throw new Exception("未找到缓存配置"),
                    };
                }
            }
        }
    }
}
