using System.Linq;
using System.Collections.Generic;
using YiSha.Cache.Factory;
using YiSha.Entity.SystemManage;
using YiSha.Service.SystemManage;
using System.Threading.Tasks;

namespace YiSha.Business.Cache
{
    public class AreaCache : BaseBusinessCache<AreaEntity>
    {
        private AreaService areaService = new AreaService();

        public override string CacheKey => this.GetType().Name;

        public override async Task<List<AreaEntity>> GetList()
        {
            var cacheList = CacheFactory.Cache.GetCache<List<AreaEntity>>(CacheKey);
            if (cacheList == null)
            {
                var result = await areaService.GetList(null);
                CacheFactory.Cache.SetCache(CacheKey, result);
                return result;
            }
            else
            {
                return cacheList;
            }
        }
    }
}
