using Newtonsoft.Json;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using YiSha.Util;

namespace YiSha.Entity.OrganizationManage
{
    /// <summary>
    /// 用户表
    /// </summary>
    [Table("SysUser")]
    public class UserEntity : BaseExtensionEntity
    {

        /// <summary>
        /// 用户名
        /// </summary>
        [Description("用户名")]
        public string? UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string? Password { get; set; }

        /// <summary>
        /// 密码盐值
        /// </summary>
        [JsonIgnore]
        public string? Salt { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        [Description("真实姓名")]
        public string? RealName { get; set; }

        /// <summary>
        /// 性别(0未知 1男 2女)
        /// </summary>
        [Description("性别")]
        public int? Gender { get; set; }

        /// <summary>
        /// 出生日期
        /// </summary>
        public string? Birthday { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public string? Portrait { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// 手机
        /// </summary>
        [Description("手机号")]
        public string? Mobile { get; set; }

        /// <summary>
        /// QQ
        /// </summary>
        public string? QQ { get; set; }

        /// <summary>
        /// 微信
        /// </summary>
        public string? Wechat { get; set; }

        /// <summary>
        /// 登录次数
        /// </summary>
        public int? LoginCount { get; set; }

        /// <summary>
        /// 用户状态(0禁用 1启用)
        /// </summary>
        public int? UserStatus { get; set; }

        /// <summary>
        /// 系统用户(0不是 1是[系统用户拥有所有的权限])
        /// </summary>
        public int? IsSystem { get; set; }

        /// <summary>
        /// 在线(0不是 1是)
        /// </summary>
        public int? IsOnline { get; set; }

        /// <summary>
        /// 首次登录时间
        /// </summary>
        public DateTime? FirstVisit { get; set; }

        /// <summary>
        /// 上一次登录时间
        /// </summary>
        public DateTime? PreviousVisit { get; set; }

        /// <summary>
        /// 最后一次登录时间
        /// </summary>
        public DateTime? LastVisit { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }

        /// <summary>
        /// 后台Token
        /// </summary>
        public string? WebToken { get; set; }

        /// <summary>
        /// ApiToken
        /// </summary>
        public string? ApiToken { get; set; }

        /// <summary>
        /// 所属部门Id
        /// </summary>
        [JsonConverter(typeof(StringJsonConverter))]
        public long? DepartmentId { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        [NotMapped]
        public string? DepartmentName { get; set; }

        /// <summary>
        /// 岗位Id
        /// </summary>
        [NotMapped]
        public string? PositionIds { get; set; }

        /// <summary>
        /// 角色Id
        /// </summary>
        [NotMapped]
        public string? RoleIds { get; set; }
    }
}
