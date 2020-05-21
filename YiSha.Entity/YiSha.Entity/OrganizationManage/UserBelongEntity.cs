using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YiSha.Util;

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
