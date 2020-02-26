using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace YiSha.Cache.Interface
{
    public interface ICache
    {
        bool SetCache<T>(string key, T value, DateTime? expireTime = null);
        T GetCache<T>(string key);
        bool RemoveCache(string key);

        #region Hash
        int SetHashFieldCache<T>(string key, string fieldKey, T fieldValue);
        int SetHashFieldCache<T>(string key, Dictionary<string, T> dict);
        T GetHashFieldCache<T>(string key, string fieldKey);
        Dictionary<string, T> GetHashFieldCache<T>(string key, Dictionary<string, T> dict);
        Dictionary<string, T> GetHashCache<T>(string key);
        List<T> GetHashToListCache<T>(string key);
        bool RemoveHashFieldCache(string key, string fieldKey);
        Dictionary<string, bool> RemoveHashFieldCache(string key, Dictionary<string, bool> dict);
        #endregion
    }
}
