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
    public class DataDictDetailController : BaseController
    {
        private readonly DataDictDetailBLL _dataDictDetailBll = new();

        #region 视图功能

        [AuthorizeFilter("system:datadict:view")]
        public IActionResult DataDictDetailIndex()
        {
            return View();
        }

        public IActionResult DataDictDetailForm()
        {
            return View();
        }

        #endregion

        #region 获取数据

        [HttpGet, AuthorizeFilter("system:datadict:search")]
        public async Task<IActionResult> GetListJson(DataDictDetailListParam param)
        {
            var obj = await _dataDictDetailBll.GetList(param);
            return Json(obj);
        }

        [HttpGet, AuthorizeFilter("system:datadict:search")]
        public async Task<IActionResult> GetPageListJson(DataDictDetailListParam param, Pagination pagination)
        {
            var obj = await _dataDictDetailBll.GetPageList(param, pagination);
            return Json(obj);
        }

        [HttpGet]
        public async Task<IActionResult> GetFormJson(long id)
        {
            var obj = await _dataDictDetailBll.GetEntity(id);
            return Json(obj);
        }

        [HttpGet]
        public async Task<IActionResult> GetMaxSortJson()
        {
            var obj = await _dataDictDetailBll.GetMaxSort();
            return Json(obj);
        }

        #endregion

        #region 提交数据

        [HttpPost, AuthorizeFilter("system:datadict:add,system:datadict:edit")]
        public async Task<IActionResult> SaveFormJson(DataDictDetailEntity entity)
        {
            var obj = await _dataDictDetailBll.SaveForm(entity);
            return Json(obj);
        }

        [HttpPost, AuthorizeFilter("system:datadict:delete")]
        public async Task<IActionResult> DeleteFormJson(string ids)
        {
            var obj = await _dataDictDetailBll.DeleteForm(ids);
            return Json(obj);
        }

        #endregion
    }
}