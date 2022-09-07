using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using YiSha.Entity.Default;

namespace YiSha.Entity.Models
{
    /// <summary>
    /// 用户表
    /// </summary>
    [Table("SysUser")]
    public class SysUser : SysDefaultModel
    {
        /// <summary>
        /// 用户名
        /// </summary>
        [Column("UserName"), Description("用户名")]
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Column("Password")]
        public string Password { get; set; }

        /// <summary>
        /// 密码盐值
        /// </summary>
        [Column("Salt"), JsonIgnore]
        public string Salt { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        [Column("RealName"), Description("真实姓名")]
        public string RealName { get; set; }

        /// <summary>
        /// 性别(0未知 1男 2女)
        /// </summary>
        [Column("Gender"), Description("性别")]
        public int Gender { get; set; }

        /// <summary>
        /// 出生日期
        /// </summary>
        [Column("Birthday")]
        public string Birthday { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        [Column("Portrait")]
        public string Portrait { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        [Column("Email")]
        public string Email { get; set; }

        /// <summary>
        /// 手机
        /// </summary>
        [Column("Mobile"), Description("手机号")]
        public string Mobile { get; set; }

        /// <summary>
        /// QQ
        /// </summary>
        [Column("QQ")]
        public string QQ { get; set; }

        /// <summary>
        /// 微信
        /// </summary>
        [Column("WeChat")]
        public string WeChat { get; set; }

        /// <summary>
        /// 登录次数
        /// </summary>
        [Column("LoginCount")]
        public int LoginCount { get; set; }

        /// <summary>
        /// 用户状态(0禁用 1启用)
        /// </summary>
        [Column("UserStatus")]
        public int UserStatus { get; set; }

        /// <summary>
        /// 系统用户(0不是 1是[系统用户拥有所有的权限])
        /// </summary>
        [Column("IsSystem")]
        public int IsSystem { get; set; }

        /// <summary>
        /// 在线(0不是 1是)
        /// </summary>
        [Column("IsOnline")]
        public int IsOnline { get; set; }

        /// <summary>
        /// 首次登录时间
        /// </summary>
        [Column("FirstVisit")]
        public DateTime FirstVisit { get; set; }

        /// <summary>
        /// 上一次登录时间
        /// </summary>
        [Column("PreviousVisit")]
        public DateTime PreviousVisit { get; set; }

        /// <summary>
        /// 最后一次登录时间
        /// </summary>
        [Column("LastVisit")]
        public DateTime LastVisit { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Column("Remark")]
        public string Remark { get; set; }

        /// <summary>
        /// 后台Token
        /// </summary>
        [Column("WebToken")]
        public string WebToken { get; set; }

        /// <summary>
        /// ApiToken
        /// </summary>
        [Column("ApiToken")]
        public string ApiToken { get; set; }

        /// <summary>
        /// 所属部门Id
        /// </summary>
        [Column("DepartmentId")]
        public long DepartmentId { get; set; }
    }
}
