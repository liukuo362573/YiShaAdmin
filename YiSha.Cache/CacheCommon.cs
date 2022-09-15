using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YiSha.Cache.CacheImp;
using YiSha.Cache.Interface;
using YiSha.Util;

namespace YiSha.Cache
{
    /// <summary>
    /// <b>缓存数据对象</b>
    ///
    /// <para>常规使用：using var chComm = new CacheCommon()</para>
    /// <para>注入使用：services.AddSingleton&lt;CacheCommon&gt;()</para>
    /// <para>继承此对象可以实现原生操作！by zgcwkj</para>
    /// </summary>
    public class CacheCommon
    {
        /// <summary>
        /// 缓存类型
        /// </summary>
        private ICache Cache { get; }

        /// <summary>
        /// 实例时
        /// </summary>
        public CacheCommon()
        {
            var cacheProvider = GlobalContext.SystemConfig.CacheProvider;
            Cache = cacheProvider.ToLower() switch
            {
                "redis" => new RedisCacheImp(),
                _ => new MemoryCacheImp(),
            };
        }

        /// <summary>
        /// 读取缓存
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="key">键</param>
        /// <returns></returns>
        public T GetCache<T>(string key)
        {
            return Cache.GetCache<T>(key);
        }

        /// <summary>
        /// 写入缓存
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="expireTime">过期时间</param>
        /// <returns></returns>
        public bool SetCache<T>(string key, T value, DateTime? expireTime = null)
        {
            return Cache.SetCache<T>(key, value, expireTime);
        }

        /// <summary>
        /// 删除缓存
        /// </summary>
        /// <param name="key">类型</param>
        /// <returns></returns>
        public bool RemoveCache(string key)
        {
            return Cache.RemoveCache(key);
        }

        /// <summary>
        /// 读取缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="fieldKey"></param>
        /// <returns></returns>
        public T GetHashFieldCache<T>(string key, string fieldKey)
        {
            var dic = new Dictionary<string, T> { { fieldKey, default } };
            var dict = GetHashFieldCache(key, dic);
            return dict[fieldKey];
        }

        /// <summary>
        /// 读取缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public List<T> GetHashToListCache<T>(string key)
        {
            return Cache.GetHashToListCache<T>(key);
        }

        /// <summary>
        /// 读取缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="dict"></param>
        /// <returns></returns>
        public Dictionary<string, T> GetHashFieldCache<T>(string key, Dictionary<string, T> dict)
        {
            return Cache.GetHashFieldCache<T>(key, dict);
        }

        /// <summary>
        /// 读取缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public Dictionary<string, T> GetHashCache<T>(string key)
        {
            return Cache.GetHashCache<T>(key);
        }

        /// <summary>
        /// 写入缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="fieldKey"></param>
        /// <param name="fieldValue"></param>
        /// <returns></returns>
        public int SetHashFieldCache<T>(string key, string fieldKey, T fieldValue)
        {
            return SetHashFieldCache(key, new Dictionary<string, T> { { fieldKey, fieldValue } });
        }

        /// <summary>
        /// 写入缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="dict"></param>
        /// <returns></returns>
        public int SetHashFieldCache<T>(string key, Dictionary<string, T> dict)
        {
            return Cache.SetHashFieldCache<T>(key, dict);
        }

        /// <summary>
        /// 删除缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="fieldKey"></param>
        /// <returns></returns>
        public bool RemoveHashFieldCache(string key, string fieldKey)
        {
            var dict = new Dictionary<string, bool> { { fieldKey, false } };
            dict = Cache.RemoveHashFieldCache(key, dict);
            return dict[fieldKey];
        }

        ///// <summary>
        ///// 删除缓存
        ///// </summary>
        ///// <param name="key"></param>
        ///// <param name="dict"></param>
        ///// <returns></returns>
        //public Dictionary<string, bool> RemoveHashFieldCache(string key, Dictionary<string, bool> dict)
        //{
        //    if (dict == null) dict = new Dictionary<string, bool> { };
        //    return Cache.RemoveHashFieldCache（key, dict);
        //}
    }
}
