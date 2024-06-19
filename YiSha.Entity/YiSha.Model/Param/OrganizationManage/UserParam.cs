using System;
using System.Collections.Generic;
using YiSha.Entity.OrganizationManage;
using YiSha.Util.Extension;

namespace YiSha.Model.Param.OrganizationManage
{
    public class UserListParam : DateTimeParam
    {
        public string UserName { get; set; }

        public string Mobile { get; set; }

        public int? UserStatus { get; set; }

        public long? DepartmentId { get; set; }
        [LinqExpressionXAttribute(IsIgnore = true)]
        public List<long> ChildrenDepartmentIdList { get; set; }
        [LinqExpressionXAttribute(IsIgnore = true)]
        public string UserIds { get; set; }
    }

    public class ChangePasswordParam
    {
        public long? Id { get; set; }
        public string Password { get; set; }
        public string NewPassword { get; set; }
    }

}
