using Microsoft.AspNetCore.Mvc;
using YiSha.Admin.WebApi.Filter;
using YiSha.Entity;
using YiSha.Model;
using YiSha.Model.Operator;

namespace YiSha.Admin.WebApi.Controllers
{
    /// <summary>
    /// 用户数据控制器
    /// </summary>
    [ApiController]
    [AuthorizeFilter]
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
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Login([FromQuery] string userName, [FromQuery] string password)
        {
            return Json("");
        }

        /// <summary>
        /// 用户退出登录
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult LoginOff([FromQuery] string token)
        {
            var obj = new TData();
            Operator.Instance.RemoveCurrent(token);
            obj.Message = "登出成功";
            return Json(obj);
        }
    }
}
