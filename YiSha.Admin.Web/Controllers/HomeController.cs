﻿using Microsoft.AspNetCore.Mvc;
using YiSha.Admin.Web.Filter;
using YiSha.Business.OrganizationManage;
using YiSha.Business.SystemManage;
using YiSha.Common;
using YiSha.Entity.Operator;
using YiSha.Enum;
using YiSha.Model.Entity.OrganizationManage;
using YiSha.Model.Entity.SystemManage;
using YiSha.Model.Result;
using YiSha.Util;
using YiSha.Util.Common;
using YiSha.Util.Model;

namespace YiSha.Admin.Web.Controllers
{
    public class HomeController : BaseController
    {
        private MenuBLL menuBLL = new MenuBLL();
        private UserBLL userBLL = new UserBLL();
        private LogLoginBLL logLoginBLL = new LogLoginBLL();
        private MenuAuthorizeBLL menuAuthorizeBLL = new MenuAuthorizeBLL();

        #region 视图功能

        [HttpGet]
        [AuthorizeFilter]
        public async Task<IActionResult> Index()
        {
            OperatorInfo operatorInfo = await Operator.Instance.Current();

            TData<List<MenuEntity>> objMenu = await menuBLL.GetList(null);
            List<MenuEntity> menuList = objMenu.Data;
            menuList = menuList.Where(p => p.MenuStatus == StatusEnum.Yes.ToInt()).ToList();

            if (operatorInfo.IsSystem != 1)
            {
                TData<List<MenuAuthorizeInfo>> objMenuAuthorize = await menuAuthorizeBLL.GetAuthorizeList(operatorInfo);
                List<long?> authorizeMenuIdList = objMenuAuthorize.Data.Select(p => p.MenuId).ToList();
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
            if (GlobalConstant.IsDevelopment)
            {
                ViewBag.UserName = "admin";
                ViewBag.Password = "123456";
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LoginOffJson()
        {
            OperatorInfo user = await Operator.Instance.Current();
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
                    LogStatus = OperateStatusEnum.Success.ToInt(),
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
            var captchaCode = CaptchaCode.GetCaptchaCode();
            var bytes = CaptchaCode.CreateCaptchaImage(captchaCode.Item1);
            SessionHelper.Set("CaptchaCode", captchaCode.Item2);
            return File(bytes, @"image/jpeg");
        }

        #endregion 获取数据

        #region 提交数据

        [HttpPost]
        public async Task<IActionResult> LoginJson(string userName, string password, string captchaCode)
        {
            TData obj = new TData();
            if (string.IsNullOrEmpty(captchaCode))
            {
                obj.Message = "验证码不能为空";
                return Json(obj);
            }
            if (captchaCode != SessionHelper.Get("CaptchaCode").ToStr())
            {
                obj.Message = "验证码错误，请重新输入";
                return Json(obj);
            }
            TData<UserEntity> userObj = await userBLL.CheckLogin(userName, password, (int)PlatformEnum.Web);
            if (userObj.Tag == 1)
            {
                await new UserBLL().UpdateUser(userObj.Data);
                await Operator.Instance.AddCurrent(userObj.Data.WebToken);
            }

            string ip = NetHelper.Ip;
            string browser = NetHelper.Browser;
            string os = NetHelper.GetOSVersion();
            string userAgent = NetHelper.UserAgent;

            Action taskAction = async () =>
            {
                LogLoginEntity logLoginEntity = new LogLoginEntity
                {
                    LogStatus = userObj.Tag == 1 ? OperateStatusEnum.Success.ToInt() : OperateStatusEnum.Fail.ToInt(),
                    Remark = userObj.Message,
                    IpAddress = ip,
                    IpLocation = IpLocationHelper.GetIpLocation(ip),
                    Browser = browser,
                    OS = os,
                    ExtraRemark = userAgent,
                    BaseCreatorId = userObj.Data.Id
                };

                // 让底层不用获取HttpContext
                logLoginEntity.BaseCreatorId = logLoginEntity.BaseCreatorId;

                await logLoginBLL.SaveForm(logLoginEntity);
            };
            AsyncTaskHelper.StartTask(taskAction);

            obj.Tag = userObj.Tag;
            obj.Message = userObj.Message;
            return Json(obj);
        }

        #endregion 提交数据
    }
}
