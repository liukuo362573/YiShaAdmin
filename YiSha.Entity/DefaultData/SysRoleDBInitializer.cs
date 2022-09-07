using YiSha.Entity.Models;
using YiSha.Util;

namespace YiSha.Entity.DefaultData
{
    /// <summary>
    /// 角色表数据初始化
    /// </summary>
    internal class SysRoleDBInitializer
    {
        /// <summary>
        /// 获取数据
        /// </summary>
        public static List<SysRole> GetData
        {
            get
            {
                var lists = new List<SysRole>();

                lists.Add(new SysRole()
                {
                    Id = 16508640061130146,
                    BaseIsDelete = 0,
                    BaseCreateTime = "2022-01-01 00:00:00".ToDate(),
                    BaseModifyTime = "2022-01-01 00:00:00".ToDate(),
                    BaseCreatorId = 0,
                    BaseModifierId = 16508640061130151,
                    BaseVersion = 0,
                    RoleName = "管理员",
                    RoleSort = 1,
                    RoleStatus = 1,
                    Remark = "管理员角色",
                });

                lists.Add(new SysRole()
                {
                    Id = 16508640061130147,
                    BaseIsDelete = 0,
                    BaseCreateTime = "2022-01-01 00:00:00".ToDate(),
                    BaseModifyTime = "2022-01-01 00:00:00".ToDate(),
                    BaseCreatorId = 0,
                    BaseModifierId = 16508640061130151,
                    BaseVersion = 0,
                    RoleName = "普通角色",
                    RoleSort = 2,
                    RoleStatus = 1,
                    Remark = "普通角色",
                });

                return lists;
            }
        }
    }
}
