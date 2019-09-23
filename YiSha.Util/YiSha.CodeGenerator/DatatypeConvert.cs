using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YiSha.CodeGenerator
{
    public class DatatypeConvert
    {
        public static string GetDatatype(string sDatatype)
        {
            string sTempDatatype = string.Empty;
            sDatatype = sDatatype.ToLower();
            switch (sDatatype)
            {
                case "int":
                case "number":
                case "integer":
                case "smallint":
                    sTempDatatype = "int?";
                    break;

                case "bigint":
                    sTempDatatype = "long?";
                    break;

                case "tinyint":
                    sTempDatatype = "byte?";
                    break;

                case "numeric":
                case "real":
                    sTempDatatype = "Single?";
                    break;

                case "float":
                    sTempDatatype = "float?";
                    break;

                case "decimal":
                case "numer(8,2)":
                    sTempDatatype = "decimal?";
                    break;

                case "bit":
                    sTempDatatype = "bool?";
                    break;

                case "datetime":
                case "date":
                case "smalldatetime":
                    sTempDatatype = "DateTime?";
                    break;

                case "money":
                case "smallmoney":
                    sTempDatatype = "double?";
                    break;

                case "char":
                case "varchar":
                case "nvarchar2":
                case "text":
                case "nchar":
                case "nvarchar":
                case "ntext":
                default:
                    sTempDatatype = "string";
                    break;
            }
            return sTempDatatype;
        }
    }
}
