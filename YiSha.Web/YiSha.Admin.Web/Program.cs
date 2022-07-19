using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.FileProviders;
using NLog.Web;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using YiSha.Admin.Web.Controllers;
using YiSha.Util;
using YiSha.Util.Model;

namespace YiSha.Admin.Web
{
    /// <summary>
    /// 程序入口
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
            //WebHost
            builder.WebHost.ConfigureLogging(logging =>
            {
                logging?.ClearProviders();
                logging?.SetMinimumLevel(LogLevel.Trace);
            }).UseNLog();
            //Service
            builder.Services.ConfigureServices();
            //Injection
            builder.Services.AddInjection();
            //App
            var app = builder.Build();
            app.Configure();
            //Run
            app.Run();
        }

        /// <summary>
        /// 该方法通过运行时调用
        /// 使用此方法将服务添加到容器中
        /// </summary>
        /// <param name="services">服务</param>
        public static void ConfigureServices(this IServiceCollection services)
        {
            //添加 Razor 运行时编译
            services.AddRazorPages().AddRazorRuntimeCompilation();
            //添加编码单例
            services.AddSingleton(HtmlEncoder.Create(UnicodeRanges.All));
            //注册编码
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            //添加 Memory 缓存功能
            services.AddMemoryCache();
            //启动 Session
            services.AddSession(options =>
            {
                options.Cookie.Name = ".AspNetCore.Session";
                options.IdleTimeout = TimeSpan.FromDays(7);//设置Session的过期时间
                options.Cookie.HttpOnly = true;//设置在浏览器不能通过js获得该Cookie的值
                options.Cookie.IsEssential = true;
            });
            //
            services.Configure<CookiePolicyOptions>(options =>
            {
                //此 lambda 确定给定请求是否需要用户对非必要 cookie 的同意
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            //添加 Options
            services.AddOptions();
            //添加 MVC
            var mvcBuilder = services.AddMvc();
            //添加 HttpContext 存取器 
            services.AddHttpContextAccessor();
            //启动数据保护服务
            var directoryInfo = new DirectoryInfo($"{GlobalConstant.GetRunPath}{Path.DirectorySeparatorChar}DataProtection");
            services.AddDataProtection().PersistKeysToFileSystem(directoryInfo);
            //添加过滤器控制器
            services.AddControllers(options =>
            {
                options.Filters.Add<GlobalExceptionFilter>();
                options.ModelMetadataDetailsProviders.Add(new ModelBindingMetadataProvider());
            });
            //返回数据首字母
            mvcBuilder.AddJsonOptions(options =>
            {
                //PropertyNamingPolicy = null 默认不改变
                //PropertyNamingPolicy = JsonNamingPolicy.CamelCase 默认小写
                //https://docs.microsoft.com/zh-cn/dotnet/api/system.text.json.jsonserializeroptions.propertynamingpolicy?view=net-6.0
                //返回数据首字不变
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
            });
            //
            GlobalContext.Services = services;
        }

        /// <summary>
        /// 该方法通过运行时调用
        /// 使用此方法配置HTTP请求流水线
        /// </summary>
        /// <param name="app">应用</param>
        public static void Configure(this WebApplication app)
        {
            //配置对象
            GlobalContext.Configuration = app.Configuration;
            //系统配置
            GlobalContext.SystemConfig = app.Configuration.GetSection("SystemConfig").Get<SystemConfig>();
            //服务提供商
            GlobalContext.ServiceProvider = app.Services;
            //主机环境
            GlobalContext.HostingEnvironment = app.Environment;
            GlobalContext.LogWhenStart(app.Environment);
            //判断运行模式
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
                //捕获全局的请求
                app.Use(async (context, next) =>
                {
                    await next();
                    //401 错误
                    if (context.Response.StatusCode == 401)
                    {
                        context.Request.Path = "/Admin/Index";
                        await next();
                    }
                    //404 错误
                    if (context.Response.StatusCode == 404)
                    {
                        context.Request.Path = "/Help/Error";
                        await next();
                    }
                    //500 错误
                    if (context.Response.StatusCode == 500)
                    {
                        context.Request.Path = "/Help/Error";
                        await next();
                    }
                });
            }
            //
            if (!string.IsNullOrEmpty(GlobalContext.SystemConfig.VirtualDirectory))
            {
                //让 Pathbase 中间件成为第一个处理请求的中间件， 才能正确的模拟虚拟路径
                app.UsePathBase(new PathString(GlobalContext.SystemConfig.VirtualDirectory));
            }
            //静态目录
            app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = GlobalContext.SetCacheControl
            });
            //自定义静态目录
            string resource = Path.Combine(app.Environment.ContentRootPath, "Resource");
            FileHelper.CreateDirectory(resource);
            app.UseStaticFiles(new StaticFileOptions
            {
                RequestPath = "/Resource",
                FileProvider = new PhysicalFileProvider(resource),
                OnPrepareResponse = GlobalContext.SetCacheControl
            });
            //用户 Session
            app.UseSession();
            //用户路由
            app.UseRouting();
            //用户鉴权
            app.UseAuthorization();
            //用户默认路由
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "areas",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        /// <summary>
        /// 依赖注入
        /// </summary>
        /// <param name="services">服务</param>
        public static void AddInjection(this IServiceCollection services)
        {

        }
    }
}
