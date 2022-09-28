using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YiSha.Admin.WebApi.Comm;
using YiSha.Common;
using YiSha.Entity;
using YiSha.Model;
using YiSha.Util;

namespace YiSha.Admin.WebApi.Controllers
{
    /// <summary>
    /// 用户数据控制器
    /// </summary>
    [Authorize]
    [Route("[controller]/[action]")]
    public class UserController : Controller
    {
        /// <summary>
        /// 数据库上下文
        /// </summary>
        private MyDbContext DbCmd { get; }

        /// <summary>
        /// 用户数据控制器
        /// </summary>
        public UserController(MyDbContext myDbContext)
        {
            this.DbCmd = myDbContext;
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="userName">用户名称</param>
        /// <param name="password">用户密码</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login(string? userName = default, string? password = default)
        {
            var tdata = new TData();
            if (userName == default || password == default)
            {
                tdata.Message = "用户名或密码不能为空";
                return Json(tdata);
            }
            //Token
            var token = MD5Help.GetMd5(password + GlobalConstant.TimeStamp);
            //登录方法体
            var myJwtv = new JwtValidator
            {
                Account = userName,
                Password = password,
                Token = token,
            };
            var jwtToken = JwtConfig.GetToken(myJwtv);
            //返回结果
            var jsonResult = new
            {
                status = jwtToken.Status,
                token = jwtToken.Token,
                validTo = jwtToken.ValidTo,
            };
            return Json(jsonResult);
        }

        /// <summary>
        /// 用户退出登录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult LoginOff()
        {
            var tdata = new TData();
            tdata.Message = "登出失败";
            //退出登录
            var isOk = JwtValidator.ExitLogin(User);
            if (isOk) tdata.Message = "登出成功";
            return Json(tdata);
        }
    }
}
