namespace YiSha.Cache.Interface
{
    /// <summary>
    /// 缓存抽象类
    /// </summary>
    public interface ICache
    {
        /// <summary>
        /// Key 是否存在
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="db">数据库索引</param>
        /// <returns>存在</returns>
        bool Exists(string key, int db = -1);

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="db">数据库索引</param>
        /// <param name="timeSpan">时间差</param>
        /// <returns>状态</returns>
        bool Set<T>(string key, T value, int db = -1, TimeSpan timeSpan = default);

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="key">键</param>
        /// <param name="db">数据库索引</param>
        /// <returns>数据</returns>
        T Get<T>(string key, int db = -1);

        /// <summary>
        /// 删除缓存
        /// </summary>
        /// <param name="key">类型</param>
        /// <param name="db">数据库索引</param>
        /// <returns>状态</returns>
        bool Remove(string key, int db = -1);

        /// <summary>
        /// Key 是否存在(哈希)
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="hashKey">哈希键</param>
        /// <param name="db">数据库索引</param>
        /// <returns>存在</returns>
        bool HashExists(string key, string hashKey, int db = -1);

        /// <summary>
        /// 设置缓存(哈希)
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="hashKey">哈希键</param>
        /// <param name="hashValue">哈希值</param>
        /// <param name="db">数据库索引</param>
        /// <returns>状态</returns>
        bool HashSet<T>(string key, string hashKey, T hashValue, int db = -1);

        /// <summary>
        /// 获取缓存(哈希)
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="key">键</param>
        /// <param name="hashKey">哈希键</param>
        /// <param name="db">数据库索引</param>
        /// <returns>数据</returns>
        T HashGet<T>(string key, string hashKey, int db = -1);

        /// <summary>
        /// 删除缓存(哈希)
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="hashKey">哈希键</param>
        /// <param name="db">数据库索引</param>
        /// <returns>状态</returns>
        long HashRemove(string key, string hashKey, int db = -1);
    }
}
