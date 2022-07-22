using System.ComponentModel.DataAnnotations.Schema;

namespace YiSha.Entity.SystemManage
{
    /// <summary>
    /// Api日志表
    /// </summary>
    [Table("SysLogApi")]
    public class LogApiEntity : BaseCreateEntity
    {
        /// <summary>
        /// 执行状态(0失败 1成功)
        /// </summary>
        public int? LogStatus { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }

        /// <summary>
        /// 接口地址
        /// </summary>
        public string? ExecuteUrl { get; set; }

        /// <summary>
        /// 请求参数
        /// </summary>
        public string? ExecuteParam { get; set; }

        /// <summary>
        /// 请求结果
        /// </summary>
        public string? ExecuteResult { get; set; }

        /// <summary>
        /// 执行时间
        /// </summary>
        public int? ExecuteTime { get; set; }

        /// <summary>
        /// 用户名称
        /// </summary>
        [NotMapped]
        public string? UserName { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        [NotMapped]
        public string? DepartmentName { get; set; }
    }
}
