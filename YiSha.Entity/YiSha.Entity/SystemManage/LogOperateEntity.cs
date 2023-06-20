using System.ComponentModel.DataAnnotations.Schema;

namespace YiSha.Entity.SystemManage
{
    [Table("SysLogOperate")]
    public class LogOperateEntity : BaseCreateEntity
    {
        public int? LogStatus { get; set; }
        public string IpAddress { get; set; }
        public string IpLocation { get; set; }
        public string Remark { get; set; }
        public string LogType { get; set; }
        public string BusinessType { get; set; }
        public string ExecuteUrl { get; set; }
        public string ExecuteParam { get; set; }
        public string ExecuteResult { get; set; }
        public int? ExecuteTime { get; set; }

        [NotMapped]
        public string UserName { get; set; }

        [NotMapped]
        public string DepartmentName { get; set; }
    }
}
