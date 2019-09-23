using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YiSha.Enum.SystemManage
{
    public enum AutoJobStatusEnum
    {
        [Description("运行中")]
        Yes = 1,

        [Description("停止")]
        No = 2
    }
}
