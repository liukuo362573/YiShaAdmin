using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using YiSha.Cache.Factory;
using YiSha.Entity.OrganizationManage;
using YiSha.Service.OrganizationManage;

namespace YiSha.Business.Cache
{
    public class UserCache : IBusinessCache<UserEntity>
    {
        private string cacheKey = typeof(UserCache).Name;
        private UserService userService = new UserService();
        public async Task<List<UserEntity>> GetList()
        {
            var cacheList = CacheFactory.Cache().GetCache<List<UserEntity>>(cacheKey);
            if (cacheList == null)
            {
                var data = await userService.GetList(null);
                var list = data.ToList();
                CacheFactory.Cache().AddCache(cacheKey, list);
                return list;
            }
            else
            {
                return cacheList;
            }
        }

        public void Remove(long id)
        {
            var cacheList = CacheFactory.Cache().GetCache<List<UserEntity>>(cacheKey);
            cacheList.RemoveAll(p => p.Id == id);
        }

        public void Update(long id)
        {
            var cacheList = CacheFactory.Cache().GetCache<List<UserEntity>>(cacheKey);
            var rawEntity = userService.GetEntity(id);
        }

        public void RemoveByToken(string token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                CacheFactory.Cache().RemoveCache(token);
            }
        }

    }
}
