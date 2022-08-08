using System.ComponentModel.DataAnnotations.Schema;

namespace YiSha.Entity.OrganizationManage
{
    /// <summary>
    /// 用户所属表
    /// </summary>
    [Table("SysUserBelong")]
    public class UserBelongEntity : BaseCreateEntity
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public long? UserId { get; set; }

        /// <summary>
        /// 职位Id或者角色Id
        /// </summary>
        public long? BelongId { get; set; }

        /// <summary>
        /// 所属类型(1职位 2角色)
        /// </summary>
        public int? BelongType { get; set; }

        /// <summary>
        /// 多个用户Id
        /// </summary>
        [NotMapped]
        public string? UserIds { get; set; }
    }
}
