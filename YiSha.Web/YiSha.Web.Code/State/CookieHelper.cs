using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using YiSha.Util.Model;

namespace YiSha.Web.Code.State
{
    public static class CookieHelper
    {
        /// <summary>
        /// 写cookie值
        /// </summary>
        public static void WriteCookie(string sName, string sValue)
        {
            var hca = GlobalContext.ServiceProvider?.GetService<IHttpContextAccessor>();
            var option = new CookieOptions { Expires = DateTime.Now.AddDays(30) };
            hca?.HttpContext?.Response.Cookies.Append(sName, sValue, option);
        }

        /// <summary>
        /// 写cookie值
        /// </summary>
        /// <param name="sName">名称</param>
        /// <param name="sValue">值</param>
        /// <param name="expires">过期时间(分钟)</param>
        public static void WriteCookie(string sName, string sValue, int expires)
        {
            var hca = GlobalContext.ServiceProvider?.GetService<IHttpContextAccessor>();
            var option = new CookieOptions { Expires = DateTime.Now.AddMinutes(expires) };
            hca?.HttpContext?.Response.Cookies.Append(sName, sValue, option);
        }

        /// <summary>
        /// 读cookie值
        /// </summary>
        /// <param name="sName">名称</param>
        /// <returns>cookie值</returns>
        public static string GetCookie(string sName)
        {
            var hca = GlobalContext.ServiceProvider?.GetService<IHttpContextAccessor>();
            return hca?.HttpContext?.Request.Cookies[sName];
        }

        /// <summary>
        /// 删除Cookie对象
        /// </summary>
        /// <param name="sName">Cookie对象名称</param>
        public static void RemoveCookie(string sName)
        {
            var hca = GlobalContext.ServiceProvider?.GetService<IHttpContextAccessor>();
            hca?.HttpContext?.Response.Cookies.Delete(sName);
        }
    }
}