using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using YiSha.Admin.Web.Controllers;
using YiSha.Admin.Web.Filter;
using YiSha.Business.SystemManage;
using YiSha.Model.Param.SystemManage;
using YiSha.Util.Model;

namespace YiSha.Admin.Web.Areas.SystemManage.Controllers
{
    [Area("SystemManage")]
    public class LogLoginController : BaseController
    {
        private readonly LogLoginBLL _logLoginBll = new();

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
            var obj = await _logLoginBll.GetList(param);
            return Json(obj);
        }

        [HttpGet, AuthorizeFilter("system:loglogin:search")]
        public async Task<IActionResult> GetPageListJson(LogLoginListParam param, Pagination pagination)
        {
            var obj = await _logLoginBll.GetPageList(param, pagination);
            return Json(obj);
        }

        [HttpGet]
        public async Task<IActionResult> GetFormJson(long id)
        {
            var obj = await _logLoginBll.GetEntity(id);
            return Json(obj);
        }

        #endregion

        #region 提交数据

        [HttpPost, AuthorizeFilter("system:loglogin:delete")]
        public async Task<IActionResult> DeleteFormJson(string ids)
        {
            var obj = await _logLoginBll.DeleteForm(ids);
            return Json(obj);
        }

        [HttpPost, AuthorizeFilter("system:loglogin:delete")]
        public async Task<IActionResult> RemoveAllFormJson()
        {
            var obj = await _logLoginBll.RemoveAllForm();
            return Json(obj);
        }

        #endregion
    }
}