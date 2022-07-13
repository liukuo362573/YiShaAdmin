using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YiSha.Admin.Web.Controllers;
using YiSha.Business.SystemManage;
using YiSha.Entity.SystemManage;
using YiSha.Model.Param.SystemManage;
using YiSha.Util.Model;

namespace YiSha.Admin.Web.Areas.SystemManage.Controllers
{
    [Area("SystemManage")]
    public class RoleController : BaseController
    {
        private RoleBLL roleBLL = new RoleBLL();

        #region 视图功能
        [AuthorizeFilter("system:role:view")]
        public IActionResult RoleIndex()
        {
            return View();
        }

        [AuthorizeFilter("system:role:view")]
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
            TData<List<RoleEntity>> obj = await roleBLL.GetList(param);
            return Json(obj);
        }

        [HttpGet]
        [AuthorizeFilter("system:role:search,organization:user:search")]
        public async Task<IActionResult> GetPageListJson(RoleListParam param, Pagination pagination)
        {
            TData<List<RoleEntity>> obj = await roleBLL.GetPageList(param, pagination);
            return Json(obj);
        }

        [HttpGet]
        [AuthorizeFilter("system:role:view")]
        public async Task<IActionResult> GetFormJson(long id)
        {
            TData<RoleEntity> obj = await roleBLL.GetEntity(id);
            return Json(obj);
        }

        [HttpGet]
        [AuthorizeFilter("system:role:view")]
        public async Task<IActionResult> GetRoleName(RoleListParam param)
        {
            TData<string> obj = new TData<string>();
            var list = await roleBLL.GetList(param);
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
            TData<int> obj = await roleBLL.GetMaxSort();
            return Json(obj);
        }
        #endregion

        #region 提交数据
        [HttpPost]
        [AuthorizeFilter("system:role:add,system:role:edit")]
        public async Task<IActionResult> SaveFormJson(RoleEntity entity)
        {
            TData<string> obj = await roleBLL.SaveForm(entity);
            return Json(obj);
        }

        [HttpPost]
        [AuthorizeFilter("system:role:delete")]
        public async Task<IActionResult> DeleteFormJson(string ids)
        {
            TData obj = await roleBLL.DeleteForm(ids);
            return Json(obj);
        }
        #endregion
    }
}