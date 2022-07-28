using YiSha.DataBase;
using YiSha.Entity.SystemManage;
using YiSha.Util;
using YiSha.Util.Extension;

namespace YiSha.Service.SystemManage
{
    public class MenuAuthorizeService : Repository
    {
        #region 获取数据
        public async Task<List<MenuAuthorizeEntity>> GetList(MenuAuthorizeEntity param)
        {
            var expression = LinqExtensions.True<MenuAuthorizeEntity>();
            if (param != null)
            {
                if (param.AuthorizeId.ParseToLong() > 0)
                {
                    expression = expression.And(t => t.AuthorizeId == param.AuthorizeId);
                }
                if (param.AuthorizeType.ParseToInt() > 0)
                {
                    expression = expression.And(t => t.AuthorizeType == param.AuthorizeType);
                }
                if (!param.AuthorizeIds.IsEmpty())
                {
                    long[] authorizeIdArr = TextHelper.SplitToArray<long>(param.AuthorizeIds, ',');
                    expression = expression.And(t => authorizeIdArr.Contains(t.AuthorizeId.Value));
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
