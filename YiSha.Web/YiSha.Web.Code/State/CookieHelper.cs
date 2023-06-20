using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace YiSha.Util
{
    /// <summary>
    /// Cookie 帮助
    /// </summary>
    public class CookieHelper
    {
        /// <summary>
        /// 获取 Cookie 对象
        /// </summary>
        /// <returns></returns>
        public static IResponseCookies? GetObj()
        {
            var hca = GlobalContext.ServiceProvider?.GetService<IHttpContextAccessor>();
            return hca?.HttpContext?.Response.Cookies;
        }

        /// <summary>
        /// 写入 Cookie
        /// </summary>
        /// <param name="sName">名称</param>
        /// <param name="sValue">值</param>
        /// <param name="httpOnly">前端脚本能否获取到的Cookie</param>
        /// <returns>状态</returns>
        public static bool Set(string sName, string sValue, bool httpOnly = true)
        {
            var hca = GlobalContext.ServiceProvider?.GetService<IHttpContextAccessor>();
            var option = new CookieOptions
            {
                Expires = DateTime.MaxValue,
                HttpOnly = httpOnly,
            };
            hca?.HttpContext?.Response.Cookies.Append(sName, sValue, option);
            return true;
        }

        /// <summary>
        /// 写入 Cookie
        /// </summary>
        /// <param name="sName">名称</param>
        /// <param name="sValue">值</param>
        /// <param name="expires">过期时间(分钟)</param>
        /// <param name="httpOnly">前端脚本能否获取到的Cookie</param>
        /// <returns>状态</returns>
        public static bool Set(string sName, string sValue, int expires, bool httpOnly = true)
        {
            var hca = GlobalContext.ServiceProvider?.GetService<IHttpContextAccessor>();
            var option = new CookieOptions
            {
                Expires = DateTime.Now.AddMinutes(expires),
                HttpOnly = httpOnly,
            };
            hca?.HttpContext?.Response.Cookies.Append(sName, sValue, option);
            return true;
        }

        /// <summary>
        /// 写入临时 Cookie
        /// </summary>
        /// <param name="sName">名称</param>
        /// <param name="sValue">值</param>
        /// <param name="httpOnly">前端脚本能否获取到的Cookie</param>
        /// <returns>状态</returns>
        public static bool SetTemp(string sName, string sValue, bool httpOnly = true)
        {
            var hca = GlobalContext.ServiceProvider?.GetService<IHttpContextAccessor>();
            var option = new CookieOptions
            {
                HttpOnly = httpOnly,
            };
            hca?.HttpContext?.Response.Cookies.Append(sName, sValue, option);
            return true;
        }

        /// <summary>
        /// 读取 Cookie
        /// </summary>
        /// <param name="sName">名称</param>
        /// <returns>值</returns>
        public static string Get(string sName)
        {
            var hca = GlobalContext.ServiceProvider?.GetService<IHttpContextAccessor>();
            return hca?.HttpContext?.Request.Cookies[sName] ?? "";
        }

        /// <summary>
        /// 删除 Cookie
        /// </summary>
        /// <param name="sName">Cookie对象名称</param>
        /// <returns>状态</returns>
        public static bool Remove(string sName)
        {
            var hca = GlobalContext.ServiceProvider?.GetService<IHttpContextAccessor>();
            hca?.HttpContext?.Response.Cookies.Delete(sName);
            return true;
        }

        /// <summary>
        /// 清空 Cookie
        /// </summary>
        /// <returns>状态</returns>
        public static bool Clear()
        {
            var hca = GlobalContext.ServiceProvider?.GetService<IHttpContextAccessor>();
            //请求
            var request = hca?.HttpContext?.Request;
            if (request == null) return false;
            //响应
            var response = hca?.HttpContext?.Response;
            if (response == null) return false;
            //循环所有 Cookie
            foreach (var cookie in request.Cookies)
            {
                var key = cookie.Key;
                //var value = cookie.Value;
                response.Cookies.Delete(key);
            }
            return true;
        }
    }
}
