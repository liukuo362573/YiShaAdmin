using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Text;
using System.Text.Json;

namespace YiSha.Util
{
    /// <summary>
    /// Session 帮助
    /// </summary>
    public class SessionHelper
    {
        /// <summary>
        /// 获取 Session 对象
        /// </summary>
        /// <returns></returns>
        public static ISession? GetObj()
        {
            var hca = GlobalContext.ServiceProvider?.GetService<IHttpContextAccessor>();
            return hca?.HttpContext?.Session;
        }

        /// <summary>
        /// 写入 Session
        /// </summary>
        /// <typeparam name="T">Session键值的类型</typeparam>
        /// <param name="key">Session的键名</param>
        /// <param name="value">Session的键值</param>
        /// <returns>状态</returns>
        public static bool Set<T>(string key, T value)
        {
            if (string.IsNullOrEmpty(key)) return false;
            var hca = GlobalContext.ServiceProvider?.GetService<IHttpContextAccessor>();
            hca?.HttpContext?.Session.SetString(key, JsonSerializer.Serialize(value));
            return true;
        }

        /// <summary>
        /// 写入 Session
        /// </summary>
        /// <param name="key">Session的键名</param>
        /// <param name="value">Session的键值</param>
        /// <returns>状态</returns>
        public static bool Set(string key, string value)
        {
            return Set<string>(key, value);
        }

        /// <summary>
        /// 读取 Session
        /// </summary>
        /// <param name="key">Session的键名</param>
        /// <returns>值</returns>
        public static T? Get<T>(string key)
        {
            if (string.IsNullOrEmpty(key)) return default;
            var hca = GlobalContext.ServiceProvider?.GetService<IHttpContextAccessor>();
            var sessionStr = hca?.HttpContext?.Session.GetString(key);
            if (string.IsNullOrEmpty(sessionStr)) return default;
            return JsonSerializer.Deserialize<T>(sessionStr);
        }

        /// <summary>
        /// 读取 Session
        /// </summary>
        /// <param name="key">Session的键名</param>
        /// <returns>值</returns>
        public static string Get(string key)
        {
            return Get<string>(key) ?? "";
        }

        /// <summary>
        /// 删除 Session
        /// </summary>
        /// <param name="key">Session的键名</param>
        /// <returns>状态</returns>
        public static bool Remove(string key)
        {
            if (string.IsNullOrEmpty(key)) return false;
            var hca = GlobalContext.ServiceProvider?.GetService<IHttpContextAccessor>();
            hca?.HttpContext?.Session.Remove(key);
            return true;
        }

        /// <summary>
        /// 清空 Session
        /// </summary>
        /// <returns>状态</returns>
        public static bool Clear()
        {
            var hca = GlobalContext.ServiceProvider?.GetService<IHttpContextAccessor>();
            hca?.HttpContext?.Session.Clear();
            return true;
        }
    }
}
