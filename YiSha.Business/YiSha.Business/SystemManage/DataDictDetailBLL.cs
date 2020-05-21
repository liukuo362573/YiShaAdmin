using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using YiSha.Util;
using YiSha.Util.Extension;
using YiSha.Util.Model;
using YiSha.Entity.SystemManage;
using YiSha.Model.Param.SystemManage;
using YiSha.Service.SystemManage;
using YiSha.Business.Cache;

namespace YiSha.Business.SystemManage
{
    public class DataDictDetailBLL
    {
        private DataDictDetailService dataDictDetailService = new DataDictDetailService();

        private DataDictDetailCache dataDictDetailCache = new DataDictDetailCache();

        #region  获取数据
        public async Task<TData<List<DataDictDetailEntity>>> GetList(DataDictDetailListParam param)
        {
            TData<List<DataDictDetailEntity>> obj = new TData<List<DataDictDetailEntity>>();
            obj.Data = await dataDictDetailService.GetList(param);
            obj.Total = obj.Data.Count;
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<List<DataDictDetailEntity>>> GetPageList(DataDictDetailListParam param, Pagination pagination)
        {
            TData<List<DataDictDetailEntity>> obj = new TData<List<DataDictDetailEntity>>();
            obj.Data = await dataDictDetailService.GetPageList(param, pagination);
            obj.Total = pagination.TotalCount;
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<DataDictDetailEntity>> GetEntity(long id)
        {
            TData<DataDictDetailEntity> obj = new TData<DataDictDetailEntity>();
            obj.Data = await dataDictDetailService.GetEntity(id);
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<int>> GetMaxSort()
        {
            TData<int> obj = new TData<int>();
            obj.Data = await dataDictDetailService.GetMaxSort();
            obj.Tag = 1;
            return obj;
        }
        #endregion

        #region 提交数据
        public async Task<TData<string>> SaveForm(DataDictDetailEntity entity)
        {
            TData<string> obj = new TData<string>();
            if (entity.DictKey <= 0)
            {
                obj.Message = "字典键必须大于0";
                return obj;
            }
            if (dataDictDetailService.ExistDictKeyValue(entity))
            {
                obj.Message = "字典键或值已经存在！";
                return obj;
            }
            await dataDictDetailService.SaveForm(entity);
            dataDictDetailCache.Remove();
            obj.Data = entity.Id.ParseToString();
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData> DeleteForm(string ids)
        {
            TData<long> obj = new TData<long>();
            await dataDictDetailService.DeleteForm(ids);
            dataDictDetailCache.Remove();
            obj.Tag = 1;
            return obj;
        }
        #endregion
    }
}
