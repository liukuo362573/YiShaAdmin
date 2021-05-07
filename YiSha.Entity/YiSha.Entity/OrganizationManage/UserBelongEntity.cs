using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using YiSha.Util.Helper;

namespace YiSha.Entity.OrganizationManage
{
    [Table("SysUserBelong")]
    public class UserBelongEntity : BaseCreateEntity
    {
        [JsonConverter(typeof(StringJsonConverter))]
        public long? UserId { get; set; }

        [JsonConverter(typeof(StringJsonConverter))]
        public long? BelongId { get; set; }

        public int? BelongType { get; set; }

        /// <summary>
        /// 多个用户Id
        /// </summary>
        [NotMapped]
        public string UserIds { get; set; }
    }
}