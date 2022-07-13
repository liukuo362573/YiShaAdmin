using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using YiSha.Util;
using YiSha.Util.Model;
using YiSha.Entity;
using YiSha.Model;
using YiSha.Admin.Web.Controllers;
using YiSha.Entity.SystemManage;
using YiSha.Business.SystemManage;
using YiSha.Model.Param.SystemManage;

namespace YiSha.Admin.Web.Areas.SystemManage.Controllers
{
    [Area("SystemManage")]
    public class LogApiController : BaseController
    {
        private LogApiBLL logApiBLL = new LogApiBLL();

        #region 视图功能
        [AuthorizeFilter("system:logapi:view")]
        public IActionResult LogApiIndex()
        {
            return View();
        }

        [AuthorizeFilter("system:logapi:view")]
        public IActionResult LogApiDetail()
        {
            return View();
        }
        #endregion

        #region 获取数据
        [HttpGet]
        [AuthorizeFilter("system:logapi:search")]
        public async Task<IActionResult> GetListJson(LogApiListParam param)
        {
            TData<List<LogApiEntity>> obj = await logApiBLL.GetList(param);
            return Json(obj);
        }

        [HttpGet]
        [AuthorizeFilter("system:logapi:search")]
        public async Task<IActionResult> GetPageListJson(LogApiListParam param, Pagination pagination)
        {
            TData<List<LogApiEntity>> obj = await logApiBLL.GetPageList(param, pagination);
            return Json(obj);
        }

        [HttpGet]
        [AuthorizeFilter("system:logapi:view")]
        public async Task<IActionResult> GetFormJson(long id)
        {
            TData<LogApiEntity> obj = await logApiBLL.GetEntity(id);
            return Json(obj);
        }
        #endregion

        #region 提交数据
        [HttpPost]
        [AuthorizeFilter("system:logapi:delete")]
        public async Task<IActionResult> DeleteFormJson(string ids)
        {
            TData obj = await logApiBLL.DeleteForm(ids);
            return Json(obj);
        }

        [HttpPost]
        [AuthorizeFilter("system:logapi:delete")]
        public async Task<IActionResult> RemoveAllFormJson()
        {
            TData obj = await logApiBLL.RemoveAllForm();
            return Json(obj);
        }
        #endregion
    }
}
