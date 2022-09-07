﻿using System.ComponentModel.DataAnnotations.Schema;
using YiSha.Entity.Default;

namespace YiSha.Entity.Models
{
    /// <summary>
    /// 部门表
    /// </summary>
    [Table("SysDepartment")]
    public class SysDepartment : SysDefaultModel
    {
        /// <summary>
        /// 父部门Id(0表示是根部门)
        /// </summary>
        [Column("ParentId")]
        public long ParentId { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        [Column("DepartmentName")]
        public string DepartmentName { get; set; }

        /// <summary>
        /// 部门电话
        /// </summary>
        [Column("Telephone")]
        public string Telephone { get; set; }

        /// <summary>
        /// 部门传真
        /// </summary>
        [Column("Fax")]
        public string Fax { get; set; }

        /// <summary>
        /// 部门Email
        /// </summary>
        [Column("Email")]
        public string Email { get; set; }

        /// <summary>
        /// 部门负责人Id
        /// </summary>
        [Column("PrincipalId")]
        public long PrincipalId { get; set; }

        /// <summary>
        /// 部门排序
        /// </summary>
        [Column("DepartmentSort")]
        public int DepartmentSort { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Column("Remark")]
        public string Remark { get; set; }
    }
}
