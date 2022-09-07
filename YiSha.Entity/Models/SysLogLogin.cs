using System.ComponentModel.DataAnnotations.Schema;
using YiSha.Entity.Default;

namespace YiSha.Entity.Models
{
    /// <summary>
    /// 登录日志表
    /// </summary>
    [Table("SysLogLogin")]
    public class SysLogLogin : SysDefaultModel
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
        /// 浏览器
        /// </summary>
        [Column("Browser")]
        public string Browser { get; set; }

        /// <summary>
        /// 操作系统
        /// </summary>
        [Column("OS")]
        public string OS { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Column("Remark")]
        public string Remark { get; set; }

        /// <summary>
        /// 额外备注
        /// </summary>
        [Column("ExtraRemark")]
        public string ExtraRemark { get; set; }
    }
}
