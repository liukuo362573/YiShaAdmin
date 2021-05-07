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
    public class DataDictService : RepositoryFactory
    {
        #region 获取数据

        public async Task<List<DataDictEntity>> GetList(DataDictListParam param)
        {
            var expression = ListFilter(param);
            return await BaseRepository().FindList(expression);
        }

        public async Task<List<DataDictEntity>> GetPageList(DataDictListParam param, Pagination pagination)
        {
            var expression = ListFilter(param);
            return await BaseRepository().FindList(expression, pagination);
        }

        public async Task<DataDictEntity> GetEntity(long id)
        {
            return await BaseRepository().FindEntity<DataDictEntity>(id);
        }

        public async Task<int> GetMaxSort()
        {
            var result = await BaseRepository().FindEntity<int>("SELECT MAX(DictSort) FROM SysDataDict");
            return result + 1;
        }

        public bool ExistDictType(DataDictEntity entity)
        {
            var expression = LinqExtensions.True<DataDictEntity>();
            expression = expression.And(t => t.BaseIsDelete == 0);
            expression = expression.And(t => t.DictType == entity.DictType);
            if (!entity.Id.IsNullOrZero())
            {
                expression = expression.And(t => t.Id != entity.Id);
            }
            return BaseRepository().AsQueryable(expression).Any();
        }

        public bool ExistDictDetail(string dictType)
        {
            return BaseRepository().AsQueryable<DataDictDetailEntity>(t => t.DictType == dictType).Any();
        }

        #endregion

        #region 提交数据

        public async Task SaveForm(DataDictEntity entity)
        {
            var repo = await BaseRepository().BeginTrans();
            try
            {
                if (!entity.Id.IsNullOrZero())
                {
                    await UpdateForm(entity, repo);
                }
                else
                {
                    await entity.Create();
                    await repo.Insert(entity);
                }
                await repo.CommitTrans();
            }
            catch
            {
                await repo.RollbackTrans();
                throw;
            }
        }

        private async Task UpdateForm(DataDictEntity entity, Repository repo)
        {
            // ReSharper disable once PossibleInvalidOperationException
            var dbEntity = await repo.FindEntity<DataDictEntity>(entity.Id.Value);
            if (dbEntity.DictType != entity.DictType)
            {
                // 更新子表的DictType，因为2个表用DictType进行关联
                var detailList = await repo.FindList<DataDictDetailEntity>(p => p.DictType == dbEntity.DictType);
                foreach (var detailEntity in detailList)
                {
                    detailEntity.DictType = entity.DictType;
                    await detailEntity.Modify();
                }
            }
            dbEntity.DictType = entity.DictType;
            dbEntity.Remark = entity.Remark;
            dbEntity.DictSort = entity.DictSort;

            await dbEntity.Modify();
            await repo.Update(dbEntity);
        }

        public async Task DeleteForm(string ids)
        {
            var idArr = TextHelper.SplitToArray<object>(ids, ',');
            await BaseRepository().Delete<DataDictEntity>(idArr);
        }

        #endregion

        #region 私有方法

        private Expression<Func<DataDictEntity, bool>> ListFilter(DataDictListParam param)
        {
            var expression = LinqExtensions.True<DataDictEntity>();
            if (param != null)
            {
                if (!param.DictType.IsEmpty())
                {
                    expression = expression.And(t => t.DictType.Contains(param.DictType));
                }
                if (!param.Remark.IsEmpty())
                {
                    expression = expression.And(t => t.Remark.Contains(param.Remark));
                }
            }
            return expression;
        }

        #endregion
    }
}