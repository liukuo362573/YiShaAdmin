using Microsoft.AspNetCore.Mvc;
using System.Linq;
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
    public class RoleController : BaseController
    {
        private readonly RoleBLL _roleBll = new();

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

        [HttpGet, AuthorizeFilter("system:role:search,organization:user:search")]
        public async Task<IActionResult> GetListJson(RoleListParam param)
        {
            var obj = await _roleBll.GetList(param);
            return Json(obj);
        }

        [HttpGet, AuthorizeFilter("system:role:search,organization:user:search")]
        public async Task<IActionResult> GetPageListJson(RoleListParam param, Pagination pagination)
        {
            var obj = await _roleBll.GetPageList(param, pagination);
            return Json(obj);
        }

        [HttpGet]
        public async Task<IActionResult> GetFormJson(long id)
        {
            var obj = await _roleBll.GetEntity(id);
            return Json(obj);
        }

        [HttpGet]
        public async Task<IActionResult> GetRoleName(RoleListParam param)
        {
            var obj = new TData<string>();
            var list = await _roleBll.GetList(param);
            if (list.Tag == 1)
            {
                obj.Data = string.Join(",", list.Data.Select(p => p.RoleName));
                obj.Tag = 1;
            }
            return Json(obj);
        }

        [HttpGet]
        public async Task<IActionResult> GetMaxSortJson()
        {
            var obj = await _roleBll.GetMaxSort();
            return Json(obj);
        }

        #endregion

        #region 提交数据

        [HttpPost, AuthorizeFilter("system:role:add,system:role:edit")]
        public async Task<IActionResult> SaveFormJson(RoleEntity entity)
        {
            var obj = await _roleBll.SaveForm(entity);
            return Json(obj);
        }

        [HttpPost, AuthorizeFilter("system:role:delete")]
        public async Task<IActionResult> DeleteFormJson(string ids)
        {
            var obj = await _roleBll.DeleteForm(ids);
            return Json(obj);
        }

        #endregion
    }
}