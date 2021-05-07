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
    public class AreaController : BaseController
    {
        private readonly AreaBLL _areaBll = new();

        #region 视图功能

        [AuthorizeFilter("system:area:view")]
        public IActionResult AreaIndex()
        {
            return View();
        }

        #endregion

        #region 获取数据

        [HttpGet, AuthorizeFilter("system:area:search")]
        public async Task<IActionResult> GetListJson(AreaListParam param)
        {
            var obj = await _areaBll.GetList(param);
            return Json(obj);
        }

        [HttpGet, AuthorizeFilter("system:area:search")]
        public async Task<IActionResult> GetPageListJson(AreaListParam param, Pagination pagination)
        {
            var obj = await _areaBll.GetPageList(param, pagination);
            return Json(obj);
        }

        [HttpGet]
        public async Task<IActionResult> GetZtreeAreaListJson(AreaListParam param)
        {
            var obj = await _areaBll.GetZtreeAreaList(param);
            return Json(obj);
        }

        [HttpGet]
        public async Task<IActionResult> GetFormJson(long id)
        {
            var obj = await _areaBll.GetEntity(id);
            return Json(obj);
        }

        #endregion

        #region 提交数据

        [HttpPost, AuthorizeFilter("system:area:add,ystem:area:edit")]
        public async Task<IActionResult> SaveFormJson(AreaEntity entity)
        {
            var obj = await _areaBll.SaveForm(entity);
            return Json(obj);
        }

        [HttpPost, AuthorizeFilter("system:area:delete")]
        public async Task<IActionResult> DeleteFormJson(string ids)
        {
            var obj = await _areaBll.DeleteForm(ids);
            return Json(obj);
        }

        #endregion
    }
}