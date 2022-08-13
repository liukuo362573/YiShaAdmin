using System.Linq.Expressions;
using YiSha.Common;
using YiSha.DataBase;
using YiSha.Entity.SystemManage;
using YiSha.Model.Param.SystemManage;
using YiSha.Util;
using YiSha.Util.Model;

namespace YiSha.Service.SystemManage
{
    public class AreaService : Repository
    {
        #region 获取数据
        public async Task<List<AreaEntity>> GetList(AreaListParam param)
        {
            var expression = ListFilter(param);
            var list = await this.FindList(expression);
            return list.ToList();
        }

        public async Task<List<AreaEntity>> GetPageList(AreaListParam param, Pagination pagination)
        {
            var expression = ListFilter(param);
            var list = await this.FindList(expression, pagination);
            return list.ToList();
        }

        public async Task<AreaEntity> GetEntity(long id)
        {
            return await this.FindEntity<AreaEntity>(id);
        }

        public async Task<AreaEntity> GetEntityByAreaCode(string areaCode)
        {
            return await this.FindEntity<AreaEntity>(p => p.AreaCode == areaCode);
        }
        #endregion

        #region 提交数据
        public async Task SaveForm(AreaEntity entity)
        {
            if (entity.Id.IsNullOrZero())
            {
                await entity.Create();
                await this.Insert<AreaEntity>(entity);
            }
            else
            {
                await entity.Modify();
                await this.Update<AreaEntity>(entity);
            }
        }

        public async Task DeleteForm(string ids)
        {
            long[] idArr = TextHelper.SplitToArray<long>(ids, ',');
            await this.Delete<AreaEntity>(idArr);
        }

        #endregion

        #region 私有方法
        private Expression<Func<AreaEntity, bool>> ListFilter(AreaListParam param)
        {
            var expression = ExtensionLinq.True<AreaEntity>();
            if (param != null)
            {
                if (!param.AreaName.IsNull())
                {
                    expression = expression.And(t => t.AreaName.Contains(param.AreaName));
                }
            }
            return expression;
        }
        #endregion
    }
}
