using System.ComponentModel.DataAnnotations.Schema;

namespace YiSha.Entity.SystemManage
{
    /// <summary>
    /// 定时任务组表
    /// </summary>
    [Table("SysAutoJobLog")]
    public class AutoJobLogEntity : BaseCreateEntity
    {
        /// <summary>
        /// 任务组名称
        /// </summary>
        public string? JobGroupName { get; set; }

        /// <summary>
        /// 任务名称
        /// </summary>
        public string? JobName { get; set; }

        /// <summary>
        /// 执行状态(0失败 1成功)
        /// </summary>
        public int? LogStatus { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }
    }
}
