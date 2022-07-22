using System.ComponentModel.DataAnnotations.Schema;

namespace YiSha.Entity.OrganizationManage
{
    /// <summary>
    /// 职位表
    /// </summary>
    [Table("SysPosition")]
    public class PositionEntity : BaseExtensionEntity
    {
        /// <summary>
        /// 职位名称
        /// </summary>
        public string? PositionName { get; set; }

        /// <summary>
        /// 职位排序
        /// </summary>
        public int? PositionSort { get; set; }

        /// <summary>
        /// 职位状态(0禁用 1启用)
        /// </summary>
        public int? PositionStatus { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }
    }
}
