using System.Linq.Expressions;
using YiSha.Common;
using YiSha.DataBase;
using YiSha.Entity.OrganizationManage;
using YiSha.Model.Param.OrganizationManage;
using YiSha.Util;
using YiSha.Util.Model;

namespace YiSha.Service.OrganizationManage
{
    public class PositionService : Repository
    {
        #region 获取数据
        public async Task<List<PositionEntity>> GetList(PositionListParam param)
        {
            var expression = ListFilter(param);
            var list = await this.FindList(expression);
            return list.ToList();
        }

        public async Task<List<PositionEntity>> GetPageList(PositionListParam param, Pagination pagination)
        {
            var expression = ListFilter(param);
            var list = await this.FindList(expression, pagination);
            return list.ToList();
        }

        public async Task<PositionEntity> GetEntity(long id)
        {
            return await this.FindEntity<PositionEntity>(id);
        }

        public async Task<int> GetMaxSort()
        {
            object result = await this.FindObject("SELECT MAX(PositionSort) FROM SysPosition");
            int sort = result.ToInt();
            sort++;
            return sort;
        }

        public bool ExistPositionName(PositionEntity entity)
        {
            var expression = ExtensionLinq.True<PositionEntity>();
            expression = expression.And(t => t.BaseIsDelete == 0);
            if (entity.Id.IsNullOrZero())
            {
                expression = expression.And(t => t.PositionName == entity.PositionName);
            }
            else
            {
                expression = expression.And(t => t.PositionName == entity.PositionName && t.Id != entity.Id);
            }
            return this.IQueryable(expression).Count() > 0 ? true : false;
        }
        #endregion

        #region 提交数据
        public async Task SaveForm(PositionEntity entity)
        {
            if (entity.Id.IsNullOrZero())
            {
                await entity.Create();
                await this.Insert<PositionEntity>(entity);
            }
            else
            {
                await entity.Modify();
                await this.Update(entity);
            }
        }

        public async Task DeleteForm(string ids)
        {
            long[] idArr = TextHelper.SplitToArray<long>(ids, ',');
            await this.Delete<PositionEntity>(idArr);
        }
        #endregion

        #region 私有方法
        private Expression<Func<PositionEntity, bool>> ListFilter(PositionListParam param)
        {
            var expression = ExtensionLinq.True<PositionEntity>();
            if (param != null)
            {
                if (!string.IsNullOrEmpty(param.PositionName))
                {
                    expression = expression.And(t => t.PositionName.Contains(param.PositionName));
                }
                if (!string.IsNullOrEmpty(param.PositionIds))
                {
                    long[] positionIdArr = TextHelper.SplitToArray<long>(param.PositionIds, ',');
                    expression = expression.And(t => positionIdArr.Contains(t.Id));
                }
            }
            return expression;
        }
        #endregion
    }
}
