using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using YiSha.Util;

namespace YiSha.Entity.OrganizationManage
{
    /// <summary>
    /// 部门表
    /// </summary>
    [Table("SysDepartment")]
    public class DepartmentEntity : BaseExtensionEntity
    {
        /// <summary>
        /// 父部门Id(0表示是根部门)
        /// </summary>
        [JsonConverter(typeof(StringJsonConverter))]
        public long? ParentId { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        public string? DepartmentName { get; set; }

        /// <summary>
        /// 部门电话
        /// </summary>
        public string? Telephone { get; set; }

        /// <summary>
        /// 部门传真
        /// </summary>
        public string? Fax { get; set; }

        /// <summary>
        /// 部门Email
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// 部门负责人Id
        /// </summary>
        [JsonConverter(typeof(StringJsonConverter))]
        public long? PrincipalId { get; set; }

        /// <summary>
        /// 部门排序
        /// </summary>
        public int? DepartmentSort { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }

        /// <summary>
        /// 多个部门Id
        /// </summary>
        [NotMapped]
        public string? Ids { get; set; }

        /// <summary>
        /// 负责人名称
        /// </summary>
        [NotMapped]
        public string? PrincipalName { get; set; }
    }
}
