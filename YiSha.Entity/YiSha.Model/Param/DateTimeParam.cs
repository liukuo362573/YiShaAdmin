using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YiSha.Util.Extension;

namespace YiSha.Model.Param
{
    public class DateTimeParam
    {
        /// <summary>
        /// 搜索条件开始时间
        /// </summary>
        [LinqExpressionXAttribute(Name = "BaseModifyTime")]
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// 搜索条件结束时间
        /// </summary>
        [LinqExpressionXAttribute(Name = "BaseModifyTime")]
        public DateTime? EndTime { get; set; }
    }
}
