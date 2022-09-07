using System.ComponentModel.DataAnnotations.Schema;
using YiSha.Entity.Models;

namespace YiSha.Model.Entity.SystemManage
{
    /// <summary>
    /// 菜单权限表拓展
    /// </summary>
    [NotMapped]
    public class MenuAuthorizeEntity : SysMenuAuthorize
    {
        /// <summary>
        /// 授权 ID
        /// </summary>
        [NotMapped]
        public string AuthorizeIds { get; set; }
    }
}
