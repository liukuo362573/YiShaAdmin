using System.ComponentModel.DataAnnotations.Schema;
using YiSha.Entity.Models;

namespace YiSha.Model.Entity.OrganizationManage
{
    /// <summary>
    /// 用户所属表拓展
    /// </summary>
    [NotMapped]
    public class UserBelongEntity : SysUserBelong
    {
        /// <summary>
        /// 多个用户Id
        /// </summary>
        [NotMapped]
        public string UserIds { get; set; }
    }
}
