using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YiSha.Data.Repository;
using YiSha.Entity.SystemManage;
using YiSha.Util;
using YiSha.Util.Extension;
using YiSha.Util.Model;

namespace YiSha.Service.SystemManage
{
    public class MenuAuthorizeService : RepositoryFactory
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
            var list = await this.BaseRepository().FindList<MenuAuthorizeEntity>(expression);
            return list.ToList();
        }

        public async Task<MenuAuthorizeEntity> GetEntity(long id)
        {
            return await this.BaseRepository().FindEntity<MenuAuthorizeEntity>(id);
        }
        #endregion

        #region 提交数据
        public async Task SaveForm(MenuAuthorizeEntity entity)
        {
            if (entity.Id.IsNullOrZero())
            {
                await entity.Create();
                await this.BaseRepository().Insert(entity);
            }
            else
            {
                await this.BaseRepository().Update(entity);
            }
        }

        public async Task DeleteForm(long id)
        {
            await this.BaseRepository().Delete<MenuAuthorizeEntity>(id);
        }
        #endregion
    }
}
