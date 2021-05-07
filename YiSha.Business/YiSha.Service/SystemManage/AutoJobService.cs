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
    public class AutoJobService : RepositoryFactory
    {
        #region 获取数据

        public async Task<List<AutoJobEntity>> GetList(AutoJobListParam param)
        {
            var expression = ListFilter(param);
            var list = await BaseRepository().FindList(expression);
            return list.ToList();
        }

        public async Task<List<AutoJobEntity>> GetPageList(AutoJobListParam param, Pagination pagination)
        {
            var expression = ListFilter(param);
            var list = await BaseRepository().FindList(expression, pagination);
            return list.ToList();
        }

        public async Task<AutoJobEntity> GetEntity(long id)
        {
            return await BaseRepository().FindEntity<AutoJobEntity>(id);
        }

        public bool ExistJob(AutoJobEntity entity)
        {
            var expression = LinqExtensions.True<AutoJobEntity>();
            expression = expression.And(t => t.BaseIsDelete == 0);
            if (entity.Id.IsNullOrZero())
            {
                expression = expression.And(t => t.JobGroupName == entity.JobGroupName && t.JobName == entity.JobName);
            }
            else
            {
                expression = expression.And(t => t.JobGroupName == entity.JobGroupName && t.JobName == entity.JobName && t.Id != entity.Id);
            }
            return BaseRepository().AsQueryable(expression).Count() > 0 ? true : false;
        }

        #endregion

        #region 提交数据

        public async Task SaveForm(AutoJobEntity entity)
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
            long[] idArr = TextHelper.SplitToArray<long>(ids, ',');
            await BaseRepository().Delete<AutoJobEntity>(idArr);
        }

        #endregion

        #region 私有方法

        private Expression<Func<AutoJobEntity, bool>> ListFilter(AutoJobListParam param)
        {
            var expression = LinqExtensions.True<AutoJobEntity>();
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