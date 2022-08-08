using System.ComponentModel.DataAnnotations.Schema;

namespace YiSha.Entity.SystemManage
{
    /// <summary>
    /// 定时任务表
    /// </summary>
    [Table("SysAutoJob")]
    public class AutoJobEntity : BaseExtensionEntity
    {
        /// <summary>
        /// 任务组名称
        /// </summary>
        /// <returns></returns>
        public string? JobGroupName { get; set; }

        /// <summary>
        /// 任务名称
        /// </summary>
        /// <returns></returns>
        public string? JobName { get; set; }

        /// <summary>
        /// 任务状态(0禁用 1启用)
        /// </summary>
        /// <returns></returns>
        public int? JobStatus { get; set; }

        /// <summary>
        /// cron表达式
        /// </summary>
        /// <returns></returns>
        public string? CronExpression { get; set; }

        /// <summary>
        /// 运行开始时间
        /// </summary>
        /// <returns></returns>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// 运行结束时间
        /// </summary>
        /// <returns></returns>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// 下次执行时间
        /// </summary>
        /// <returns></returns>
        public DateTime? NextStartTime { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }
    }
}
