using System.ComponentModel.DataAnnotations.Schema;
using YiSha.Entity.Default;

namespace YiSha.Entity.Models
{
    /// <summary>
    /// Api日志表
    /// </summary>
    [Table("SysLogApi")]
    public class SysLogApi : SysDefaultModel
    {
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

        /// <summary>
        /// 接口地址
        /// </summary>
        [Column("ExecuteUrl")]
        public string ExecuteUrl { get; set; }

        /// <summary>
        /// 请求参数
        /// </summary>
        [Column("ExecuteParam")]
        public string ExecuteParam { get; set; }

        /// <summary>
        /// 请求结果
        /// </summary>
        [Column("ExecuteResult")]
        public string ExecuteResult { get; set; }

        /// <summary>
        /// 执行时间
        /// </summary>
        [Column("ExecuteTime")]
        public int ExecuteTime { get; set; }
    }
}
