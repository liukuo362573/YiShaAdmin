using System.ComponentModel.DataAnnotations.Schema;
using YiSha.Entity.Models;

namespace YiSha.Model.Entity.SystemManage
{
    /// <summary>
    /// Api日志表拓展
    /// </summary>
    [NotMapped]
    public class LogApiEntity : SysLogApi
    {
        /// <summary>
        /// 用户名称
        /// </summary>
        [NotMapped]
        public string UserName { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        [NotMapped]
        public string DepartmentName { get; set; }
    }
}
