using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using YiSha.Util;
using YiSha.Util.Extension;
using YiSha.Util.Model;
using YiSha.Entity.SystemManage;
using YiSha.Model.Param.SystemManage;
using YiSha.Service.SystemManage;
using YiSha.Entity.OrganizationManage;
using YiSha.Service.OrganizationManage;

namespace YiSha.Business.SystemManage
{
    public class LogApiBLL
    {
        private LogApiService logApiService = new LogApiService();

        #region 获取数据
        public async Task<TData<List<LogApiEntity>>> GetList(LogApiListParam param)
        {
            TData<List<LogApiEntity>> obj = new TData<List<LogApiEntity>>();
            obj.Result = await logApiService.GetList(param);
            obj.TotalCount = obj.Result.Count;
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<List<LogApiEntity>>> GetPageList(LogApiListParam param, Pagination pagination)
        {
            TData<List<LogApiEntity>> obj = new TData<List<LogApiEntity>>();
            obj.Result = await logApiService.GetPageList(param, pagination);
            obj.TotalCount = pagination.TotalCount;
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<LogApiEntity>> GetEntity(long id)
        {
            TData<LogApiEntity> obj = new TData<LogApiEntity>();
            obj.Result = await logApiService.GetEntity(id);
            if (obj.Result != null)
            {
                UserEntity userEntity = await new UserService().GetEntity(obj.Result.BaseCreatorId.Value);
                if (userEntity != null)
                {
                    obj.Result.UserName = userEntity.UserName;
                    DepartmentEntity departmentEntitty = await new DepartmentService().GetEntity(userEntity.DepartmentId.Value);
                    if (departmentEntitty != null)
                    {
                        obj.Result.DepartmentName = departmentEntitty.DepartmentName;
                    }
                }
            }
            obj.Tag = 1;
            return obj;
        }
        #endregion

        #region 提交数据
        public async Task<TData<string>> SaveForm(LogApiEntity entity)
        {
            TData<string> obj = new TData<string>();
            await logApiService.SaveForm(entity);
            obj.Result = entity.Id.ParseToString();
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData> DeleteForm(string ids)
        {
            TData obj = new TData();
            await logApiService.DeleteForm(ids);
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData> RemoveAllForm()
        {
            TData obj = new TData();
            await logApiService.RemoveAllForm();
            obj.Tag = 1;
            return obj;
        }
        #endregion

        #region 私有方法
        #endregion
    }
}
