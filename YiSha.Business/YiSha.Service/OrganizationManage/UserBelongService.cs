using System.Collections.Generic;
using System.Threading.Tasks;
using YiSha.Data.Repository;
using YiSha.Entity.OrganizationManage;
using YiSha.Util.Extension;

namespace YiSha.Service.OrganizationManage
{
    public class UserBelongService : RepositoryFactory
    {
        #region 获取数据

        public async Task<List<UserBelongEntity>> GetList(UserBelongEntity entity)
        {
            var expression = LinqExtensions.True<UserBelongEntity>();
            if (entity?.BelongType != null)
            {
                expression = expression.And(t => t.BelongType == entity.BelongType);
            }
            if (entity?.UserId != null)
            {
                expression = expression.And(t => t.UserId == entity.UserId);
            }
            return await BaseRepository().FindList(expression);
        }

        public async Task<UserBelongEntity> GetEntity(long id)
        {
            return await BaseRepository().FindEntity<UserBelongEntity>(id);
        }

        #endregion

        #region 提交数据

        public async Task SaveForm(UserBelongEntity entity)
        {
            if (entity.Id.IsNullOrZero())
            {
                await entity.Create();
                await BaseRepository().Insert(entity);
            }
            else
            {
                await BaseRepository().Update(entity);
            }
        }

        public async Task DeleteForm(long id)
        {
            await BaseRepository().Delete<UserBelongEntity>(id);
        }

        #endregion
    }
}