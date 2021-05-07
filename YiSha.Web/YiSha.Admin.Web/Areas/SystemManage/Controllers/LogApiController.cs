using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using YiSha.Admin.Web.Controllers;
using YiSha.Business.SystemManage;
using YiSha.Entity.SystemManage;
using YiSha.Model.Param.SystemManage;
using YiSha.Util.Model;

namespace YiSha.Admin.Web.Areas.SystemManage.Controllers
{
    [Area("SystemManage")]
    public class LogApiController : BaseController
    {
        private readonly LogApiBLL _logApiBLL = new();

        #region 视图功能

        public IActionResult LogApiIndex()
        {
            return View();
        }

        public IActionResult LogApiDetail()
        {
            return View();
        }

        #endregion

        #region 获取数据

        [HttpGet]
        public async Task<IActionResult> GetListJson(LogApiListParam param)
        {
            TData<List<LogApiEntity>> obj = await _logApiBLL.GetList(param);
            return Json(obj);
        }

        [HttpGet]
        public async Task<IActionResult> GetPageListJson(LogApiListParam param, Pagination pagination)
        {
            TData<List<LogApiEntity>> obj = await _logApiBLL.GetPageList(param, pagination);
            return Json(obj);
        }

        [HttpGet]
        public async Task<IActionResult> GetFormJson(long id)
        {
            TData<LogApiEntity> obj = await _logApiBLL.GetEntity(id);
            return Json(obj);
        }

        #endregion

        #region 提交数据

        [HttpPost]
        public async Task<IActionResult> DeleteFormJson(string ids)
        {
            TData obj = await _logApiBLL.DeleteForm(ids);
            return Json(obj);
        }

        [HttpPost]
        public async Task<IActionResult> RemoveAllFormJson()
        {
            TData obj = await _logApiBLL.RemoveAllForm();
            return Json(obj);
        }

        #endregion
    }
}