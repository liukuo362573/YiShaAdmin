using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using YiSha.Admin.Web.Controllers;
using YiSha.Business.OrganizationManage;
using YiSha.Entity.OrganizationManage;
using YiSha.Model;
using YiSha.Model.Param.OrganizationManage;
using YiSha.Util;
using YiSha.Util.Export;
using YiSha.Util.Model;
using YiSha.Web.Code;

namespace YiSha.Admin.Web.Areas.OrganizationManage.Controllers
{
    [Area("OrganizationManage")]
    public class UserController : BaseController
    {
        private UserBLL userBLL = new UserBLL();
        private DepartmentBLL departmentBLL = new DepartmentBLL();

        #region 视图功能
        [AuthorizeFilter("organization:user:view")]
        public IActionResult UserIndex()
        {
            return View();
        }
        public IActionResult UserForm()
        {
            return View();
        }
        public IActionResult UserDetail()
        {
            ViewBag.Ip = NetHelper.Ip;
            return View();
        }
        public IActionResult ResetPassword()
        {
            return View();
        }
        public async Task<IActionResult> ChangePassword()
        {
            ViewBag.OperatorInfo = await Operator.Instance.Current();
            return View();
        }
        public IActionResult ChangeUser()
        {
            return View();
        }
        public async Task<IActionResult> UserPortrait()
        {
            ViewBag.OperatorInfo = await Operator.Instance.Current();
            return View();
        }
        #endregion

        #region 获取数据
        [HttpGet]
        [AuthorizeFilter("organization:user:list")]
        public async Task<IActionResult> GetListJson(UserListParam param)
        {
            TData<List<UserEntity>> obj = await userBLL.GetList(param);
            return Json(obj);
        }
        [HttpGet]
        [AuthorizeFilter("organization:user:list")]
        public async Task<IActionResult> GetPageListJson(UserListParam param, Pagination pagination)
        {
            TData<List<UserEntity>> obj = await userBLL.GetPageList(param, pagination);
            return Json(obj);
        }
        [HttpGet]
        public async Task<IActionResult> GetFormJson(long id)
        {
            TData<UserEntity> obj = await userBLL.GetEntity(id);
            return Json(obj);
        }
        #endregion

        #region 提交数据
        [HttpPost]
        [AuthorizeFilter("organization:user:add,organization:user:edit")]
        public async Task<IActionResult> SaveFormJson(UserEntity entity)
        {
            TData<string> obj = await userBLL.SaveForm(entity);
            return Json(obj);
        }
        [HttpPost]
        [AuthorizeFilter("organization:user:delete")]
        public async Task<IActionResult> DeleteFormJson(string ids)
        {
            TData obj = await userBLL.DeleteForm(ids);
            return Json(obj);
        }
        [HttpPost]
        [AuthorizeFilter("organization:user:resetpassword")]
        public async Task<IActionResult> ResetPasswordJson(UserEntity entity)
        {
            TData<long> obj = await userBLL.ResetPassword(entity);
            return Json(obj);
        }
        [HttpPost]
        public async Task<IActionResult> ChangePasswordJson(ChangePasswordParam entity)
        {
            TData<long> obj = await userBLL.ChangePassword(entity);
            return Json(obj);
        }
        [HttpPost]
        public async Task<IActionResult> ChangeUserJson(UserEntity entity)
        {
            TData<long> obj = await userBLL.ChangeUser(entity);
            return Json(obj);
        }

        [HttpPost]
        public async Task<IActionResult> ExportJson(UserListParam param)
        {
            TData<string> obj = new TData<string>();
            TData<List<UserEntity>> userObj = await userBLL.GetList(param);
            if (userObj.Tag == 1)
            {
                string file = new ExcelHelper<UserEntity>().ExportToFile("用户列表.xls",
                                                                         "用户列表",
                                                                         userObj.Result,
                                                                         new string[] { "UserName", "RealName" });
                obj.Result = file;
                obj.Tag = 1;
            }
            return Json(obj);
        }
        #endregion
    }
}