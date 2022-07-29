using System.ComponentModel.DataAnnotations.Schema;

namespace YiSha.Web.Code
{
    /// <summary>
    /// 操作员信息
    /// </summary>
    public class OperatorInfo
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public long? UserId { get; set; }

        /// <summary>
        /// 用户状态
        /// </summary>
        public int? UserStatus { get; set; }

        /// <summary>
        /// 是否在线
        /// </summary>
        public int? IsOnline { get; set; }

        /// <summary>
        /// 用户名称
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 真正名称
        /// </summary>
        public string RealName { get; set; }

        /// <summary>
        /// WebToken
        /// </summary>
        public string WebToken { get; set; }

        /// <summary>
        /// ApiToken
        /// </summary>
        public string ApiToken { get; set; }

        /// <summary>
        /// 是否系统用户
        /// </summary>
        public int? IsSystem { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public string Portrait { get; set; }

        /// <summary>
        /// 部门ID
        /// </summary>
        public long? DepartmentId { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        [NotMapped]
        public string DepartmentName { get; set; }

        /// <summary>
        /// 岗位ID
        /// </summary>
        [NotMapped]
        public string PositionIds { get; set; }

        /// <summary>
        /// 角色ID
        /// </summary>
        [NotMapped]
        public string RoleIds { get; set; }
    }

    /// <summary>
    /// 角色信息
    /// </summary>
    public class RoleInfo
    {
        /// <summary>
        /// 角色ID
        /// </summary>
        public long RoleId { get; set; }
    }

}
