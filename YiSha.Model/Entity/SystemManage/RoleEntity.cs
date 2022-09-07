using System.ComponentModel.DataAnnotations.Schema;
using YiSha.Entity.Models;

namespace YiSha.Model.Entity.SystemManage
{
    /// <summary>
    /// 角色表拓展
    /// </summary>
    [NotMapped]
    public class RoleEntity : SysRole
    {
        /// <summary>
        /// 角色对应的菜单，页面和按钮
        /// </summary>
        [NotMapped]
        public string MenuIds { get; set; }
    }
}
