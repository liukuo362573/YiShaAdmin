using YiSha.Entity.Models;
using YiSha.Util;

namespace YiSha.Entity.DefaultData
{
    /// <summary>
    /// 用户表数据初始化
    /// </summary>
    internal class SysUserDBInitializer
    {
        /// <summary>
        /// 获取数据
        /// </summary>
        public static List<SysUser> GetData
        {
            get
            {
                var lists = new List<SysUser>();

                lists.Add(new SysUser()
                {
                    Id = 16508640061130151,
                    BaseIsDelete = 0,
                    BaseCreateTime = "2022-01-01 00:00:00".ToDate(),
                    BaseModifyTime = "2022-01-01 00:00:00".ToDate(),
                    BaseCreatorId = 0,
                    BaseModifierId = 16508640061130151,
                    BaseVersion = 0,
                    UserName = "admin",
                    Password = "dd78b910e8842cf7b42b6b0522a0b0ed",
                    Salt = "28585",
                    RealName = "管理员",
                    DepartmentId = 16508640061124402,
                    Gender = 0,
                    Birthday = "2019-01-01",
                    Portrait = "",
                    Email = "admin@163.com",
                    Mobile = "15766666666",
                    QQ = "",
                    WeChat = "",
                    LoginCount = 525,
                    UserStatus = 1,
                    IsSystem = 1,
                    IsOnline = 1,
                    FirstVisit = "2022-01-01 00:00:00".ToDate(),
                    PreviousVisit = "2022-01-01 00:00:00".ToDate(),
                    LastVisit = "2022-01-01 00:00:00".ToDate(),
                    Remark = "",
                    WebToken = "265d8570ac504b588c85018c7974a431",
                    ApiToken = "a5f3d50ab2084821953d4d45925a042a"
                });

                lists.Add(new SysUser()
                {
                    Id = 16508640061130148,
                    BaseIsDelete = 0,
                    BaseCreateTime = "2022-01-01 00:00:00".ToDate(),
                    BaseModifyTime = "2022-01-01 00:00:00".ToDate(),
                    BaseCreatorId = 0,
                    BaseModifierId = 0,
                    BaseVersion = 0,
                    UserName = "lisi",
                    Password = "e0dc5bc0da011584308fdc3d5dca04a1",
                    Salt = "70212",
                    RealName = "李四",
                    DepartmentId = 181201190700000005,
                    Gender = 1,
                    Birthday = "",
                    Portrait = "",
                    Email = "",
                    Mobile = "16812345678",
                    QQ = "",
                    WeChat = "",
                    LoginCount = 0,
                    UserStatus = 2,
                    IsSystem = 0,
                    IsOnline = 0,
                    FirstVisit = "2022-01-01 00:00:00".ToDate(),
                    PreviousVisit = "2022-01-01 00:00:00".ToDate(),
                    LastVisit = "2022-01-01 00:00:00".ToDate(),
                    Remark = "",
                    WebToken = "",
                    ApiToken = ""
                });

                lists.Add(new SysUser()
                {
                    Id = 16508640061130149,
                    BaseIsDelete = 0,
                    BaseCreateTime = "2022-01-01 00:00:00".ToDate(),
                    BaseModifyTime = "2022-01-01 00:00:00".ToDate(),
                    BaseCreatorId = 0,
                    BaseModifierId = 16508640061130151,
                    BaseVersion = 0,
                    UserName = "zhangsan",
                    Password = "3",
                    Salt = "",
                    RealName = "张三",
                    DepartmentId = 16508640061124410,
                    Gender = 0,
                    Birthday = "",
                    Portrait = "",
                    Email = "",
                    Mobile = "18712345678",
                    QQ = "",
                    WeChat = "",
                    LoginCount = 0,
                    UserStatus = 1,
                    IsSystem = 0,
                    IsOnline = 0,
                    FirstVisit = "2022-01-01 00:00:00".ToDate(),
                    PreviousVisit = "2022-01-01 00:00:00".ToDate(),
                    LastVisit = "2022-01-01 00:00:00".ToDate(),
                    Remark = "",
                    WebToken = "",
                    ApiToken = ""
                });

                lists.Add(new SysUser()
                {
                    Id = 16508640061130150,
                    BaseIsDelete = 0,
                    BaseCreateTime = "2022-01-01 00:00:00".ToDate(),
                    BaseModifyTime = "2022-01-01 00:00:00".ToDate(),
                    BaseCreatorId = 0,
                    BaseModifierId = 16508640061130151,
                    BaseVersion = 0,
                    UserName = "wangxue",
                    Password = "518cd8c7e019f06ad7bf68f7532a5a73",
                    Salt = "8941",
                    RealName = "王雪",
                    DepartmentId = 16508640061124408,
                    Gender = 0,
                    Birthday = "1993-10-06",
                    Portrait = "",
                    Email = "",
                    Mobile = "15612345678",
                    QQ = "",
                    WeChat = "",
                    LoginCount = 1,
                    UserStatus = 1,
                    IsSystem = 0,
                    IsOnline = 1,
                    FirstVisit = "2022-01-01 00:00:00".ToDate(),
                    PreviousVisit = "2022-01-01 00:00:00".ToDate(),
                    LastVisit = "2022-01-01 00:00:00".ToDate(),
                    Remark = "",
                    WebToken = "a066e89f44894cb284c4dc2920234acb",
                    ApiToken = ""
                });

                lists.Add(new SysUser()
                {
                    Id = 16508640061130152,
                    BaseIsDelete = 0,
                    BaseCreateTime = "2022-01-01 00:00:00".ToDate(),
                    BaseModifyTime = "2022-01-01 00:00:00".ToDate(),
                    BaseCreatorId = 0,
                    BaseModifierId = 16508640061130151,
                    BaseVersion = 0,
                    UserName = "liusitan",
                    Password = "c84603a610bd35a1283d750334da49c7",
                    Salt = "59654",
                    RealName = "刘斯坦",
                    DepartmentId = 16508640061124405,
                    Gender = 1,
                    Birthday = "1987-12-17",
                    Portrait = "",
                    Email = "test@163.com",
                    Mobile = "15733333333",
                    QQ = "",
                    WeChat = "",
                    LoginCount = 65,
                    UserStatus = 1,
                    IsSystem = 0,
                    IsOnline = 1,
                    FirstVisit = "2022-01-01 00:00:00".ToDate(),
                    PreviousVisit = "2022-01-01 00:00:00".ToDate(),
                    LastVisit = "2022-01-01 00:00:00".ToDate(),
                    Remark = "",
                    WebToken = "",
                    ApiToken = "f595873c50bf4ddbac3c2b896e8075c4"
                });

                lists.Add(new SysUser()
                {
                    Id = 16508640061130153,
                    BaseIsDelete = 0,
                    BaseCreateTime = "2022-01-01 00:00:00".ToDate(),
                    BaseModifyTime = "2022-01-01 00:00:00".ToDate(),
                    BaseCreatorId = 0,
                    BaseModifierId = 16508640061130151,
                    BaseVersion = 0,
                    UserName = "zhujuanjuan",
                    Password = "e64939cb927238d770e1f8bd77f84813",
                    Salt = "91836",
                    RealName = "朱娟",
                    DepartmentId = 16508640061124402,
                    Gender = 2,
                    Birthday = "2018-12-10",
                    Portrait = "",
                    Email = "",
                    Mobile = "15566666666",
                    QQ = "",
                    WeChat = "",
                    LoginCount = 0,
                    UserStatus = 1,
                    IsSystem = 0,
                    IsOnline = 0,
                    FirstVisit = "2022-01-01 00:00:00".ToDate(),
                    PreviousVisit = "2022-01-01 00:00:00".ToDate(),
                    LastVisit = "2022-01-01 00:00:00".ToDate(),
                    Remark = "",
                    WebToken = "",
                    ApiToken = ""
                });

                return lists;
            }
        }
    }
}
