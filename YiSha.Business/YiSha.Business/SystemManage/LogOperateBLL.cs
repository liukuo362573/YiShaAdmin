using System.Collections.Generic;
using System.Threading.Tasks;
using YiSha.Entity.SystemManage;
using YiSha.Enum;
using YiSha.Model.Param.SystemManage;
using YiSha.Service.OrganizationManage;
using YiSha.Service.SystemManage;
using YiSha.Util.Extension;
using YiSha.Util.Model;

namespace YiSha.Business.SystemManage
{
    public class LogOperateBLL
    {
        private readonly LogOperateService _logOperateService = new();

        #region 获取数据

        public async Task<TData<List<LogOperateEntity>>> GetList(LogOperateListParam param)
        {
            return new() { Data = await _logOperateService.GetList(param), Tag = 1 };
        }

        public async Task<TData<List<LogOperateEntity>>> GetPageList(LogOperateListParam param, Pagination pagination)
        {
            return new()
            {
                Data = await _logOperateService.GetPageList(param, pagination),
                Total = pagination.TotalCount,
                Tag = 1
            };
        }

        public async Task<TData<LogOperateEntity>> GetEntity(long id)
        {
            var logEntity = await _logOperateService.GetEntity(id);
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

        public async Task<TData<string>> SaveForm(LogOperateEntity entity)
        {
            await _logOperateService.SaveForm(entity);
            return new() { Data = entity.Id.ParseToString(), Tag = 1 };
        }

        public async Task<TData<string>> SaveForm(string remark)
        {
            var entity = new LogOperateEntity();
            await _logOperateService.SaveForm(entity);
            entity.LogStatus = OperateStatusEnum.Success.ParseToInt();
            entity.ExecuteUrl = remark;
            return new() { Data = entity.Id.ParseToString(), Tag = 1 };
        }

        public async Task<TData> DeleteForm(string ids)
        {
            await _logOperateService.DeleteForm(ids);
            return new() { Tag = 1 };
        }

        public async Task<TData> RemoveAllForm()
        {
            await _logOperateService.RemoveAllForm();
            return new() { Tag = 1 };
        }

        #endregion
    }
}