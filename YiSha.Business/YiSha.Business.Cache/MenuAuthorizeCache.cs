using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using YiSha.Cache.Factory;
using YiSha.Entity.SystemManage;
using YiSha.Service.SystemManage;

namespace YiSha.Business.Cache
{
    public class MenuAuthorizeCache
    {
        private string cacheKey = typeof(MenuAuthorizeCache).Name;

        private MenuAuthorizeService menuAuthorizeService = new MenuAuthorizeService();

        public async Task<List<MenuAuthorizeEntity>> GetList()
        {
            var cacheList = CacheFactory.Cache().GetCache<List<MenuAuthorizeEntity>>(cacheKey);
            if (cacheList == null)
            {
                var data = await menuAuthorizeService.GetList(null);
                var list = data.ToList();
                CacheFactory.Cache().AddCache(cacheKey, list);
                return list;
            }
            else
            {
                return cacheList;
            }
        }

        public void Remove()
        {
            CacheFactory.Cache().RemoveCache(cacheKey);
        }
    }
}
