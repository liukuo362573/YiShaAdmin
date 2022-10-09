using StackExchange.Redis;
using System.Text.Json;
using YiSha.Cache.Interface;
using YiSha.Util;

namespace YiSha.Cache.CacheImp
{
    /// <summary>
    /// Redis 缓存
    /// </summary>
    internal class RedisCacheImp : ICache
    {
        /// <summary>
        /// Redis 缓存
        /// </summary>
        private static ConnectionMultiplexer multiplexer;

        /// <summary>
        /// Redis 缓存实例时
        /// </summary>
        public RedisCacheImp()
        {
            if (multiplexer.IsNull())
            {
                var configuration = ConfigurationOptions.Parse(CacheFactory.Connect, true);
                configuration.ResolveDns = true;
                multiplexer = ConnectionMultiplexer.Connect(configuration);
            }
        }

        /// <summary>
        /// Key 是否存在
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="db">数据库索引</param>
        /// <returns>存在</returns>
        public bool Exists(string key, int db = -1)
        {
            var redisBase = multiplexer.GetDatabase(db);
            return redisBase.KeyExists(key);
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="key">键</param>
        /// <param name="db">数据库索引</param>
        /// <returns>数据</returns>
        public T Get<T>(string key, int db = -1)
        {
            var redisBase = multiplexer.GetDatabase(db);
            var data = redisBase.StringGet(key);
            return JsonSerializer.Deserialize<T>(data);
        }

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="db">数据库索引</param>
        /// <param name="timeSpan">时间差</param>
        /// <returns>状态</returns>
        public bool Set<T>(string key, T value, int db = -1, TimeSpan timeSpan = default)
        {
            var redisBase = multiplexer.GetDatabase(db);
            try
            {
                if (timeSpan == default)
                {
                    return redisBase.StringSet(key, JsonSerializer.Serialize(value));
                }
                return redisBase.StringSet(key, JsonSerializer.Serialize(value), timeSpan);
            }
            catch { }
            return false;
        }

        /// <summary>
        /// 删除缓存
        /// </summary>
        /// <param name="key">类型</param>
        /// <param name="db">数据库索引</param>
        /// <returns>状态</returns>
        public bool Remove(string key, int db = -1)
        {
            var redisBase = multiplexer.GetDatabase(db);
            return redisBase.KeyDelete(key);
        }

        /// <summary>
        /// Key 是否存在(哈希)
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="hashKey">哈希键</param>
        /// <param name="db">数据库索引</param>
        /// <returns>存在</returns>
        public bool HashExists(string key, string hashKey, int db = -1)
        {
            var redisBase = multiplexer.GetDatabase(db);
            return redisBase.HashExists(key, hashKey);
        }

        /// <summary>
        /// 获取缓存(哈希)
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="key">键</param>
        /// <param name="hashKey">哈希键</param>
        /// <param name="db">数据库索引</param>
        /// <returns>数据</returns>
        public T HashGet<T>(string key, string hashKey, int db = -1)
        {
            var redisBase = multiplexer.GetDatabase(db);
            var data = redisBase.HashGet(key, hashKey);
            return JsonSerializer.Deserialize<T>(data);
        }

        /// <summary>
        /// 设置缓存(哈希)
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="hashKey">哈希键</param>
        /// <param name="hashValue">哈希值</param>
        /// <param name="db">数据库索引</param>
        /// <returns>状态</returns>
        public bool HashSet<T>(string key, string hashKey, T hashValue, int db = -1)
        {
            var redisBase = multiplexer.GetDatabase(db);
            return redisBase.HashSet(key, hashKey, JsonSerializer.Serialize(hashValue));
        }

        /// <summary>
        /// 删除缓存(哈希)
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="hashKey">哈希键</param>
        /// <param name="db">数据库索引</param>
        /// <returns>状态</returns>
        public long HashRemove(string key, string hashKey, int db = -1)
        {
            var redisBase = multiplexer.GetDatabase(db);
            var remove = redisBase.HashDelete(key, hashKey);
            return remove ? 1 : 0;
        }
    }
}
