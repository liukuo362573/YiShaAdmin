using System.ComponentModel.DataAnnotations.Schema;
using YiSha.Entity.Default;

namespace YiSha.Entity.Models
{
    /// <summary>
    /// 操作日志表
    /// </summary>
    [Table("SysLogOperate")]
    public class SysLogOperate : SysDefaultModel
    {
        /// <summary>
        /// 执行状态(0失败 1成功)
        /// </summary>
        [Column("LogStatus")]
        public int LogStatus { get; set; }

        /// <summary>
        /// ip地址
        /// </summary>
        [Column("IpAddress")]
        public string IpAddress { get; set; }

        /// <summary>
        /// ip位置
        /// </summary>
        [Column("IpLocation")]
        public string IpLocation { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Column("Remark")]
        public string Remark { get; set; }

        /// <summary>
        /// 日志类型(暂未用到)
        /// </summary>
        [Column("LogType")]
        public string LogType { get; set; }

        /// <summary>
        /// 业务类型(暂未用到)
        /// </summary>
        [Column("BusinessType")]
        public string BusinessType { get; set; }

        /// <summary>
        /// 页面地址
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
