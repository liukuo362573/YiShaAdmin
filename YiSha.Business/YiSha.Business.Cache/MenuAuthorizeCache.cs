using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YiSha.Cache.Factory;
using YiSha.Entity.SystemManage;
using YiSha.Service.SystemManage;

namespace YiSha.Business.Cache
{
    public class MenuAuthorizeCache : BaseBusinessCache<MenuAuthorizeEntity>
    {
        private readonly MenuAuthorizeService _menuAuthorizeService = new();

        protected override string CacheKey => GetType().Name;

        public override async Task<List<MenuAuthorizeEntity>> GetList()
        {
            var cacheList = CacheFactory.Cache.GetCache<List<MenuAuthorizeEntity>>(CacheKey);
            if (cacheList == null)
            {
                var list = await _menuAuthorizeService.GetList(null);
                CacheFactory.Cache.SetCache(CacheKey, list);
                return list;
            }
            return cacheList;
        }

        public async Task<List<MenuAuthorizeEntity>> GetList(Func<MenuAuthorizeEntity, bool> predicate)
        {
            var cacheList = CacheFactory.Cache.GetCache<List<MenuAuthorizeEntity>>(CacheKey);
            if (cacheList == null)
            {
                var list = await _menuAuthorizeService.GetList(null);
                CacheFactory.Cache.SetCache(CacheKey, list);
                cacheList = list;
            }
            return cacheList.Where(predicate).ToList();
        }
    }
}