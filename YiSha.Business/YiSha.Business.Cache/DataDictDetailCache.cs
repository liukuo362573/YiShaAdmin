using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YiSha.Cache.Factory;
using YiSha.Entity.SystemManage;
using YiSha.Service.SystemManage;

namespace YiSha.Business.Cache
{
    public class DataDictDetailCache
    {
        private string cacheKey = typeof(DataDictDetailCache).Name;

        private DataDictDetailService dataDictDetailService = new DataDictDetailService();

        public async Task<List<DataDictDetailEntity>> GetList()
        {
            var cacheList = CacheFactory.Cache().GetCache<List<DataDictDetailEntity>>(cacheKey);
            if (cacheList == null)
            {
                var data = await dataDictDetailService.GetList(null);
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
