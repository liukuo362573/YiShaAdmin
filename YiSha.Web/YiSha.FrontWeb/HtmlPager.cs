using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YiSha.Util.Model;

namespace YiSha.FrontWeb
{
    public class HtmlPager
    {
        #region Mini分页
        /// <summary>
        /// Mini分页
        /// </summary>
        /// <param name="pagerName"></param>
        /// <param name="pagerInfo">从0开始</param>
        /// <param name="url">脚本分页事件 如：LoadList</param>
        /// <param name="totalCountName">总条数hidden id</param>
        /// <param name="nothingMsg">没有数据时显示的文本，默认-暂无数据</param>
        /// <returns></returns>
        public static string MiniPager(string pagerName, Pagination pagerInfo, string url, string nothingMsg = "暂无数据")
        {
            StringBuilder sb = new StringBuilder();
            if (pagerInfo == null)
            {
                return string.Empty;
            }
            if (url.Contains("?"))
            {
                url = url + "&";
            }
            else
            {
                url = url + "?";
            }
            url += "PageIndex={0}";
            if (pagerInfo.PageIndex < 1)
            {
                pagerInfo.PageIndex = 1;
            }

            int pageCount = pagerInfo.TotalCount / pagerInfo.PageSize;
            if (pagerInfo.TotalCount % pagerInfo.PageSize > 0)
            {
                pageCount = pageCount + 1;
            }
            if (pagerInfo.TotalCount == 0)
            {
                sb.Append("<div class='text-center'>" + nothingMsg + "</div>");
                return sb.ToString();
            }

            sb.Append("<div id='" + pagerName + "' class='paginatoin-area text-center mt-30'>");
            sb.Append("<ul class='pagination-box'>");

            // 当前为第一页，禁用首页按钮
            if (pagerInfo.PageIndex == 1)
            {
                sb.Append("<li class='disabled'><a class='Previous' href='#'><i class='fa fa-angle-left'></i></a></li>");
            }
            else
            {
                sb.Append("<li><a class='Previous' href='" + string.Format(url, pagerInfo.PageIndex - 1) + "'><i class='fa fa-angle-left'></i></a></li>");
            }
            int range = 2;
            //当前页超过3
            if (pagerInfo.PageIndex - range > 1)
            {
                sb.Append("<li><span>…</span></li>");
            }
            for (int i = pagerInfo.PageIndex - range; i <= pagerInfo.PageIndex + range; i++)
            {
                if (i <= pageCount && i > 0)
                {
                    if (i == pagerInfo.PageIndex)
                    {
                        sb.Append("<li class='active'><a href='" + string.Format(url, i) + "'>" + i + "</a></li>");//当前页
                    }
                    else
                    {
                        sb.Append("<li><a href='" + string.Format(url, i) + "'>" + i + "</a></li>");
                    }
                }
            }

            if (pagerInfo.PageIndex + range < pageCount)
            {
                sb.AppendFormat("<li><span>…</span></li><li><a class='Next' href='" + string.Format(url, pagerInfo.PageIndex + 1) + "'><i class='fa fa-angle-right'></i></a></li>", url, pageCount - 1);
            }
            else if (pagerInfo.PageIndex == pageCount)
            {
                sb.Append("<li class='disabled'><a class='Next' href='#'><i class='fa fa-angle-right'></i></a></li>");
            }
            else
            {
                sb.Append("<li><a class='Next' href='" + string.Format(url, pagerInfo.PageIndex + 1) + "'><i class='fa fa-angle-right'></i></a></li>");
            }
            sb.Append("</ul>");
            sb.Append("</div>");

            return sb.ToString();
        }
        #endregion
    }
}
