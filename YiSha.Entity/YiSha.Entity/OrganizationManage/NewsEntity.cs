using System.ComponentModel.DataAnnotations.Schema;
using YiSha.Entity.SystemManage;

namespace YiSha.Entity.OrganizationManage
{
    [Table("SysNews")]
    public class NewsEntity : BaseAreaEntity
    {
        /// <summary>
        /// 文章标题
        /// </summary>
        public string NewsTitle { get; set; }

        /// <summary>
        /// 文章内容
        /// </summary>
        /// <returns></returns>
        public string NewsContent { get; set; }

        /// <summary>
        /// 文章标签
        /// </summary>
        public string NewsTag { get; set; }

        /// <summary>
        /// 缩略图
        /// </summary>
        public string ThumbImage { get; set; }

        /// <summary>
        /// 作者
        /// </summary>
        public string NewsAuthor { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int? NewsSort { get; set; }

        /// <summary>
        /// 发布时间
        /// </summary>
        public DateTime? NewsDate { get; set; }

        /// <summary>
        /// 文章类别
        /// </summary>
        public int? NewsType { get; set; }

        /// <summary>
        /// 阅读量
        /// </summary>
        public int? ViewTimes { get; set; }
    }
}
