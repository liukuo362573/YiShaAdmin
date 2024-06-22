using System;
using System.Collections.Generic;
using YiSha.Entity.OrganizationManage;
using YiSha.Util.Extension;

namespace YiSha.Model.Param.OrganizationManage
{
    public class UserListParam : DateTimeParam
    {
        [QueryCompareAttribute(Compare = CompareEnum.Contains)]
        public string UserName { get; set; }
        [QueryCompareAttribute(Compare = CompareEnum.Contains)]
        public string Mobile { get; set; }
        [QueryCompareAttribute(Compare = CompareEnum.Equals)]
        public int? UserStatus { get; set; }
        public long? DepartmentId { get; set; }
        [QueryCompareAttribute(FieldName = "DepartmentId", Compare = CompareEnum.In)]
        public List<long> ChildrenDepartmentIdList { get; set; }
        [QueryCompareAttribute(FieldName ="Id",Compare = CompareEnum.In)]
        public long[] UserIds { get; set; }
    }

    public class ChangePasswordParam
    {
        public long? Id { get; set; }
        public string Password { get; set; }
        public string NewPassword { get; set; }
    }

}
