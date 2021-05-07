using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using YiSha.Cache.Interface;
using YiSha.Util.Helper;
using YiSha.Util.Model;

namespace YiSha.MemoryCache
{
    public class MemoryCacheImp : ICache
    {
        private readonly IMemoryCache _cache = GlobalContext.ServiceProvider.GetService<IMemoryCache>();

        public bool SetCache<T>(string key, T value, DateTime? expireTime = null)
        {
            try
            {
                if (expireTime == null)
                {
                    return _cache.Set(key, value) != null;
                }
                return _cache.Set(key, value, expireTime.Value - DateTime.Now) != null;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return false;
        }

        public bool RemoveCache(string key)
        {
            _cache.Remove(key);
            return true;
        }

        public T GetCache<T>(string key)
        {
            return _cache.Get<T>(key);
        }

        #region Hash

        public int SetHashFieldCache<T>(string key, string fieldKey, T fieldValue)
        {
            return SetHashFieldCache(key, new Dictionary<string, T> { { fieldKey, fieldValue } });
        }

        public int SetHashFieldCache<T>(string key, Dictionary<string, T> dict)
        {
            return dict.Keys.Sum(unused => _cache.Set(key, dict) != null ? 1 : 0);
        }

        public T GetHashFieldCache<T>(string key, string fieldKey)
        {
            var dict = GetHashFieldCache(key, new Dictionary<string, T> { { fieldKey, default } });
            return dict[fieldKey];
        }

        public Dictionary<string, T> GetHashFieldCache<T>(string key, Dictionary<string, T> dict)
        {
            var hashFields = _cache.Get<Dictionary<string, T>>(key);
            foreach (var keyValuePair in hashFields.Where(p => dict.Keys.Contains(p.Key)))
            {
                dict[keyValuePair.Key] = keyValuePair.Value;
            }
            return dict;
        }

        public Dictionary<string, T> GetHashCache<T>(string key)
        {
            var dict = new Dictionary<string, T>();
            var hashFields = _cache.Get<Dictionary<string, T>>(key);
            foreach (string field in hashFields.Keys)
            {
                dict[field] = hashFields[field];
            }
            return dict;
        }

        public List<T> GetHashToListCache<T>(string key)
        {
            var hashFields = _cache.Get<Dictionary<string, T>>(key);
            return hashFields.Keys.Select(field => hashFields[field]).ToList();
        }

        public bool RemoveHashFieldCache(string key, string fieldKey)
        {
            var dict = new Dictionary<string, bool> { { fieldKey, false } };
            dict = RemoveHashFieldCache(key, dict);
            return dict[fieldKey];
        }

        public Dictionary<string, bool> RemoveHashFieldCache(string key, Dictionary<string, bool> dict)
        {
            var hashFields = _cache.Get<Dictionary<string, object>>(key);
            foreach (string fieldKey in dict.Keys)
            {
                dict[fieldKey] = hashFields.Remove(fieldKey);
            }
            return dict;
        }

        #endregion
    }
}