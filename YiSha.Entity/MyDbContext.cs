using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YiSha.DataBase;
using YiSha.Entity.OrganizationManage;
using YiSha.Entity.SystemManage;

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
            //在此初始化数据库

            //
            base.OnModelCreating(modelBuilder);
        }

        /// <summary>
        /// 部门表
        /// </summary>
        public DbSet<DepartmentEntity> DepartmentEntity { get; set; }

        /// <summary>
        /// 新闻表
        /// </summary>
        public DbSet<NewsEntity> NewsEntity { get; set; }

        /// <summary>
        /// 职位表
        /// </summary>
        public DbSet<PositionEntity> PositionEntity { get; set; }

        /// <summary>
        /// 用户所属表
        /// </summary>
        public DbSet<UserBelongEntity> UserBelongEntity { get; set; }

        /// <summary>
        /// 用户表
        /// </summary>
        public DbSet<UserEntity> UserEntity { get; set; }

        /// <summary>
        /// 中国省市县表
        /// </summary>
        public DbSet<AreaEntity> AreaEntity { get; set; }

        /// <summary>
        /// 定时任务表
        /// </summary>
        public DbSet<AutoJobEntity> AutoJobEntity { get; set; }

        /// <summary>
        /// 定时任务组表
        /// </summary>
        public DbSet<AutoJobLogEntity> AutoJobLogEntity { get; set; }

        /// <summary>
        /// 字典数据表
        /// </summary>
        public DbSet<DataDictDetailEntity> DataDictDetailEntity { get; set; }

        /// <summary>
        /// 字典类型表
        /// </summary>
        public DbSet<DataDictEntity> DataDictEntity { get; set; }

        /// <summary>
        /// Api日志表
        /// </summary>
        public DbSet<LogApiEntity> LogApiEntity { get; set; }

        /// <summary>
        /// 登录日志表
        /// </summary>
        public DbSet<LogLoginEntity> LogLoginEntity { get; set; }

        /// <summary>
        /// 操作日志表
        /// </summary>
        public DbSet<LogOperateEntity> LogOperateEntity { get; set; }

        /// <summary>
        /// 菜单权限表
        /// </summary>
        public DbSet<MenuAuthorizeEntity> MenuAuthorizeEntity { get; set; }

        /// <summary>
        /// 菜单表
        /// </summary>
        public DbSet<MenuEntity> MenuEntity { get; set; }

        /// <summary>
        /// 角色表
        /// </summary>
        public DbSet<RoleEntity> RoleEntity { get; set; }
    }
}
