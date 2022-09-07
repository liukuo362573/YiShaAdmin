﻿using Microsoft.AspNetCore.Mvc;
using YiSha.Admin.Web.Controllers;
using YiSha.Admin.Web.Filter;
using YiSha.Business.SystemManage;
using YiSha.Model.Entity.SystemManage;
using YiSha.Model.Param.SystemManage;
using YiSha.Util.Model;

namespace YiSha.Admin.Web.Areas.SystemManage.Controllers
{
    [Area("SystemManage")]
    public class AutoJobLogController : BaseController
    {
        private AutoJobLogBLL autoJobLogBLL = new AutoJobLogBLL();

        #region 视图功能

        [AuthorizeFilter("system:autojob:logview")]
        public IActionResult AutoJobLogIndex()
        {
            return View();
        }

        public IActionResult AutoJobLogForm()
        {
            return View();
        }

        #endregion 视图功能

        #region 获取数据

        [HttpGet]
        [AuthorizeFilter("system:autojob:logview")]
        public async Task<IActionResult> GetListJson(AutoJobLogListParam param)
        {
            TData<List<AutoJobLogEntity>> obj = await autoJobLogBLL.GetList(param);
            return Json(obj);
        }

        [HttpGet]
        [AuthorizeFilter("system:autojob:logview")]
        public async Task<IActionResult> GetPageListJson(AutoJobLogListParam param, Pagination pagination)
        {
            TData<List<AutoJobLogEntity>> obj = await autoJobLogBLL.GetPageList(param, pagination);
            return Json(obj);
        }

        [HttpGet]
        [AuthorizeFilter("system:autojob:logview")]
        public async Task<IActionResult> GetFormJson(long id)
        {
            TData<AutoJobLogEntity> obj = await autoJobLogBLL.GetEntity(id);
            return Json(obj);
        }

        #endregion 获取数据

        #region 提交数据

        [HttpPost]
        [AuthorizeFilter("system:autojob:logview")]
        public async Task<IActionResult> SaveFormJson(AutoJobLogEntity entity)
        {
            TData<string> obj = await autoJobLogBLL.SaveForm(entity);
            return Json(obj);
        }

        [HttpPost]
        [AuthorizeFilter("system:autojob:logview")]
        public async Task<IActionResult> DeleteFormJson(string ids)
        {
            TData obj = await autoJobLogBLL.DeleteForm(ids);
            return Json(obj);
        }

        #endregion 提交数据
    }
}
