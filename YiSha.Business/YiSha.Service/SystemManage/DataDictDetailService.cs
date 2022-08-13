using System.Linq.Expressions;
using YiSha.Common;
using YiSha.DataBase;
using YiSha.Entity.SystemManage;
using YiSha.Model.Param.SystemManage;
using YiSha.Util;
using YiSha.Util.Model;

namespace YiSha.Service.SystemManage
{
    public class DataDictDetailService : Repository
    {
        #region 获取数据
        public async Task<List<DataDictDetailEntity>> GetList(DataDictDetailListParam param)
        {
            var expression = ListFilter(param);
            var list = await this.FindList(expression);
            return list.OrderBy(p => p.DictSort).ToList();
        }

        public async Task<List<DataDictDetailEntity>> GetPageList(DataDictDetailListParam param, Pagination pagination)
        {
            var expression = ListFilter(param);
            var list = await this.FindList(expression, pagination);
            return list.ToList();
        }

        public async Task<DataDictDetailEntity> GetEntity(long id)
        {
            return await this.FindEntity<DataDictDetailEntity>(id);
        }

        public async Task<int> GetMaxSort()
        {
            object result = await this.FindObject("SELECT MAX(DictSort) FROM SysDataDictDetail");
            int sort = result.ToInt();
            sort++;
            return sort;
        }

        public bool ExistDictKeyValue(DataDictDetailEntity entity)
        {
            var expression = ExtensionLinq.True<DataDictDetailEntity>();
            expression = expression.And(t => t.BaseIsDelete == 0);
            if (entity.Id.IsNullOrZero())
            {
                expression = expression.And(t => t.DictType == entity.DictType && (t.DictKey == entity.DictKey || t.DictValue == entity.DictValue));
            }
            else
            {
                expression = expression.And(t => t.DictType == entity.DictType && (t.DictKey == entity.DictKey || t.DictValue == entity.DictValue) && t.Id != entity.Id);
            }
            return this.IQueryable(expression).Count() > 0 ? true : false;
        }
        #endregion

        #region 提交数据
        public async Task SaveForm(DataDictDetailEntity entity)
        {
            if (entity.Id.IsNullOrZero())
            {
                await entity.Create();
                await this.Insert<DataDictDetailEntity>(entity);
            }
            else
            {
                await entity.Modify();
                await this.Update<DataDictDetailEntity>(entity);
            }
        }

        public async Task DeleteForm(string ids)
        {
            long[] idArr = TextHelper.SplitToArray<long>(ids, ',');
            await this.Delete<DataDictDetailEntity>(idArr);
        }
        #endregion

        #region 私有方法
        private Expression<Func<DataDictDetailEntity, bool>> ListFilter(DataDictDetailListParam param)
        {
            var expression = ExtensionLinq.True<DataDictDetailEntity>();
            if (param != null)
            {
                if (param.DictKey.ToInt() > 0)
                {
                    expression = expression.And(t => t.DictKey == param.DictKey);
                }

                if (!string.IsNullOrEmpty(param.DictValue))
                {
                    expression = expression.And(t => t.DictValue.Contains(param.DictValue));
                }

                if (!string.IsNullOrEmpty(param.DictType))
                {
                    expression = expression.And(t => t.DictType.Contains(param.DictType));
                }
            }
            return expression;
        }
        #endregion
    }
}
