using System.Collections.Generic;
using System.Threading.Tasks;
using YiSha.Entity.SystemManage;
using YiSha.Model.Param.SystemManage;
using YiSha.Service.SystemManage;
using YiSha.Util.Extension;
using YiSha.Util.Model;

namespace YiSha.Business.SystemManage
{
    public class AutoJobLogBLL
    {
        private readonly AutoJobLogService _autoJobLogService = new();

        #region 获取数据

        public async Task<TData<List<AutoJobLogEntity>>> GetList(AutoJobLogListParam param)
        {
            var list = await _autoJobLogService.GetList(param);
            return new() { Data = list, Total = list.Count, Tag = 1 };
        }

        public async Task<TData<List<AutoJobLogEntity>>> GetPageList(AutoJobLogListParam param, Pagination pagination)
        {
            return new()
            {
                Data = await _autoJobLogService.GetPageList(param, pagination),
                Total = pagination.TotalCount,
                Tag = 1
            };
        }

        public async Task<TData<AutoJobLogEntity>> GetEntity(long id)
        {
            return new() { Data = await _autoJobLogService.GetEntity(id), Tag = 1 };
        }

        #endregion

        #region 提交数据

        public async Task<TData<string>> SaveForm(AutoJobLogEntity entity)
        {
            await _autoJobLogService.SaveForm(entity);
            return new() { Data = entity.Id.ParseToString(), Tag = 1 };
        }

        public async Task<TData> DeleteForm(string ids)
        {
            await _autoJobLogService.DeleteForm(ids);
            return new() { Tag = 1 };
        }

        #endregion
    }
}