using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using YiSha.Util.Helper;

namespace YiSha.Entity.SystemManage
{
    [Table("SysMenuAuthorize")]
    public class MenuAuthorizeEntity : BaseCreateEntity
    {
        [JsonConverter(typeof(StringJsonConverter))]
        public long? MenuId { get; set; }

        [JsonConverter(typeof(StringJsonConverter))]
        public long? AuthorizeId { get; set; }

        public int? AuthorizeType { get; set; }

        [NotMapped]
        public string AuthorizeIds { get; set; }
    }
}