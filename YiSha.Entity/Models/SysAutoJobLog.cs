using System.ComponentModel.DataAnnotations.Schema;
using YiSha.Entity.Default;

namespace YiSha.Entity.Models
{
    /// <summary>
    /// 定时任务组表
    /// </summary>
    [Table("SysAutoJobLog")]
    public class SysAutoJobLog : SysDefaultModel
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
        /// 执行状态(0失败 1成功)
        /// </summary>
        [Column("LogStatus")]
        public int LogStatus { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Column("Remark")]
        public string Remark { get; set; }
    }
}
