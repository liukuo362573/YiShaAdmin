using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace YiSha.Entity.SystemManage
{
    [Table("sys_data_dict")]
    public class DataDictEntity : BaseExtensionEntity
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
        /// 
        /// </summary>
        /// <returns></returns>
        public string Remark { get; set; }
    }
}
