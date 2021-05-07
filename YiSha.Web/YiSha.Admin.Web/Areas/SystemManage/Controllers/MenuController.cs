using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using YiSha.Admin.Web.Controllers;
using YiSha.Admin.Web.Filter;
using YiSha.Business.SystemManage;
using YiSha.Entity.SystemManage;
using YiSha.Model.Param.SystemManage;

namespace YiSha.Admin.Web.Areas.SystemManage.Controllers
{
    [Area("SystemManage")]
    public class MenuController : BaseController
    {
        private readonly MenuBLL _menuBll = new();

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
            var obj = await _menuBll.GetList(param);
            return Json(obj);
        }

        [HttpGet, AuthorizeFilter("system:menu:search,system:role:search")]
        public async Task<IActionResult> GetMenuTreeListJson(MenuListParam param)
        {
            var obj = await _menuBll.GetZtreeList(param);
            return Json(obj);
        }

        [HttpGet]
        public async Task<IActionResult> GetFormJson(long id)
        {
            var obj = await _menuBll.GetEntity(id);
            return Json(obj);
        }

        [HttpGet]
        public async Task<IActionResult> GetMaxSortJson(long parentId = 0)
        {
            var obj = await _menuBll.GetMaxSort(parentId);
            return Json(obj);
        }

        #endregion

        #region 提交数据

        [HttpPost, AuthorizeFilter("system:menu:add,system:menu:edit")]
        public async Task<IActionResult> SaveFormJson(MenuEntity entity)
        {
            var obj = await _menuBll.SaveForm(entity);
            return Json(obj);
        }

        [HttpPost, AuthorizeFilter("system:menu:delete")]
        public async Task<IActionResult> DeleteFormJson(string ids)
        {
            var obj = await _menuBll.DeleteForm(ids);
            return Json(obj);
        }

        #endregion
    }
}