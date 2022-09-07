﻿namespace YiSha.Model.Param.SystemManage
{
    public class LogLoginListParam : DateTimeParam
    {
        public string UserName { get; set; }
        public int? LogStatus { get; set; }
        public string IpAddress { get; set; }
    }

    public class LogOperateListParam : DateTimeParam
    {
        public string UserName { get; set; }
        public string ExecuteUrl { get; set; }
        public int? LogStatus { get; set; }
    }
}
