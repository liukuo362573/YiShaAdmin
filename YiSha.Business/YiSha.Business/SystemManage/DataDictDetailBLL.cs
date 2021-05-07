using System.Collections.Generic;
using System.Threading.Tasks;
using YiSha.Business.Cache;
using YiSha.Entity.SystemManage;
using YiSha.Model.Param.SystemManage;
using YiSha.Service.SystemManage;
using YiSha.Util.Extension;
using YiSha.Util.Model;

namespace YiSha.Business.SystemManage
{
    public class DataDictDetailBLL
    {
        private readonly DataDictDetailService _dataDictDetailService = new();

        private readonly DataDictDetailCache _dataDictDetailCache = new();

        #region 获取数据

        public async Task<TData<List<DataDictDetailEntity>>> GetList(DataDictDetailListParam param)
        {
            TData<List<DataDictDetailEntity>> obj = new TData<List<DataDictDetailEntity>>();
            obj.Data = await _dataDictDetailService.GetList(param);
            obj.Total = obj.Data.Count;
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<List<DataDictDetailEntity>>> GetPageList(DataDictDetailListParam param, Pagination pagination)
        {
            TData<List<DataDictDetailEntity>> obj = new TData<List<DataDictDetailEntity>>();
            obj.Data = await _dataDictDetailService.GetPageList(param, pagination);
            obj.Total = pagination.TotalCount;
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<DataDictDetailEntity>> GetEntity(long id)
        {
            TData<DataDictDetailEntity> obj = new TData<DataDictDetailEntity>();
            obj.Data = await _dataDictDetailService.GetEntity(id);
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<int>> GetMaxSort()
        {
            TData<int> obj = new TData<int>();
            obj.Data = await _dataDictDetailService.GetMaxSort();
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
            if (_dataDictDetailService.ExistDictKeyValue(entity))
            {
                obj.Message = "字典键或值已经存在！";
                return obj;
            }
            await _dataDictDetailService.SaveForm(entity);
            _dataDictDetailCache.Remove();
            obj.Data = entity.Id.ParseToString();
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData> DeleteForm(string ids)
        {
            TData<long> obj = new TData<long>();
            await _dataDictDetailService.DeleteForm(ids);
            _dataDictDetailCache.Remove();
            obj.Tag = 1;
            return obj;
        }

        #endregion
    }
}