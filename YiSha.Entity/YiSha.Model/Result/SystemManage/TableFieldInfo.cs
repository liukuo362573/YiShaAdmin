using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YiSha.Model.Result.SystemManage
{
    public class TableFieldInfo
    {
        public string TableColumn { get; set; }

        public string Datatype { get; set; }

        public string FieldLength { get; set; }

        public string IsNullable { get; set; }
        public string TableIdentity { get; set; }
        public string Key { get; set; }
        public string FieldDefault { get; set; }
        public string Remark { get; set; }
    }
}
