using System.ComponentModel.DataAnnotations.Schema;
using YiSha.Entity.Default;

namespace YiSha.Entity.Models
{
    /// <summary>
    /// 中国省市县表
    /// </summary>
    [Table("SysArea")]
    public class SysArea : SysDefaultModel
    {
        /// <summary>
        /// 地区编码
        /// </summary>
        [Column("AreaCode")]
        public string AreaCode { get; set; }

        /// <summary>
        /// 父地区编码
        /// </summary>
        [Column("ParentAreaCode")]
        public string ParentAreaCode { get; set; }

        /// <summary>
        /// 地区名称
        /// </summary>
        [Column("AreaName")]
        public string AreaName { get; set; }

        /// <summary>
        /// 邮政编码
        /// </summary>
        [Column("ZipCode")]
        public string ZipCode { get; set; }

        /// <summary>
        /// 地区层级(1省份 2城市 3区县)
        /// </summary>
        [Column("AreaLevel")]
        public int AreaLevel { get; set; }
    }
}
