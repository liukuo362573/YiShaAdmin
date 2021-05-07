using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using YiSha.Data.Repository;
using YiSha.Entity.SystemManage;
using YiSha.Model.Param.SystemManage;
using YiSha.Util.Extension;
using YiSha.Util.Helper;
using YiSha.Util.Model;

namespace YiSha.Service.SystemManage
{
    public class AutoJobLogService : RepositoryFactory
    {
        #region 获取数据

        public async Task<List<AutoJobLogEntity>> GetList(AutoJobLogListParam param)
        {
            var expression = ListFilter(param);
            var list = await BaseRepository().FindList(expression);
            return list.ToList();
        }

        public async Task<List<AutoJobLogEntity>> GetPageList(AutoJobLogListParam param, Pagination pagination)
        {
            var expression = ListFilter(param);
            var list = await BaseRepository().FindList(expression, pagination);
            return list.ToList();
        }

        public async Task<AutoJobLogEntity> GetEntity(long id)
        {
            return await BaseRepository().FindEntity<AutoJobLogEntity>(id);
        }

        #endregion

        #region 提交数据

        public async Task SaveForm(AutoJobLogEntity entity)
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

        public async Task DeleteForm(string ids)
        {
            long[] idArr = TextHelper.SplitToArray<long>(ids, ',');
            await BaseRepository().Delete<AutoJobLogEntity>(idArr);
        }

        #endregion

        #region 私有方法

        private Expression<Func<AutoJobLogEntity, bool>> ListFilter(AutoJobLogListParam param)
        {
            var expression = LinqExtensions.True<AutoJobLogEntity>();
            if (param != null)
            {
                if (!string.IsNullOrEmpty(param.JobName))
                {
                    expression = expression.And(t => t.JobName.Contains(param.JobName));
                }
            }
            return expression;
        }

        #endregion
    }
}