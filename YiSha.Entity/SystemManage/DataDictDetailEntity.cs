using System.ComponentModel.DataAnnotations.Schema;

namespace YiSha.Entity.SystemManage
{
    /// <summary>
    /// 字典数据表
    /// </summary>
    [Table("SysDataDictDetail")]
    public class DataDictDetailEntity : BaseExtensionEntity
    {
        /// <summary>
        /// 字典类型(外键)
        /// </summary>
        [Column("DictType")]
        public string DictType { get; set; }

        /// <summary>
        /// 字典排序
        /// </summary>
        [Column("DictSort")]
        public int DictSort { get; set; }

        /// <summary>
        /// 字典键(一般从1开始)
        /// </summary>
        [Column("DictKey")]
        public int DictKey { get; set; }

        /// <summary>
        /// 字典值
        /// </summary>
        [Column("DictValue")]
        public string DictValue { get; set; }

        /// <summary>
        /// 显示样式(default primary success info warning danger)
        /// </summary>
        [Column("ListClass")]
        public string ListClass { get; set; }

        /// <summary>
        /// 字典状态(0禁用 1启用)
        /// </summary>
        [Column("DictStatus")]
        public int DictStatus { get; set; }

        /// <summary>
        /// 默认选中(0不是 1是)
        /// </summary>
        [Column("IsDefault")]
        public int IsDefault { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Column("Remark")]
        public string Remark { get; set; }
    }
}
