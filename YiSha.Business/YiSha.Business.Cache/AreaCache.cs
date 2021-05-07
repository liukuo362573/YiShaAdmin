using System.Collections.Generic;
using System.Threading.Tasks;
using YiSha.Cache.Factory;
using YiSha.Entity.SystemManage;
using YiSha.Service.SystemManage;

namespace YiSha.Business.Cache
{
    public class AreaCache : BaseBusinessCache<AreaEntity>
    {
        private readonly AreaService _areaService = new();

        protected override string CacheKey => GetType().Name;

        public override async Task<List<AreaEntity>> GetList()
        {
            var cacheList = CacheFactory.Cache.GetCache<List<AreaEntity>>(CacheKey);
            if (cacheList == null)
            {
                var result = await _areaService.GetList(null);
                CacheFactory.Cache.SetCache(CacheKey, result);
                return result;
            }
            return cacheList;
        }
    }
}