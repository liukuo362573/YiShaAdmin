using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using YiSha.Util;

namespace YiSha.Data.EF
{
    public class MySqlDbContext : DbContext
    {
        private string ConnectionString { get; }

        private ServerVersion ServerVersion { get; }

        public MySqlDbContext(string connectionString)
        {
            ConnectionString = connectionString;
            var versions = TextHelper.SplitToArray<int>(GlobalContext.SystemConfig.DBVersion, '.');
            ServerVersion = new MySqlServerVersion(new Version(versions[0], versions[1], versions[2]));
        }

        #region 重载

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(ConnectionString, ServerVersion, p => p.CommandTimeout(GlobalContext.SystemConfig.DBCommandTimeout));
            optionsBuilder.AddInterceptors(new DbCommandCustomInterceptor());
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