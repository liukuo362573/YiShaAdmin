﻿namespace YiSha.Model.Param.SystemManage
{
    public class RoleListParam : DateTimeParam
    {
        public string RoleName { get; set; }

        public int? RoleStatus { get; set; }

        /// <summary>
        /// 多个角色Id
        /// </summary>
        public string RoleIds { get; set; }
    }
}