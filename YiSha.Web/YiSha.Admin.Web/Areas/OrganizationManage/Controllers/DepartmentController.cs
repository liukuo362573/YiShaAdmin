using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using YiSha.Admin.Web.Controllers;
using YiSha.Admin.Web.Filter;
using YiSha.Business.OrganizationManage;
using YiSha.Entity.OrganizationManage;
using YiSha.Model.Param.OrganizationManage;

namespace YiSha.Admin.Web.Areas.OrganizationManage.Controllers
{
    [Area("OrganizationManage")]
    public class DepartmentController : BaseController
    {
        private readonly DepartmentBLL _departmentBll = new();

        #region 视图功能

        [AuthorizeFilter("organization:department:view")]
        public IActionResult DepartmentIndex()
        {
            return View();
        }

        public IActionResult DepartmentForm()
        {
            return View();
        }

        #endregion

        #region 获取数据

        [HttpGet, AuthorizeFilter("organization:department:search,organization:user:search")]
        public async Task<IActionResult> GetListJson(DepartmentListParam param)
        {
            var obj = await _departmentBll.GetList(param);
            return Json(obj);
        }

        [HttpGet, AuthorizeFilter("organization:department:search,organization:user:search")]
        public async Task<IActionResult> GetDepartmentTreeListJson(DepartmentListParam param)
        {
            var obj = await _departmentBll.GetZtreeDepartmentList(param);
            return Json(obj);
        }

        [HttpGet]
        public async Task<IActionResult> GetUserTreeListJson(DepartmentListParam param)
        {
            var obj = await _departmentBll.GetZtreeUserList(param);
            return Json(obj);
        }

        [HttpGet]
        public async Task<IActionResult> GetFormJson(long id)
        {
            var obj = await _departmentBll.GetEntity(id);
            return Json(obj);
        }

        [HttpGet]
        public async Task<IActionResult> GetMaxSortJson()
        {
            var obj = await _departmentBll.GetMaxSort();
            return Json(obj);
        }

        #endregion

        #region 提交数据

        [HttpPost, AuthorizeFilter("organization:department:add,organization:department:edit")]
        public async Task<IActionResult> SaveFormJson(DepartmentEntity entity)
        {
            var obj = await _departmentBll.SaveForm(entity);
            return Json(obj);
        }

        [HttpPost, AuthorizeFilter("organization:department:delete")]
        public async Task<IActionResult> DeleteFormJson(string ids)
        {
            var obj = await _departmentBll.DeleteForm(ids);
            return Json(obj);
        }

        #endregion
    }
}