using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using YiSha.Admin.Web.Controllers;
using YiSha.Business.OrganizationManage;
using YiSha.Entity.OrganizationManage;
using YiSha.Model;
using YiSha.Model.Param.OrganizationManage;
using YiSha.Util.Model;

namespace YiSha.Admin.Web.Areas.OrganizationManage.Controllers
{
    [Area("OrganizationManage")]
    public class PositionController : BaseController
    {
        private PositionBLL sysPositionBLL = new PositionBLL();

        #region 视图功能
        [AuthorizeFilter("organization:position:view")]
        public IActionResult PositionIndex()
        {
            return View();
        }

        public IActionResult PositionForm()
        {
            return View();
        }
        #endregion

        #region 获取数据
        [HttpGet]
        [AuthorizeFilter("organization:position:search,organization:user:view")]
        public async Task<IActionResult> GetListJson(PositionListParam param)
        {
            TData<List<PositionEntity>> obj = await sysPositionBLL.GetList(param);
            return Json(obj);
        }

        [HttpGet]
        [AuthorizeFilter("organization:position:search,organization:user:view")]
        public async Task<IActionResult> GetPageListJson(PositionListParam param, Pagination pagination)
        {
            TData<List<PositionEntity>> obj = await sysPositionBLL.GetPageList(param, pagination);
            return Json(obj);
        }

        [HttpGet]
        public async Task<IActionResult> GetFormJson(long id)
        {
            TData<PositionEntity> obj = await sysPositionBLL.GetEntity(id);
            return Json(obj);
        }

        [HttpGet]
        public async Task<IActionResult> GetMaxSortJson()
        {
            TData<int> obj = await sysPositionBLL.GetMaxSort();
            return Json(obj);
        }

        [HttpGet]
        public async Task<IActionResult> GetPositionName(PositionListParam param)
        {
            TData<string> obj = new TData<string>();
            var list = await sysPositionBLL.GetList(param);
            if (list.Tag == 1)
            {
                obj.Result = string.Join(",", list.Result.Select(p => p.PositionName));
                obj.Tag = 1;
            }
            return Json(obj);
        }
        #endregion

        #region 提交数据
        [HttpPost]
        [AuthorizeFilter("organization:position:add,organization:position:edit")]
        public async Task<IActionResult> SaveFormJson(PositionEntity entity)
        {
            TData<string> obj = await sysPositionBLL.SaveForm(entity);
            return Json(obj);
        }

        [HttpPost]
        [AuthorizeFilter("organization:position:delete")]
        public async Task<IActionResult> DeleteFormJson(string ids)
        {
            TData obj = await sysPositionBLL.DeleteForm(ids);
            return Json(obj);
        }
        #endregion
    }
}