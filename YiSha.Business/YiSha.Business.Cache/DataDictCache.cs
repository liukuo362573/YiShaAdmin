using System.Collections.Generic;
using System.Threading.Tasks;
using YiSha.Cache.Factory;
using YiSha.Entity.SystemManage;
using YiSha.Service.SystemManage;

namespace YiSha.Business.Cache
{
    public class DataDictCache : BaseBusinessCache<DataDictEntity>
    {
        private readonly DataDictService _dataDictService = new();

        protected override string CacheKey => GetType().Name;

        public override async Task<List<DataDictEntity>> GetList()
        {
            var cacheList = CacheFactory.Cache.GetCache<List<DataDictEntity>>(CacheKey);
            if (cacheList == null)
            {
                var list = await _dataDictService.GetList(null);
                CacheFactory.Cache.SetCache(CacheKey, list);
                return list;
            }
            return cacheList;
        }
    }
}