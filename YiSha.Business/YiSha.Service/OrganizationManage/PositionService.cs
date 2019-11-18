using System;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Threading.Tasks;
using YiSha.Data.Repository;
using YiSha.Entity.OrganizationManage;
using YiSha.Model.Param.OrganizationManage;
using YiSha.Util.Extension;
using YiSha.Util.Model;
using YiSha.Util;

namespace YiSha.Service.OrganizationManage
{
    public class PositionService : RepositoryFactory
    {
        #region 获取数据
        public async Task<List<PositionEntity>> GetList(PositionListParam param)
        {
            var expression = ListFilter(param);
            var list = await this.BaseRepository().FindList(expression);
            return list.ToList();
        }

        public async Task<List<PositionEntity>> GetPageList(PositionListParam param, Pagination pagination)
        {
            var expression = ListFilter(param);
            var list = await this.BaseRepository().FindList(expression, pagination);
            return list.ToList();
        }

        public async Task<PositionEntity> GetEntity(long id)
        {
            return await this.BaseRepository().FindEntity<PositionEntity>(id);
        }

        public async Task<int> GetMaxSort()
        {
            object result = await this.BaseRepository().FindObject("SELECT MAX(position_sort) FROM sys_position");
            int sort = result.ParseToInt();
            sort++;
            return sort;
        }

        public bool ExistPositionName(PositionEntity entity)
        {
            var expression = LinqExtensions.True<PositionEntity>();
            expression = expression.And(t => t.BaseIsDelete == 0);
            if (entity.Id.IsNullOrZero())
            {
                expression = expression.And(t => t.PositionName == entity.PositionName);
            }
            else
            {
                expression = expression.And(t => t.PositionName == entity.PositionName && t.Id != entity.Id);
            }
            return this.BaseRepository().IQueryable(expression).Count() > 0 ? true : false;
        }
        #endregion

        #region 提交数据
        public async Task SaveForm(PositionEntity entity)
        {
            if (entity.Id.IsNullOrZero())
            {
                await entity.Create();
                await this.BaseRepository().Insert<PositionEntity>(entity);
            }
            else
            {
                await entity.Modify();
                await this.BaseRepository().Update(entity);
            }
        }

        public async Task DeleteForm(string ids)
        {
            long[] idArr = TextHelper.SplitToArray<long>(ids, ',');
            await this.BaseRepository().Delete<PositionEntity>(idArr);
        }
        #endregion

        #region 私有方法
        private Expression<Func<PositionEntity, bool>> ListFilter(PositionListParam param)
        {
            var expression = LinqExtensions.True<PositionEntity>();
            if (param != null)
            {
                if (!string.IsNullOrEmpty(param.PositionName))
                {
                    expression = expression.And(t => t.PositionName.Contains(param.PositionName));
                }
                if (!string.IsNullOrEmpty(param.PositionIds))
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
