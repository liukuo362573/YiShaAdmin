using System.ComponentModel.DataAnnotations.Schema;

namespace YiSha.Entity.OrganizationManage
{
    [Table("SysPosition")]
    public class PositionEntity : BaseExtensionEntity
    {
        public string PositionName { get; set; }

        public int? PositionSort { get; set; }

        public int? PositionStatus { get; set; }

        public string Remark { get; set; }
    }
}