﻿using System.ComponentModel.DataAnnotations.Schema;

namespace YiSha.Entity.SystemManage
{
    [Table("SysAutoJob")]
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
        public DateTime? StartTime { get; set; }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public DateTime? EndTime { get; set; }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public DateTime? NextStartTime { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
