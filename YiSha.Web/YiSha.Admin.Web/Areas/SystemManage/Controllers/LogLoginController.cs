using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using YiSha.Admin.Web.Controllers;
using YiSha.Business.SystemManage;
using YiSha.Entity.SystemManage;
using YiSha.Util.Model;
using YiSha.Model.Param.SystemManage;

namespace YiSha.Admin.Web.Areas.SystemManage.Controllers
{
    [Area("SystemManage")]
    public class LogLoginController : BaseController
    {
        private LogLoginBLL logLoginBLL = new LogLoginBLL();

        #region 视图功能
        [AuthorizeFilter("system:loglogin:view")]
        public IActionResult LogLoginIndex()
        {
            return View();
        }
        #endregion

        #region 获取数据
        [HttpGet]
        [AuthorizeFilter("system:loglogin:search")]
        public async Task<IActionResult> GetListJson(LogLoginListParam param)
        {
            TData<List<LogLoginEntity>> obj = await logLoginBLL.GetList(param);
            return Json(obj);
        }
        [HttpGet]
        [AuthorizeFilter("system:loglogin:search")]
        public async Task<IActionResult> GetPageListJson(LogLoginListParam param, Pagination pagination)
        {
            TData<List<LogLoginEntity>> obj = await logLoginBLL.GetPageList(param, pagination);
            return Json(obj);
        }
        [HttpGet]
        public async Task<IActionResult> GetFormJson(long id)
        {
            TData<LogLoginEntity> obj = await logLoginBLL.GetEntity(id);
            return Json(obj);
        }
        #endregion

        #region 提交数据
        [HttpPost]
        [AuthorizeFilter("system:loglogin:delete")]
        public async Task<IActionResult> DeleteFormJson(string ids)
        {
            TData obj = await logLoginBLL.DeleteForm(ids);
            return Json(obj);
        }
        [HttpPost]
        [AuthorizeFilter("system:loglogin:delete")]
        public async Task<IActionResult> RemoveAllFormJson()
        {
            TData obj = await logLoginBLL.RemoveAllForm();
            return Json(obj);
        }
        #endregion
    }
}