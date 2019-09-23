using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YiSha.Enum.OrganizationManage
{
    public enum GenderTypeEnum
    {
        [Description("未知")]
        Unknown = 0,

        [Description("男")]
        Male = 1,

        [Description("女")]
        Female = 2
    }
}
