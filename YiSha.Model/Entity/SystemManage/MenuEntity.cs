using System.ComponentModel.DataAnnotations.Schema;
using YiSha.Entity.Models;

namespace YiSha.Model.Entity.SystemManage
{
    /// <summary>
    /// 菜单表拓展
    /// </summary>
    [NotMapped]
    public class MenuEntity : SysMenu
    {
        /// <summary>
        /// 父级名称
        /// </summary>
        [NotMapped]
        public string ParentName { get; set; }
    }
}
