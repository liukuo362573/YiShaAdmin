using System.Collections.Generic;
using System.Threading.Tasks;
using YiSha.Entity.SystemManage;
using YiSha.Model.Param.SystemManage;
using YiSha.Service.OrganizationManage;
using YiSha.Service.SystemManage;
using YiSha.Util.Extension;
using YiSha.Util.Model;

namespace YiSha.Business.SystemManage
{
    public class LogApiBLL
    {
        private readonly LogApiService _logApiService = new();

        #region 获取数据

        public async Task<TData<List<LogApiEntity>>> GetList(LogApiListParam param)
        {
            var list = await _logApiService.GetList(param);
            return new() { Data = list, Total = list.Count, Tag = 1 };
        }

        public async Task<TData<List<LogApiEntity>>> GetPageList(LogApiListParam param, Pagination pagination)
        {
            return new()
            {
                Data = await _logApiService.GetPageList(param, pagination),
                Total = pagination.TotalCount,
                Tag = 1
            };
        }

        public async Task<TData<LogApiEntity>> GetEntity(long id)
        {
            var logEntity = await _logApiService.GetEntity(id);
            if (logEntity != null)
            {
                var userEntity = await new UserService().GetEntity(logEntity.BaseCreatorId!.Value);
                if (userEntity != null)
                {
                    var department = await new DepartmentService().GetEntity(userEntity.DepartmentId!.Value);
                    logEntity.DepartmentName = department?.DepartmentName;
                    logEntity.UserName = userEntity.UserName;
                }
            }
            return new() { Data = logEntity, Tag = 1 };
        }

        #endregion

        #region 提交数据

        public async Task<TData<string>> SaveForm(LogApiEntity entity)
        {
            await _logApiService.SaveForm(entity);
            return new() { Data = entity.Id.ParseToString(), Tag = 1 };
        }

        public async Task<TData> DeleteForm(string ids)
        {
            await _logApiService.DeleteForm(ids);
            return new() { Tag = 1 };
        }

        public async Task<TData> RemoveAllForm()
        {
            await _logApiService.RemoveAllForm();
            return new() { Tag = 1 };
        }

        #endregion
    }
}