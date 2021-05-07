﻿using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using YiSha.Admin.Web.Controllers;
using YiSha.Admin.Web.Filter;
using YiSha.Business.SystemManage;
using YiSha.Entity.SystemManage;
using YiSha.Model.Param.SystemManage;
using YiSha.Model.Result;
using YiSha.Util.Model;

namespace YiSha.Admin.Web.Areas.SystemManage.Controllers
{
    [Area("SystemManage")]
    public class MenuController : BaseController
    {
        private readonly MenuBLL _menuBLL = new();

        #region 视图功能

        [AuthorizeFilter("system:menu:view")]
        public IActionResult MenuIndex()
        {
            return View();
        }

        public IActionResult MenuForm()
        {
            return View();
        }

        public IActionResult MenuChoose()
        {
            return View();
        }

        public IActionResult MenuIcon()
        {
            return View();
        }

        #endregion

        #region 获取数据

        [HttpGet, AuthorizeFilter("system:menu:search,system:role:search")]
        public async Task<IActionResult> GetListJson(MenuListParam param)
        {
            TData<List<MenuEntity>> obj = await _menuBLL.GetList(param);
            return Json(obj);
        }

        [HttpGet, AuthorizeFilter("system:menu:search,system:role:search")]
        public async Task<IActionResult> GetMenuTreeListJson(MenuListParam param)
        {
            TData<List<ZtreeInfo>> obj = await _menuBLL.GetZtreeList(param);
            return Json(obj);
        }

        [HttpGet]
        public async Task<IActionResult> GetFormJson(long id)
        {
            TData<MenuEntity> obj = await _menuBLL.GetEntity(id);
            return Json(obj);
        }

        [HttpGet]
        public async Task<IActionResult> GetMaxSortJson(long parentId = 0)
        {
            TData<int> obj = await _menuBLL.GetMaxSort(parentId);
            return Json(obj);
        }

        #endregion

        #region 提交数据

        [HttpPost, AuthorizeFilter("system:menu:add,system:menu:edit")]
        public async Task<IActionResult> SaveFormJson(MenuEntity entity)
        {
            TData<string> obj = await _menuBLL.SaveForm(entity);
            return Json(obj);
        }

        [HttpPost, AuthorizeFilter("system:menu:delete")]
        public async Task<IActionResult> DeleteFormJson(string ids)
        {
            TData obj = await _menuBLL.DeleteForm(ids);
            return Json(obj);
        }

        #endregion
    }
}