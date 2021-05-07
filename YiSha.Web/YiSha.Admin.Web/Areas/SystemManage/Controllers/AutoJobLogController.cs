using Microsoft.AspNetCore.Mvc;
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
    public class AutoJobLogController : BaseController
    {
        private readonly AutoJobLogBLL _autoJobLogBll = new();

        #region 视图功能

        [AuthorizeFilter("system:autojob:logview")]
        public IActionResult AutoJobLogIndex()
        {
            return View();
        }

        #endregion

        #region 获取数据

        [HttpGet]
        public async Task<IActionResult> GetListJson(AutoJobLogListParam param)
        {
            var obj = await _autoJobLogBll.GetList(param);
            return Json(obj);
        }

        [HttpGet]
        public async Task<IActionResult> GetPageListJson(AutoJobLogListParam param, Pagination pagination)
        {
            var obj = await _autoJobLogBll.GetPageList(param, pagination);
            return Json(obj);
        }

        [HttpGet]
        public async Task<IActionResult> GetFormJson(long id)
        {
            var obj = await _autoJobLogBll.GetEntity(id);
            return Json(obj);
        }

        #endregion

        #region 提交数据

        [HttpPost]
        public async Task<IActionResult> SaveFormJson(AutoJobLogEntity entity)
        {
            var obj = await _autoJobLogBll.SaveForm(entity);
            return Json(obj);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteFormJson(string ids)
        {
            var obj = await _autoJobLogBll.DeleteForm(ids);
            return Json(obj);
        }

        #endregion
    }
}