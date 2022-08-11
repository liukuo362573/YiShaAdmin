using YiSha.Common;
using YiSha.DataBase;
using YiSha.Entity.SystemManage;
using YiSha.Util;

namespace YiSha.Service.SystemManage
{
    public class MenuAuthorizeService : Repository
    {
        #region 获取数据
        public async Task<List<MenuAuthorizeEntity>> GetList(MenuAuthorizeEntity param)
        {
            var expression = ExtensionLinq.True<MenuAuthorizeEntity>();
            if (param != null)
            {
                if (param.AuthorizeId.ToLong() > 0)
                {
                    expression = expression.And(t => t.AuthorizeId == param.AuthorizeId);
                }
                if (param.AuthorizeType.ToInt() > 0)
                {
                    expression = expression.And(t => t.AuthorizeType == param.AuthorizeType);
                }
                if (!param.AuthorizeIds.IsNull())
                {
                    long[] authorizeIdArr = TextHelper.SplitToArray<long>(param.AuthorizeIds, ',');
                    expression = expression.And(t => authorizeIdArr.Contains(t.AuthorizeId));
                }
            }
            var list = await this.FindList<MenuAuthorizeEntity>(expression);
            return list.ToList();
        }

        public async Task<MenuAuthorizeEntity> GetEntity(long id)
        {
            return await this.FindEntity<MenuAuthorizeEntity>(id);
        }
        #endregion

        #region 提交数据
        public async Task SaveForm(MenuAuthorizeEntity entity)
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
            await this.Delete<MenuAuthorizeEntity>(id);
        }
        #endregion
    }
}
