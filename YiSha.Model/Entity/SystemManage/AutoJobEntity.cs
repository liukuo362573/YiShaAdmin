using System.ComponentModel.DataAnnotations.Schema;
using YiSha.Entity.Models;

namespace YiSha.Model.Entity.SystemManage
{
    /// <summary>
    /// 定时任务表拓展
    /// </summary>
    [NotMapped]
    public class AutoJobEntity : SysAutoJob
    {
    }
}
