using System.ComponentModel.DataAnnotations.Schema;
using YiSha.Entity.Models;

namespace YiSha.Model.Entity.SystemManage
{
    /// <summary>
    /// 登录日志表拓展
    /// </summary>
    [NotMapped]
    public class LogLoginEntity : SysLogLogin
    {
        /// <summary>
        /// 用户名称
        /// </summary>
        [NotMapped]
        public string UserName { get; set; }
    }
}
