using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using YiSha.Admin.Web.Controllers;
using YiSha.Admin.Web.Filter;
using YiSha.Business.SystemManage;
using YiSha.Entity.SystemManage;
using YiSha.Model.Param.SystemManage;
using YiSha.Model.Result.SystemManage;
using YiSha.Util.Model;

namespace YiSha.Admin.Web.Areas.SystemManage.Controllers
{
    [Area("SystemManage")]
    public class DataDictController : BaseController
    {
        private readonly DataDictBLL _dataDictBLL = new();

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
            TData<List<DataDictEntity>> obj = await _dataDictBLL.GetList(param);
            return Json(obj);
        }

        [HttpGet, AuthorizeFilter("system:datadict:search")]
        public async Task<IActionResult> GetPageListJson(DataDictListParam param, Pagination pagination)
        {
            TData<List<DataDictEntity>> obj = await _dataDictBLL.GetPageList(param, pagination);
            return Json(obj);
        }

        [HttpGet]
        public async Task<IActionResult> GetFormJson(long id)
        {
            TData<DataDictEntity> obj = await _dataDictBLL.GetEntity(id);
            return Json(obj);
        }

        [HttpGet]
        public async Task<IActionResult> GetMaxSortJson()
        {
            TData<int> obj = await _dataDictBLL.GetMaxSort();
            return Json(obj);
        }

        [HttpGet]
        public async Task<IActionResult> GetDataDictListJson()
        {
            TData<List<DataDictInfo>> obj = await _dataDictBLL.GetDataDictList();
            return Json(obj);
        }

        #endregion

        #region 提交数据

        [HttpPost, AuthorizeFilter("system:datadict:add,system:datadict:edit")]
        public async Task<IActionResult> SaveFormJson(DataDictEntity entity)
        {
            TData<string> obj = await _dataDictBLL.SaveForm(entity);
            return Json(obj);
        }

        [HttpPost, AuthorizeFilter("system:datadict:delete")]
        public async Task<IActionResult> DeleteFormJson(string ids)
        {
            TData obj = await _dataDictBLL.DeleteForm(ids);
            return Json(obj);
        }

        #endregion
    }
}