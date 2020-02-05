using System;
using System.Collections.Generic;
using System.Text;

namespace YiSha.Model.Param
{
    public class ImportParam
    {
        /// <summary>
        /// 导入文件上传服务器后的路径
        /// </summary>
        public string FilePath { get; set; }
        /// <summary>
        /// 是否更新已有的数据
        /// </summary>
        public int? IsOverride { get; set; }
    }
}
