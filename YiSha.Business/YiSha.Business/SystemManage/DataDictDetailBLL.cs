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
            var list = await _dataDictDetailService.GetList(param);
            return new() { Data = list, Total = list.Count, Tag = 1 };
        }

        public async Task<TData<List<DataDictDetailEntity>>> GetPageList(DataDictDetailListParam param, Pagination pagination)
        {
            return new()
            {
                Data = await _dataDictDetailService.GetPageList(param, pagination),
                Total = pagination.TotalCount,
                Tag = 1
            };
        }

        public async Task<TData<DataDictDetailEntity>> GetEntity(long id)
        {
            return new() { Data = await _dataDictDetailService.GetEntity(id), Tag = 1 };
        }

        public async Task<TData<int>> GetMaxSort()
        {
            return new() { Data = await _dataDictDetailService.GetMaxSort(), Tag = 1 };
        }

        #endregion

        #region 提交数据

        public async Task<TData<string>> SaveForm(DataDictDetailEntity entity)
        {
            if (entity.DictKey <= 0)
            {
                return new() { Tag = 0, Message = "字典键必须大于0" };
            }
            if (_dataDictDetailService.ExistDictKeyValue(entity))
            {
                return new() { Tag = 0, Message = "字典键或值已经存在！" };
            }
            await _dataDictDetailService.SaveForm(entity);
            _dataDictDetailCache.Remove();
            return new() { Data = entity.Id.ParseToString(), Tag = 1 };
        }

        public async Task<TData> DeleteForm(string ids)
        {
            await _dataDictDetailService.DeleteForm(ids);
            _dataDictDetailCache.Remove();
            return new() { Tag = 1 };
        }

        #endregion
    }
}