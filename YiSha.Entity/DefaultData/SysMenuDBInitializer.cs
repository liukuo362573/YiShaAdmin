using YiSha.Entity.Models;
using YiSha.Util;

namespace YiSha.Entity.DefaultData
{
    /// <summary>
    /// 菜单表数据初始化
    /// </summary>
    internal class SysMenuDBInitializer
    {
        /// <summary>
        /// 获取数据
        /// </summary>
        public static List<SysMenu> GetData
        {
            get
            {
                var lists = new List<SysMenu>();

                lists.Add(new SysMenu
                {
                    Id = 16508640061130069,
                    BaseIsDelete = 0,
                    BaseCreateTime = "2022-01-01 00:00:00".ToDate(),
                    BaseModifyTime = "2022-01-01 00:00:00".ToDate(),
                    BaseCreatorId = 0,
                    BaseModifierId = 0,
                    BaseVersion = 0,
                    ParentId = 0,
                    MenuName = "单位组织",
                    MenuIcon = "fa fa-home",
                    MenuUrl = "",
                    MenuTarget = "",
                    MenuSort = 1,
                    MenuType = 1,
                    MenuStatus = 1,
                    Authorize = "",
                    Remark = "",
                });

                lists.Add(new SysMenu
                {
                    Id = 16508640061130070,
                    BaseIsDelete = 0,
                    BaseCreateTime = "2022-01-01 00:00:00".ToDate(),
                    BaseModifyTime = "2022-01-01 00:00:00".ToDate(),
                    BaseCreatorId = 0,
                    BaseModifierId = 0,
                    BaseVersion = 0,
                    ParentId = 0,
                    MenuName = "系统管理",
                    MenuIcon = "fa fa-gear",
                    MenuUrl = "",
                    MenuTarget = "",
                    MenuSort = 2,
                    MenuType = 1,
                    MenuStatus = 1,
                    Authorize = "",
                    Remark = "",
                });

                lists.Add(new SysMenu
                {
                    Id = 16508640061130071,
                    BaseIsDelete = 0,
                    BaseCreateTime = "2022-01-01 00:00:00".ToDate(),
                    BaseModifyTime = "2022-01-01 00:00:00".ToDate(),
                    BaseCreatorId = 0,
                    BaseModifierId = 0,
                    BaseVersion = 0,
                    ParentId = 0,
                    MenuName = "系统工具",
                    MenuIcon = "fa fa-gears",
                    MenuUrl = "",
                    MenuTarget = "",
                    MenuSort = 3,
                    MenuType = 1,
                    MenuStatus = 1,
                    Authorize = "",
                    Remark = "",
                });

                lists.Add(new SysMenu
                {
                    Id = 16508640061130072,
                    BaseIsDelete = 0,
                    BaseCreateTime = "2022-01-01 00:00:00".ToDate(),
                    BaseModifyTime = "2022-01-01 00:00:00".ToDate(),
                    BaseCreatorId = 0,
                    BaseModifierId = 16508640061130151,
                    BaseVersion = 0,
                    ParentId = 16508640061130069,
                    MenuName = "员工管理",
                    MenuIcon = "",
                    MenuUrl = "OrganizationManage/User/UserIndex",
                    MenuTarget = "",
                    MenuSort = 1,
                    MenuType = 2,
                    MenuStatus = 1,
                    Authorize = "organization:user:view",
                    Remark = "",
                });

                lists.Add(new SysMenu
                {
                    Id = 16508640061130073,
                    BaseIsDelete = 0,
                    BaseCreateTime = "2022-01-01 00:00:00".ToDate(),
                    BaseModifyTime = "2022-01-01 00:00:00".ToDate(),
                    BaseCreatorId = 0,
                    BaseModifierId = 0,
                    BaseVersion = 0,
                    ParentId = 16508640061130069,
                    MenuName = "部门管理",
                    MenuIcon = "",
                    MenuUrl = "OrganizationManage/Department/DepartmentIndex",
                    MenuTarget = "",
                    MenuSort = 2,
                    MenuType = 2,
                    MenuStatus = 1,
                    Authorize = "organization:department:view",
                    Remark = "",
                });

                lists.Add(new SysMenu
                {
                    Id = 16508640061130074,
                    BaseIsDelete = 0,
                    BaseCreateTime = "2022-01-01 00:00:00".ToDate(),
                    BaseModifyTime = "2022-01-01 00:00:00".ToDate(),
                    BaseCreatorId = 0,
                    BaseModifierId = 0,
                    BaseVersion = 0,
                    ParentId = 16508640061130070,
                    MenuName = "角色管理",
                    MenuIcon = "",
                    MenuUrl = "SystemManage/Role/RoleIndex",
                    MenuTarget = "",
                    MenuSort = 1,
                    MenuType = 2,
                    MenuStatus = 1,
                    Authorize = "system:role:view",
                    Remark = ""
                });

                lists.Add(new SysMenu
                {
                    Id = 16508640061130075,
                    BaseIsDelete = 0,
                    BaseCreateTime = "2022-01-01 00:00:00".ToDate(),
                    BaseModifyTime = "2022-01-01 00:00:00".ToDate(),
                    BaseCreatorId = 0,
                    BaseModifierId = 0,
                    BaseVersion = 0,
                    ParentId = 16508640061130070,
                    MenuName = "菜单管理",
                    MenuIcon = "",
                    MenuUrl = "SystemManage/Menu/MenuIndex",
                    MenuTarget = "",
                    MenuSort = 2,
                    MenuType = 2,
                    MenuStatus = 1,
                    Authorize = "system:menu:view",
                    Remark = ""
                });

                lists.Add(new SysMenu
                {
                    Id = 16508640061130076,
                    BaseIsDelete = 0,
                    BaseCreateTime = "2022-01-01 00:00:00".ToDate(),
                    BaseModifyTime = "2022-01-01 00:00:00".ToDate(),
                    BaseCreatorId = 0,
                    BaseModifierId = 0,
                    BaseVersion = 0,
                    ParentId = 16508640061130070,
                    MenuName = "系统日志",
                    MenuIcon = "",
                    MenuUrl = "",
                    MenuTarget = "",
                    MenuSort = 10,
                    MenuType = 1,
                    MenuStatus = 1,
                    Authorize = "",
                    Remark = ""
                });

                lists.Add(new SysMenu
                {
                    Id = 16508640061130077,
                    BaseIsDelete = 0,
                    BaseCreateTime = "2022-01-01 00:00:00".ToDate(),
                    BaseModifyTime = "2022-01-01 00:00:00".ToDate(),
                    BaseCreatorId = 0,
                    BaseModifierId = 0,
                    BaseVersion = 0,
                    ParentId = 16508640061130070,
                    MenuName = "通用字典",
                    MenuIcon = "",
                    MenuUrl = "SystemManage/DataDict/DataDictIndex",
                    MenuTarget = "",
                    MenuSort = 5,
                    MenuType = 2,
                    MenuStatus = 1,
                    Authorize = "system:datadict:view",
                    Remark = ""
                });

                lists.Add(new SysMenu
                {
                    Id = 16508640061130078,
                    BaseIsDelete = 0,
                    BaseCreateTime = "2022-01-01 00:00:00".ToDate(),
                    BaseModifyTime = "2022-01-01 00:00:00".ToDate(),
                    BaseCreatorId = 0,
                    BaseModifierId = 0,
                    BaseVersion = 0,
                    ParentId = 16508640061130070,
                    MenuName = "行政区划",
                    MenuIcon = "",
                    MenuUrl = "SystemManage/Area/AreaIndex",
                    MenuTarget = "",
                    MenuSort = 7,
                    MenuType = 2,
                    MenuStatus = 1,
                    Authorize = "system:area:view",
                    Remark = ""
                });

                lists.Add(new SysMenu
                {
                    Id = 16508640061130079,
                    BaseIsDelete = 0,
                    BaseCreateTime = "2022-01-01 00:00:00".ToDate(),
                    BaseModifyTime = "2022-01-01 00:00:00".ToDate(),
                    BaseCreatorId = 0,
                    BaseModifierId = 0,
                    BaseVersion = 0,
                    ParentId = 16508640061130070,
                    MenuName = "数据表管理",
                    MenuIcon = "",
                    MenuUrl = "SystemManage/Database/DatatableIndex",
                    MenuTarget = "",
                    MenuSort = 14,
                    MenuType = 2,
                    MenuStatus = 1,
                    Authorize = "system:datatable:view",
                    Remark = ""
                });

                lists.Add(new SysMenu
                {
                    Id = 16508640061130080,
                    BaseIsDelete = 1,
                    BaseCreateTime = "2022-01-01 00:00:00".ToDate(),
                    BaseModifyTime = "2022-01-01 00:00:00".ToDate(),
                    BaseCreatorId = 0,
                    BaseModifierId = 0,
                    BaseVersion = 0,
                    ParentId = 16508640061130071,
                    MenuName = "代码生成",
                    MenuIcon = "",
                    MenuUrl = "ToolManage/CodeGenerator/CodeGeneratorIndex",
                    MenuTarget = "",
                    MenuSort = 1,
                    MenuType = 2,
                    MenuStatus = 1,
                    Authorize = "tool:codegenerator:view",
                    Remark = ""
                });

                lists.Add(new SysMenu
                {
                    Id = 16508640061130081,
                    BaseIsDelete = 0,
                    BaseCreateTime = "2022-01-01 00:00:00".ToDate(),
                    BaseModifyTime = "2022-01-01 00:00:00".ToDate(),
                    BaseCreatorId = 0,
                    BaseModifierId = 0,
                    BaseVersion = 0,
                    ParentId = 16508640061130076,
                    MenuName = "操作日志",
                    MenuIcon = "",
                    MenuUrl = "SystemManage/LogOperate/LogOperateIndex",
                    MenuTarget = "",
                    MenuSort = 2,
                    MenuType = 2,
                    MenuStatus = 1,
                    Authorize = "system:logoperate:view",
                    Remark = ""
                });

                lists.Add(new SysMenu
                {
                    Id = 16508640061130082,
                    BaseIsDelete = 0,
                    BaseCreateTime = "2022-01-01 00:00:00".ToDate(),
                    BaseModifyTime = "2022-01-01 00:00:00".ToDate(),
                    BaseCreatorId = 0,
                    BaseModifierId = 0,
                    BaseVersion = 0,
                    ParentId = 16508640061130076,
                    MenuName = "登录日志",
                    MenuIcon = "",
                    MenuUrl = "SystemManage/LogLogin/LogLoginIndex",
                    MenuTarget = "",
                    MenuSort = 1,
                    MenuType = 2,
                    MenuStatus = 1,
                    Authorize = "system:loglogin:view",
                    Remark = ""
                });

                lists.Add(new SysMenu
                {
                    Id = 16508640061130083,
                    BaseIsDelete = 0,
                    BaseCreateTime = "2022-01-01 00:00:00".ToDate(),
                    BaseModifyTime = "2022-01-01 00:00:00".ToDate(),
                    BaseCreatorId = 0,
                    BaseModifierId = 16508640061130151,
                    BaseVersion = 0,
                    ParentId = 16508640061130069,
                    MenuName = "职位管理",
                    MenuIcon = "",
                    MenuUrl = "OrganizationManage/Position/PositionIndex",
                    MenuTarget = "",
                    MenuSort = 3,
                    MenuType = 2,
                    MenuStatus = 1,
                    Authorize = "organization:position:view",
                    Remark = ""
                });

                lists.Add(new SysMenu
                {
                    Id = 16508640061130084,
                    BaseIsDelete = 0,
                    BaseCreateTime = "2022-01-01 00:00:00".ToDate(),
                    BaseModifyTime = "2022-01-01 00:00:00".ToDate(),
                    BaseCreatorId = 0,
                    BaseModifierId = 16508640061130151,
                    BaseVersion = 0,
                    ParentId = 16508640061130072,
                    MenuName = "员工查询",
                    MenuIcon = "",
                    MenuUrl = "",
                    MenuTarget = "",
                    MenuSort = 1,
                    MenuType = 3,
                    MenuStatus = 1,
                    Authorize = "organization:user:search",
                    Remark = ""
                });

                lists.Add(new SysMenu
                {
                    Id = 16508640061130085,
                    BaseIsDelete = 0,
                    BaseCreateTime = "2022-01-01 00:00:00".ToDate(),
                    BaseModifyTime = "2022-01-01 00:00:00".ToDate(),
                    BaseCreatorId = 0,
                    BaseModifierId = 16508640061130151,
                    BaseVersion = 0,
                    ParentId = 16508640061130072,
                    MenuName = "员工新增",
                    MenuIcon = "",
                    MenuUrl = "",
                    MenuTarget = "",
                    MenuSort = 2,
                    MenuType = 3,
                    MenuStatus = 1,
                    Authorize = "organization:user:add",
                    Remark = ""
                });

                lists.Add(new SysMenu
                {
                    Id = 16508640061130086,
                    BaseIsDelete = 0,
                    BaseCreateTime = "2022-01-01 00:00:00".ToDate(),
                    BaseModifyTime = "2022-01-01 00:00:00".ToDate(),
                    BaseCreatorId = 0,
                    BaseModifierId = 16508640061130151,
                    BaseVersion = 0,
                    ParentId = 16508640061130072,
                    MenuName = "员工修改",
                    MenuIcon = "",
                    MenuUrl = "",
                    MenuTarget = "",
                    MenuSort = 3,
                    MenuType = 3,
                    MenuStatus = 1,
                    Authorize = "organization:user:edit",
                    Remark = ""
                });

                lists.Add(new SysMenu
                {
                    Id = 16508640061130087,
                    BaseIsDelete = 0,
                    BaseCreateTime = "2022-01-01 00:00:00".ToDate(),
                    BaseModifyTime = "2022-01-01 00:00:00".ToDate(),
                    BaseCreatorId = 0,
                    BaseModifierId = 16508640061130151,
                    BaseVersion = 0,
                    ParentId = 16508640061130072,
                    MenuName = "员工删除",
                    MenuIcon = "",
                    MenuUrl = "",
                    MenuTarget = "",
                    MenuSort = 4,
                    MenuType = 3,
                    MenuStatus = 1,
                    Authorize = "organization:user:delete",
                    Remark = ""
                });

                lists.Add(new SysMenu
                {
                    Id = 16508640061130088,
                    BaseIsDelete = 0,
                    BaseCreateTime = "2022-01-01 00:00:00".ToDate(),
                    BaseModifyTime = "2022-01-01 00:00:00".ToDate(),
                    BaseCreatorId = 0,
                    BaseModifierId = 16508640061130151,
                    BaseVersion = 0,
                    ParentId = 16508640061130072,
                    MenuName = "员工导出",
                    MenuIcon = "",
                    MenuUrl = "",
                    MenuTarget = "",
                    MenuSort = 5,
                    MenuType = 3,
                    MenuStatus = 1,
                    Authorize = "organization:user:export",
                    Remark = ""
                });

                lists.Add(new SysMenu
                {
                    Id = 16508640061130089,
                    BaseIsDelete = 0,
                    BaseCreateTime = "2022-01-01 00:00:00".ToDate(),
                    BaseModifyTime = "2022-01-01 00:00:00".ToDate(),
                    BaseCreatorId = 0,
                    BaseModifierId = 0,
                    BaseVersion = 0,
                    ParentId = 16508640061130072,
                    MenuName = "重置密码",
                    MenuIcon = "",
                    MenuUrl = "",
                    MenuTarget = "",
                    MenuSort = 6,
                    MenuType = 3,
                    MenuStatus = 1,
                    Authorize = "system:user:resetpassword",
                    Remark = ""
                });

                lists.Add(new SysMenu
                {
                    Id = 16508640061130090,
                    BaseIsDelete = 0,
                    BaseCreateTime = "2022-01-01 00:00:00".ToDate(),
                    BaseModifyTime = "2022-01-01 00:00:00".ToDate(),
                    BaseCreatorId = 0,
                    BaseModifierId = 0,
                    BaseVersion = 0,
                    ParentId = 16508640061130073,
                    MenuName = "部门查询",
                    MenuIcon = "",
                    MenuUrl = "",
                    MenuTarget = "",
                    MenuSort = 1,
                    MenuType = 3,
                    MenuStatus = 1,
                    Authorize = "organization:department:search",
                    Remark = ""
                });

                lists.Add(new SysMenu
                {
                    Id = 16508640061130091,
                    BaseIsDelete = 0,
                    BaseCreateTime = "2022-01-01 00:00:00".ToDate(),
                    BaseModifyTime = "2022-01-01 00:00:00".ToDate(),
                    BaseCreatorId = 0,
                    BaseModifierId = 0,
                    BaseVersion = 0,
                    ParentId = 16508640061130073,
                    MenuName = "部门新增",
                    MenuIcon = "",
                    MenuUrl = "",
                    MenuTarget = "",
                    MenuSort = 2,
                    MenuType = 3,
                    MenuStatus = 1,
                    Authorize = "organization:department:add",
                    Remark = ""
                });

                lists.Add(new SysMenu
                {
                    Id = 16508640061130092,
                    BaseIsDelete = 0,
                    BaseCreateTime = "2022-01-01 00:00:00".ToDate(),
                    BaseModifyTime = "2022-01-01 00:00:00".ToDate(),
                    BaseCreatorId = 0,
                    BaseModifierId = 0,
                    BaseVersion = 0,
                    ParentId = 16508640061130073,
                    MenuName = "部门修改",
                    MenuIcon = "",
                    MenuUrl = "",
                    MenuTarget = "",
                    MenuSort = 3,
                    MenuType = 3,
                    MenuStatus = 1,
                    Authorize = "organization:department:edit",
                    Remark = ""
                });

                lists.Add(new SysMenu
                {
                    Id = 16508640061130093,
                    BaseIsDelete = 0,
                    BaseCreateTime = "2022-01-01 00:00:00".ToDate(),
                    BaseModifyTime = "2022-01-01 00:00:00".ToDate(),
                    BaseCreatorId = 0,
                    BaseModifierId = 0,
                    BaseVersion = 0,
                    ParentId = 16508640061130073,
                    MenuName = "部门删除",
                    MenuIcon = "",
                    MenuUrl = "",
                    MenuTarget = "",
                    MenuSort = 4,
                    MenuType = 3,
                    MenuStatus = 1,
                    Authorize = "organization:department:delete",
                    Remark = ""
                });

                lists.Add(new SysMenu
                {
                    Id = 16508640061130094,
                    BaseIsDelete = 0,
                    BaseCreateTime = "2022-01-01 00:00:00".ToDate(),
                    BaseModifyTime = "2022-01-01 00:00:00".ToDate(),
                    BaseCreatorId = 0,
                    BaseModifierId = 0,
                    BaseVersion = 0,
                    ParentId = 16508640061130083,
                    MenuName = "职位查询",
                    MenuIcon = "",
                    MenuUrl = "",
                    MenuTarget = "",
                    MenuSort = 1,
                    MenuType = 3,
                    MenuStatus = 1,
                    Authorize = "organization:position:search",
                    Remark = ""
                });

                lists.Add(new SysMenu
                {
                    Id = 16508640061130095,
                    BaseIsDelete = 0,
                    BaseCreateTime = "2022-01-01 00:00:00".ToDate(),
                    BaseModifyTime = "2022-01-01 00:00:00".ToDate(),
                    BaseCreatorId = 0,
                    BaseModifierId = 0,
                    BaseVersion = 0,
                    ParentId = 16508640061130083,
                    MenuName = "职位新增",
                    MenuIcon = "",
                    MenuUrl = "",
                    MenuTarget = "",
                    MenuSort = 2,
                    MenuType = 3,
                    MenuStatus = 1,
                    Authorize = "organization:position:add",
                    Remark = ""
                });

                lists.Add(new SysMenu
                {
                    Id = 16508640061130096,
                    BaseIsDelete = 0,
                    BaseCreateTime = "2022-01-01 00:00:00".ToDate(),
                    BaseModifyTime = "2022-01-01 00:00:00".ToDate(),
                    BaseCreatorId = 0,
                    BaseModifierId = 0,
                    BaseVersion = 0,
                    ParentId = 16508640061130083,
                    MenuName = "职位修改",
                    MenuIcon = "",
                    MenuUrl = "",
                    MenuTarget = "",
                    MenuSort = 3,
                    MenuType = 3,
                    MenuStatus = 1,
                    Authorize = "organization:position:edit",
                    Remark = ""
                });

                lists.Add(new SysMenu
                {
                    Id = 16508640061130097,
                    BaseIsDelete = 0,
                    BaseCreateTime = "2022-01-01 00:00:00".ToDate(),
                    BaseModifyTime = "2022-01-01 00:00:00".ToDate(),
                    BaseCreatorId = 0,
                    BaseModifierId = 0,
                    BaseVersion = 0,
                    ParentId = 16508640061130083,
                    MenuName = "职位删除",
                    MenuIcon = "",
                    MenuUrl = "",
                    MenuTarget = "",
                    MenuSort = 4,
                    MenuType = 3,
                    MenuStatus = 1,
                    Authorize = "organization:position:delete",
                    Remark = ""
                });

                lists.Add(new SysMenu
                {
                    Id = 16508640061130098,
                    BaseIsDelete = 0,
                    BaseCreateTime = "2022-01-01 00:00:00".ToDate(),
                    BaseModifyTime = "2022-01-01 00:00:00".ToDate(),
                    BaseCreatorId = 0,
                    BaseModifierId = 0,
                    BaseVersion = 0,
                    ParentId = 16508640061130074,
                    MenuName = "角色查询",
                    MenuIcon = "",
                    MenuUrl = "",
                    MenuTarget = "",
                    MenuSort = 1,
                    MenuType = 3,
                    MenuStatus = 1,
                    Authorize = "system:role:search",
                    Remark = ""
                });

                lists.Add(new SysMenu
                {
                    Id = 16508640061130099,
                    BaseIsDelete = 0,
                    BaseCreateTime = "2022-01-01 00:00:00".ToDate(),
                    BaseModifyTime = "2022-01-01 00:00:00".ToDate(),
                    BaseCreatorId = 0,
                    BaseModifierId = 0,
                    BaseVersion = 0,
                    ParentId = 16508640061130074,
                    MenuName = "角色新增",
                    MenuIcon = "",
                    MenuUrl = "",
                    MenuTarget = "",
                    MenuSort = 2,
                    MenuType = 3,
                    MenuStatus = 1,
                    Authorize = "system:role:add",
                    Remark = ""
                });

                lists.Add(new SysMenu
                {
                    Id = 16508640061130100,
                    BaseIsDelete = 0,
                    BaseCreateTime = "2022-01-01 00:00:00".ToDate(),
                    BaseModifyTime = "2022-01-01 00:00:00".ToDate(),
                    BaseCreatorId = 0,
                    BaseModifierId = 0,
                    BaseVersion = 0,
                    ParentId = 16508640061130074,
                    MenuName = "角色修改",
                    MenuIcon = "",
                    MenuUrl = "",
                    MenuTarget = "",
                    MenuSort = 3,
                    MenuType = 3,
                    MenuStatus = 1,
                    Authorize = "system:role:edit",
                    Remark = ""
                });

                lists.Add(new SysMenu
                {
                    Id = 16508640061130101,
                    BaseIsDelete = 0,
                    BaseCreateTime = "2022-01-01 00:00:00".ToDate(),
                    BaseModifyTime = "2022-01-01 00:00:00".ToDate(),
                    BaseCreatorId = 0,
                    BaseModifierId = 0,
                    BaseVersion = 0,
                    ParentId = 16508640061130074,
                    MenuName = "角色删除",
                    MenuIcon = "",
                    MenuUrl = "",
                    MenuTarget = "",
                    MenuSort = 4,
                    MenuType = 3,
                    MenuStatus = 1,
                    Authorize = "system:role:delete",
                    Remark = ""
                });

                lists.Add(new SysMenu
                {
                    Id = 16508640061130102,
                    BaseIsDelete = 0,
                    BaseCreateTime = "2022-01-01 00:00:00".ToDate(),
                    BaseModifyTime = "2022-01-01 00:00:00".ToDate(),
                    BaseCreatorId = 0,
                    BaseModifierId = 0,
                    BaseVersion = 0,
                    ParentId = 16508640061130075,
                    MenuName = "菜单查询",
                    MenuIcon = "",
                    MenuUrl = "",
                    MenuTarget = "",
                    MenuSort = 1,
                    MenuType = 3,
                    MenuStatus = 1,
                    Authorize = "system:menu:search",
                    Remark = ""
                });

                lists.Add(new SysMenu
                {
                    Id = 16508640061130103,
                    BaseIsDelete = 0,
                    BaseCreateTime = "2022-01-01 00:00:00".ToDate(),
                    BaseModifyTime = "2022-01-01 00:00:00".ToDate(),
                    BaseCreatorId = 0,
                    BaseModifierId = 0,
                    BaseVersion = 0,
                    ParentId = 16508640061130075,
                    MenuName = "菜单新增",
                    MenuIcon = "",
                    MenuUrl = "",
                    MenuTarget = "",
                    MenuSort = 2,
                    MenuType = 3,
                    MenuStatus = 1,
                    Authorize = "system:menu:add",
                    Remark = ""
                });

                lists.Add(new SysMenu
                {
                    Id = 16508640061130104,
                    BaseIsDelete = 0,
                    BaseCreateTime = "2022-01-01 00:00:00".ToDate(),
                    BaseModifyTime = "2022-01-01 00:00:00".ToDate(),
                    BaseCreatorId = 0,
                    BaseModifierId = 0,
                    BaseVersion = 0,
                    ParentId = 16508640061130075,
                    MenuName = "菜单修改",
                    MenuIcon = "",
                    MenuUrl = "",
                    MenuTarget = "",
                    MenuSort = 3,
                    MenuType = 3,
                    MenuStatus = 1,
                    Authorize = "system:menu:edit",
                    Remark = ""
                });

                lists.Add(new SysMenu
                {
                    Id = 16508640061130105,
                    BaseIsDelete = 0,
                    BaseCreateTime = "2022-01-01 00:00:00".ToDate(),
                    BaseModifyTime = "2022-01-01 00:00:00".ToDate(),
                    BaseCreatorId = 0,
                    BaseModifierId = 0,
                    BaseVersion = 0,
                    ParentId = 16508640061130075,
                    MenuName = "菜单删除",
                    MenuIcon = "",
                    MenuUrl = "",
                    MenuTarget = "",
                    MenuSort = 4,
                    MenuType = 3,
                    MenuStatus = 1,
                    Authorize = "system:menu:delete",
                    Remark = ""
                });

                lists.Add(new SysMenu
                {
                    Id = 16508640061130106,
                    BaseIsDelete = 0,
                    BaseCreateTime = "2022-01-01 00:00:00".ToDate(),
                    BaseModifyTime = "2022-01-01 00:00:00".ToDate(),
                    BaseCreatorId = 0,
                    BaseModifierId = 0,
                    BaseVersion = 0,
                    ParentId = 16508640061130077,
                    MenuName = "字典查询",
                    MenuIcon = "",
                    MenuUrl = "",
                    MenuTarget = "",
                    MenuSort = 1,
                    MenuType = 3,
                    MenuStatus = 1,
                    Authorize = "system:datadict:search",
                    Remark = ""
                });

                lists.Add(new SysMenu
                {
                    Id = 16508640061130107,
                    BaseIsDelete = 0,
                    BaseCreateTime = "2022-01-01 00:00:00".ToDate(),
                    BaseModifyTime = "2022-01-01 00:00:00".ToDate(),
                    BaseCreatorId = 0,
                    BaseModifierId = 0,
                    BaseVersion = 0,
                    ParentId = 16508640061130077,
                    MenuName = "字典新增",
                    MenuIcon = "",
                    MenuUrl = "",
                    MenuTarget = "",
                    MenuSort = 2,
                    MenuType = 3,
                    MenuStatus = 1,
                    Authorize = "system:datadict:add",
                    Remark = ""
                });

                lists.Add(new SysMenu
                {
                    Id = 16508640061130108,
                    BaseIsDelete = 0,
                    BaseCreateTime = "2022-01-01 00:00:00".ToDate(),
                    BaseModifyTime = "2022-01-01 00:00:00".ToDate(),
                    BaseCreatorId = 0,
                    BaseModifierId = 0,
                    BaseVersion = 0,
                    ParentId = 16508640061130077,
                    MenuName = "字典修改",
                    MenuIcon = "",
                    MenuUrl = "",
                    MenuTarget = "",
                    MenuSort = 3,
                    MenuType = 3,
                    MenuStatus = 1,
                    Authorize = "system:datadict:edit",
                    Remark = ""
                });

                lists.Add(new SysMenu
                {
                    Id = 16508640061130109,
                    BaseIsDelete = 0,
                    BaseCreateTime = "2022-01-01 00:00:00".ToDate(),
                    BaseModifyTime = "2022-01-01 00:00:00".ToDate(),
                    BaseCreatorId = 0,
                    BaseModifierId = 0,
                    BaseVersion = 0,
                    ParentId = 16508640061130077,
                    MenuName = "字典删除",
                    MenuIcon = "",
                    MenuUrl = "",
                    MenuTarget = "",
                    MenuSort = 4,
                    MenuType = 3,
                    MenuStatus = 1,
                    Authorize = "system:datadict:delete",
                    Remark = ""
                });

                lists.Add(new SysMenu
                {
                    Id = 16508640061130110,
                    BaseIsDelete = 0,
                    BaseCreateTime = "2022-01-01 00:00:00".ToDate(),
                    BaseModifyTime = "2022-01-01 00:00:00".ToDate(),
                    BaseCreatorId = 0,
                    BaseModifierId = 0,
                    BaseVersion = 0,
                    ParentId = 16508640061130078,
                    MenuName = "地区查询",
                    MenuIcon = "",
                    MenuUrl = "",
                    MenuTarget = "",
                    MenuSort = 1,
                    MenuType = 3,
                    MenuStatus = 1,
                    Authorize = "system:area:search",
                    Remark = ""
                });

                lists.Add(new SysMenu
                {
                    Id = 16508640061130111,
                    BaseIsDelete = 0,
                    BaseCreateTime = "2022-01-01 00:00:00".ToDate(),
                    BaseModifyTime = "2022-01-01 00:00:00".ToDate(),
                    BaseCreatorId = 0,
                    BaseModifierId = 0,
                    BaseVersion = 0,
                    ParentId = 16508640061130078,
                    MenuName = "地区新增",
                    MenuIcon = "",
                    MenuUrl = "",
                    MenuTarget = "",
                    MenuSort = 2,
                    MenuType = 3,
                    MenuStatus = 1,
                    Authorize = "system:area:add",
                    Remark = ""
                });

                lists.Add(new SysMenu
                {
                    Id = 16508640061130112,
                    BaseIsDelete = 0,
                    BaseCreateTime = "2022-01-01 00:00:00".ToDate(),
                    BaseModifyTime = "2022-01-01 00:00:00".ToDate(),
                    BaseCreatorId = 0,
                    BaseModifierId = 0,
                    BaseVersion = 0,
                    ParentId = 16508640061130078,
                    MenuName = "地区修改",
                    MenuIcon = "",
                    MenuUrl = "",
                    MenuTarget = "",
                    MenuSort = 3,
                    MenuType = 3,
                    MenuStatus = 1,
                    Authorize = "system:area:edit",
                    Remark = ""
                });

                lists.Add(new SysMenu
                {
                    Id = 16508640061130113,
                    BaseIsDelete = 0,
                    BaseCreateTime = "2022-01-01 00:00:00".ToDate(),
                    BaseModifyTime = "2022-01-01 00:00:00".ToDate(),
                    BaseCreatorId = 0,
                    BaseModifierId = 0,
                    BaseVersion = 0,
                    ParentId = 16508640061130078,
                    MenuName = "地区删除",
                    MenuIcon = "",
                    MenuUrl = "",
                    MenuTarget = "",
                    MenuSort = 4,
                    MenuType = 3,
                    MenuStatus = 1,
                    Authorize = "system:area:delete",
                    Remark = ""
                });

                lists.Add(new SysMenu
                {
                    Id = 16508640061130114,
                    BaseIsDelete = 0,
                    BaseCreateTime = "2022-01-01 00:00:00".ToDate(),
                    BaseModifyTime = "2022-01-01 00:00:00".ToDate(),
                    BaseCreatorId = 0,
                    BaseModifierId = 0,
                    BaseVersion = 0,
                    ParentId = 16508640061130082,
                    MenuName = "登录日志查询",
                    MenuIcon = "",
                    MenuUrl = "",
                    MenuTarget = "",
                    MenuSort = 1,
                    MenuType = 3,
                    MenuStatus = 1,
                    Authorize = "system:loglogin:search",
                    Remark = ""
                });

                lists.Add(new SysMenu
                {
                    Id = 16508640061130115,
                    BaseIsDelete = 0,
                    BaseCreateTime = "2022-01-01 00:00:00".ToDate(),
                    BaseModifyTime = "2022-01-01 00:00:00".ToDate(),
                    BaseCreatorId = 0,
                    BaseModifierId = 0,
                    BaseVersion = 0,
                    ParentId = 16508640061130082,
                    MenuName = "登录日志删除",
                    MenuIcon = "",
                    MenuUrl = "",
                    MenuTarget = "",
                    MenuSort = 2,
                    MenuType = 3,
                    MenuStatus = 1,
                    Authorize = "system:loglogin:delete",
                    Remark = ""
                });

                lists.Add(new SysMenu
                {
                    Id = 16508640061130116,
                    BaseIsDelete = 0,
                    BaseCreateTime = "2022-01-01 00:00:00".ToDate(),
                    BaseModifyTime = "2022-01-01 00:00:00".ToDate(),
                    BaseCreatorId = 0,
                    BaseModifierId = 0,
                    BaseVersion = 0,
                    ParentId = 16508640061130081,
                    MenuName = "操作日志查询",
                    MenuIcon = "",
                    MenuUrl = "",
                    MenuTarget = "",
                    MenuSort = 1,
                    MenuType = 3,
                    MenuStatus = 1,
                    Authorize = "system:logoperate:search",
                    Remark = ""
                });

                lists.Add(new SysMenu
                {
                    Id = 16508640061130117,
                    BaseIsDelete = 0,
                    BaseCreateTime = "2022-01-01 00:00:00".ToDate(),
                    BaseModifyTime = "2022-01-01 00:00:00".ToDate(),
                    BaseCreatorId = 0,
                    BaseModifierId = 0,
                    BaseVersion = 0,
                    ParentId = 16508640061130081,
                    MenuName = "操作日志删除",
                    MenuIcon = "",
                    MenuUrl = "",
                    MenuTarget = "",
                    MenuSort = 2,
                    MenuType = 3,
                    MenuStatus = 1,
                    Authorize = "system:logoperate:delete",
                    Remark = ""
                });

                lists.Add(new SysMenu
                {
                    Id = 16508640061130118,
                    BaseIsDelete = 0,
                    BaseCreateTime = "2022-01-01 00:00:00".ToDate(),
                    BaseModifyTime = "2022-01-01 00:00:00".ToDate(),
                    BaseCreatorId = 0,
                    BaseModifierId = 0,
                    BaseVersion = 0,
                    ParentId = 16508640061130079,
                    MenuName = "数据表查询",
                    MenuIcon = "",
                    MenuUrl = "",
                    MenuTarget = "",
                    MenuSort = 1,
                    MenuType = 3,
                    MenuStatus = 1,
                    Authorize = "system:datatable:search",
                    Remark = ""
                });

                lists.Add(new SysMenu
                {
                    Id = 16508640061130119,
                    BaseIsDelete = 0,
                    BaseCreateTime = "2022-01-01 00:00:00".ToDate(),
                    BaseModifyTime = "2022-01-01 00:00:00".ToDate(),
                    BaseCreatorId = 0,
                    BaseModifierId = 0,
                    BaseVersion = 0,
                    ParentId = 16508640061130080,
                    MenuName = "代码生成新增",
                    MenuIcon = "",
                    MenuUrl = "",
                    MenuTarget = "",
                    MenuSort = 2,
                    MenuType = 3,
                    MenuStatus = 1,
                    Authorize = "tool:codegenerator:add",
                    Remark = ""
                });

                lists.Add(new SysMenu
                {
                    Id = 16508640061130120,
                    BaseIsDelete = 0,
                    BaseCreateTime = "2022-01-01 00:00:00".ToDate(),
                    BaseModifyTime = "2022-01-01 00:00:00".ToDate(),
                    BaseCreatorId = 0,
                    BaseModifierId = 0,
                    BaseVersion = 0,
                    ParentId = 16508640061130080,
                    MenuName = "代码生成查询",
                    MenuIcon = "",
                    MenuUrl = "",
                    MenuTarget = "",
                    MenuSort = 1,
                    MenuType = 3,
                    MenuStatus = 1,
                    Authorize = "tool:codegenerator:search",
                    Remark = ""
                });

                lists.Add(new SysMenu
                {
                    Id = 16508640061130121,
                    BaseIsDelete = 0,
                    BaseCreateTime = "2022-01-01 00:00:00".ToDate(),
                    BaseModifyTime = "2022-01-01 00:00:00".ToDate(),
                    BaseCreatorId = 0,
                    BaseModifierId = 16508640061130151,
                    BaseVersion = 0,
                    ParentId = 16508640061130071,
                    MenuName = "服务器信息",
                    MenuIcon = "",
                    MenuUrl = "ToolManage/Server/ServerIndex",
                    MenuTarget = "",
                    MenuSort = 15,
                    MenuType = 2,
                    MenuStatus = 1,
                    Authorize = "tool:server:view",
                    Remark = ""
                });

                lists.Add(new SysMenu
                {
                    Id = 16508640061130122,
                    BaseIsDelete = 0,
                    BaseCreateTime = "2022-01-01 00:00:00".ToDate(),
                    BaseModifyTime = "2022-01-01 00:00:00".ToDate(),
                    BaseCreatorId = 0,
                    BaseModifierId = 0,
                    BaseVersion = 0,
                    ParentId = 16508640061130070,
                    MenuName = "定时任务",
                    MenuIcon = "",
                    MenuUrl = "SystemManage/AutoJob/AutoJobIndex",
                    MenuTarget = "",
                    MenuSort = 12,
                    MenuType = 2,
                    MenuStatus = 1,
                    Authorize = "system:autojob:view",
                    Remark = ""
                });

                lists.Add(new SysMenu
                {
                    Id = 16508640061130123,
                    BaseIsDelete = 0,
                    BaseCreateTime = "2022-01-01 00:00:00".ToDate(),
                    BaseModifyTime = "2022-01-01 00:00:00".ToDate(),
                    BaseCreatorId = 0,
                    BaseModifierId = 0,
                    BaseVersion = 0,
                    ParentId = 16508640061130122,
                    MenuName = "定时任务查询",
                    MenuIcon = "",
                    MenuUrl = "",
                    MenuTarget = "",
                    MenuSort = 1,
                    MenuType = 3,
                    MenuStatus = 1,
                    Authorize = "system:autojob:search",
                    Remark = ""
                });

                lists.Add(new SysMenu
                {
                    Id = 16508640061130124,
                    BaseIsDelete = 0,
                    BaseCreateTime = "2022-01-01 00:00:00".ToDate(),
                    BaseModifyTime = "2022-01-01 00:00:00".ToDate(),
                    BaseCreatorId = 0,
                    BaseModifierId = 0,
                    BaseVersion = 0,
                    ParentId = 16508640061130122,
                    MenuName = "定时任务新增",
                    MenuIcon = "",
                    MenuUrl = "",
                    MenuTarget = "",
                    MenuSort = 2,
                    MenuType = 3,
                    MenuStatus = 1,
                    Authorize = "system:autojob:add",
                    Remark = ""
                });

                lists.Add(new SysMenu
                {
                    Id = 16508640061130125,
                    BaseIsDelete = 0,
                    BaseCreateTime = "2022-01-01 00:00:00".ToDate(),
                    BaseModifyTime = "2022-01-01 00:00:00".ToDate(),
                    BaseCreatorId = 0,
                    BaseModifierId = 0,
                    BaseVersion = 0,
                    ParentId = 16508640061130122,
                    MenuName = "定时任务修改",
                    MenuIcon = "",
                    MenuUrl = "",
                    MenuTarget = "",
                    MenuSort = 3,
                    MenuType = 3,
                    MenuStatus = 1,
                    Authorize = "system:autojob:edit",
                    Remark = ""
                });

                lists.Add(new SysMenu
                {
                    Id = 16508640061130126,
                    BaseIsDelete = 0,
                    BaseCreateTime = "2022-01-01 00:00:00".ToDate(),
                    BaseModifyTime = "2022-01-01 00:00:00".ToDate(),
                    BaseCreatorId = 0,
                    BaseModifierId = 0,
                    BaseVersion = 0,
                    ParentId = 16508640061130122,
                    MenuName = "定时任务删除",
                    MenuIcon = "",
                    MenuUrl = "",
                    MenuTarget = "",
                    MenuSort = 4,
                    MenuType = 3,
                    MenuStatus = 1,
                    Authorize = "system:autojob:delete",
                    Remark = ""
                });

                lists.Add(new SysMenu
                {
                    Id = 16508640061130127,
                    BaseIsDelete = 0,
                    BaseCreateTime = "2022-01-01 00:00:00".ToDate(),
                    BaseModifyTime = "2022-01-01 00:00:00".ToDate(),
                    BaseCreatorId = 0,
                    BaseModifierId = 0,
                    BaseVersion = 0,
                    ParentId = 16508640061130122,
                    MenuName = "定时任务暂停",
                    MenuIcon = "",
                    MenuUrl = "",
                    MenuTarget = "",
                    MenuSort = 5,
                    MenuType = 3,
                    MenuStatus = 1,
                    Authorize = "system:autojob:pause",
                    Remark = ""
                });

                lists.Add(new SysMenu
                {
                    Id = 16508640061130128,
                    BaseIsDelete = 0,
                    BaseCreateTime = "2022-01-01 00:00:00".ToDate(),
                    BaseModifyTime = "2022-01-01 00:00:00".ToDate(),
                    BaseCreatorId = 0,
                    BaseModifierId = 0,
                    BaseVersion = 0,
                    ParentId = 16508640061130122,
                    MenuName = "定时任务日志查看",
                    MenuIcon = "",
                    MenuUrl = "",
                    MenuTarget = "",
                    MenuSort = 6,
                    MenuType = 3,
                    MenuStatus = 1,
                    Authorize = "system:autojob:logview",
                    Remark = ""
                });

                lists.Add(new SysMenu
                {
                    Id = 16508640061130129,
                    BaseIsDelete = 0,
                    BaseCreateTime = "2022-01-01 00:00:00".ToDate(),
                    BaseModifyTime = "2022-01-01 00:00:00".ToDate(),
                    BaseCreatorId = 0,
                    BaseModifierId = 0,
                    BaseVersion = 0,
                    ParentId = 16508640061130069,
                    MenuName = "文章中心",
                    MenuIcon = "",
                    MenuUrl = "OrganizationManage/News/NewsIndex",
                    MenuTarget = "",
                    MenuSort = 4,
                    MenuType = 2,
                    MenuStatus = 1,
                    Authorize = "organization:news:view",
                    Remark = ""
                });

                lists.Add(new SysMenu
                {
                    Id = 16508640061130130,
                    BaseIsDelete = 0,
                    BaseCreateTime = "2022-01-01 00:00:00".ToDate(),
                    BaseModifyTime = "2022-01-01 00:00:00".ToDate(),
                    BaseCreatorId = 0,
                    BaseModifierId = 0,
                    BaseVersion = 0,
                    ParentId = 16508640061130129,
                    MenuName = "文章查看",
                    MenuIcon = "",
                    MenuUrl = "",
                    MenuTarget = "",
                    MenuSort = 1,
                    MenuType = 3,
                    MenuStatus = 1,
                    Authorize = "organization:news:search",
                    Remark = ""
                });

                lists.Add(new SysMenu
                {
                    Id = 16508640061130131,
                    BaseIsDelete = 0,
                    BaseCreateTime = "2022-01-01 00:00:00".ToDate(),
                    BaseModifyTime = "2022-01-01 00:00:00".ToDate(),
                    BaseCreatorId = 0,
                    BaseModifierId = 0,
                    BaseVersion = 0,
                    ParentId = 16508640061130129,
                    MenuName = "文章新增",
                    MenuIcon = "",
                    MenuUrl = "",
                    MenuTarget = "",
                    MenuSort = 2,
                    MenuType = 3,
                    MenuStatus = 1,
                    Authorize = "organization:news:add",
                    Remark = ""
                });

                lists.Add(new SysMenu
                {
                    Id = 16508640061130132,
                    BaseIsDelete = 0,
                    BaseCreateTime = "2022-01-01 00:00:00".ToDate(),
                    BaseModifyTime = "2022-01-01 00:00:00".ToDate(),
                    BaseCreatorId = 0,
                    BaseModifierId = 0,
                    BaseVersion = 0,
                    ParentId = 16508640061130129,
                    MenuName = "文章修改",
                    MenuIcon = "",
                    MenuUrl = "",
                    MenuTarget = "",
                    MenuSort = 3,
                    MenuType = 3,
                    MenuStatus = 1,
                    Authorize = "organization:news:edit",
                    Remark = ""
                });

                lists.Add(new SysMenu
                {
                    Id = 16508640061130133,
                    BaseIsDelete = 0,
                    BaseCreateTime = "2022-01-01 00:00:00".ToDate(),
                    BaseModifyTime = "2022-01-01 00:00:00".ToDate(),
                    BaseCreatorId = 0,
                    BaseModifierId = 0,
                    BaseVersion = 0,
                    ParentId = 16508640061130129,
                    MenuName = "文章删除",
                    MenuIcon = "",
                    MenuUrl = "",
                    MenuTarget = "",
                    MenuSort = 4,
                    MenuType = 3,
                    MenuStatus = 1,
                    Authorize = "organization:news:delete",
                    Remark = ""
                });

                lists.Add(new SysMenu
                {
                    Id = 16508640061130134,
                    BaseIsDelete = 0,
                    BaseCreateTime = "2022-01-01 00:00:00".ToDate(),
                    BaseModifyTime = "2022-01-01 00:00:00".ToDate(),
                    BaseCreatorId = 0,
                    BaseModifierId = 16508640061130151,
                    BaseVersion = 0,
                    ParentId = 16508640061130070,
                    MenuName = "系统api",
                    MenuIcon = "",
                    MenuUrl = "http://localhost:5001/api/api-doc",
                    MenuTarget = "",
                    MenuSort = 13,
                    MenuType = 2,
                    MenuStatus = 1,
                    Authorize = "system:api:view",
                    Remark = ""
                });

                lists.Add(new SysMenu
                {
                    Id = 16508640061130135,
                    BaseIsDelete = 0,
                    BaseCreateTime = "2022-01-01 00:00:00".ToDate(),
                    BaseModifyTime = "2022-01-01 00:00:00".ToDate(),
                    BaseCreatorId = 0,
                    BaseModifierId = 0,
                    BaseVersion = 0,
                    ParentId = 16508640061130076,
                    MenuName = "Api日志",
                    MenuIcon = "",
                    MenuUrl = "SystemManage/LogApi/LogApiIndex",
                    MenuTarget = "",
                    MenuSort = 3,
                    MenuType = 2,
                    MenuStatus = 1,
                    Authorize = "system:logapi:view",
                    Remark = ""
                });

                lists.Add(new SysMenu
                {
                    Id = 16508640061130136,
                    BaseIsDelete = 0,
                    BaseCreateTime = "2022-01-01 00:00:00".ToDate(),
                    BaseModifyTime = "2022-01-01 00:00:00".ToDate(),
                    BaseCreatorId = 0,
                    BaseModifierId = 0,
                    BaseVersion = 0,
                    ParentId = 16508640061130135,
                    MenuName = "Api日志查询",
                    MenuIcon = "",
                    MenuUrl = "",
                    MenuTarget = "",
                    MenuSort = 1,
                    MenuType = 3,
                    MenuStatus = 1,
                    Authorize = "system:logapi:search",
                    Remark = ""
                });

                lists.Add(new SysMenu
                {
                    Id = 16508640061130137,
                    BaseIsDelete = 0,
                    BaseCreateTime = "2022-01-01 00:00:00".ToDate(),
                    BaseModifyTime = "2022-01-01 00:00:00".ToDate(),
                    BaseCreatorId = 0,
                    BaseModifierId = 0,
                    BaseVersion = 0,
                    ParentId = 16508640061130135,
                    MenuName = "Api日志删除",
                    MenuIcon = "",
                    MenuUrl = "",
                    MenuTarget = "",
                    MenuSort = 2,
                    MenuType = 3,
                    MenuStatus = 1,
                    Authorize = "system:logapi:delete",
                    Remark = ""
                });

                lists.Add(new SysMenu
                {
                    Id = 112911997946826752,
                    BaseIsDelete = 0,
                    BaseCreateTime = "2022-01-01 00:00:00".ToDate(),
                    BaseModifyTime = "2022-01-01 00:00:00".ToDate(),
                    BaseCreatorId = 16508640061130151,
                    BaseModifierId = 16508640061130151,
                    BaseVersion = 0,
                    ParentId = 0,
                    MenuName = "实例演示",
                    MenuIcon = "fa fa-square-o",
                    MenuUrl = "",
                    MenuTarget = "",
                    MenuSort = 16,
                    MenuType = 1,
                    MenuStatus = 1,
                    Authorize = "",
                    Remark = ""
                });

                lists.Add(new SysMenu
                {
                    Id = 112912256928321536,
                    BaseIsDelete = 0,
                    BaseCreateTime = "2022-01-01 00:00:00".ToDate(),
                    BaseModifyTime = "2022-01-01 00:00:00".ToDate(),
                    BaseCreatorId = 16508640061130151,
                    BaseModifierId = 16508640061130151,
                    BaseVersion = 0,
                    ParentId = 113707268142272512,
                    MenuName = "按钮",
                    MenuIcon = "",
                    MenuUrl = "DemoManage/Form/Button",
                    MenuTarget = "",
                    MenuSort = 17,
                    MenuType = 2,
                    MenuStatus = 1,
                    Authorize = "",
                    Remark = ""
                });

                lists.Add(new SysMenu
                {
                    Id = 112943305909604352,
                    BaseIsDelete = 0,
                    BaseCreateTime = "2022-01-01 00:00:00".ToDate(),
                    BaseModifyTime = "2022-01-01 00:00:00".ToDate(),
                    BaseCreatorId = 16508640061130151,
                    BaseModifierId = 16508640061130151,
                    BaseVersion = 0,
                    ParentId = 113707268142272512,
                    MenuName = "栅栏",
                    MenuIcon = "",
                    MenuUrl = "DemoManage/Form/Grid",
                    MenuTarget = "",
                    MenuSort = 18,
                    MenuType = 2,
                    MenuStatus = 1,
                    Authorize = "",
                    Remark = ""
                });

                lists.Add(new SysMenu
                {
                    Id = 112943568473034752,
                    BaseIsDelete = 0,
                    BaseCreateTime = "2022-01-01 00:00:00".ToDate(),
                    BaseModifyTime = "2022-01-01 00:00:00".ToDate(),
                    BaseCreatorId = 16508640061130151,
                    BaseModifierId = 16508640061130151,
                    BaseVersion = 0,
                    ParentId = 113707268142272512,
                    MenuName = "文件上传",
                    MenuIcon = "",
                    MenuUrl = "DemoManage/Form/Upload",
                    MenuTarget = "",
                    MenuSort = 30,
                    MenuType = 2,
                    MenuStatus = 1,
                    Authorize = "",
                    Remark = ""
                });

                lists.Add(new SysMenu
                {
                    Id = 112955374490882048,
                    BaseIsDelete = 0,
                    BaseCreateTime = "2022-01-01 00:00:00".ToDate(),
                    BaseModifyTime = "2022-01-01 00:00:00".ToDate(),
                    BaseCreatorId = 16508640061130151,
                    BaseModifierId = 16508640061130151,
                    BaseVersion = 0,
                    ParentId = 113707268142272512,
                    MenuName = "下拉框",
                    MenuIcon = "",
                    MenuUrl = "DemoManage/Form/Select",
                    MenuTarget = "",
                    MenuSort = 20,
                    MenuType = 2,
                    MenuStatus = 1,
                    Authorize = "",
                    Remark = ""
                });

                lists.Add(new SysMenu
                {
                    Id = 113706370221477888,
                    BaseIsDelete = 0,
                    BaseCreateTime = "2022-01-01 00:00:00".ToDate(),
                    BaseModifyTime = "2022-01-01 00:00:00".ToDate(),
                    BaseCreatorId = 16508640061130151,
                    BaseModifierId = 16508640061130151,
                    BaseVersion = 0,
                    ParentId = 113707268142272512,
                    MenuName = "时间轴",
                    MenuIcon = "",
                    MenuUrl = "DemoManage/Form/Timeline",
                    MenuTarget = "",
                    MenuSort = 31,
                    MenuType = 2,
                    MenuStatus = 1,
                    Authorize = "",
                    Remark = ""
                });

                lists.Add(new SysMenu
                {
                    Id = 113707268142272512,
                    BaseIsDelete = 0,
                    BaseCreateTime = "2022-01-01 00:00:00".ToDate(),
                    BaseModifyTime = "2022-01-01 00:00:00".ToDate(),
                    BaseCreatorId = 16508640061130151,
                    BaseModifierId = 16508640061130151,
                    BaseVersion = 0,
                    ParentId = 112911997946826752,
                    MenuName = "表单",
                    MenuIcon = "",
                    MenuUrl = "",
                    MenuTarget = "",
                    MenuSort = 32,
                    MenuType = 1,
                    MenuStatus = 1,
                    Authorize = "",
                    Remark = ""
                });

                lists.Add(new SysMenu
                {
                    Id = 113708424717406208,
                    BaseIsDelete = 0,
                    BaseCreateTime = "2022-01-01 00:00:00".ToDate(),
                    BaseModifyTime = "2022-01-01 00:00:00".ToDate(),
                    BaseCreatorId = 16508640061130151,
                    BaseModifierId = 16508640061130151,
                    BaseVersion = 0,
                    ParentId = 113707268142272512,
                    MenuName = "卡片列表",
                    MenuIcon = "",
                    MenuUrl = "DemoManage/Form/Card",
                    MenuTarget = "",
                    MenuSort = 32,
                    MenuType = 2,
                    MenuStatus = 1,
                    Authorize = "",
                    Remark = ""
                });

                lists.Add(new SysMenu
                {
                    Id = 113733108645236736,
                    BaseIsDelete = 0,
                    BaseCreateTime = "2022-01-01 00:00:00".ToDate(),
                    BaseModifyTime = "2022-01-01 00:00:00".ToDate(),
                    BaseCreatorId = 16508640061130151,
                    BaseModifierId = 16508640061130151,
                    BaseVersion = 0,
                    ParentId = 113707268142272512,
                    MenuName = "选项卡",
                    MenuIcon = "",
                    MenuUrl = "DemoManage/Form/Tab",
                    MenuTarget = "",
                    MenuSort = 33,
                    MenuType = 2,
                    MenuStatus = 1,
                    Authorize = "",
                    Remark = ""
                });

                lists.Add(new SysMenu
                {
                    Id = 113733191331745792,
                    BaseIsDelete = 0,
                    BaseCreateTime = "2022-01-01 00:00:00".ToDate(),
                    BaseModifyTime = "2022-01-01 00:00:00".ToDate(),
                    BaseCreatorId = 16508640061130151,
                    BaseModifierId = 16508640061130151,
                    BaseVersion = 0,
                    ParentId = 113707268142272512,
                    MenuName = "面板",
                    MenuIcon = "",
                    MenuUrl = "DemoManage/Form/Panel",
                    MenuTarget = "",
                    MenuSort = 34,
                    MenuType = 2,
                    MenuStatus = 1,
                    Authorize = "",
                    Remark = ""
                });

                lists.Add(new SysMenu
                {
                    Id = 113734387522080768,
                    BaseIsDelete = 0,
                    BaseCreateTime = "2022-01-01 00:00:00".ToDate(),
                    BaseModifyTime = "2022-01-01 00:00:00".ToDate(),
                    BaseCreatorId = 16508640061130151,
                    BaseModifierId = 16508640061130151,
                    BaseVersion = 0,
                    ParentId = 113707268142272512,
                    MenuName = "日期与时间",
                    MenuIcon = "",
                    MenuUrl = "DemoManage/Form/Datetime",
                    MenuTarget = "",
                    MenuSort = 35,
                    MenuType = 2,
                    MenuStatus = 1,
                    Authorize = "",
                    Remark = ""
                });

                lists.Add(new SysMenu
                {
                    Id = 113942846855188480,
                    BaseIsDelete = 0,
                    BaseCreateTime = "2022-01-01 00:00:00".ToDate(),
                    BaseModifyTime = "2022-01-01 00:00:00".ToDate(),
                    BaseCreatorId = 16508640061130151,
                    BaseModifierId = 16508640061130151,
                    BaseVersion = 0,
                    ParentId = 112911997946826752,
                    MenuName = "图标",
                    MenuIcon = "",
                    MenuUrl = "",
                    MenuTarget = "",
                    MenuSort = 36,
                    MenuType = 1,
                    MenuStatus = 1,
                    Authorize = "",
                    Remark = ""
                });

                lists.Add(new SysMenu
                {
                    Id = 113946356757827584,
                    BaseIsDelete = 0,
                    BaseCreateTime = "2022-01-01 00:00:00".ToDate(),
                    BaseModifyTime = "2022-01-01 00:00:00".ToDate(),
                    BaseCreatorId = 16508640061130151,
                    BaseModifierId = 16508640061130151,
                    BaseVersion = 0,
                    ParentId = 113942846855188480,
                    MenuName = "FontAwesome",
                    MenuIcon = "",
                    MenuUrl = "DemoManage/Icon/FontAwesome",
                    MenuTarget = "",
                    MenuSort = 37,
                    MenuType = 2,
                    MenuStatus = 1,
                    Authorize = "",
                    Remark = ""
                });

                lists.Add(new SysMenu
                {
                    Id = 115420512615665664,
                    BaseIsDelete = 0,
                    BaseCreateTime = "2022-01-01 00:00:00".ToDate(),
                    BaseModifyTime = "2022-01-01 00:00:00".ToDate(),
                    BaseCreatorId = 16508640061130151,
                    BaseModifierId = 16508640061130151,
                    BaseVersion = 0,
                    ParentId = 113707268142272512,
                    MenuName = "富文本编辑器",
                    MenuIcon = "",
                    MenuUrl = "DemoManage/Form/Editor",
                    MenuTarget = "",
                    MenuSort = 36,
                    MenuType = 2,
                    MenuStatus = 1,
                    Authorize = "",
                    Remark = ""
                });

                lists.Add(new SysMenu
                {
                    Id = 115427643100237824,
                    BaseIsDelete = 0,
                    BaseCreateTime = "2022-01-01 00:00:00".ToDate(),
                    BaseModifyTime = "2022-01-01 00:00:00".ToDate(),
                    BaseCreatorId = 16508640061130151,
                    BaseModifierId = 16508640061130151,
                    BaseVersion = 0,
                    ParentId = 113707268142272512,
                    MenuName = "输入自动提示",
                    MenuIcon = "",
                    MenuUrl = "DemoManage/Form/AutoComplete",
                    MenuTarget = "",
                    MenuSort = 37,
                    MenuType = 2,
                    MenuStatus = 0,
                    Authorize = "",
                    Remark = ""
                });

                lists.Add(new SysMenu
                {
                    Id = 119408151295430656,
                    BaseIsDelete = 0,
                    BaseCreateTime = "2022-01-01 00:00:00".ToDate(),
                    BaseModifyTime = "2022-01-01 00:00:00".ToDate(),
                    BaseCreatorId = 16508640061130151,
                    BaseModifierId = 16508640061130151,
                    BaseVersion = 0,
                    ParentId = 112911997946826752,
                    MenuName = "报表",
                    MenuIcon = "",
                    MenuUrl = "",
                    MenuTarget = "",
                    MenuSort = 34,
                    MenuType = 1,
                    MenuStatus = 1,
                    Authorize = "",
                    Remark = ""
                });

                lists.Add(new SysMenu
                {
                    Id = 119408346968100864,
                    BaseIsDelete = 0,
                    BaseCreateTime = "2022-01-01 00:00:00".ToDate(),
                    BaseModifyTime = "2022-01-01 00:00:00".ToDate(),
                    BaseCreatorId = 16508640061130151,
                    BaseModifierId = 16508640061130151,
                    BaseVersion = 0,
                    ParentId = 119408151295430656,
                    MenuName = "ECharts",
                    MenuIcon = "",
                    MenuUrl = "DemoManage/Report/ECharts",
                    MenuTarget = "",
                    MenuSort = 37,
                    MenuType = 2,
                    MenuStatus = 1,
                    Authorize = "",
                    Remark = ""
                });

                lists.Add(new SysMenu
                {
                    Id = 119409432990846976,
                    BaseIsDelete = 0,
                    BaseCreateTime = "2022-01-01 00:00:00".ToDate(),
                    BaseModifyTime = "2022-01-01 00:00:00".ToDate(),
                    BaseCreatorId = 16508640061130151,
                    BaseModifierId = 16508640061130151,
                    BaseVersion = 0,
                    ParentId = 119408151295430656,
                    MenuName = "Peity",
                    MenuIcon = "",
                    MenuUrl = "DemoManage/Report/Peity",
                    MenuTarget = "",
                    MenuSort = 38,
                    MenuType = 2,
                    MenuStatus = 1,
                    Authorize = "",
                    Remark = ""
                });

                lists.Add(new SysMenu
                {
                    Id = 174125371522813952,
                    BaseIsDelete = 0,
                    BaseCreateTime = "2022-01-01 00:00:00".ToDate(),
                    BaseModifyTime = "2022-01-01 00:00:00".ToDate(),
                    BaseCreatorId = 16508640061130151,
                    BaseModifierId = 16508640061130151,
                    BaseVersion = 0,
                    ParentId = 112911997946826752,
                    MenuName = "表格",
                    MenuIcon = "",
                    MenuUrl = "",
                    MenuTarget = "",
                    MenuSort = 33,
                    MenuType = 1,
                    MenuStatus = 1,
                    Authorize = "",
                    Remark = ""
                });

                lists.Add(new SysMenu
                {
                    Id = 174125752109764608,
                    BaseIsDelete = 0,
                    BaseCreateTime = "2022-01-01 00:00:00".ToDate(),
                    BaseModifyTime = "2022-01-01 00:00:00".ToDate(),
                    BaseCreatorId = 16508640061130151,
                    BaseModifierId = 16508640061130151,
                    BaseVersion = 0,
                    ParentId = 174125371522813952,
                    MenuName = "表格行内编辑",
                    MenuIcon = "",
                    MenuUrl = "DemoManage/Table/Editable",
                    MenuTarget = "",
                    MenuSort = 37,
                    MenuType = 2,
                    MenuStatus = 1,
                    Authorize = "",
                    Remark = ""
                });

                lists.Add(new SysMenu
                {
                    Id = 174487294873440256,
                    BaseIsDelete = 0,
                    BaseCreateTime = "2022-01-01 00:00:00".ToDate(),
                    BaseModifyTime = "2022-01-01 00:00:00".ToDate(),
                    BaseCreatorId = 16508640061130151,
                    BaseModifierId = 16508640061130151,
                    BaseVersion = 0,
                    ParentId = 174125371522813952,
                    MenuName = "表格图片预览",
                    MenuIcon = "",
                    MenuUrl = "DemoManage/Table/Image",
                    MenuTarget = "",
                    MenuSort = 38,
                    MenuType = 2,
                    MenuStatus = 1,
                    Authorize = "",
                    Remark = ""
                });

                lists.Add(new SysMenu
                {
                    Id = 174986857728184320,
                    BaseIsDelete = 0,
                    BaseCreateTime = "2022-01-01 00:00:00".ToDate(),
                    BaseModifyTime = "2022-01-01 00:00:00".ToDate(),
                    BaseCreatorId = 16508640061130151,
                    BaseModifierId = 16508640061130151,
                    BaseVersion = 0,
                    ParentId = 174125371522813952,
                    MenuName = "数据汇总",
                    MenuIcon = "",
                    MenuUrl = "DemoManage/Table/Footer",
                    MenuTarget = "",
                    MenuSort = 1,
                    MenuType = 2,
                    MenuStatus = 1,
                    Authorize = "",
                    Remark = ""
                });

                lists.Add(new SysMenu
                {
                    Id = 174987038288777216,
                    BaseIsDelete = 0,
                    BaseCreateTime = "2022-01-01 00:00:00".ToDate(),
                    BaseModifyTime = "2022-01-01 00:00:00".ToDate(),
                    BaseCreatorId = 16508640061130151,
                    BaseModifierId = 16508640061130151,
                    BaseVersion = 0,
                    ParentId = 174125371522813952,
                    MenuName = "组合表头",
                    MenuIcon = "",
                    MenuUrl = "DemoManage/Table/GroupHeader",
                    MenuTarget = "",
                    MenuSort = 2,
                    MenuType = 2,
                    MenuStatus = 1,
                    Authorize = "",
                    Remark = ""
                });

                lists.Add(new SysMenu
                {
                    Id = 187281331867095040,
                    BaseIsDelete = 0,
                    BaseCreateTime = "2022-01-01 00:00:00".ToDate(),
                    BaseModifyTime = "2022-01-01 00:00:00".ToDate(),
                    BaseCreatorId = 16508640061130151,
                    BaseModifierId = 16508640061130151,
                    BaseVersion = 0,
                    ParentId = 174125371522813952,
                    MenuName = "多工具栏",
                    MenuIcon = "",
                    MenuUrl = "DemoManage/Table/MultiToolbar",
                    MenuTarget = "",
                    MenuSort = 39,
                    MenuType = 2,
                    MenuStatus = 1,
                    Authorize = "",
                    Remark = ""
                });

                return lists;
            }
        }
    }
}
