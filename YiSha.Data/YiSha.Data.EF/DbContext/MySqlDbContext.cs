using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using YiSha.Util;

namespace YiSha.Data.EF
{
    public class MySqlDbContext : DbContext
    {
        private static readonly ILoggerFactory _loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());

        private string ConnectionString { get; set; }

        public MySqlDbContext(string connectionString)
        {
            ConnectionString = connectionString;
        }

        #region 重载

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(ConnectionString, ServerVersion.AutoDetect(ConnectionString), p => p.CommandTimeout(GlobalContext.SystemConfig.DBCommandTimeout));
            optionsBuilder.AddInterceptors(new DbCommandCustomInterceptor());
            optionsBuilder.UseLoggerFactory(_loggerFactory);
            // 这里需要注意，不能采用这种写法：optionsBuilder.UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole()));
            // 会导致内存泄露的问题
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            Assembly entityAssembly = Assembly.Load(new AssemblyName("YiSha.Entity"));
            IEnumerable<Type> typesToRegister = entityAssembly.GetTypes().Where(p => !string.IsNullOrEmpty(p.Namespace))
                                                                         .Where(p => !string.IsNullOrEmpty(p.GetCustomAttribute<TableAttribute>()?.Name));
            foreach (Type type in typesToRegister)
            {
                dynamic configurationInstance = Activator.CreateInstance(type);
                modelBuilder.Model.AddEntityType(type);
            }
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                PrimaryKeyConvention.SetPrimaryKey(modelBuilder, entity.Name);
                string currentTableName = modelBuilder.Entity(entity.Name).Metadata.GetTableName();
                modelBuilder.Entity(entity.Name).ToTable(currentTableName);

                //var properties = entity.GetProperties();
                //foreach (var property in properties)
                //{
                //    ColumnConvention.SetColumnName(modelBuilder, entity.Name, property.Name);
                //}
            }

            base.OnModelCreating(modelBuilder);
        }

        #endregion
    }
}