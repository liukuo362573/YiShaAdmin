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
    public class AutoJobController : BaseController
    {
        private readonly AutoJobBLL _autoJobBLL = new();

        #region 视图功能

        [AuthorizeFilter("system:autojob:view")]
        public IActionResult AutoJobIndex()
        {
            return View();
        }

        public IActionResult AutoJobForm()
        {
            return View();
        }

        #endregion

        #region 获取数据

        [HttpGet, AuthorizeFilter("system:autojob:search")]
        public async Task<IActionResult> GetListJson(AutoJobListParam param)
        {
            TData<List<AutoJobEntity>> obj = await _autoJobBLL.GetList(param);
            return Json(obj);
        }

        [HttpGet, AuthorizeFilter("system:autojob:search")]
        public async Task<IActionResult> GetPageListJson(AutoJobListParam param, Pagination pagination)
        {
            TData<List<AutoJobEntity>> obj = await _autoJobBLL.GetPageList(param, pagination);
            return Json(obj);
        }

        [HttpGet]
        public async Task<IActionResult> GetFormJson(long id)
        {
            TData<AutoJobEntity> obj = await _autoJobBLL.GetEntity(id);
            return Json(obj);
        }

        #endregion

        #region 提交数据

        [HttpPost, AuthorizeFilter("system:autojob:add,ystem:autojob:edit")]
        public async Task<IActionResult> SaveFormJson(AutoJobEntity entity)
        {
            TData<string> obj = await _autoJobBLL.SaveForm(entity);
            return Json(obj);
        }

        [HttpPost, AuthorizeFilter("system:autojob:delete")]
        public async Task<IActionResult> DeleteFormJson(string ids)
        {
            TData obj = await _autoJobBLL.DeleteForm(ids);
            return Json(obj);
        }

        [HttpPost, AuthorizeFilter("system:autojob:pause")]
        public async Task<IActionResult> ChangeJobStatusJson(AutoJobEntity entity)
        {
            TData obj = await _autoJobBLL.ChangeJobStatus(entity);
            return Json(obj);
        }

        #endregion
    }
}