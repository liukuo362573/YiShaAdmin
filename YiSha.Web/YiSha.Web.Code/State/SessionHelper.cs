using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using YiSha.Util.Model;

namespace YiSha.Web.Code.State
{
    public static class SessionHelper
    {
        /// <summary>
        /// 写Session
        /// </summary>
        /// <typeparam name="T">Session键值的类型</typeparam>
        /// <param name="key">Session的键名</param>
        /// <param name="value">Session的键值</param>
        public static void WriteSession<T>(string key, T value)
        {
            if (!string.IsNullOrEmpty(key))
            {
                var hca = GlobalContext.ServiceProvider?.GetService<IHttpContextAccessor>();
                hca?.HttpContext?.Session.SetString(key, JsonConvert.SerializeObject(value));
            }
        }

        /// <summary>
        /// 读取Session的值
        /// </summary>
        /// <param name="key">Session的键名</param>
        public static string GetSession(string key)
        {
            if (!string.IsNullOrEmpty(key))
            {
                var hca = GlobalContext.ServiceProvider?.GetService<IHttpContextAccessor>();
                return hca?.HttpContext?.Session.GetString(key);
            }
            return string.Empty;
        }

        /// <summary>
        /// 删除指定Session
        /// </summary>
        /// <param name="key">Session的键名</param>
        public static void RemoveSession(string key)
        {
            if (!string.IsNullOrEmpty(key))
            {
                var hca = GlobalContext.ServiceProvider?.GetService<IHttpContextAccessor>();
                hca?.HttpContext?.Session.Remove(key);
            }
        }
    }
}