using System.ComponentModel.DataAnnotations.Schema;

namespace YiSha.Entity.SystemManage
{
    /// <summary>
    /// 中国省市县表
    /// </summary>
    [Table("SysArea")]
    public class AreaEntity : BaseExtensionEntity
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

    /// <summary>
    /// 此类给其他需要省市县的业务表继承
    /// </summary>
    public class BaseAreaEntity : BaseExtensionEntity
    {
        /// <summary>
        /// 省份ID
        /// </summary>
        [Column("ProvinceId")]
        public long ProvinceId { get; set; }

        /// <summary>
        /// 城市ID
        /// </summary>
        [Column("CityId")]
        public long CityId { get; set; }

        /// <summary>
        /// 区域ID
        /// </summary>
        [Column("CountyId")]
        public long CountyId { get; set; }

        /// <summary>
        /// 省份名
        /// </summary>
        [NotMapped]
        public string ProvinceName { get; set; }

        /// <summary>
        /// 城市名
        /// </summary>
        [NotMapped]
        public string CityName { get; set; }

        /// <summary>
        /// 国家名
        /// </summary>
        [NotMapped]
        public string CountryName { get; set; }

        /// <summary>
        /// 区域编号
        /// </summary>
        [NotMapped]
        public string AreaId { get; set; }
    }
}
