using System.ComponentModel.DataAnnotations.Schema;
using YiSha.Entity.Default;

namespace YiSha.Entity.Models
{
    /// <summary>
    /// 新闻表
    /// </summary>
    [Table("SysNews")]
    public class SysNews : SysDefaultModel
    {
        /// <summary>
        /// 文章标题
        /// </summary>
        [Column("NewsTitle")]
        public string NewsTitle { get; set; }

        /// <summary>
        /// 文章内容
        /// </summary>
        [Column("NewsContent")]
        public string NewsContent { get; set; }

        /// <summary>
        /// 文章标签
        /// </summary>
        [Column("NewsTag")]
        public string NewsTag { get; set; }

        /// <summary>
        /// 缩略图
        /// </summary>
        [Column("ThumbImage")]
        public string ThumbImage { get; set; }

        /// <summary>
        /// 作者
        /// </summary>
        [Column("NewsAuthor")]
        public string NewsAuthor { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        [Column("NewsSort")]
        public int NewsSort { get; set; }

        /// <summary>
        /// 发布时间
        /// </summary>
        [Column("NewsDate")]
        public DateTime NewsDate { get; set; }

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
        /// 文章类别
        /// </summary>
        [Column("NewsType")]
        public int NewsType { get; set; }

        /// <summary>
        /// 阅读量
        /// </summary>
        [Column("ViewTimes")]
        public int ViewTimes { get; set; }
    }
}
