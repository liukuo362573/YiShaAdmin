using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using YiSha.Util;
using YiSha.Util.Extension;
using YiSha.Util.Model;
using YiSha.Data.Repository;
using YiSha.Entity.SystemManage;
using YiSha.Model.Param.SystemManage;
using System.Linq.Expressions;

namespace YiSha.Service.SystemManage
{
    public class DataDictService : RepositoryFactory
    {
        #region 获取数据
        public async Task<List<DataDictEntity>> GetList(DataDictListParam param)
        {
            var expression = ListFilter(param);
            var list = await this.BaseRepository().FindList(expression);
            return list.ToList();
        }

        public async Task<List<DataDictEntity>> GetPageList(DataDictListParam param, Pagination pagination)
        {
            var expression = ListFilter(param);
            var list = await this.BaseRepository().FindList(expression, pagination);
            return list.ToList();
        }

        public async Task<DataDictEntity> GetEntity(long id)
        {
            return await this.BaseRepository().FindEntity<DataDictEntity>(id);
        }

        public async Task<int> GetMaxSort()
        {
            object result = await this.BaseRepository().FindObject("SELECT MAX(dict_sort) FROM sys_data_dict");
            int sort = result.ParseToInt();
            sort++;
            return sort;
        }

        public bool ExistDictType(DataDictEntity entity)
        {
            var expression = LinqExtensions.True<DataDictEntity>();
            expression = expression.And(t => t.BaseIsDelete == 0);
            if (entity.Id.IsNullOrZero())
            {
                expression = expression.And(t => t.DictType == entity.DictType);
            }
            else
            {
                expression = expression.And(t => t.DictType == entity.DictType && t.Id != entity.Id);
            }
            return this.BaseRepository().IQueryable(expression).Count() > 0 ? true : false;
        }

        /// <summary>
        /// 是否存在字典值
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool ExistDictDetail(string dictType)
        {
            var expression = LinqExtensions.True<DataDictDetailEntity>();
            expression = expression.And(t => t.DictType == dictType);
            return this.BaseRepository().IQueryable(expression).Count() > 0 ? true : false;
        }
        #endregion

        #region 提交数据
        public async Task SaveForm(DataDictEntity entity)
        {
            if (entity.Id.IsNullOrZero())
            {
                await entity.Create();
                await this.BaseRepository().Insert<DataDictEntity>(entity);
            }
            else
            {
                await entity.Modify();
                await this.BaseRepository().Update<DataDictEntity>(entity);
            }
        }

        public async Task DeleteForm(string ids)
        {
            long[] idArr = CommonHelper.SplitToArray<long>(ids, ',');
            await this.BaseRepository().Delete<DataDictEntity>(idArr);
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
