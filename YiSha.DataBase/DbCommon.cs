using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Text.RegularExpressions;
using YiSha.DataBase.Common;
using YiSha.DataBase.Enum;
using YiSha.DataBase.Interceptor;
using YiSha.Util;

namespace YiSha.DataBase
{
    /// <summary>
    /// <b>数据库连接对象</b>
    ///
    /// <para>常规使用：using var dbComm = new DbContext()</para>
    /// <para>注入使用：services.AddDbContext&lt;DbContext&gt;()</para>
    /// <para>继承此对象可以实现原生操作！by zgcwkj</para>
    /// </summary>
    public class DbCommon : DbContext, IDisposable
    {
        /// <summary>
        /// 数据库类型
        /// </summary>
        private DbType dbType { get; }

        /// <summary>
        /// 连接字符
        /// </summary>
        private string dbConnect { get; }

        /// <summary>
        /// 连接超时
        /// </summary>
        private int dbTimeout { get; }

        /// <summary>
        /// 数据库版本
        /// </summary>
        private int dbVersion { get; }

        /// <summary>
        /// <para>数据库连接对象</para>
        /// <para><b>使用配置文件中信息来连接</b></para>
        /// </summary>
        public DbCommon()
        {
            this.dbType = DbFactory.Type;
            this.dbConnect = DbFactory.Connect;
            this.dbTimeout = DbFactory.Timeout;
            if (!string.IsNullOrEmpty(Regex.Match(DbFactory.Connect, "version=.+?;").Value))
            {
                this.dbConnect = DbFactory.Connect.Replace(Regex.Match(DbFactory.Connect, "version=.+?;").Value, "");
                this.dbVersion = Convert.ToInt32(Regex.Match(DbFactory.Connect, "(?<=version=).+?(?=;)").Value);
            }
        }

        /// <summary>
        /// <para>数据库连接对象</para>
        /// <para><b>使用自定义配置来连接</b></para>
        /// <para>用法：base(DbType, "SQLConnect")</para>
        /// </summary>
        /// <param name="dbType">数据库类型</param>
        /// <param name="dbConnect">连接字符</param>
        /// <param name="dbTimeout">连接超时</param>
        public DbCommon(DbType dbType, string dbConnect, int dbTimeout = 10)
        {
            this.dbType = dbType;
            this.dbConnect = dbConnect;
            this.dbTimeout = dbTimeout == 10 ? dbTimeout : DbFactory.Timeout;
            if (!string.IsNullOrEmpty(Regex.Match(dbConnect, "version=.+?;").Value))
            {
                this.dbConnect = dbConnect.Replace(Regex.Match(dbConnect, "version=.+?;").Value, "");
                this.dbVersion = Convert.ToInt32(Regex.Match(dbConnect, "(?<=version=).+?(?=;)").Value);
            }
        }

        /// <summary>-
        /// <para>数据库连接对象</para>
        /// <para><b>使用自定义配置来连接，数据库类型相同</b></para>
        /// <para>用法：base("SQLConnect")</para>
        /// </summary>
        /// <param name="dbConnect">连接字符</param>
        /// <param name="dbTimeout">连接超时</param>
        public DbCommon(string dbConnect, int dbTimeout = 10)
        {
            this.dbType = DbFactory.Type;
            this.dbConnect = dbConnect;
            this.dbTimeout = dbTimeout == 10 ? dbTimeout : DbFactory.Timeout;
            if (!string.IsNullOrEmpty(Regex.Match(dbConnect, "version=.+?;").Value))
            {
                this.dbConnect = dbConnect.Replace(Regex.Match(dbConnect, "version=.+?;").Value, "");
                this.dbVersion = Convert.ToInt32(Regex.Match(dbConnect, "(?<=version=).+?(?=;)").Value);
            }
        }

        /// <summary>
        /// 配置要使用的数据库
        /// </summary>
        /// <param name="optionsBuilder">上下文选项生成器</param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //SqlServer
            if (dbType == DbType.SqlServer)
            {
                optionsBuilder.UseSqlServer(dbConnect, p =>
                {
                    p.CommandTimeout(dbTimeout);
                });
            }
            //MySql
            else if (dbType == DbType.MySql)
            {
                optionsBuilder.UseMySql(dbConnect, ServerVersion.AutoDetect(dbConnect), p =>
                {
                    p.CommandTimeout(dbTimeout);
                });
            }
            //Oracle
            else if (dbType == DbType.Oracle)
            {
                optionsBuilder.UseOracle(dbConnect, p =>
                {
                    if (dbVersion != 0) p.UseOracleSQLCompatibility($"{dbVersion}");
                    p.CommandTimeout(dbTimeout);
                });
            }
            //数据库拦截器
            optionsBuilder.AddInterceptors(new DbInterceptor());
            //输出日志
            EFLogger.Add(optionsBuilder);
            //
            base.OnConfiguring(optionsBuilder);
        }

        /// <summary>
        /// 配置通过约定发现的模型
        /// </summary>
        /// <param name="modelBuilder">模型制作者</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //提取所有模型
            var filePath = GlobalConstant.GetRunPath;
            var root = new DirectoryInfo(filePath);
            var files = root.GetFiles("*.dll");
            foreach (var file in files)
            {
                try
                {
                    if (file.FullName.Contains("Microsoft")) continue;
                    if (file.FullName.Contains("System")) continue;
                    var fileName = file.Name.Replace(file.Extension, "");
                    var assemblyName = new AssemblyName(fileName);
                    var entityAssembly = Assembly.Load(assemblyName);
                    var typesToRegister = entityAssembly.GetTypes()
                        .Where(p => !string.IsNullOrEmpty(p.Namespace))
                        .Where(p => !string.IsNullOrEmpty(p.GetCustomAttribute<TableAttribute>()?.Name));
                    foreach (var type in typesToRegister)
                    {
                        var createInstance = Activator.CreateInstance(type);
                        modelBuilder.Model.AddEntityType(type);
                    }
                }
                catch { }
            }
            //
            base.OnModelCreating(modelBuilder);
        }
    }
}
