using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using YiSha.Util;
using YiSha.Util.Model;
using YiSha.Entity;
using YiSha.Model;
using YiSha.Admin.Web.Controllers;
using YiSha.Entity.SystemManage;
using YiSha.Business.SystemManage;
using YiSha.Model.Param.SystemManage;
using YiSha.Model.Result;

namespace YiSha.Admin.Web.Areas.SystemManage.Controllers
{
    [Area("SystemManage")]
    public class AreaController : BaseController
    {
        private AreaBLL areaBLL = new AreaBLL();

        #region 视图功能
        [AuthorizeFilter("system:area:view")]
        public IActionResult AreaIndex()
        {
            return View();
        }

        public IActionResult AreaForm()
        {
            return View();
        }
        #endregion

        #region 获取数据
        [HttpGet]
        [AuthorizeFilter("system:area:search")]
        public async Task<IActionResult> GetListJson(AreaListParam param)
        {
            TData<List<AreaEntity>> obj = await areaBLL.GetList(param);
            return Json(obj);
        }

        [HttpGet]
        [AuthorizeFilter("system:area:search")]
        public async Task<IActionResult> GetPageListJson(AreaListParam param, Pagination pagination)
        {
            TData<List<AreaEntity>> obj = await areaBLL.GetPageList(param, pagination);
            return Json(obj);
        }

        [HttpGet]
        [AuthorizeFilter("system:area:view")]
        public async Task<IActionResult> GetZtreeAreaListJson(AreaListParam param)
        {
            TData<List<ZtreeInfo>> obj = await areaBLL.GetZtreeAreaList(param);
            return Json(obj);
        }

        [HttpGet]
        [AuthorizeFilter("system:area:view")]
        public async Task<IActionResult> GetFormJson(long id)
        {
            TData<AreaEntity> obj = await areaBLL.GetEntity(id);
            return Json(obj);
        }
        #endregion

        #region 提交数据
        [HttpPost]
        [AuthorizeFilter("system:area:add,ystem:area:edit")]
        public async Task<IActionResult> SaveFormJson(AreaEntity entity)
        {
            TData<string> obj = await areaBLL.SaveForm(entity);
            return Json(obj);
        }

        [HttpPost]
        [AuthorizeFilter("system:area:delete")]
        public async Task<IActionResult> DeleteFormJson(string ids)
        {
            TData obj = await areaBLL.DeleteForm(ids);
            return Json(obj);
        }
        #endregion
    }
}
