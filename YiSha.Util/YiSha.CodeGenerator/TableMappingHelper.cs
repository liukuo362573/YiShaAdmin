using System;
using System.Linq;
using YiSha.Util.Extension;

namespace YiSha.CodeGenerator
{
    public static class TableMappingHelper
    {
        /// <summary>
        /// UserService转成userService
        /// </summary>
        public static string FirstLetterLowercase(string instanceName)
        {
            instanceName = instanceName.ParseToString();
            if (string.IsNullOrEmpty(instanceName))
            {
                return instanceName;
            }
            return instanceName[0].ToString().ToLower() + instanceName[1..];
        }

        /// <summary>
        /// UserService转成userService
        /// </summary>
        public static string FirstLetterUppercase(string instanceName)
        {
            instanceName = instanceName.ParseToString();
            if (string.IsNullOrEmpty(instanceName))
            {
                return instanceName;
            }
            return instanceName[0].ToString().ToUpper() + instanceName[1..];
        }

        /// <summary>
        /// base_district => BaseDistrict
        /// </summary>
        public static string GetClassNamePrefix(string tableName)
        {
            return string.Join("", tableName?.Split('_').Select(FirstLetterUppercase) ?? Array.Empty<string>());
        }

        public static string GetPropertyDatatype(string datatype)
        {
            return datatype?.ToLower() switch
            {
                "int" => "int?",
                "number" => "int?",
                "integer" => "int?",
                "smallint" => "int?",
                "bigint" => "long?",
                "tinyint" => "byte?",
                "numeric" => "Single?",
                "real" => "Single?",
                "float" => "float?",
                "decimal" => "decimal?",
                "numer(8,2)" => "decimal?",
                "bit" => "bool?",
                "date" => "DateTime?",
                "time" => "TimeSpan?",
                "datetime" => "DateTime?",
                "datetime2" => "DateTime?",
                "smalldatetime" => "DateTime?",
                "money" => "double?",
                "smallmoney" => "double?",
                "char" => "string",
                "varchar" => "string",
                "nvarchar2" => "string",
                "text" => "string",
                "nchar" => "string",
                "nvarchar" => "string",
                "ntext" => "string",
                _ => "string"
            };
        }
    }
}