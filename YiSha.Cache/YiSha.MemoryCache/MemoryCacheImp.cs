using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Caching.Memory;
using YiSha.Cache.Interface;
using YiSha.Util;

namespace YiSha.MemoryCache
{
    public class MemoryCacheImp : ICache
    {
        private IMemoryCache cache = GlobalContext.ServiceProvider.GetService<IMemoryCache>();

        public void AddCache<T>(string cacheKey, T value)
        {
            cache.Set(cacheKey, value, DateTimeOffset.Now.AddMinutes(10));
        }

        public void AddCache<T>(string cacheKey, T value, DateTime expireTime)
        {
            cache.Set(cacheKey, value, expireTime);
        }

        public void RemoveCache(string cacheKey)
        {
            cache.Remove(cacheKey);
        }

        public void RemoveAllCache()
        {

        }

        public T GetCache<T>(string cacheKey)
        {
            if (cache.Get(cacheKey) != null)
            {
                return (T)cache.Get(cacheKey);
            }
            else
            {
                return default(T);
            }
        }
    }
}
