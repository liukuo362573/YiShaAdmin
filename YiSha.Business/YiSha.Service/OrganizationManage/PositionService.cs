using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using YiSha.Data.Repository;
using YiSha.Entity.OrganizationManage;
using YiSha.Model.Param.OrganizationManage;
using YiSha.Util.Extension;
using YiSha.Util.Helper;
using YiSha.Util.Model;

namespace YiSha.Service.OrganizationManage
{
    public class PositionService : RepositoryFactory
    {
        #region 获取数据

        public async Task<List<PositionEntity>> GetList(PositionListParam param)
        {
            var expression = ListFilter(param);
            return await BaseRepository().FindList(expression);
        }

        public async Task<List<PositionEntity>> GetPageList(PositionListParam param, Pagination pagination)
        {
            var expression = ListFilter(param);
            return await BaseRepository().FindList(expression, pagination);
        }

        public async Task<PositionEntity> GetEntity(long id)
        {
            return await BaseRepository().FindEntity<PositionEntity>(id);
        }

        public async Task<int> GetMaxSort()
        {
            var result = await BaseRepository().FindEntity<int>("SELECT MAX(PositionSort) FROM SysPosition");
            return result + 1;
        }

        public bool ExistPositionName(PositionEntity entity)
        {
            var expression = LinqExtensions.True<PositionEntity>();
            expression = expression.And(t => t.BaseIsDelete == 0);
            expression = expression.And(t => t.PositionName == entity.PositionName);
            if (!entity.Id.IsNullOrZero())
            {
                expression = expression.And(t => t.Id != entity.Id);
            }
            return BaseRepository().AsQueryable(expression).Any();
        }

        #endregion

        #region 提交数据

        public async Task SaveForm(PositionEntity entity)
        {
            if (entity.Id.IsNullOrZero())
            {
                await entity.Create();
                await BaseRepository().Insert(entity);
            }
            else
            {
                await entity.Modify();
                await BaseRepository().Update(entity);
            }
        }

        public async Task DeleteForm(string ids)
        {
            var idArr = TextHelper.SplitToArray<long>(ids, ',');
            await BaseRepository().Delete<PositionEntity>(idArr);
        }

        #endregion

        #region 私有方法

        private Expression<Func<PositionEntity, bool>> ListFilter(PositionListParam param)
        {
            var expression = LinqExtensions.True<PositionEntity>();
            if (param != null)
            {
                if (param.PositionName?.Length > 0)
                {
                    expression = expression.And(t => t.PositionName.Contains(param.PositionName));
                }
                if (param.PositionIds?.Length > 0)
                {
                    long[] positionIdArr = TextHelper.SplitToArray<long>(param.PositionIds, ',');
                    expression = expression.And(t => positionIdArr.Contains(t.Id.Value));
                }
            }
            return expression;
        }

        #endregion
    }
}