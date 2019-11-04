using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YiSha.Entity.OrganizationManage
{
    [Table("sys_position")]
    public class PositionEntity : BaseExtensionEntity
    {
        public string PositionName { get; set; }
        public int? PositionSort { get; set; }
        public int? PositionStatus { get; set; }
        public string Remark { get; set; }
    }
}
