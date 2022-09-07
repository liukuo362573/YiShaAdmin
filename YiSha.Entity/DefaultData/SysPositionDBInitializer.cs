using YiSha.Entity.Models;
using YiSha.Util;

namespace YiSha.Entity.DefaultData
{
    /// <summary>
    /// 职位表数据初始化
    /// </summary>
    internal class SysPositionDBInitializer
    {
        /// <summary>
        /// 获取数据
        /// </summary>
        public static List<SysPosition> GetData
        {
            get
            {
                var lists = new List<SysPosition>();

                lists.Add(new SysPosition()
                {
                    Id = 16508640061130139,
                    BaseIsDelete = 0,
                    BaseCreateTime = "2022-01-01 00:00:00".ToDate(),
                    BaseModifyTime = "2022-01-01 00:00:00".ToDate(),
                    BaseCreatorId = 0,
                    BaseModifierId = 16508640061130151,
                    BaseVersion = 0,
                    PositionName = "董事长",
                    PositionSort = 1,
                    PositionStatus = 1,
                    Remark = "CEO"
                });

                lists.Add(new SysPosition()
                {
                    Id = 16508640061130140,
                    BaseIsDelete = 0,
                    BaseCreateTime = "2022-01-01 00:00:00".ToDate(),
                    BaseModifyTime = "2022-01-01 00:00:00".ToDate(),
                    BaseCreatorId = 0,
                    BaseModifierId = 16508640061130151,
                    BaseVersion = 0,
                    PositionName = "总经理",
                    PositionSort = 2,
                    PositionStatus = 1,
                    Remark = ""
                });

                lists.Add(new SysPosition()
                {
                    Id = 16508640061130141,
                    BaseIsDelete = 0,
                    BaseCreateTime = "2022-01-01 00:00:00".ToDate(),
                    BaseModifyTime = "2022-01-01 00:00:00".ToDate(),
                    BaseCreatorId = 0,
                    BaseModifierId = 0,
                    BaseVersion = 0,
                    PositionName = "项目经理",
                    PositionSort = 3,
                    PositionStatus = 1,
                    Remark = ""
                });

                lists.Add(new SysPosition()
                {
                    Id = 16508640061130142,
                    BaseIsDelete = 0,
                    BaseCreateTime = "2022-01-01 00:00:00".ToDate(),
                    BaseModifyTime = "2022-01-01 00:00:00".ToDate(),
                    BaseCreatorId = 0,
                    BaseModifierId = 0,
                    BaseVersion = 0,
                    PositionName = "测试经理",
                    PositionSort = 4,
                    PositionStatus = 1,
                    Remark = ""
                });

                lists.Add(new SysPosition()
                {
                    Id = 16508640061130143,
                    BaseIsDelete = 0,
                    BaseCreateTime = "2022-01-01 00:00:00".ToDate(),
                    BaseModifyTime = "2022-01-01 00:00:00".ToDate(),
                    BaseCreatorId = 0,
                    BaseModifierId = 0,
                    BaseVersion = 0,
                    PositionName = "程序员",
                    PositionSort = 5,
                    PositionStatus = 2,
                    Remark = ""
                });

                lists.Add(new SysPosition()
                {
                    Id = 16508640061130144,
                    BaseIsDelete = 0,
                    BaseCreateTime = "2022-01-01 00:00:00".ToDate(),
                    BaseModifyTime = "2022-01-01 00:00:00".ToDate(),
                    BaseCreatorId = 0,
                    BaseModifierId = 0,
                    BaseVersion = 0,
                    PositionName = "前端",
                    PositionSort = 6,
                    PositionStatus = 1,
                    Remark = ""
                });

                lists.Add(new SysPosition()
                {
                    Id = 16508640061130145,
                    BaseIsDelete = 0,
                    BaseCreateTime = "2022-01-01 00:00:00".ToDate(),
                    BaseModifyTime = "2022-01-01 00:00:00".ToDate(),
                    BaseCreatorId = 0,
                    BaseModifierId = 0,
                    BaseVersion = 0,
                    PositionName = "财务专员",
                    PositionSort = 7,
                    PositionStatus = 1,
                    Remark = ""
                });

                return lists;
            }
        }
    }
}
