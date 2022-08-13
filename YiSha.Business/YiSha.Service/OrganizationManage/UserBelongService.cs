using YiSha.Common;
using YiSha.DataBase;
using YiSha.Entity.OrganizationManage;
using YiSha.Util;

namespace YiSha.Service.OrganizationManage
{
    public class UserBelongService : Repository
    {
        #region 获取数据
        public async Task<List<UserBelongEntity>> GetList(UserBelongEntity entity)
        {
            var expression = ExtensionLinq.True<UserBelongEntity>();
            if (entity != null)
            {
                if (entity.BelongType != null)
                {
                    expression = expression.And(t => t.BelongType == entity.BelongType);
                }
                if (entity.UserId != null)
                {
                    expression = expression.And(t => t.UserId == entity.UserId);
                }
            }
            var list = await this.FindList(expression);
            return list.ToList();
        }

        public async Task<UserBelongEntity> GetEntity(long id)
        {
            return await this.FindEntity<UserBelongEntity>(id);
        }
        #endregion

        #region 提交数据
        public async Task SaveForm(UserBelongEntity entity)
        {
            if (entity.Id.IsNullOrZero())
            {
                await entity.Create();
                await this.Insert(entity);
            }
            else
            {
                await this.Update(entity);
            }
        }

        public async Task DeleteForm(long id)
        {
            await this.Delete<UserBelongEntity>(id);
        }
        #endregion
    }
}
