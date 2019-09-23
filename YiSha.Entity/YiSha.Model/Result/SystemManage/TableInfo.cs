using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YiSha.Model.Result.SystemManage
{
    public class TableInfo
    {
        /// <summary>
        /// 表的Id
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 表名
        /// </summary>
        public string TableName { get; set; }
        /// <summary>
        /// 主键名
        /// </summary>
        public string TableKeyName { get; set; }
        /// <summary>
        /// 主键
        /// </summary>
        public string TableKey { get; set; }
        /// <summary>
        /// 记录数
        /// </summary>
        public int TableCount { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }


}
