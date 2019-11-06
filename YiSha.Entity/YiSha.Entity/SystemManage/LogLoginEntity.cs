using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YiSha.Entity.SystemManage
{
    [Table("sys_log_login")]
    public class LogLoginEntity : BaseCreateEntity
    {
        public int? LogStatus { get; set; }
        public string IpAddress { get; set; }
        public string IpLocation { get; set; }
        public string Browser { get; set; }
        public string OS { get; set; }
        public string Remark { get; set; }
        public string ExtraRemark { get; set; }

        [NotMapped]
        public string UserName { get; set; }
    }
}
