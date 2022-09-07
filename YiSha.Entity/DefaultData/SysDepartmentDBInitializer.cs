using YiSha.Entity.Models;
using YiSha.Util;

namespace YiSha.Entity.DefaultData
{
    /// <summary>
    /// 部门表数据初始化
    /// </summary>
    internal class SysDepartmentDBInitializer
    {
        /// <summary>
        /// 获取数据
        /// </summary>
        public static List<SysDepartment> GetData
        {
            get
            {
                var lists = new List<SysDepartment>();

                lists.Add(new SysDepartment()
                {
                    Id = 16508640061124402,
                    BaseIsDelete = 0,
                    BaseCreateTime = "2022-01-01 00:00:00".ToDate(),
                    BaseModifyTime = "2022-01-01 00:00:00".ToDate(),
                    BaseCreatorId = 0,
                    BaseModifierId = 16508640061130151,
                    BaseVersion = 0,
                    ParentId = 0,
                    DepartmentName = "一沙软件",
                    Telephone = "0551-6666666",
                    Fax = "0551-8888888",
                    Email = "",
                    Remark = "",
                    PrincipalId = 16508640061130152,
                    DepartmentSort = 1,
                });

                lists.Add(new SysDepartment()
                {
                    Id = 16508640061124403,
                    BaseIsDelete = 1,
                    BaseCreateTime = "2022-01-01 00:00:00".ToDate(),
                    BaseModifyTime = "2022-01-01 00:00:00".ToDate(),
                    BaseCreatorId = 0,
                    BaseModifierId = 16508640061130151,
                    BaseVersion = 0,
                    ParentId = 16508640061124402,
                    DepartmentName = "合肥总公司",
                    Telephone = "1",
                    Fax = "",
                    Email = "",
                    Remark = "",
                    PrincipalId = 16508640061130150,
                    DepartmentSort = 1,
                });

                lists.Add(new SysDepartment()
                {
                    Id = 16508640061124404,
                    BaseIsDelete = 1,
                    BaseCreateTime = "2022-01-01 00:00:00".ToDate(),
                    BaseModifyTime = "2022-01-01 00:00:00".ToDate(),
                    BaseCreatorId = 0,
                    BaseModifierId = 0,
                    BaseVersion = 0,
                    ParentId = 16508640061124402,
                    DepartmentName = "南京分公司",
                    Telephone = "1",
                    Fax = "",
                    Email = "",
                    Remark = "",
                    PrincipalId = 0,
                    DepartmentSort = 2,
                });

                lists.Add(new SysDepartment()
                {
                    Id = 16508640061124405,
                    BaseIsDelete = 1,
                    BaseCreateTime = "2022-01-01 00:00:00".ToDate(),
                    BaseModifyTime = "2022-01-01 00:00:00".ToDate(),
                    BaseCreatorId = 0,
                    BaseModifierId = 0,
                    BaseVersion = 0,
                    ParentId = 16508640061124403,
                    DepartmentName = "研发部",
                    Telephone = "1",
                    Fax = "",
                    Email = "",
                    Remark = "专注前端与后端结合的开发模式",
                    PrincipalId = 0,
                    DepartmentSort = 1,
                });

                lists.Add(new SysDepartment()
                {
                    Id = 16508640061124406,
                    BaseIsDelete = 1,
                    BaseCreateTime = "2022-01-01 00:00:00".ToDate(),
                    BaseModifyTime = "2022-01-01 00:00:00".ToDate(),
                    BaseCreatorId = 0,
                    BaseModifierId = 0,
                    BaseVersion = 0,
                    ParentId = 16508640061124403,
                    DepartmentName = "测试部",
                    Telephone = "1",
                    Fax = "",
                    Email = "",
                    Remark = "",
                    PrincipalId = 0,
                    DepartmentSort = 3,
                });

                lists.Add(new SysDepartment()
                {
                    Id = 16508640061124407,
                    BaseIsDelete = 1,
                    BaseCreateTime = "2022-01-01 00:00:00".ToDate(),
                    BaseModifyTime = "2022-01-01 00:00:00".ToDate(),
                    BaseCreatorId = 0,
                    BaseModifierId = 0,
                    BaseVersion = 0,
                    ParentId = 16508640061124403,
                    DepartmentName = "前端设计部",
                    Telephone = "1",
                    Fax = "",
                    Email = "",
                    Remark = "",
                    PrincipalId = 0,
                    DepartmentSort = 2,
                });

                lists.Add(new SysDepartment()
                {
                    Id = 16508640061124408,
                    BaseIsDelete = 0,
                    BaseCreateTime = "2022-01-01 00:00:00".ToDate(),
                    BaseModifyTime = "2022-01-01 00:00:00".ToDate(),
                    BaseCreatorId = 0,
                    BaseModifierId = 0,
                    BaseVersion = 0,
                    ParentId = 16508640061124403,
                    DepartmentName = "财务部",
                    Telephone = "0551-87654321",
                    Fax = "0551-12345678",
                    Email = "wangxue@yishasoft.com",
                    Remark = "2",
                    PrincipalId = 0,
                    DepartmentSort = 15,
                });

                lists.Add(new SysDepartment()
                {
                    Id = 16508640061124409,
                    BaseIsDelete = 0,
                    BaseCreateTime = "2022-01-01 00:00:00".ToDate(),
                    BaseModifyTime = "2022-01-01 00:00:00".ToDate(),
                    BaseCreatorId = 0,
                    BaseModifierId = 0,
                    BaseVersion = 0,
                    ParentId = 16508640061124403,
                    DepartmentName = "市场部",
                    Telephone = "",
                    Fax = "",
                    Email = "",
                    Remark = "",
                    PrincipalId = 0,
                    DepartmentSort = 7,
                });

                lists.Add(new SysDepartment()
                {
                    Id = 16508640061124410,
                    BaseIsDelete = 0,
                    BaseCreateTime = "2022-01-01 00:00:00".ToDate(),
                    BaseModifyTime = "2022-01-01 00:00:00".ToDate(),
                    BaseCreatorId = 0,
                    BaseModifierId = 0,
                    BaseVersion = 0,
                    ParentId = 16508640061124403,
                    DepartmentName = "行政部",
                    Telephone = "",
                    Fax = "",
                    Email = "",
                    Remark = "",
                    PrincipalId = 0,
                    DepartmentSort = 10,
                });

                return lists;
            }
        }
    }
}
