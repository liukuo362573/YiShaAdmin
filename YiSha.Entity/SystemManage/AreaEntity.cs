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
        /// <returns></returns>
        public string? AreaCode { get; set; }

        /// <summary>
        /// 父地区编码
        /// </summary>
        /// <returns></returns>
        public string? ParentAreaCode { get; set; }

        /// <summary>
        /// 地区名称
        /// </summary>
        /// <returns></returns>
        public string? AreaName { get; set; }

        /// <summary>
        /// 邮政编码
        /// </summary>
        /// <returns></returns>
        public string? ZipCode { get; set; }

        /// <summary>
        /// 地区层级(1省份 2城市 3区县)
        /// </summary>
        /// <returns></returns>
        public int? AreaLevel { get; set; }
    }

    /// <summary>
    /// 此类给其他需要省市县的业务表继承
    /// </summary>
    public class BaseAreaEntity : BaseExtensionEntity
    {
        /// <summary>
        /// 省份ID
        /// </summary>
        /// <returns></returns>
        public long? ProvinceId { get; set; }

        /// <summary>
        /// 城市ID
        /// </summary>
        /// <returns></returns>
        public long? CityId { get; set; }

        /// <summary>
        /// 区域ID
        /// </summary>
        /// <returns></returns>
        public long? CountyId { get; set; }

        /// <summary>
        /// 省份名
        /// </summary>
        [NotMapped]
        public string? ProvinceName { get; set; }

        /// <summary>
        /// 城市名
        /// </summary>
        [NotMapped]
        public string? CityName { get; set; }

        /// <summary>
        /// 国家名
        /// </summary>
        [NotMapped]
        public string? CountryName { get; set; }

        /// <summary>
        /// 区域编号
        /// </summary>
        [NotMapped]
        public string? AreaId { get; set; }
    }
}
