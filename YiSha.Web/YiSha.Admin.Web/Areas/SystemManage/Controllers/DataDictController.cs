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
    public class DataDictController : BaseController
    {
        private readonly DataDictBLL _dataDictBll = new();

        #region 视图功能

        [AuthorizeFilter("system:datadict:view")]
        public IActionResult DataDictIndex()
        {
            return View();
        }

        public IActionResult DataDictForm()
        {
            return View();
        }

        #endregion

        #region 获取数据

        [HttpGet, AuthorizeFilter("system:datadict:search")]
        public async Task<IActionResult> GetListJson(DataDictListParam param)
        {
            var obj = await _dataDictBll.GetList(param);
            return Json(obj);
        }

        [HttpGet, AuthorizeFilter("system:datadict:search")]
        public async Task<IActionResult> GetPageListJson(DataDictListParam param, Pagination pagination)
        {
            var obj = await _dataDictBll.GetPageList(param, pagination);
            return Json(obj);
        }

        [HttpGet]
        public async Task<IActionResult> GetFormJson(long id)
        {
            var obj = await _dataDictBll.GetEntity(id);
            return Json(obj);
        }

        [HttpGet]
        public async Task<IActionResult> GetMaxSortJson()
        {
            var obj = await _dataDictBll.GetMaxSort();
            return Json(obj);
        }

        [HttpGet]
        public async Task<IActionResult> GetDataDictListJson()
        {
            var obj = await _dataDictBll.GetDataDictList();
            return Json(obj);
        }

        #endregion

        #region 提交数据

        [HttpPost, AuthorizeFilter("system:datadict:add,system:datadict:edit")]
        public async Task<IActionResult> SaveFormJson(DataDictEntity entity)
        {
            var obj = await _dataDictBll.SaveForm(entity);
            return Json(obj);
        }

        [HttpPost, AuthorizeFilter("system:datadict:delete")]
        public async Task<IActionResult> DeleteFormJson(string ids)
        {
            var obj = await _dataDictBll.DeleteForm(ids);
            return Json(obj);
        }

        #endregion
    }
}