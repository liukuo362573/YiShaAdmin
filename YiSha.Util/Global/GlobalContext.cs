using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using YiSha.Util.Model;

namespace YiSha.Util
{
    /// <summary>
    /// 全局上下文
    /// </summary>
    public static class GlobalContext
    {
        /// <summary>
        /// 所有注册服务和类实例容器。用于依赖注入
        /// </summary>
        public static IServiceCollection Services { get; set; }

        /// <summary>
        /// 已配置的服务提供商
        /// </summary>
        public static IServiceProvider ServiceProvider { get; set; }

        /// <summary>
        /// 配置对象
        /// </summary>
        public static IConfiguration Configuration { get; set; }

        /// <summary>
        /// 主机环境
        /// </summary>
        public static IWebHostEnvironment HostingEnvironment { get; set; }

        /// <summary>
        /// 系统配置
        /// </summary>
        public static SystemConfig SystemConfig { get; set; }

        /// <summary>
        /// 获取版本号
        /// </summary>
        /// <returns></returns>
        public static string GetVersion()
        {
            var version = Assembly.GetEntryAssembly().GetName().Version;
            return $"{version?.Major}.{version?.Minor}";
        }

        /// <summary>
        /// 设置 CacheControl
        /// </summary>
        /// <param name="context"></param>
        public static void SetCacheControl(StaticFileResponseContext context)
        {
            if (context == null) return;
            int second = 365 * 24 * 60 * 60;
            context.Context.Response.Headers.Add("Cache-Control", new[] { "public,max-age=" + second });
            context.Context.Response.Headers.Add("Expires", new[] { DateTime.UtcNow.AddYears(1).ToString("R") }); // Format RFC1123
        }
    }
}
