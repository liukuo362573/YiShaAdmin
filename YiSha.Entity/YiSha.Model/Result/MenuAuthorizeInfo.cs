using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YiSha.Util;

namespace YiSha.Model.Result
{
    public class MenuAuthorizeInfo
    {
        [JsonConverter(typeof(StringJsonConverter))]
        public long? MenuId { get; set; }
        /// <summary>
        /// 用户Id或者角色Id
        /// </summary>
        [JsonConverter(typeof(StringJsonConverter))]
        public long? AuthorizeId { get; set; }
        /// <summary>
        ///  用户或者角色
        /// </summary>
        public int? AuthorizeType { get; set; }
        /// <summary>
        /// 权限标识
        /// </summary>
        public string Authorize { get; set; }

    }
}
