using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;
using YiSha.Admin.Web.Filter;
using YiSha.Business.OrganizationManage;
using YiSha.Business.SystemManage;
using YiSha.Entity.OrganizationManage;
using YiSha.Entity.SystemManage;
using YiSha.Enum;
using YiSha.Util.Extension;
using YiSha.Util.Helper;
using YiSha.Util.Model;
using YiSha.Web.Code;
using YiSha.Web.Code.State;

namespace YiSha.Admin.Web.Controllers
{
    public class HomeController : BaseController
    {
        private readonly MenuBLL _menuBll = new();
        private readonly UserBLL _userBll = new();
        private readonly LogLoginBLL _logLoginBll = new();
        private readonly MenuAuthorizeBLL _menuAuthorizeBll = new();

        #region 视图功能

        [HttpGet, AuthorizeFilter]
        public async Task<IActionResult> Index()
        {
            OperatorInfo operatorInfo = await Operator.Instance.Current();

            var data = await _menuBll.GetList(null);
            var menuList = data.Data.Where(p => p.MenuStatus == StatusEnum.Yes.ParseToInt()).ToList();

            if (operatorInfo.IsSystem != 1)
            {
                var objMenuAuthorize = await _menuAuthorizeBll.GetAuthorizeList(operatorInfo);
                var authorizeMenuIdList = objMenuAuthorize.Data.Select(p => p.MenuId).ToList();
                menuList = menuList.Where(p => authorizeMenuIdList.Contains(p.Id)).ToList();
            }

            ViewBag.MenuList = menuList;
            ViewBag.OperatorInfo = operatorInfo;
            return View();
        }

        [HttpGet]
        public IActionResult Welcome()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (GlobalContext.SystemConfig.Demo)
            {
                ViewBag.UserName = "admin";
                ViewBag.Password = "123456";
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> LoginOff()
        {
            #region 退出系统

            OperatorInfo user = await Operator.Instance.Current();
            if (user != null)
            {
                // 如果不允许同一个用户多次登录，当用户登出的时候，就不在线了
                if (!GlobalContext.SystemConfig.LoginMultiple)
                {
                    await _userBll.UpdateUser(new UserEntity { Id = user.UserId, IsOnline = 0 });
                }

                // 登出日志
                await _logLoginBll.SaveForm(new LogLoginEntity
                {
                    LogStatus = OperateStatusEnum.Success.ParseToInt(),
                    Remark = "退出系统",
                    IpAddress = NetHelper.Ip,
                    IpLocation = string.Empty,
                    Browser = NetHelper.Browser,
                    Os = NetHelper.GetOsVersion(),
                    ExtraRemark = NetHelper.UserAgent,
                    BaseCreatorId = user.UserId
                });

                Operator.Instance.RemoveCurrent();
                CookieHelper.RemoveCookie("RememberMe");
            }

            #endregion 退出系统

            return View(nameof(Login));
        }

        [HttpGet]
        public IActionResult NoPermission()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Error(string message)
        {
            ViewBag.Message = message;
            return View();
        }

        [HttpGet]
        public IActionResult Skin()
        {
            return View();
        }

        #endregion 视图功能

        #region 获取数据

        public IActionResult GetCaptchaImage()
        {
            _ = GlobalContext.ServiceProvider?.GetService<IHttpContextAccessor>().HttpContext.Session.Id;

            Tuple<string, int> captchaCode = CaptchaHelper.GetCaptchaCode();
            byte[] bytes = CaptchaHelper.CreateCaptchaImage(captchaCode.Item1);
            SessionHelper.WriteSession("CaptchaCode", captchaCode.Item2);
            return File(bytes, @"image/jpeg");
        }

        #endregion 获取数据

        #region 提交数据

        [HttpPost]
        public async Task<IActionResult> LoginJson(string userName, string password, string captchaCode)
        {
            if (string.IsNullOrEmpty(captchaCode))
            {
                return Json(new TData { Tag = 0, Message = "验证码不能为空" });
            }
            if (captchaCode != SessionHelper.GetSession("CaptchaCode").ParseToString())
            {
                return Json(new TData { Tag = 0, Message = "验证码错误，请重新输入" });
            }

            var userObj = await _userBll.CheckLogin(userName, password, (int)PlatformEnum.Web);
            if (userObj.Tag == 1)
            {
                await new UserBLL().UpdateUser(userObj.Data);
                await Operator.Instance.AddCurrent(userObj.Data.WebToken);
            }

            string ip = NetHelper.Ip;
            string browser = NetHelper.Browser;
            string os = NetHelper.GetOsVersion();
            string userAgent = NetHelper.UserAgent;
            int logStatus = userObj.Tag == 1 ? OperateStatusEnum.Success.ParseToInt() : OperateStatusEnum.Fail.ParseToInt();
            var logLoginEntity = new LogLoginEntity
            {
                LogStatus = logStatus,
                Remark = userObj.Message,
                IpAddress = ip,
                IpLocation = IpLocationHelper.GetIpLocation(ip),
                Browser = browser,
                Os = os,
                ExtraRemark = userAgent,
                BaseCreatorId = userObj.Data?.Id
            };
            logLoginEntity.BaseCreatorId ??= 0;

            // 让底层不用获取HttpContext
            AsyncTaskHelper.StartTask(async () => await _logLoginBll.SaveForm(logLoginEntity));

            return Json(new TData { Tag = userObj.Tag, Message = userObj.Message });
        }

        #endregion 提交数据
    }
}