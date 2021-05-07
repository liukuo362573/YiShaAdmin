using System.Collections.Generic;
using System.Threading.Tasks;
using YiSha.Entity.OrganizationManage;
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
            TData<List<LogOperateEntity>> obj = new TData<List<LogOperateEntity>>();
            obj.Data = await _logOperateService.GetList(param);
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<List<LogOperateEntity>>> GetPageList(LogOperateListParam param, Pagination pagination)
        {
            TData<List<LogOperateEntity>> obj = new TData<List<LogOperateEntity>>();
            obj.Data = await _logOperateService.GetPageList(param, pagination);
            obj.Total = pagination.TotalCount;
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<LogOperateEntity>> GetEntity(long id)
        {
            TData<LogOperateEntity> obj = new TData<LogOperateEntity>();
            obj.Data = await _logOperateService.GetEntity(id);
            if (obj.Data != null)
            {
                UserEntity userEntity = await new UserService().GetEntity(obj.Data.BaseCreatorId.Value);
                if (userEntity != null)
                {
                    obj.Data.UserName = userEntity.UserName;
                    DepartmentEntity departmentEntitty = await new DepartmentService().GetEntity(userEntity.DepartmentId.Value);
                    if (departmentEntitty != null)
                    {
                        obj.Data.DepartmentName = departmentEntitty.DepartmentName;
                    }
                }
            }
            obj.Tag = 1;
            return obj;
        }

        #endregion

        #region 提交数据

        public async Task<TData<string>> SaveForm(LogOperateEntity entity)
        {
            TData<string> obj = new TData<string>();
            await _logOperateService.SaveForm(entity);
            obj.Data = entity.Id.ParseToString();
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<string>> SaveForm(string remark)
        {
            TData<string> obj = new TData<string>();
            LogOperateEntity entity = new LogOperateEntity();
            await _logOperateService.SaveForm(entity);
            entity.LogStatus = OperateStatusEnum.Success.ParseToInt();
            entity.ExecuteUrl = remark;
            obj.Data = entity.Id.ParseToString();
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData> DeleteForm(string ids)
        {
            TData obj = new TData();
            await _logOperateService.DeleteForm(ids);
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData> RemoveAllForm()
        {
            TData obj = new TData();
            await _logOperateService.RemoveAllForm();
            obj.Tag = 1;
            return obj;
        }

        #endregion
    }
}