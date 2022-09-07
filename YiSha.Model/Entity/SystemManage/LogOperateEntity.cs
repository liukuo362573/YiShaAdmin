using System.ComponentModel.DataAnnotations.Schema;
using YiSha.Entity.Models;

namespace YiSha.Model.Entity.SystemManage
{
    /// <summary>
    /// 操作日志表拓展
    /// </summary>
    [NotMapped]
    public class LogOperateEntity : SysLogOperate
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
