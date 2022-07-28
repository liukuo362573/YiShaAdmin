namespace YiSha.Util.Model
{
    /// <summary>
    /// 分页参数
    /// </summary>
    public class Pagination
    {
        /// <summary>
        /// 实例分页参数对象
        /// </summary>
        public Pagination()
        {
            Sort = "Id"; // 默认按Id排序
            SortType = " desc ";
            PageIndex = 1;
            PageSize = 10;
        }

        /// <summary>
        /// 每页行数
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 当前页
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 排序列
        /// </summary>
        public string Sort { get; set; }

        /// <summary>
        /// 排序类型
        /// </summary>
        public string SortType { get; set; }

        /// <summary>
        /// 总记录数
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// 总页数
        /// </summary>
        public int TotalPage
        {
            get
            {
                if (TotalCount > 0)
                {
                    var pageSize = TotalCount / this.PageSize;
                    pageSize += TotalCount % this.PageSize == 0 ? 0 : 1;
                    return pageSize;
                }
                else
                {
                    return 0;
                }
            }
        }
    }
}
