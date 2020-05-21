using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace YiSha.Entity.SystemManage
{
    [Table("SysAutoJobLog")]
    public class AutoJobLogEntity : BaseCreateEntity
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string JobGroupName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string JobName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int? LogStatus { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string Remark { get; set; }
    }
}
