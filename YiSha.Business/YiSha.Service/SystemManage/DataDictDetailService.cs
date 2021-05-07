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
    public class DataDictDetailService : RepositoryFactory
    {
        #region 获取数据

        public async Task<List<DataDictDetailEntity>> GetList(DataDictDetailListParam param)
        {
            var expression = ListFilter(param);
            var list = await BaseRepository().FindList(expression);
            return list.OrderBy(p => p.DictSort).ToList();
        }

        public async Task<List<DataDictDetailEntity>> GetPageList(DataDictDetailListParam param, Pagination pagination)
        {
            var expression = ListFilter(param);
            return await BaseRepository().FindList(expression, pagination);
        }

        public async Task<DataDictDetailEntity> GetEntity(long id)
        {
            return await BaseRepository().FindEntity<DataDictDetailEntity>(id);
        }

        public async Task<int> GetMaxSort()
        {
            var result = await BaseRepository().FindEntity<int>("SELECT MAX(DictSort) FROM SysDataDictDetail");
            return result + 1;
        }

        public bool ExistDictKeyValue(DataDictDetailEntity entity)
        {
            var expression = LinqExtensions.True<DataDictDetailEntity>();
            expression = expression.And(t => t.BaseIsDelete == 0);
            expression = expression.And(t => t.DictType == entity.DictType);
            expression = expression.And(t => t.DictKey == entity.DictKey || t.DictValue == entity.DictValue);
            if (!entity.Id.IsNullOrZero())
            {
                expression = expression.And(t => t.Id != entity.Id);
            }
            return BaseRepository().AsQueryable(expression).Any();
        }

        #endregion

        #region 提交数据

        public async Task SaveForm(DataDictDetailEntity entity)
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
            var idArr = TextHelper.SplitToArray<object>(ids, ',');
            await BaseRepository().Delete<DataDictDetailEntity>(idArr);
        }

        #endregion

        #region 私有方法

        private Expression<Func<DataDictDetailEntity, bool>> ListFilter(DataDictDetailListParam param)
        {
            var expression = LinqExtensions.True<DataDictDetailEntity>();
            if (param != null)
            {
                if (param.DictKey.ParseToInt() > 0)
                {
                    expression = expression.And(t => t.DictKey == param.DictKey);
                }

                if (param.DictValue?.Length > 0)
                {
                    expression = expression.And(t => t.DictValue.Contains(param.DictValue));
                }

                if (param.DictType?.Length > 0)
                {
                    expression = expression.And(t => t.DictType.Contains(param.DictType));
                }
            }
            return expression;
        }

        #endregion
    }
}