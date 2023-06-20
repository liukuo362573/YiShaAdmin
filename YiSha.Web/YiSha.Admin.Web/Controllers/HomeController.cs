using Microsoft.AspNetCore.Mvc;
using YiSha.Business.OrganizationManage;
using YiSha.Business.SystemManage;
using YiSha.Entity.OrganizationManage;
using YiSha.Entity.SystemManage;
using YiSha.Enum;
using YiSha.Util;
using YiSha.Util.Extension;
using YiSha.Util.Model;
using YiSha.Web.Code;

namespace YiSha.Admin.Web.Controllers
{
    public class HomeController : BaseController
    {
        private MenuBLL menuBLL { get; set; }

        private UserBLL userBLL { get; set; }

        private LogLoginBLL logLoginBLL { get; set; }

        private MenuAuthorizeBLL menuAuthorizeBLL { get; set; }

        public HomeController()
        {
            menuBLL = new MenuBLL();
            userBLL = new UserBLL();
            logLoginBLL = new LogLoginBLL();
            menuAuthorizeBLL = new MenuAuthorizeBLL();
        }

        /// <summary>
        /// Index
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AuthorizeFilter]
        public async Task<IActionResult> Index()
        {
            var operatorInfo = await Operator.Instance.Current();

            var objMenu = await menuBLL.GetList();
            var menuList = objMenu.Data;
            menuList = menuList.Where(p => p.MenuStatus == StatusEnum.Yes.ParseToInt()).ToList();

            if (operatorInfo.IsSystem != 1)
            {
                var objMenuAuthorize = await menuAuthorizeBLL.GetAuthorizeList(operatorInfo);
                var authorizeMenuIdList = objMenuAuthorize.Data.Select(p => p.MenuId).ToList();
                menuList = menuList.Where(p => authorizeMenuIdList.Contains(p.Id)).ToList();
            }

            ViewBag.MenuList = menuList;
            ViewBag.OperatorInfo = operatorInfo;
            return View();
        }

        /// <summary>
        /// Welcome
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Welcome()
        {
            return View();
        }

        /// <summary>
        /// Login
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// 退出登录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> LoginOffJson()
        {
            var user = await Operator.Instance.Current();
            if (user != null)
            {
                #region 退出系统

                // 如果不允许同一个用户多次登录，当用户登出的时候，就不在线了
                if (!GlobalContext.SystemConfig.LoginMultiple)
                {
                    await userBLL.UpdateUser(new UserEntity { Id = user.UserId, IsOnline = 0 });
                }

                // 登出日志
                await logLoginBLL.SaveForm(new LogLoginEntity
                {
                    LogStatus = OperateStatusEnum.Success.ParseToInt(),
                    Remark = "退出系统",
                    IpAddress = NetHelper.Ip,
                    IpLocation = string.Empty,
                    Browser = NetHelper.Browser,
                    OS = NetHelper.GetOSVersion(),
                    ExtraRemark = NetHelper.UserAgent,
                    BaseCreatorId = user.UserId
                });

                Operator.Instance.RemoveCurrent();
                CookieHelper.Remove("RememberMe");

                return Json(new TData { Tag = 1 });

                #endregion 退出系统
            }
            else
            {
                throw new Exception("非法请求");
            }
        }

        /// <summary>
        /// NoPermission
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult NoPermission()
        {
            return View();
        }

        /// <summary>
        /// Error
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Error(string message)
        {
            ViewBag.Message = message;
            return View();
        }

        /// <summary>
        /// Skin
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Skin()
        {
            return View();
        }

        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <returns></returns>
        public IActionResult GetCaptchaImage()
        {
            var captchaCode = CaptchaHelper.GetCaptchaCode();
            var bytes = CaptchaHelper.CreateCaptchaImage(captchaCode.Item1);
            SessionHelper.Set("CaptchaCode", captchaCode.Item2);
            return File(bytes, @"image/jpeg");
        }

        /// <summary>
        /// 登录帐号
        /// </summary>
        /// <param name="userName">帐号</param>
        /// <param name="password">密码</param>
        /// <param name="captchaCode">验证码</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> LoginJson(string userName, string password, string captchaCode)
        {
            var obj = new TData();
            if (string.IsNullOrEmpty(captchaCode))
            {
                obj.Message = "验证码不能为空";
                return Json(obj);
            }
            if (captchaCode != SessionHelper.Get("CaptchaCode").ParseToString())
            {
                obj.Message = "验证码错误，请重新输入";
                return Json(obj);
            }
            var userObj = await userBLL.CheckLogin(userName, password, (int)PlatformEnum.Web);
            if (userObj.Tag == 1)
            {
                await new UserBLL().UpdateUser(userObj.Data);
                await Operator.Instance.AddCurrent(userObj.Data.WebToken);
            }

            var ip = NetHelper.Ip;
            var browser = NetHelper.Browser;
            var os = NetHelper.GetOSVersion();
            var userAgent = NetHelper.UserAgent;

            Action taskAction = async () =>
            {
                LogLoginEntity logLoginEntity = new LogLoginEntity
                {
                    LogStatus = userObj.Tag == 1 ? OperateStatusEnum.Success.ParseToInt() : OperateStatusEnum.Fail.ParseToInt(),
                    Remark = userObj.Message,
                    IpAddress = ip,
                    IpLocation = IpLocationHelper.GetIpLocation(ip),
                    Browser = browser,
                    OS = os,
                    ExtraRemark = userAgent,
                    BaseCreatorId = userObj.Data?.Id
                };

                // 让底层不用获取HttpContext
                logLoginEntity.BaseCreatorId = logLoginEntity.BaseCreatorId ?? 0;

                await logLoginBLL.SaveForm(logLoginEntity);
            };
            AsyncTaskHelper.StartTask(taskAction);

            obj.Tag = userObj.Tag;
            obj.Message = userObj.Message;
            return Json(obj);
        }
    }
}
