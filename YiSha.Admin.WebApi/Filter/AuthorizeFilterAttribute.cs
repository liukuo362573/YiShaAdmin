using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using YiSha.Entity;
using YiSha.Enum;
using YiSha.Model.Entity.SystemManage;
using YiSha.Util;
using YiSha.Model.Operator;

namespace YiSha.Admin.WebApi.Filter
{
    /// <summary>
    /// 验证 Token 和 记录日志
    /// </summary>
    public class AuthorizeFilterAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// 忽略 Token 的方法
        /// </summary>
        public static readonly string[] IgnoreToken = { "GetWxOpenId", "Login", "LoginOff" };

        /// <summary>
        /// 异步接口日志
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            using var dbCmd = GlobalContext.ServiceProvider.CreateScope().ServiceProvider.GetService<MyDbContext>();


            var resultContext = await next();
        }
    }
}
