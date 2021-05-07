using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YiSha.Business.Cache;
using YiSha.Entity.SystemManage;
using YiSha.Model.Param.SystemManage;
using YiSha.Model.Result.SystemManage;
using YiSha.Service.SystemManage;
using YiSha.Util.Extension;
using YiSha.Util.Helper;
using YiSha.Util.Model;

namespace YiSha.Business.SystemManage
{
    public class DataDictBLL
    {
        private readonly DataDictService _dataDictService = new();

        private readonly DataDictCache _dataDictCache = new();

        private readonly DataDictDetailCache _dataDictDetailCache = new();

        #region 获取数据

        public async Task<TData<List<DataDictEntity>>> GetList(DataDictListParam param)
        {
            var list = await _dataDictService.GetList(param);
            return new() { Data = list, Total = list.Count, Tag = 1 };
        }

        public async Task<TData<List<DataDictEntity>>> GetPageList(DataDictListParam param, Pagination pagination)
        {
            return new()
            {
                Data = await _dataDictService.GetPageList(param, pagination),
                Total = pagination.TotalCount,
                Tag = 1
            };
        }

        public async Task<TData<DataDictEntity>> GetEntity(long id)
        {
            return new() { Data = await _dataDictService.GetEntity(id), Tag = 1 };
        }

        public async Task<TData<int>> GetMaxSort()
        {
            return new() { Data = await _dataDictService.GetMaxSort(), Tag = 1 };
        }

        /// <summary>
        /// 获取所有的数据字典
        /// </summary>
        public async Task<TData<List<DataDictInfo>>> GetDataDictList()
        {
            var dataDictList = await _dataDictCache.GetList();
            var dataDictDetailList = await _dataDictDetailCache.GetList();
            var dataDictInfoList = new List<DataDictInfo>();
            foreach (var dataDict in dataDictList)
            {
                dataDictInfoList.Add(new DataDictInfo
                {
                    DictType = dataDict.DictType,
                    Detail = dataDictDetailList.Where(p => p.DictType == dataDict.DictType)
                                               .OrderBy(p => p.DictSort)
                                               .Select(p => new DataDictDetailInfo
                                               {
                                                   DictKey = p.DictKey,
                                                   DictValue = p.DictValue,
                                                   ListClass = p.ListClass,
                                                   IsDefault = p.IsDefault,
                                                   DictStatus = p.DictStatus,
                                                   Remark = p.Remark
                                               }).ToList()
                });
            }
            return new() { Data = dataDictInfoList, Tag = 1 };
        }

        #endregion

        #region 提交数据

        public async Task<TData<string>> SaveForm(DataDictEntity entity)
        {
            if (_dataDictService.ExistDictType(entity))
            {
                return new() { Tag = 0, Message = "字典类型已经存在！" };
            }

            await _dataDictService.SaveForm(entity);
            _dataDictCache.Remove();
            return new() { Data = entity.Id.ParseToString(), Tag = 1 };
        }

        public async Task<TData> DeleteForm(string ids)
        {
            foreach (long id in TextHelper.SplitToArray<long>(ids, ','))
            {
                var dbEntity = await _dataDictService.GetEntity(id);
                if (_dataDictService.ExistDictDetail(dbEntity.DictType))
                {
                    return new() { Tag = 0, Message = "请先删除字典值！" };
                }
            }
            await _dataDictService.DeleteForm(ids);
            _dataDictCache.Remove();
            return new() { Tag = 1 };
        }

        #endregion
    }
}