using System.ComponentModel.DataAnnotations.Schema;

namespace YiSha.Entity.SystemManage
{
    /// <summary>
    /// 菜单权限表
    /// </summary>
    [Table("SysMenuAuthorize")]
    public class MenuAuthorizeEntity : BaseCreateEntity
    {
        /// <summary>
        /// 菜单Id
        /// </summary>
        [Column("MenuId")]
        public long MenuId { get; set; }

        /// <summary>
        /// 授权Id(角色Id或者用户Id)
        /// </summary>
        [Column("AuthorizeId")]
        public long AuthorizeId { get; set; }

        /// <summary>
        /// 授权类型(1角色 2用户)
        /// </summary>
        [Column("AuthorizeType")]
        public int AuthorizeType { get; set; }

        /// <summary>
        /// 授权 ID
        /// </summary>
        [NotMapped]
        public string AuthorizeIds { get; set; }
    }
}
