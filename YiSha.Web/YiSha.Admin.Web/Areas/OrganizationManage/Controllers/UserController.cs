using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using YiSha.Admin.Web.Controllers;
using YiSha.Admin.Web.Filter;
using YiSha.Business.OrganizationManage;
using YiSha.Business.SystemManage;
using YiSha.Entity.OrganizationManage;
using YiSha.Model.Param;
using YiSha.Model.Param.OrganizationManage;
using YiSha.Model.Result;
using YiSha.Model.Result.SystemManage;
using YiSha.Util.Helper;
using YiSha.Util.Model;
using YiSha.Web.Code;

namespace YiSha.Admin.Web.Areas.OrganizationManage.Controllers
{
    [Area("OrganizationManage")]
    public class UserController : BaseController
    {
        private readonly UserBLL _userBLL = new();

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

        public IActionResult UserImport()
        {
            return View();
        }

        #endregion

        #region 获取数据

        [HttpGet, AuthorizeFilter("organization:user:search")]
        public async Task<IActionResult> GetListJson(UserListParam param)
        {
            TData<List<UserEntity>> obj = await _userBLL.GetList(param);
            return Json(obj);
        }

        [HttpGet, AuthorizeFilter("organization:user:search")]
        public async Task<IActionResult> GetPageListJson(UserListParam param, Pagination pagination)
        {
            TData<List<UserEntity>> obj = await _userBLL.GetPageList(param, pagination);
            return Json(obj);
        }

        [HttpGet]
        public async Task<IActionResult> GetFormJson(long id)
        {
            TData<UserEntity> obj = await _userBLL.GetEntity(id);
            return Json(obj);
        }

        [HttpGet]
        public async Task<IActionResult> GetUserAuthorizeJson()
        {
            TData<UserAuthorizeInfo> obj = new TData<UserAuthorizeInfo>();
            OperatorInfo operatorInfo = await Operator.Instance.Current();
            TData<List<MenuAuthorizeInfo>> objMenuAuthorizeInfo = await new MenuAuthorizeBLL().GetAuthorizeList(operatorInfo);
            obj.Data = new UserAuthorizeInfo();
            obj.Data.IsSystem = operatorInfo.IsSystem;
            if (objMenuAuthorizeInfo.Tag == 1)
            {
                obj.Data.MenuAuthorize = objMenuAuthorizeInfo.Data;
            }
            obj.Tag = 1;
            return Json(obj);
        }

        #endregion

        #region 提交数据

        [HttpPost, AuthorizeFilter("organization:user:add,organization:user:edit")]
        public async Task<IActionResult> SaveFormJson(UserEntity entity)
        {
            TData<string> obj = await _userBLL.SaveForm(entity);
            return Json(obj);
        }

        [HttpPost, AuthorizeFilter("organization:user:delete")]
        public async Task<IActionResult> DeleteFormJson(string ids)
        {
            TData obj = await _userBLL.DeleteForm(ids);
            return Json(obj);
        }

        [HttpPost, AuthorizeFilter("organization:user:resetpassword")]
        public async Task<IActionResult> ResetPasswordJson(UserEntity entity)
        {
            TData<long> obj = await _userBLL.ResetPassword(entity);
            return Json(obj);
        }

        [HttpPost]
        public async Task<IActionResult> ChangePasswordJson(ChangePasswordParam entity)
        {
            TData<long> obj = await _userBLL.ChangePassword(entity);
            return Json(obj);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeUserJson(UserEntity entity)
        {
            TData<long> obj = await _userBLL.ChangeUser(entity);
            return Json(obj);
        }

        [HttpPost]
        public async Task<IActionResult> ImportUserJson(ImportParam param)
        {
            List<UserEntity> list = ExcelHelper.ImportFromExcel<UserEntity>(param.FilePath);
            TData obj = await _userBLL.ImportUser(param, list);
            return Json(obj);
        }

        [HttpPost]
        public async Task<IActionResult> ExportUserJson(UserListParam param)
        {
            TData<string> obj = new TData<string>();
            TData<List<UserEntity>> userObj = await _userBLL.GetList(param);
            if (userObj.Tag == 1)
            {
                string file = ExcelHelper.ExportToExcel("用户列表.xls",
                                                                          "用户列表",
                                                                          userObj.Data,
                                                                          new[] { "UserName", "RealName", "Gender", "Mobile", "Email" });
                obj.Data = file;
                obj.Tag = 1;
            }
            return Json(obj);
        }

        #endregion
    }
}