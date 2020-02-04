using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YiSha.Model.Result.SystemManage
{
    public class DataDictInfo
    {
        /// <summary>
        /// 数据字典类型
        /// </summary>
        public string DictType { get; set; }

        public List<DataDictDetailInfo> Detail { get; set; }
    }

    public class DataDictDetailInfo
    {
        /// <summary>
        /// 字典键
        /// </summary>
        public int? DictKey { get; set; }
        /// <summary>
        /// 字典值
        /// </summary>
        public string DictValue { get; set; }
        public string ListClass { get; set; }
        public int? DictStatus { get; set; }
        public int? IsDefault { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
