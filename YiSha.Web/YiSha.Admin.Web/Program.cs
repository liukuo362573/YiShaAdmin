using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.FileProviders;
using NLog.Web;
using System.Text;
using YiSha.Admin.Web.Controllers;
using YiSha.Util;
using YiSha.Util.Model;

namespace YiSha.Admin.Web
{
    /// <summary>
    /// Program
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// 程序入口
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.ConfigureServices(builder);
            builder.Services.AddInjection(builder);
            var app = builder.Build();
            app.Configure(builder);
            app.Run();
        }

        /// <summary>
        /// 该方法通过运行时调用
        /// 使用此方法将服务添加到容器中
        /// </summary>
        /// <param name="services">服务</param>
        /// <param name="builder">网站程序</param>
        public static void ConfigureServices(this IServiceCollection services, WebApplicationBuilder builder)
        {
            //全局环境变量
            GlobalContext.Services = services;
            GlobalContext.Configuration = builder.Configuration;
            GlobalContext.HostingEnvironment = builder.Environment;
            GlobalContext.SystemConfig = builder.Configuration.GetSection("SystemConfig").Get<SystemConfig>();
            GlobalContext.LogWhenStart(builder.Environment);
            //注册Encoding
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            //日志组件
            builder.WebHost.ConfigureLogging(logging =>
            {
                logging.ClearProviders();
                logging.SetMinimumLevel(LogLevel.Trace);
            }).UseNLog();
            //运行模式
            if (builder.Environment.IsDevelopment())
            {
                //Razor
                services.AddRazorPages().AddRazorRuntimeCompilation();
            }
            //Configure
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            //启用缓存功能
            services.AddMemoryCache();
            //启动数据保护服务
            services.AddDataProtection().PersistKeysToFileSystem(new DirectoryInfo(GlobalContext.HostingEnvironment.ContentRootPath + Path.DirectorySeparatorChar + "DataProtection"));
            //启动 Session
            services.AddSession(options =>
            {
                options.Cookie.Name = ".AspNetCore.Session";//设置Session在Cookie的Key
                options.IdleTimeout = TimeSpan.FromMinutes(20);//设置Session的过期时间
                options.Cookie.HttpOnly = true;//设置在浏览器不能通过js获得该Cookie的值
                options.Cookie.IsEssential = true;
            });
            //添加 Options 模式
            services.AddOptions();
            //添加 MVC
            services.AddMvc();
            //返回数据首字母不小写
            services.AddMvc().AddJsonOptions(options =>
            {
                //返回数据首字不变
                //PropertyNamingPolicy = null 默认不改变
                //PropertyNamingPolicy = JsonNamingPolicy.CamelCase 默认小写
                //https://docs.microsoft.com/zh-cn/dotnet/api/system.text.json.jsonserializeroptions.propertynamingpolicy?view=net-6.0
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
                //数据序列化
                options.JsonSerializerOptions.Converters.Add(new JsonDateTime());
                options.JsonSerializerOptions.Converters.Add(new JsonLong());
                //取消 Unicode 编码
                //options.JsonSerializerOptions.Encoder = JavaScriptEncoder.Create(UnicodeRanges.All);
                //空值不反回前端
                //options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                //允许额外符号
                //options.JsonSerializerOptions.AllowTrailingCommas = true;
                //反序列化过程中属性名称是否使用不区分大小写的比较
                //options.JsonSerializerOptions.PropertyNameCaseInsensitive = false;
            });
            //添加 HttpContext 存取器
            services.AddHttpContextAccessor();
            //添加 Options
            services.AddOptions();
            //全局异常捕获
            services.AddControllers(options =>
            {
                options.Filters.Add<GlobalExceptionFilter>();
                options.ModelMetadataDetailsProviders.Add(new ModelBindingMetadataProvider());
            });
        }

        /// <summary>
        /// 该方法通过运行时调用
        /// 使用此方法配置HTTP请求流水线
        /// </summary>
        /// <param name="app">应用</param>
        /// <param name="builder">网站程序</param>
        public static void Configure(this WebApplication app, WebApplicationBuilder builder)
        {
            //全局环境变量
            GlobalContext.ServiceProvider = app.Services;
            GlobalContext.Configuration = app.Configuration;
            GlobalContext.HostingEnvironment = app.Environment;
            //让 Pathbase 中间件成为第一个处理请求的中间件， 才能正确的模拟虚拟路径
            if (!string.IsNullOrEmpty(GlobalContext.SystemConfig.VirtualDirectory))
            {
                app.UsePathBase(new PathString(GlobalContext.SystemConfig.VirtualDirectory));
            }
            //运行模式
            if (app.Environment.IsDevelopment())
            {
                GlobalContext.SystemConfig.Debug = true;
                //开发环境展示错误堆栈页
                app.UseDeveloperExceptionPage();
            }
            else
            {
                //正式环境自定义错误页
                app.UseExceptionHandler("/Help/Error");
            }
            //默认的静态目录路径
            app.UseStaticFiles();
            //用户自定义静态目录
            string resource = Path.Combine(app.Environment.ContentRootPath, "Resource");
            if (!Directory.Exists(resource)) Directory.CreateDirectory(resource);
            app.UseStaticFiles(new StaticFileOptions
            {
                RequestPath = "/Resource",
                FileProvider = new PhysicalFileProvider(resource),
                OnPrepareResponse = GlobalContext.SetCacheControl,
            });
            //用户路由
            app.UseRouting();
            //用户 Session
            app.UseSession();
            //用户默认路由
            app.MapControllerRoute(
                name: "areaRoute",
                pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
        }

        /// <summary>
        /// 依赖注入
        /// </summary>
        /// <param name="services">服务</param>
        /// <param name="builder">网站程序</param>
        public static void AddInjection(this IServiceCollection services, WebApplicationBuilder builder)
        {
        }
    }
}
