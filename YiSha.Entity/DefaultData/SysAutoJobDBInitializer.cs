using YiSha.Entity.Models;
using YiSha.Util;

namespace YiSha.Entity.DefaultData
{
    /// <summary>
    /// 定时任务表数据初始化
    /// </summary>
    internal class SysAutoJobDBInitializer
    {
        /// <summary>
        /// 获取数据
        /// </summary>
        public static List<SysAutoJob> GetData
        {
            get
            {
                var lists = new List<SysAutoJob>();

                lists.Add(new SysAutoJob()
                {
                    Id = 16508640061124370,
                    BaseIsDelete = 0,
                    BaseCreateTime = "2022-01-01 00:00:00".ToDate(),
                    BaseModifyTime = "2022-01-01 00:00:00".ToDate(),
                    BaseCreatorId = 0,
                    BaseModifierId = 0,
                    BaseVersion = 0,
                    JobGroupName = "YiShaAdmin",
                    JobName = "数据库备份",
                    JobStatus = 1,
                    CronExpression = "0 0 3 1/1 * ?",
                    StartTime = "2022-01-01 00:00:00".ToDate(),
                    EndTime = "2022-01-01 00:00:00".ToDate(),
                    NextStartTime = "2022-01-01 00:00:00".ToDate(),
                    Remark = "",
                });

                return lists;
            }
        }
    }
}
