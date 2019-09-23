using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using YiSha.Cache.Factory;
using YiSha.Entity.SystemManage;
using YiSha.Service.SystemManage;

namespace YiSha.Business.Cache
{
    public class MenuCache
    {
        private string cacheKey = typeof(MenuCache).Name;

        private MenuService menuService = new MenuService();

        public async Task<List<MenuEntity>> GetList()
        {
            var cacheList = CacheFactory.Cache().GetCache<List<MenuEntity>>(cacheKey);
            if (cacheList == null)
            {
                var data = await menuService.GetList(null);
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
