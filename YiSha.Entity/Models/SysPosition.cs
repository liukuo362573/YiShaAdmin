using System.ComponentModel.DataAnnotations.Schema;
using YiSha.Entity.Default;

namespace YiSha.Entity.Models
{
    /// <summary>
    /// 职位表
    /// </summary>
    [Table("SysPosition")]
    public class SysPosition : SysDefaultModel
    {
        /// <summary>
        /// 职位名称
        /// </summary>
        [Column("PositionName")]
        public string PositionName { get; set; }

        /// <summary>
        /// 职位排序
        /// </summary>
        [Column("PositionSort")]
        public int PositionSort { get; set; }

        /// <summary>
        /// 职位状态(0禁用 1启用)
        /// </summary>
        [Column("PositionStatus")]
        public int PositionStatus { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Column("Remark")]
        public string Remark { get; set; }
    }
}
