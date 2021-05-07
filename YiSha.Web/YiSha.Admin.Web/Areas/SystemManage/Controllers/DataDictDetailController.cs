using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
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
        private readonly DataDictDetailBLL _dataDictDetailBLL = new();

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
            TData<List<DataDictDetailEntity>> obj = await _dataDictDetailBLL.GetList(param);
            return Json(obj);
        }

        [HttpGet, AuthorizeFilter("system:datadict:search")]
        public async Task<IActionResult> GetPageListJson(DataDictDetailListParam param, Pagination pagination)
        {
            TData<List<DataDictDetailEntity>> obj = await _dataDictDetailBLL.GetPageList(param, pagination);
            return Json(obj);
        }

        [HttpGet]
        public async Task<IActionResult> GetFormJson(long id)
        {
            TData<DataDictDetailEntity> obj = await _dataDictDetailBLL.GetEntity(id);
            return Json(obj);
        }

        [HttpGet]
        public async Task<IActionResult> GetMaxSortJson()
        {
            TData<int> obj = await _dataDictDetailBLL.GetMaxSort();
            return Json(obj);
        }

        #endregion

        #region 提交数据

        [HttpPost, AuthorizeFilter("system:datadict:add,system:datadict:edit")]
        public async Task<IActionResult> SaveFormJson(DataDictDetailEntity entity)
        {
            TData<string> obj = await _dataDictDetailBLL.SaveForm(entity);
            return Json(obj);
        }

        [HttpPost, AuthorizeFilter("system:datadict:delete")]
        public async Task<IActionResult> DeleteFormJson(string ids)
        {
            TData obj = await _dataDictDetailBLL.DeleteForm(ids);
            return Json(obj);
        }

        #endregion
    }
}