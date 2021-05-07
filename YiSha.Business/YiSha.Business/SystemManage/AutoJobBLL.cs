using System.Collections.Generic;
using System.Threading.Tasks;
using YiSha.Entity.SystemManage;
using YiSha.Enum;
using YiSha.Model.Param.SystemManage;
using YiSha.Service.SystemManage;
using YiSha.Util.Extension;
using YiSha.Util.Helper;
using YiSha.Util.Model;

namespace YiSha.Business.SystemManage
{
    public class AutoJobBLL
    {
        private readonly AutoJobService _autoJobService = new();

        #region 获取数据

        public async Task<TData<List<AutoJobEntity>>> GetList(AutoJobListParam param)
        {
            TData<List<AutoJobEntity>> obj = new TData<List<AutoJobEntity>>();
            obj.Data = await _autoJobService.GetList(param);
            obj.Total = obj.Data.Count;
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<List<AutoJobEntity>>> GetPageList(AutoJobListParam param, Pagination pagination)
        {
            TData<List<AutoJobEntity>> obj = new TData<List<AutoJobEntity>>();
            obj.Data = await _autoJobService.GetPageList(param, pagination);
            obj.Total = pagination.TotalCount;
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<AutoJobEntity>> GetEntity(long id)
        {
            TData<AutoJobEntity> obj = new TData<AutoJobEntity>();
            obj.Data = await _autoJobService.GetEntity(id);
            obj.Tag = 1;
            return obj;
        }

        #endregion

        #region 提交数据

        public async Task<TData<string>> SaveForm(AutoJobEntity entity)
        {
            TData<string> obj = new TData<string>();
            if (_autoJobService.ExistJob(entity))
            {
                obj.Message = "任务已经存在！";
                return obj;
            }
            await _autoJobService.SaveForm(entity);
            obj.Data = entity.Id.ParseToString();
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData> DeleteForm(string ids)
        {
            TData<long> obj = new TData<long>();
            foreach (long id in TextHelper.SplitToArray<long>(ids, ','))
            {
                AutoJobEntity dbEntity = await _autoJobService.GetEntity(id);
                if (dbEntity.JobStatus == StatusEnum.Yes.ParseToInt())
                {
                    obj.Message = "请先暂停 " + dbEntity.JobName + " 定时任务";
                    return obj;
                }
            }
            await _autoJobService.DeleteForm(ids);

            obj.Tag = 1;
            return obj;
        }

        public async Task<TData> ChangeJobStatus(AutoJobEntity entity)
        {
            TData obj = new TData();

            await _autoJobService.SaveForm(entity);

            obj.Tag = 1;
            return obj;
        }

        #endregion

        #region 私有方法

        #endregion
    }
}