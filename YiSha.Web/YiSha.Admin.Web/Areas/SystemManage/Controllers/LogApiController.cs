using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using YiSha.Admin.Web.Controllers;
using YiSha.Business.SystemManage;
using YiSha.Model.Param.SystemManage;
using YiSha.Util.Model;

namespace YiSha.Admin.Web.Areas.SystemManage.Controllers
{
    [Area("SystemManage")]
    public class LogApiController : BaseController
    {
        private readonly LogApiBLL _logApiBll = new();

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
            var obj = await _logApiBll.GetList(param);
            return Json(obj);
        }

        [HttpGet]
        public async Task<IActionResult> GetPageListJson(LogApiListParam param, Pagination pagination)
        {
            var obj = await _logApiBll.GetPageList(param, pagination);
            return Json(obj);
        }

        [HttpGet]
        public async Task<IActionResult> GetFormJson(long id)
        {
            var obj = await _logApiBll.GetEntity(id);
            return Json(obj);
        }

        #endregion

        #region 提交数据

        [HttpPost]
        public async Task<IActionResult> DeleteFormJson(string ids)
        {
            var obj = await _logApiBll.DeleteForm(ids);
            return Json(obj);
        }

        [HttpPost]
        public async Task<IActionResult> RemoveAllFormJson()
        {
            var obj = await _logApiBll.RemoveAllForm();
            return Json(obj);
        }

        #endregion
    }
}