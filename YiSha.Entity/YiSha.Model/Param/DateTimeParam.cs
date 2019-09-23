using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YiSha.Model.Param
{
    public class DateTimeParam
    {
        /// <summary>
        /// 搜索条件开始时间
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// 搜索条件结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }
    }
}
