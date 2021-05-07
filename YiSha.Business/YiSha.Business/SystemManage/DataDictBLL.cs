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
        private DataDictDetailService _dataDictDetailService = new();

        private readonly DataDictCache _dataDictCache = new();
        private readonly DataDictDetailCache _dataDictDetailCache = new();

        #region 获取数据

        public async Task<TData<List<DataDictEntity>>> GetList(DataDictListParam param)
        {
            TData<List<DataDictEntity>> obj = new TData<List<DataDictEntity>>();
            obj.Data = await _dataDictService.GetList(param);
            obj.Total = obj.Data.Count;
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<List<DataDictEntity>>> GetPageList(DataDictListParam param, Pagination pagination)
        {
            TData<List<DataDictEntity>> obj = new TData<List<DataDictEntity>>();
            obj.Data = await _dataDictService.GetPageList(param, pagination);
            obj.Total = pagination.TotalCount;
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<DataDictEntity>> GetEntity(long id)
        {
            TData<DataDictEntity> obj = new TData<DataDictEntity>();
            obj.Data = await _dataDictService.GetEntity(id);
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<int>> GetMaxSort()
        {
            TData<int> obj = new TData<int>();
            obj.Data = await _dataDictService.GetMaxSort();
            obj.Tag = 1;
            return obj;
        }

        /// <summary>
        /// 获取所有的数据字典
        /// </summary>
        /// <returns></returns>
        public async Task<TData<List<DataDictInfo>>> GetDataDictList()
        {
            TData<List<DataDictInfo>> obj = new TData<List<DataDictInfo>>();
            List<DataDictEntity> dataDictList = await _dataDictCache.GetList();
            List<DataDictDetailEntity> dataDictDetailList = await _dataDictDetailCache.GetList();
            List<DataDictInfo> dataDictInfoList = new List<DataDictInfo>();
            foreach (DataDictEntity dataDict in dataDictList)
            {
                List<DataDictDetailInfo> detailList = dataDictDetailList.Where(p => p.DictType == dataDict.DictType).OrderBy(p => p.DictSort).Select(p => new DataDictDetailInfo
                {
                    DictKey = p.DictKey,
                    DictValue = p.DictValue,
                    ListClass = p.ListClass,
                    IsDefault = p.IsDefault,
                    DictStatus = p.DictStatus,
                    Remark = p.Remark
                }).ToList();
                dataDictInfoList.Add(new DataDictInfo
                {
                    DictType = dataDict.DictType,
                    Detail = detailList
                });
            }
            obj.Data = dataDictInfoList;
            obj.Tag = 1;
            return obj;
        }

        #endregion

        #region 提交数据

        public async Task<TData<string>> SaveForm(DataDictEntity entity)
        {
            TData<string> obj = new TData<string>();
            if (_dataDictService.ExistDictType(entity))
            {
                obj.Message = "字典类型已经存在！";
                return obj;
            }

            await _dataDictService.SaveForm(entity);
            _dataDictCache.Remove();
            obj.Data = entity.Id.ParseToString();
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData> DeleteForm(string ids)
        {
            TData obj = new TData();
            foreach (long id in TextHelper.SplitToArray<long>(ids, ','))
            {
                DataDictEntity dbEntity = await _dataDictService.GetEntity(id);
                if (_dataDictService.ExistDictDetail(dbEntity.DictType))
                {
                    obj.Message = "请先删除字典值！";
                    return obj;
                }
            }
            await _dataDictService.DeleteForm(ids);
            _dataDictCache.Remove();
            obj.Tag = 1;
            return obj;
        }

        #endregion
    }
}