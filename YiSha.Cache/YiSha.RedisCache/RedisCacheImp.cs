using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using StackExchange.Redis;
using YiSha.Cache.Interface;
using YiSha.Util;

namespace YiSha.RedisCache
{
    public class RedisCacheImp : ICache
    {
        private IDatabase cache;
        private ConnectionMultiplexer connection;

        public RedisCacheImp()
        {
            connection = ConnectionMultiplexer.Connect(GlobalContext.SystemConfig.RedisConnectionString);
            cache = connection.GetDatabase();
        }

        public bool SetCache<T>(string key, T value, DateTime? expireTime = null)
        {
            try
            {
                var jsonOption = new JsonSerializerSettings()
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
                    return cache.StringSet(key, strValue);
                }
                else
                {
                    return cache.StringSet(key, strValue, (expireTime.Value - DateTime.Now));
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteWithTime(ex);
            }
            return false;
        }

        public bool RemoveCache(string key)
        {
            return cache.KeyDelete(key);
        }

        public T GetCache<T>(string key)
        {
            var t = default(T);
            try
            {
                var value = cache.StringGet(key);
                if (string.IsNullOrEmpty(value))
                {
                    return t;
                }
                t = JsonConvert.DeserializeObject<T>(value);
            }
            catch (Exception ex)
            {
                LogHelper.WriteWithTime(ex);
            }
            return t;
        }

        #region Hash
        public int SetHashFieldCache<T>(string key, string fieldKey, T fieldValue)
        {
            return SetHashFieldCache<T>(key, new Dictionary<string, T> { { fieldKey, fieldValue } });
        }

        public int SetHashFieldCache<T>(string key, Dictionary<string, T> dict)
        {
            int count = 0;
            var jsonOption = new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
            foreach (string fieldKey in dict.Keys)
            {
                string fieldValue = JsonConvert.SerializeObject(dict[fieldKey], jsonOption);
                count += cache.HashSet(key, fieldKey, fieldValue) ? 1 : 0;
            }
            return count;
        }

        public T GetHashFieldCache<T>(string key, string fieldKey)
        {
            var dict = GetHashFieldCache<T>(key, new Dictionary<string, T> { { fieldKey, default(T) } });
            return dict[fieldKey];
        }

        public Dictionary<string, T> GetHashFieldCache<T>(string key, Dictionary<string, T> dict)
        {
            foreach (string fieldKey in dict.Keys)
            {
                string fieldValue = cache.HashGet(key, fieldKey);
                dict[fieldKey] = JsonConvert.DeserializeObject<T>(fieldValue);
            }
            return dict;
        }

        public Dictionary<string, T> GetHashCache<T>(string key)
        {
            Dictionary<string, T> dict = new Dictionary<string, T>();
            var hashFields = cache.HashGetAll(key);
            foreach (HashEntry field in hashFields)
            {
                dict[field.Name] = JsonConvert.DeserializeObject<T>(field.Value);
            }
            return dict;
        }

        public List<T> GetHashToListCache<T>(string key)
        {
            List<T> list = new List<T>();
            var hashFields = cache.HashGetAll(key);
            foreach (HashEntry field in hashFields)
            {
                list.Add(JsonConvert.DeserializeObject<T>(field.Value));
            }
            return list;
        }

        public bool RemoveHashFieldCache(string key, string fieldKey)
        {
            Dictionary<string, bool> dict = new Dictionary<string, bool> { { fieldKey, false } };
            dict = RemoveHashFieldCache(key, dict);
            return dict[fieldKey];
        }

        public Dictionary<string, bool> RemoveHashFieldCache(string key, Dictionary<string, bool> dict)
        {
            foreach (string fieldKey in dict.Keys)
            {
                dict[fieldKey] = cache.HashDelete(key, fieldKey);
            }
            return dict;
        }
        #endregion

        public void Dispose()
        {
            if (connection != null)
                connection.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
