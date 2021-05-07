using System.Collections.Generic;
using System.Threading.Tasks;
using YiSha.Entity.SystemManage;
using YiSha.Model.Param.SystemManage;
using YiSha.Service.SystemManage;
using YiSha.Util.Extension;
using YiSha.Util.Model;

namespace YiSha.Business.SystemManage
{
    public class LogLoginBLL
    {
        private readonly LogLoginService _logLoginService = new();

        #region 获取数据

        public async Task<TData<List<LogLoginEntity>>> GetList(LogLoginListParam param)
        {
            return new() { Data = await _logLoginService.GetList(param), Tag = 1 };
        }

        public async Task<TData<List<LogLoginEntity>>> GetPageList(LogLoginListParam param, Pagination pagination)
        {
            var data = await _logLoginService.GetPageList(param, pagination);
            return new() { Data = data, Total = pagination.TotalCount, Tag = 1 };
        }

        public async Task<TData<LogLoginEntity>> GetEntity(long id)
        {
            return new() { Data = await _logLoginService.GetEntity(id), Tag = 1 };
        }

        #endregion

        #region 提交数据

        public async Task<TData<string>> SaveForm(LogLoginEntity entity)
        {
            await _logLoginService.SaveForm(entity);
            return new() { Data = entity.Id.ParseToString(), Tag = 1 };
        }

        public async Task<TData> DeleteForm(string ids)
        {
            await _logLoginService.DeleteForm(ids);
            return new() { Tag = 1 };
        }

        public async Task<TData> RemoveAllForm()
        {
            await _logLoginService.RemoveAllForm();
            return new() { Tag = 1 };
        }

        #endregion
    }
}