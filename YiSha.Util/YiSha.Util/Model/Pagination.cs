﻿namespace YiSha.Util.Model
{
    /// <summary>
    /// 分页参数
    /// </summary>
    public class Pagination
    {
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
                    return TotalCount % PageSize == 0 ? TotalCount / PageSize : TotalCount / PageSize + 1;
                }
                return 0;
            }
        }
    }
}