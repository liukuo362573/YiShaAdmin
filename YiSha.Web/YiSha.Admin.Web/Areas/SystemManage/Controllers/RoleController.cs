using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using YiSha.Admin.Web.Controllers;
using YiSha.Business.SystemManage;
using YiSha.Entity;
using YiSha.Entity.SystemManage;
using YiSha.Model;
using YiSha.Model.Param.SystemManage;
using YiSha.Util;
using YiSha.Util.Model;

namespace YiSha.Admin.Web.Areas.SystemManage.Controllers
{
    [Area("SystemManage")]
    public class RoleController : BaseController
    {
        private RoleBLL sysRoleBLL = new RoleBLL();

        #region 视图功能
        [AuthorizeFilter("system:role:view")]
        public IActionResult RoleIndex()
        {
            return View();
        }

        public IActionResult RoleForm()
        {
            return View();
        }
        #endregion

        #region 获取数据
        [HttpGet]
        [AuthorizeFilter("system:role:search,organization:user:search")]
        public async Task<IActionResult> GetListJson(RoleListParam param)
        {
            TData<List<RoleEntity>> obj = await sysRoleBLL.GetList(param);
            return Json(obj);
        }

        [HttpGet]
        [AuthorizeFilter("system:role:search,organization:user:search")]
        public async Task<IActionResult> GetPageListJson(RoleListParam param, Pagination pagination)
        {
            TData<List<RoleEntity>> obj = await sysRoleBLL.GetPageList(param, pagination);
            return Json(obj);
        }

        [HttpGet]
        public async Task<IActionResult> GetFormJson(long id)
        {
            TData<RoleEntity> obj = await sysRoleBLL.GetEntity(id);
            return Json(obj);
        }

        [HttpGet]
        public async Task<IActionResult> GetRoleName(RoleListParam param)
        {
            TData<string> obj = new TData<string>();
            var list = await sysRoleBLL.GetList(param);
            if (list.Tag == 1)
            {
                obj.Result = string.Join(",", list.Result.Select(p => p.RoleName));
                obj.Tag = 1;
            }
            return Json(obj);
        }

        [HttpGet]
        public async Task<IActionResult> GetMaxSortJson()
        {
            TData<int> obj = await sysRoleBLL.GetMaxSort();
            return Json(obj);
        }
        #endregion

        #region 提交数据
        [HttpPost]
        [AuthorizeFilter("system:role:add,system:role:edit")]
        public async Task<IActionResult> SaveFormJson(RoleEntity entity)
        {
            TData<string> obj = await sysRoleBLL.SaveForm(entity);
            return Json(obj);
        }

        [HttpPost]
        [AuthorizeFilter("system:role:delete")]
        public async Task<IActionResult> DeleteFormJson(string ids)
        {
            TData obj = await sysRoleBLL.DeleteForm(ids);
            return Json(obj);
        }
        #endregion
    }
}