using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace YiSha.Entity.SystemManage
{
    [Table("SysDataDictDetail")]
    public class DataDictDetailEntity : BaseExtensionEntity
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string DictType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int? DictSort { get; set; }
        /// <summary>
        /// 字典键
        /// </summary>
        /// <returns></returns>
        public int? DictKey { get; set; }
        /// <summary>
        /// 字典值
        /// </summary>
        /// <returns></returns>
        public string DictValue { get; set; }
        public string ListClass { get; set; }
        public int? DictStatus { get; set; }
        public int? IsDefault { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string Remark { get; set; }
    }
}
