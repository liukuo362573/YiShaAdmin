using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using YiSha.Admin.Web.Controllers;
using YiSha.Admin.Web.Filter;
using YiSha.Business.OrganizationManage;
using YiSha.Business.SystemManage;
using YiSha.Entity.OrganizationManage;
using YiSha.Model.Param;
using YiSha.Model.Param.OrganizationManage;
using YiSha.Model.Result.SystemManage;
using YiSha.Util.Helper;
using YiSha.Util.Model;
using YiSha.Web.Code;

namespace YiSha.Admin.Web.Areas.OrganizationManage.Controllers
{
    [Area("OrganizationManage")]
    public class UserController : BaseController
    {
        private readonly UserBLL _userBll = new();

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
            var obj = await _userBll.GetList(param);
            return Json(obj);
        }

        [HttpGet, AuthorizeFilter("organization:user:search")]
        public async Task<IActionResult> GetPageListJson(UserListParam param, Pagination pagination)
        {
            var obj = await _userBll.GetPageList(param, pagination);
            return Json(obj);
        }

        [HttpGet]
        public async Task<IActionResult> GetFormJson(long id)
        {
            var obj = await _userBll.GetEntity(id);
            return Json(obj);
        }

        [HttpGet]
        public async Task<IActionResult> GetUserAuthorizeJson()
        {
            var operatorInfo = await Operator.Instance.Current();
            var list = await new MenuAuthorizeBLL().GetAuthorizeList(operatorInfo);
            var data = new UserAuthorizeInfo { IsSystem = operatorInfo.IsSystem };
            if (list.Tag == 1)
            {
                data.MenuAuthorize = list.Data;
            }
            return base.Json(new TData<UserAuthorizeInfo> { Data = data, Tag = 1 });
        }

        #endregion

        #region 提交数据

        [HttpPost, AuthorizeFilter("organization:user:add,organization:user:edit")]
        public async Task<IActionResult> SaveFormJson(UserEntity entity)
        {
            var obj = await _userBll.SaveForm(entity);
            return Json(obj);
        }

        [HttpPost, AuthorizeFilter("organization:user:delete")]
        public async Task<IActionResult> DeleteFormJson(string ids)
        {
            var obj = await _userBll.DeleteForm(ids);
            return Json(obj);
        }

        [HttpPost, AuthorizeFilter("organization:user:resetpassword")]
        public async Task<IActionResult> ResetPasswordJson(UserEntity entity)
        {
            var obj = await _userBll.ResetPassword(entity);
            return Json(obj);
        }

        [HttpPost]
        public async Task<IActionResult> ChangePasswordJson(ChangePasswordParam entity)
        {
            var obj = await _userBll.ChangePassword(entity);
            return Json(obj);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeUserJson(UserEntity entity)
        {
            var obj = await _userBll.ChangeUser(entity);
            return Json(obj);
        }

        [HttpPost]
        public async Task<IActionResult> ImportUserJson(ImportParam param)
        {
            var list = ExcelHelper.ImportFromExcel<UserEntity>(param.FilePath);
            var obj = await _userBll.ImportUser(param, list);
            return Json(obj);
        }

        [HttpPost]
        public async Task<IActionResult> ExportUserJson(UserListParam param)
        {
            var obj = new TData<string>();
            var data = await _userBll.GetList(param);
            if (data.Tag == 1)
            {
                string file = ExcelHelper.ExportToExcel("用户列表.xls", "用户列表", data.Data, new[]
                {
                    "UserName", "RealName", "Gender", "Mobile", "Email"
                });
                obj.Data = file;
                obj.Tag = 1;
            }
            return Json(obj);
        }

        #endregion
    }
}