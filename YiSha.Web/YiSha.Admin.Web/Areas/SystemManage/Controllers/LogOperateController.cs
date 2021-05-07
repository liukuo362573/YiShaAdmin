using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using YiSha.Admin.Web.Controllers;
using YiSha.Admin.Web.Filter;
using YiSha.Business.SystemManage;
using YiSha.Entity.SystemManage;
using YiSha.Model.Param.SystemManage;
using YiSha.Util.Model;

namespace YiSha.Admin.Web.Areas.SystemManage.Controllers
{
    [Area("SystemManage")]
    public class LogOperateController : BaseController
    {
        private readonly LogOperateBLL _logOperateBLL = new();

        #region 视图功能

        [AuthorizeFilter("system:logoperate:view")]
        public IActionResult LogOperateIndex()
        {
            return View();
        }

        public IActionResult LogOperateDetail()
        {
            return View();
        }

        #endregion

        #region 获取数据

        [HttpGet, AuthorizeFilter("system:logoperate:search")]
        public async Task<IActionResult> GetListJson(LogOperateListParam param)
        {
            TData<List<LogOperateEntity>> obj = await _logOperateBLL.GetList(param);
            return Json(obj);
        }

        [HttpGet, AuthorizeFilter("system:logoperate:search")]
        public async Task<IActionResult> GetPageListJson(LogOperateListParam param, Pagination pagination)
        {
            TData<List<LogOperateEntity>> obj = await _logOperateBLL.GetPageList(param, pagination);
            return Json(obj);
        }

        [HttpGet]
        public async Task<IActionResult> GetFormJson(long id)
        {
            TData<LogOperateEntity> obj = await _logOperateBLL.GetEntity(id);
            return Json(obj);
        }

        #endregion

        #region 提交数据

        [HttpPost, AuthorizeFilter("system:logoperate:delete")]
        public async Task<IActionResult> DeleteFormJson(string ids)
        {
            TData obj = await _logOperateBLL.DeleteForm(ids);
            return Json(obj);
        }

        [HttpPost, AuthorizeFilter("system:logoperate:delete")]
        public async Task<IActionResult> RemoveAllFormJson()
        {
            TData obj = await _logOperateBLL.RemoveAllForm();
            return Json(obj);
        }

        #endregion
    }
}