using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.DependencyInjection;

namespace YiSha.Util
{
    /// <summary>
    /// 全局网站配置
    /// </summary>
    public static class GlobalWebConfig
    {
        /// <summary>
        /// 添加本地文件的数据保护
        /// </summary>
        /// <param name="services"></param>
        public static void AddFileDataProtection(this IServiceCollection services)
        {
            string protection = Path.Combine(GlobalConstant.GetRunPath, "Protection");
            if (!Directory.Exists(protection)) Directory.CreateDirectory(protection);
            services.AddDataProtection().PersistKeysToFileSystem(new DirectoryInfo(protection));
        }
    }
}
