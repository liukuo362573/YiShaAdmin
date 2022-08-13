using System.ComponentModel.DataAnnotations.Schema;

namespace YiSha.Entity.SystemManage
{
    /// <summary>
    /// 角色表
    /// </summary>
    [Table("SysRole")]
    public class RoleEntity : BaseExtensionEntity
    {
        /// <summary>
        /// 角色名称
        /// </summary>
        [Column("RoleName")]
        public string RoleName { get; set; }

        /// <summary>
        /// 角色排序
        /// </summary>
        [Column("RoleSort")]
        public int RoleSort { get; set; }

        /// <summary>
        /// 角色状态(0禁用 1启用)
        /// </summary>
        [Column("RoleStatus")]
        public int RoleStatus { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Column("Remark")]
        public string Remark { get; set; }

        /// <summary>
        /// 角色对应的菜单，页面和按钮
        /// </summary>
        [NotMapped]
        public string MenuIds { get; set; }

    }
}
