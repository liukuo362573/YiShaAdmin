using System.ComponentModel.DataAnnotations.Schema;
using YiSha.Entity.Models;

namespace YiSha.Model.Entity.OrganizationManage
{
    /// <summary>
    /// 用户表拓展
    /// </summary>
    [NotMapped]
    public class UserEntity : SysUser
    {
        /// <summary>
        /// 所属部门Id
        /// </summary>
        [Column("DepartmentId")]
        public long DepartmentId { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        [NotMapped]
        public string DepartmentName { get; set; }

        /// <summary>
        /// 岗位Id
        /// </summary>
        [NotMapped]
        public string PositionIds { get; set; }

        /// <summary>
        /// 角色Id
        /// </summary>
        [NotMapped]
        public string RoleIds { get; set; }
    }
}
