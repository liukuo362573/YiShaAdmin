using System.ComponentModel.DataAnnotations.Schema;

namespace YiSha.Entity.SystemManage
{
    /// <summary>
    /// 登录日志表
    /// </summary>
    [Table("SysLogLogin")]
    public class LogLoginEntity : BaseCreateEntity
    {
        /// <summary>
        /// 执行状态(0失败 1成功)
        /// </summary>
        public int? LogStatus { get; set; }

        /// <summary>
        /// ip地址
        /// </summary>
        public string? IpAddress { get; set; }

        /// <summary>
        /// ip位置
        /// </summary>
        public string? IpLocation { get; set; }

        /// <summary>
        /// 浏览器
        /// </summary>
        public string? Browser { get; set; }

        /// <summary>
        /// 操作系统
        /// </summary>
        public string? OS { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }

        /// <summary>
        /// 额外备注
        /// </summary>
        public string? ExtraRemark { get; set; }

        /// <summary>
        /// 用户名称
        /// </summary>
        [NotMapped]
        public string? UserName { get; set; }
    }
}
