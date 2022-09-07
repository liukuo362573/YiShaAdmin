using Microsoft.EntityFrameworkCore;
using YiSha.DataBase;
using YiSha.Entity.DefaultData;
using YiSha.Entity.Models;

namespace YiSha.Entity
{
    /// <summary>
    /// 数据库上下文
    /// </summary>
    public class MyDbContext : DbCommon
    {
        /// <summary>
        /// 配置要使用的数据库
        /// </summary>
        /// <param name="optionsBuilder">上下文选项生成器</param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        /// <summary>
        /// 配置通过约定发现的模型
        /// </summary>
        /// <param name="modelBuilder">模型制作者</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //初始化数据库
            //
            //用户表初始化
            modelBuilder.Entity<SysUser>().HasData(SysUserDBInitializer.GetData);
            //用户所属表初始化
            modelBuilder.Entity<SysUserBelong>().HasData(SysUserBelongDBInitializer.GetData);
            //职位表初始化
            modelBuilder.Entity<SysPosition>().HasData(SysPositionDBInitializer.GetData);
            //部门表初始化
            modelBuilder.Entity<SysDepartment>().HasData(SysDepartmentDBInitializer.GetData);
            //角色表初始化
            modelBuilder.Entity<SysRole>().HasData(SysRoleDBInitializer.GetData);
            //菜单表初始化
            modelBuilder.Entity<SysMenu>().HasData(SysMenuDBInitializer.GetData);
            //菜单权限表初始化
            modelBuilder.Entity<SysMenuAuthorize>().HasData(SysMenuAuthorizeDBInitializer.GetData);
            //Api日志表初始化
            modelBuilder.Entity<SysLogApi>().HasData(SysLogApiDBInitializer.GetData);
            //登录日志表初始化
            modelBuilder.Entity<SysLogLogin>().HasData(SysLogLoginDBInitializer.GetData);
            //操作日志表初始化
            modelBuilder.Entity<SysLogOperate>().HasData(SysLogOperateDBInitializer.GetData);
            //中国省市县表初始化
            modelBuilder.Entity<SysArea>().HasData(SysAreaDBInitializer.GetData);
            //字典类型表初始化
            modelBuilder.Entity<SysDataDict>().HasData(SysDataDictDBInitializer.GetData);
            //字典数据表初始化
            modelBuilder.Entity<SysDataDictDetail>().HasData(SysDataDictDetailDBInitializer.GetData);
            //定时任务表初始化
            modelBuilder.Entity<SysAutoJob>().HasData(SysAutoJobDBInitializer.GetData);
            //定时任务组表初始化
            modelBuilder.Entity<SysAutoJobLog>().HasData(SysAutoJobLogDBInitializer.GetData);
            //新闻表初始化
            modelBuilder.Entity<SysNews>().HasData(SysNewsDBInitializer.GetData);
            //
            base.OnModelCreating(modelBuilder);
        }

        /// <summary>
        /// 用户表
        /// </summary>
        public DbSet<SysUser> SysUser { get; set; }

        /// <summary>
        /// 用户所属表
        /// </summary>
        public DbSet<SysUserBelong> SysUserBelong { get; set; }

        /// <summary>
        /// 职位表
        /// </summary>
        public DbSet<SysPosition> SysPosition { get; set; }

        /// <summary>
        /// 部门表
        /// </summary>
        public DbSet<SysDepartment> SysDepartment { get; set; }

        /// <summary>
        /// 角色表
        /// </summary>
        public DbSet<SysRole> SysRole { get; set; }

        /// <summary>
        /// 菜单表
        /// </summary>
        public DbSet<SysMenu> SysMenu { get; set; }

        /// <summary>
        /// 菜单权限表
        /// </summary>
        public DbSet<SysMenuAuthorize> SysMenuAuthorize { get; set; }

        /// <summary>
        /// Api日志表
        /// </summary>
        public DbSet<SysLogApi> SysLogApi { get; set; }

        /// <summary>
        /// 登录日志表
        /// </summary>
        public DbSet<SysLogLogin> SysLogLogin { get; set; }

        /// <summary>
        /// 操作日志表
        /// </summary>
        public DbSet<SysLogOperate> SysLogOperate { get; set; }

        /// <summary>
        /// 中国省市县表
        /// </summary>
        public DbSet<SysArea> SysArea { get; set; }

        /// <summary>
        /// 字典类型表
        /// </summary>
        public DbSet<SysDataDict> SysDataDict { get; set; }

        /// <summary>
        /// 字典数据表
        /// </summary>
        public DbSet<SysDataDictDetail> SysDataDictDetail { get; set; }

        /// <summary>
        /// 定时任务表
        /// </summary>
        public DbSet<SysAutoJob> SysAutoJob { get; set; }

        /// <summary>
        /// 定时任务组表
        /// </summary>
        public DbSet<SysAutoJobLog> SysAutoJobLog { get; set; }

        /// <summary>
        /// 新闻表
        /// </summary>
        public DbSet<SysNews> SysNews { get; set; }
    }
}
