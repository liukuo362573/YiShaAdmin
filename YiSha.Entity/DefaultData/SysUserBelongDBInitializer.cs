using YiSha.Entity.Models;
using YiSha.Util;

namespace YiSha.Entity.DefaultData
{
    /// <summary>
    /// 用户所属表数据初始化
    /// </summary>
    internal class SysUserBelongDBInitializer
    {
        /// <summary>
        /// 获取数据
        /// </summary>
        public static List<SysUserBelong> GetData
        {
            get
            {
                var lists = new List<SysUserBelong>();

                lists.Add(new SysUserBelong()
                {
                    Id = 20152771193868288,
                    BaseCreateTime = "2022-01-01 00:00:00".ToDate(),
                    BaseCreatorId = 16508640061130151,
                    UserId = 16508640061130151,
                    BelongId = 16508640061130139,
                    BelongType = 1,
                });

                lists.Add(new SysUserBelong()
                {
                    Id = 20152771193868289,
                    BaseCreateTime = "2022-01-01 00:00:00".ToDate(),
                    BaseCreatorId = 16508640061130151,
                    UserId = 16508640061130151,
                    BelongId = 16508640061130146,
                    BelongType = 2,
                });

                lists.Add(new SysUserBelong()
                {
                    Id = 103181410411483136,
                    BaseCreateTime = "2022-01-01 00:00:00".ToDate(),
                    BaseCreatorId = 16508640061130151,
                    UserId = 16508640061130153,
                    BelongId = 16508640061130140,
                    BelongType = 1,
                });

                lists.Add(new SysUserBelong()
                {
                    Id = 103181410411483137,
                    BaseCreateTime = "2022-01-01 00:00:00".ToDate(),
                    BaseCreatorId = 16508640061130151,
                    UserId = 16508640061130153,
                    BelongId = 16508640061130141,
                    BelongType = 1,
                });

                lists.Add(new SysUserBelong()
                {
                    Id = 103181410415677440,
                    BaseCreateTime = "2022-01-01 00:00:00".ToDate(),
                    BaseCreatorId = 16508640061130151,
                    UserId = 16508640061130153,
                    BelongId = 16508640061130142,
                    BelongType = 1,
                });

                lists.Add(new SysUserBelong()
                {
                    Id = 103193128348946432,
                    BaseCreateTime = "2022-01-01 00:00:00".ToDate(),
                    BaseCreatorId = 16508640061130151,
                    UserId = 16508640061130152,
                    BelongId = 16508640061130143,
                    BelongType = 1,
                });

                lists.Add(new SysUserBelong()
                {
                    Id = 103193280182751232,
                    BaseCreateTime = "2022-01-01 00:00:00".ToDate(),
                    BaseCreatorId = 16508640061130151,
                    UserId = 16508640061130150,
                    BelongId = 16508640061130147,
                    BelongType = 2,
                });

                return lists;
            }
        }
    }
}
