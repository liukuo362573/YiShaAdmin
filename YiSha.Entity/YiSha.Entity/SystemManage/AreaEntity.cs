using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using YiSha.Util;

namespace YiSha.Entity.SystemManage
{
    [Table("SysArea")]
    public class AreaEntity : BaseExtensionEntity
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string AreaCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string ParentAreaCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string AreaName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string ZipCode { get; set; }
        /// <summary>
        /// 
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
        [JsonConverter(typeof(StringJsonConverter))]
        public long? ProvinceId { get; set; }
        /// <summary>
        /// 城市ID
        /// </summary>
        /// <returns></returns>
        [JsonConverter(typeof(StringJsonConverter))]
        public long? CityId { get; set; }
        /// <summary>
        /// 区域ID
        /// </summary>
        /// <returns></returns>
        [JsonConverter(typeof(StringJsonConverter))]
        public long? CountyId { get; set; }
        [NotMapped]
        public string ProvinceName { get; set; }
        [NotMapped]
        public string CityName { get; set; }
        [NotMapped]
        public string CountryName { get; set; }
        [NotMapped]
        public string AreaId { get; set; }
    }
}
