using System.ComponentModel.DataAnnotations.Schema;

namespace YiSha.Entity.SystemManage
{
    [Table("SysMenu")]
    public class MenuEntity : BaseExtensionEntity
    {
        public long? ParentId { get; set; }

        public string MenuName { get; set; }

        public string MenuIcon { get; set; }

        public string MenuUrl { get; set; }

        public string MenuTarget { get; set; }

        public int? MenuSort { get; set; }

        public int? MenuType { get; set; }

        public int? MenuStatus { get; set; }
        public string Authorize { get; set; }

        public string Remark { get; set; }

        [NotMapped]
        public string ParentName { get; set; }
    }
}
