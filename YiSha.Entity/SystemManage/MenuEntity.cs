using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using YiSha.Util;

namespace YiSha.Entity.SystemManage
{
    /// <summary>
    /// 菜单表
    /// </summary>
    [Table("SysMenu")]
    public class MenuEntity : BaseExtensionEntity
    {
        /// <summary>
        /// 父菜单Id(0表示是根菜单)
        /// </summary>
        [JsonConverter(typeof(StringJsonConverter))]
        public long? ParentId { get; set; }

        /// <summary>
        /// 菜单名称
        /// </summary>
        public string? MenuName { get; set; }

        /// <summary>
        /// 菜单图标
        /// </summary>
        public string? MenuIcon { get; set; }

        /// <summary>
        /// 菜单Url
        /// </summary>
        public string? MenuUrl { get; set; }

        /// <summary>
        /// 链接打开方式
        /// </summary>
        public string? MenuTarget { get; set; }

        /// <summary>
        /// 菜单排序
        /// </summary>
        public int? MenuSort { get; set; }

        /// <summary>
        /// 菜单类型(1目录 2页面 3按钮)
        /// </summary>
        public int? MenuType { get; set; }

        /// <summary>
        /// 菜单状态(0禁用 1启用)
        /// </summary>
        public int? MenuStatus { get; set; }

        /// <summary>
        /// 菜单权限标识
        /// </summary>
        public string? Authorize { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }

        /// <summary>
        /// 父级名称
        /// </summary>
        [NotMapped]
        public string? ParentName { get; set; }
    }
}
