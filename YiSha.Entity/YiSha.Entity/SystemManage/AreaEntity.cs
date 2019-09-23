using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using YiSha.Util;

namespace YiSha.Entity.SystemManage
{
    [Table("sys_area")]
    public class AreaEntity : BaseEntity
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

    public class BaseAreaEntity : BaseEntity
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
        public string CountyName { get; set; }
        [NotMapped]
        public string AreaId { get; set; }
    }
}
