using System.ComponentModel.DataAnnotations.Schema;
using YiSha.Entity.Default;

namespace YiSha.Entity.Models
{
    /// <summary>
    /// 定时任务表
    /// </summary>
    [Table("SysAutoJob")]
    public class SysAutoJob : SysDefaultModel
    {
        /// <summary>
        /// 任务组名称
        /// </summary>
        [Column("JobGroupName")]
        public string JobGroupName { get; set; }

        /// <summary>
        /// 任务名称
        /// </summary>
        [Column("JobName")]
        public string JobName { get; set; }

        /// <summary>
        /// 任务状态(0禁用 1启用)
        /// </summary>
        [Column("JobStatus")]
        public int JobStatus { get; set; }

        /// <summary>
        /// cron表达式
        /// </summary>
        [Column("CronExpression")]
        public string CronExpression { get; set; }

        /// <summary>
        /// 运行开始时间
        /// </summary>
        [Column("StartTime")]
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 运行结束时间
        /// </summary>
        [Column("EndTime")]
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 下次执行时间
        /// </summary>
        [Column("NextStartTime")]
        public DateTime NextStartTime { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Column("Remark")]
        public string Remark { get; set; }
    }
}
