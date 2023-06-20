using System.ComponentModel.DataAnnotations.Schema;

namespace YiSha.Entity.OrganizationManage
{
    [Table("SysDepartment")]
    public class DepartmentEntity : BaseExtensionEntity
    {
        public long? ParentId { get; set; }
        public string DepartmentName { get; set; }
        public string Telephone { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public long? PrincipalId { get; set; }
        public int? DepartmentSort { get; set; }
        public string Remark { get; set; }

        /// <summary>
        /// 多个部门Id
        /// </summary>
        [NotMapped]
        public string Ids { get; set; }

        /// <summary>
        /// 负责人名称
        /// </summary>
        [NotMapped]
        public string PrincipalName { get; set; }
    }
}
