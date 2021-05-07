using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using YiSha.Admin.Web.Controllers;
using YiSha.Admin.Web.Filter;
using YiSha.Business.OrganizationManage;
using YiSha.Entity.OrganizationManage;
using YiSha.Model.Param.OrganizationManage;
using YiSha.Util.Model;
using YiSha.Web.Code;

namespace YiSha.Admin.Web.Areas.OrganizationManage.Controllers
{
    [Area("OrganizationManage")]
    public class NewsController : BaseController
    {
        private readonly NewsBLL _newsBLL = new();

        #region 视图功能

        [AuthorizeFilter("organization:news:view")]
        public IActionResult NewsIndex()
        {
            return View();
        }

        public async Task<IActionResult> NewsForm()
        {
            ViewBag.OperatorInfo = await Operator.Instance.Current();
            return View();
        }

        #endregion

        #region 获取数据

        [HttpGet, AuthorizeFilter("organization:news:search")]
        public async Task<IActionResult> GetListJson(NewsListParam param)
        {
            TData<List<NewsEntity>> obj = await _newsBLL.GetList(param);
            return Json(obj);
        }

        [HttpGet, AuthorizeFilter("organization:news:search")]
        public async Task<IActionResult> GetPageListJson(NewsListParam param, Pagination pagination)
        {
            TData<List<NewsEntity>> obj = await _newsBLL.GetPageList(param, pagination);
            return Json(obj);
        }

        [HttpGet]
        public async Task<IActionResult> GetFormJson(long id)
        {
            TData<NewsEntity> obj = await _newsBLL.GetEntity(id);
            return Json(obj);
        }

        [HttpGet]
        public async Task<IActionResult> GetMaxSortJson()
        {
            TData<int> obj = await _newsBLL.GetMaxSort();
            return Json(obj);
        }

        #endregion

        #region 提交数据

        [HttpPost, AuthorizeFilter("organization:news:add,organization:news:edit")]
        public async Task<IActionResult> SaveFormJson(NewsEntity entity)
        {
            TData<string> obj = await _newsBLL.SaveForm(entity);
            return Json(obj);
        }

        [HttpPost, AuthorizeFilter("organization:news:delete")]
        public async Task<IActionResult> DeleteFormJson(string ids)
        {
            TData obj = await _newsBLL.DeleteForm(ids);
            return Json(obj);
        }

        #endregion
    }
}