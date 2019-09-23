using System;
using YiSha.Cache.Interface;
using YiSha.WebCache;

namespace YiSha.Cache.Factory
{
    public class CacheFactory
    {
        public static ICache Cache()
        {
            return new WebCacheImp();
        }
    }
}
