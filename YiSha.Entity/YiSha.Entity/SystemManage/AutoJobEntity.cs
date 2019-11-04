using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using YiSha.Util;

namespace YiSha.Entity.SystemManage
{
    [Table("sys_auto_job")]
    public class AutoJobEntity : BaseExtensionEntity
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
        public int? JobStatus { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string CronExpression { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [JsonConverter(typeof(DateTimeJsonConverter))]
        public DateTime? StartTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [JsonConverter(typeof(DateTimeJsonConverter))]
        public DateTime? EndTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [JsonConverter(typeof(DateTimeJsonConverter))]
        public DateTime? NextStartTime { get; set; }
    }
}
