﻿using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.FileProviders;
using Microsoft.OpenApi.Models;
using NLog.Web;
using System.Reflection;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using YiSha.Admin.WebApi.Filter;
using YiSha.Common;
using YiSha.Entity;
using YiSha.Util;

namespace YiSha.Admin.WebApi
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
            //日志组件
            services.AddLogging(loging =>
            {
                loging.AddNLog("nlog.config");
            });
            //添加 Memory 缓存功能
            services.AddMemoryCache();
            //启动 Session
            services.AddSession(options =>
            {
                options.Cookie.Name = ".AspNetCore.Session";
                options.IdleTimeout = TimeSpan.FromDays(7);//设置Session的过期时间
                options.Cookie.HttpOnly = true;//设置在浏览器不能通过js获得该Cookie的值
                options.Cookie.IsEssential = true;//启用Cookie
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
            //添加 Cors
            services.AddCors();
            //添加 MVC
            var mvcBuilder = services.AddMvc();
            //添加 HttpContext 存取器
            services.AddHttpContextAccessor();
            //启动数据保护服务
            services.AddFileDataProtection();
            //添加过滤器控制器
            services.AddControllers(options =>
            {
                options.Filters.Add<GlobalException>();
            });
            //添加过滤器控制器
            services.AddControllers(options =>
            {
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
                //格式化时间
                options.JsonSerializerOptions.Converters.Add(new DateTimeJsonConverter());
                //Long 转为字符串
                options.JsonSerializerOptions.Converters.Add(new LongJsonConverter());
                //取消 Unicode 编码
                options.JsonSerializerOptions.Encoder = JavaScriptEncoder.Create(UnicodeRanges.All);
                //空值不反回前端
                //options.JsonSerializerOptions.IgnoreNullValues = true;
                //允许额外符号
                //options.JsonSerializerOptions.AllowTrailingCommas = true;
                //反序列化过程中属性名称是否使用不区分大小写的比较
                //options.JsonSerializerOptions.PropertyNameCaseInsensitive = false;
            });
            //添加 Swagger
            services.AddSwaggerGen(options =>
            {
                var xmlPath = Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml");
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "YiSha Api", Version = "v1" });
                options.IncludeXmlComments(xmlPath, true);
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
            //服务提供商
            GlobalContext.ServiceProvider = app.Services;
            //主机环境
            GlobalContext.HostingEnvironment = app.Environment;
            //判断运行模式
            if (app.Environment.IsDevelopment())
            {
                //开发环境展示错误堆栈页
                app.UseDeveloperExceptionPage();
            }
            else
            {
                //正式环境自定义错误页
                app.UseExceptionHandler("/Help/Error");
                //定时任务
                //new JobCenter().Start();
            }
            //跨域
            app.UseCors(builder =>
            {
                builder.WithOrigins(GlobalContext.SystemConfig.AllowCorsSite.Split(',')).AllowAnyHeader().AllowAnyMethod().AllowCredentials();
            });
            //Swagger
            app.UseSwagger(c =>
            {
                c.RouteTemplate = "api-doc/{documentName}/swagger.json";
            });
            //SwaggerUI
            app.UseSwaggerUI(c =>
            {
                c.RoutePrefix = "api-doc";
                c.SwaggerEndpoint("v1/swagger.json", "YiSha Api v1");
            });
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
            if (!Directory.Exists(resource)) Directory.CreateDirectory(resource);
            app.UseStaticFiles(new StaticFileOptions
            {
                RequestPath = "/Resource",
                FileProvider = new PhysicalFileProvider(resource),
                OnPrepareResponse = GlobalContext.SetCacheControl
            });
            //用户路由
            app.UseRouting();
            //用户默认路由
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=ApiHome}/{action=Index}/{id?}");
            });
        }

        /// <summary>
        /// 依赖注入
        /// </summary>
        /// <param name="services">服务</param>
        public static void AddInjection(this IServiceCollection services)
        {
            //数据库上下文
            services.AddDbContext<MyDbContext>();
        }
    }
}
