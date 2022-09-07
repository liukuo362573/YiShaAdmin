using System.ComponentModel.DataAnnotations.Schema;
using YiSha.Entity.Models;

namespace YiSha.Model.Entity.OrganizationManage
{
    /// <summary>
    /// 部门表拓展
    /// </summary>
    [NotMapped]
    public class DepartmentEntity : SysDepartment
    {
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
