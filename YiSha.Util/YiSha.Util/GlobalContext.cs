using System;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using YiSha.Util.Model;
using Microsoft.AspNetCore.StaticFiles;

namespace YiSha.Util
{
    public class GlobalContext
    {
        /// <summary>
        /// All registered service and class instance container. Which are used for dependency injection.
        /// </summary>
        public static IServiceCollection Services { get; set; }

        /// <summary>
        /// Configured service provider.
        /// </summary>
        public static IServiceProvider ServiceProvider { get; set; }

        public static IConfiguration Configuration { get; set; }

        public static IHostingEnvironment HostingEnvironment { get; set; }

        public static SystemConfig SystemConfig { get; set; }

        /// <summary>
        /// 程序启动时，记录目录
        /// </summary>
        /// <param name="env"></param>
        public static void LogWhenStart(IHostingEnvironment env)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("程序启动");
            sb.AppendLine("ContentRootPath:" + env.ContentRootPath);
            sb.AppendLine("WebRootPath:" + env.WebRootPath);
            sb.AppendLine("IsDevelopment:" + env.IsDevelopment());
            LogHelper.WriteWithTime(sb.ToString());
        }

        /// <summary>
        /// 设置cache control
        /// </summary>
        /// <param name="context"></param>
        public static void SetCacheControl(StaticFileResponseContext context)
        {
            int second = 365 * 24 * 60 * 60;
            context.Context.Response.Headers.Add("Cache-Control", new[] { "public,max-age=" + second });
            context.Context.Response.Headers.Add("Expires", new[] { DateTime.UtcNow.AddYears(1).ToString("R") }); // Format RFC1123
        }
    }
}
