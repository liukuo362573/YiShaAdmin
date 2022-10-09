using Microsoft.Extensions.Caching.Memory;
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
        private ICache Cache
        {
            get
            {
                return CacheFactory.Cache;
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
            return Cache.Exists(key, db);
        }

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="db">数据库索引</param>
        /// <param name="timeSpan">时间差</param>
        /// <returns></returns>
        public bool Set<T>(string key, T value, int db = -1, TimeSpan timeSpan = default)
        {
            return Cache.Set(key, value, db, timeSpan);
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="key">键</param>
        /// <param name="db">数据库索引</param>
        /// <returns></returns>
        public T Get<T>(string key, int db = -1)
        {
            return Cache.Get<T>(key, db);
        }

        /// <summary>
        /// 删除缓存
        /// </summary>
        /// <param name="key">类型</param>
        /// <param name="db">数据库索引</param>
        /// <returns></returns>
        public bool Remove(string key, int db = -1)
        {
            return Cache.Remove(key, db);
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
            return Cache.HashExists(key, hashKey, db);
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
            return Cache.HashSet<T>(key, hashKey, hashValue, db);
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
            return Cache.HashGet<T>(key, hashKey, db);
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
            return Cache.HashRemove(key, hashKey);
        }
    }
}
