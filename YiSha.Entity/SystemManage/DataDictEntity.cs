using System.ComponentModel.DataAnnotations.Schema;

namespace YiSha.Entity.SystemManage
{
    /// <summary>
    /// 字典类型表
    /// </summary>
    [Table("SysDataDict")]
    public class DataDictEntity : BaseExtensionEntity
    {
        /// <summary>
        /// 字典类型
        /// </summary>
        [Column("DictType")]
        public string DictType { get; set; }

        /// <summary>
        /// 字典排序
        /// </summary>
        [Column("DictSort")]
        public int DictSort { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Column("Remark")]
        public string Remark { get; set; }
    }
}
