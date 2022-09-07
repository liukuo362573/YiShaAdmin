using System.ComponentModel.DataAnnotations.Schema;
using YiSha.Entity.Models;

namespace YiSha.Model.Entity.OrganizationManage
{
    /// <summary>
    /// 新闻表拓展
    /// </summary>
    [NotMapped]
    public class NewsEntity : SysNews
    {
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
