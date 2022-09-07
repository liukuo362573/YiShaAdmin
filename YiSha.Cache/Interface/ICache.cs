namespace YiSha.Cache.Interface
{
    /// <summary>
    /// 缓存抽象
    /// </summary>
    public interface ICache
    {
        /// <summary>
        /// 读取缓存
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="key">键</param>
        /// <returns></returns>
        T GetCache<T>(string key);

        /// <summary>
        /// 写入缓存
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="expireTime">过期时间</param>
        /// <returns></returns>
        bool SetCache<T>(string key, T value, DateTime? expireTime = null);

        /// <summary>
        /// 删除缓存
        /// </summary>
        /// <param name="key">类型</param>
        /// <returns></returns>
        bool RemoveCache(string key);

        /// <summary>
        /// 读取缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="fieldKey"></param>
        /// <returns></returns>
        T GetHashFieldCache<T>(string key, string fieldKey);

        /// <summary>
        /// 读取缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        List<T> GetHashToListCache<T>(string key);

        /// <summary>
        /// 读取缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="dict"></param>
        /// <returns></returns>
        Dictionary<string, T> GetHashFieldCache<T>(string key, Dictionary<string, T> dict);

        /// <summary>
        /// 读取缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        Dictionary<string, T> GetHashCache<T>(string key);

        /// <summary>
        /// 写入缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="fieldKey"></param>
        /// <param name="fieldValue"></param>
        /// <returns></returns>
        int SetHashFieldCache<T>(string key, string fieldKey, T fieldValue);

        /// <summary>
        /// 写入缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="dict"></param>
        /// <returns></returns>
        int SetHashFieldCache<T>(string key, Dictionary<string, T> dict);

        /// <summary>
        /// 删除缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="fieldKey"></param>
        /// <returns></returns>
        bool RemoveHashFieldCache(string key, string fieldKey);

        /// <summary>
        /// 删除缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dict"></param>
        /// <returns></returns>
        Dictionary<string, bool> RemoveHashFieldCache(string key, Dictionary<string, bool> dict);
    }
}
