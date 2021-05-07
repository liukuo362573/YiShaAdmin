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
            var list = await _autoJobService.GetList(param);
            return new() { Data = list, Total = list.Count, Tag = 1 };
        }

        public async Task<TData<List<AutoJobEntity>>> GetPageList(AutoJobListParam param, Pagination pagination)
        {
            return new()
            {
                Data = await _autoJobService.GetPageList(param, pagination),
                Total = pagination.TotalCount,
                Tag = 1
            };
        }

        public async Task<TData<AutoJobEntity>> GetEntity(long id)
        {
            return new() { Data = await _autoJobService.GetEntity(id), Tag = 1 };
        }

        #endregion

        #region 提交数据

        public async Task<TData<string>> SaveForm(AutoJobEntity entity)
        {
            if (_autoJobService.ExistJob(entity))
            {
                return new() { Tag = 0, Message = "任务已经存在！" };
            }
            await _autoJobService.SaveForm(entity);
            return new() { Data = entity.Id.ParseToString(), Tag = 1 };
        }

        public async Task<TData> DeleteForm(string ids)
        {
            foreach (long id in TextHelper.SplitToArray<long>(ids, ','))
            {
                var dbEntity = await _autoJobService.GetEntity(id);
                if (dbEntity.JobStatus == StatusEnum.Yes.ParseToInt())
                {
                    return new() { Tag = 0, Message = "请先暂停 " + dbEntity.JobName + " 定时任务" };
                }
            }
            await _autoJobService.DeleteForm(ids);
            return new() { Tag = 1 };
        }

        public async Task<TData> ChangeJobStatus(AutoJobEntity entity)
        {
            await _autoJobService.SaveForm(entity);
            return new() { Tag = 1 };
        }

        #endregion
    }
}