using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using YiSha.Cache.Interface;
using YiSha.Util;

namespace YiSha.Cache.CacheImp
{
    /// <summary>
    /// Memory 缓存
    /// </summary>
    public class MemoryCacheImp : ICache
    {
        /// <summary>
        /// Memory 缓存
        /// </summary>
        private IMemoryCache cache;

        /// <summary>
        /// Memory 缓存实例时
        /// </summary>
        public MemoryCacheImp()
        {
            cache = GlobalContext.ServiceProvider.GetService<IMemoryCache>();
        }

        /// <summary>
        /// 读取缓存
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="key">键</param>
        /// <returns></returns>
        public T GetCache<T>(string key)
        {
            var value = cache.Get<T>(key);
            return value;
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
            try
            {
                if (expireTime == null) cache.Set(key, value);
                else cache.Set(key, value, expireTime.Value - DateTime.Now);
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return false;
        }

        /// <summary>
        /// 删除缓存
        /// </summary>
        /// <param name="key">类型</param>
        /// <returns></returns>
        public bool RemoveCache(string key)
        {
            cache?.Remove(key);
            return true;
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
            var list = new List<T>();
            var hashFields = cache.Get<Dictionary<string, T>>(key);
            foreach (var field in hashFields.Keys)
            {
                list.Add(hashFields[field]);
            }
            return list;
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
            var hashFields = cache.Get<Dictionary<string, T>>(key);
            foreach (var keyValuePair in hashFields.Where(p => dict.Keys.Contains(p.Key)))
            {
                dict[keyValuePair.Key] = keyValuePair.Value;
            }
            return dict;
        }

        /// <summary>
        /// 读取缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public Dictionary<string, T> GetHashCache<T>(string key)
        {
            var dict = new Dictionary<string, T>();
            var hashFields = cache.Get<Dictionary<string, T>>(key);
            foreach (var field in hashFields.Keys)
            {
                dict[field] = hashFields[field];
            }
            return dict;
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
            int count = 0;
            var dictpre = GetHashCache<T>(key);
            if (dictpre == null)
            {
                cache.Set(key, dict);
                return dict.Count;
            }
            else
            {
                foreach (string fieldKey in dict.Keys)
                {
                    count += dictpre.TryAdd(fieldKey, dict[fieldKey]) ? 1 : 0;
                }
                cache.Set(key, dict);
                return count;
            }
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
            dict = RemoveHashFieldCache(key, dict);
            return dict[fieldKey];
        }

        /// <summary>
        /// 删除缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dict"></param>
        /// <returns></returns>
        public Dictionary<string, bool> RemoveHashFieldCache(string key, Dictionary<string, bool> dict)
        {
            var hashFields = cache.Get<Dictionary<string, object>>(key);
            foreach (var fieldKey in dict.Keys)
            {
                dict[fieldKey] = hashFields.Remove(fieldKey);
            }
            return dict;
        }
    }
}
