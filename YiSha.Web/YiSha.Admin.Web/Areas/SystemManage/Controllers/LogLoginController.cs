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
    public class LogLoginController : BaseController
    {
        private readonly LogLoginBLL _logLoginBLL = new();

        #region 视图功能

        [AuthorizeFilter("system:loglogin:view")]
        public IActionResult LogLoginIndex()
        {
            return View();
        }

        #endregion

        #region 获取数据

        [HttpGet, AuthorizeFilter("system:loglogin:search")]
        public async Task<IActionResult> GetListJson(LogLoginListParam param)
        {
            TData<List<LogLoginEntity>> obj = await _logLoginBLL.GetList(param);
            return Json(obj);
        }

        [HttpGet, AuthorizeFilter("system:loglogin:search")]
        public async Task<IActionResult> GetPageListJson(LogLoginListParam param, Pagination pagination)
        {
            TData<List<LogLoginEntity>> obj = await _logLoginBLL.GetPageList(param, pagination);
            return Json(obj);
        }

        [HttpGet]
        public async Task<IActionResult> GetFormJson(long id)
        {
            TData<LogLoginEntity> obj = await _logLoginBLL.GetEntity(id);
            return Json(obj);
        }

        #endregion

        #region 提交数据

        [HttpPost, AuthorizeFilter("system:loglogin:delete")]
        public async Task<IActionResult> DeleteFormJson(string ids)
        {
            TData obj = await _logLoginBLL.DeleteForm(ids);
            return Json(obj);
        }

        [HttpPost, AuthorizeFilter("system:loglogin:delete")]
        public async Task<IActionResult> RemoveAllFormJson()
        {
            TData obj = await _logLoginBLL.RemoveAllForm();
            return Json(obj);
        }

        #endregion
    }
}