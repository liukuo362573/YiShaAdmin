using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using YiSha.Cache.Interface;
using YiSha.Util.Helper;
using YiSha.Util.Model;

namespace YiSha.RedisCache
{
    public class RedisCacheImp : ICache
    {
        private readonly IDatabase _cache;

        private readonly ConnectionMultiplexer _connection;

        public RedisCacheImp()
        {
            _connection = ConnectionMultiplexer.Connect(GlobalContext.SystemConfig.RedisConnectionString);
            _cache = _connection.GetDatabase();
        }

        public bool SetCache<T>(string key, T value, DateTime? expireTime = null)
        {
            try
            {
                var jsonOption = new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                };
                string strValue = JsonConvert.SerializeObject(value, jsonOption);
                if (string.IsNullOrEmpty(strValue))
                {
                    return false;
                }
                if (expireTime == null)
                {
                    return _cache.StringSet(key, strValue);
                }
                return _cache.StringSet(key, strValue, expireTime.Value - DateTime.Now);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return false;
        }

        public bool RemoveCache(string key)
        {
            return _cache.KeyDelete(key);
        }

        public T GetCache<T>(string key)
        {
            var t = default(T);
            try
            {
                var value = _cache.StringGet(key);
                if (string.IsNullOrEmpty(value))
                {
                    return t;
                }
                t = JsonConvert.DeserializeObject<T>(value);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return t;
        }

        #region Hash

        public int SetHashFieldCache<T>(string key, string fieldKey, T fieldValue)
        {
            return SetHashFieldCache(key, new Dictionary<string, T> { { fieldKey, fieldValue } });
        }

        public int SetHashFieldCache<T>(string key, Dictionary<string, T> dict)
        {
            int count = 0;
            var jsonOption = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
            foreach (string fieldKey in dict.Keys)
            {
                string fieldValue = JsonConvert.SerializeObject(dict[fieldKey], jsonOption);
                count += _cache.HashSet(key, fieldKey, fieldValue) ? 1 : 0;
            }
            return count;
        }

        public T GetHashFieldCache<T>(string key, string fieldKey)
        {
            var dict = GetHashFieldCache(key, new Dictionary<string, T> { { fieldKey, default } });
            return dict[fieldKey];
        }

        public Dictionary<string, T> GetHashFieldCache<T>(string key, Dictionary<string, T> dict)
        {
            foreach (string fieldKey in dict.Keys)
            {
                string fieldValue = _cache.HashGet(key, fieldKey);
                dict[fieldKey] = JsonConvert.DeserializeObject<T>(fieldValue);
            }
            return dict;
        }

        public Dictionary<string, T> GetHashCache<T>(string key)
        {
            Dictionary<string, T> dict = new Dictionary<string, T>();
            var hashFields = _cache.HashGetAll(key);
            foreach (HashEntry field in hashFields)
            {
                dict[field.Name] = JsonConvert.DeserializeObject<T>(field.Value);
            }
            return dict;
        }

        public List<T> GetHashToListCache<T>(string key)
        {
            List<T> list = new List<T>();
            var hashFields = _cache.HashGetAll(key);
            foreach (HashEntry field in hashFields)
            {
                list.Add(JsonConvert.DeserializeObject<T>(field.Value));
            }
            return list;
        }

        public bool RemoveHashFieldCache(string key, string fieldKey)
        {
            var dict = RemoveHashFieldCache(key, new Dictionary<string, bool> { { fieldKey, false } });
            return dict[fieldKey];
        }

        public Dictionary<string, bool> RemoveHashFieldCache(string key, Dictionary<string, bool> dict)
        {
            foreach (string fieldKey in dict.Keys)
            {
                dict[fieldKey] = _cache.HashDelete(key, fieldKey);
            }
            return dict;
        }

        #endregion Hash

        public void Dispose()
        {
            _connection?.Close();
            GC.SuppressFinalize(this);
        }
    }
}