using System;
using System.Data.Common;
using Microsoft.Data.SqlClient;
using MySqlConnector;
using Oracle.ManagedDataAccess.Client;
using System.Linq;
using YiSha.Util.Extension;

namespace YiSha.Data
{
    public static class DbParameterHelper
    {
        /// <summary>
        /// 根据配置文件中所配置的数据库类型
        /// 来创建相应数据库的参数对象
        /// </summary>
        public static DbParameter CreateDbParameter(string name, object value)
        {
            return DbHelper.DbType switch
            {
                DatabaseType.SqlServer => new SqlParameter(name, value),
                DatabaseType.MySql => new MySqlParameter(name, value),
                DatabaseType.Oracle => new OracleParameter(name, value),
                _ => throw new Exception("数据库类型目前不支持！")
            };
        }

        public static DbParameter[] CreateDbParameters(string name, object[] values)
        {
            return values.TryAny() ? values.Select((_, i) => CreateDbParameter(name + i, values[i])).ToArray() : null;
        }
    }
}
