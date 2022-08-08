using System.Data.Common;
using System.Text;
using YiSha.DataBase.Enum;
using YiSha.Util;

namespace YiSha.DataBase.Extension
{
    /// <summary>
    /// 数据库分页扩展
    /// </summary>
    public class DbPageExtension
    {
        /// <summary>
        /// 获取分页脚本
        /// </summary>
        /// <param name="strSql">语句</param>
        /// <param name="dbParameter">参数</param>
        /// <param name="sort">排序</param>
        /// <param name="isAsc">Asc 排序</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="pageIndex">页索引</param>
        /// <returns>总行数，表数据</returns>
        /// <returns></returns>
        public static StringBuilder GetPageSql(string strSql, DbParameter[] dbParameter, string sort, bool isAsc, int pageSize, int pageIndex)
        {
            var sb = DbFactory.Type switch
            {
                DbType.SqlServer => SqlServerPageSql(strSql, dbParameter, sort, isAsc, pageSize, pageIndex),
                DbType.MySql => MySqlPageSql(strSql, dbParameter, sort, isAsc, pageSize, pageIndex),
                DbType.Oracle => OraclePageSql(strSql, dbParameter, sort, isAsc, pageSize, pageIndex),
                _ => throw new NotImplementedException(),
            };

            return sb;
        }

        /// <summary>
        /// 获取 SqlServer 分页脚本
        /// </summary>
        /// <param name="strSql">语句</param>
        /// <param name="dbParameter">参数</param>
        /// <param name="sort">排序</param>
        /// <param name="isAsc">Asc 排序</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="pageIndex">页索引</param>
        /// <returns></returns>
        private static StringBuilder SqlServerPageSql(string strSql, DbParameter[] dbParameter, string sort, bool isAsc, int pageSize, int pageIndex)
        {
            CheckSqlParam(sort);

            StringBuilder sb = new StringBuilder();
            if (pageIndex == 0)
            {
                pageIndex = 1;
            }
            int startNum = (pageIndex - 1) * pageSize;
            int endNum = (pageIndex) * pageSize;
            string orderBy = string.Empty;

            if (!string.IsNullOrEmpty(sort))
            {
                if (sort.ToUpper().IndexOf("ASC") + sort.ToUpper().IndexOf("DESC") > 0)
                {
                    orderBy = " ORDER BY " + sort;
                }
                else
                {
                    orderBy = " ORDER BY " + sort + " " + (isAsc ? "ASC" : "DESC");
                }
            }
            else
            {
                orderBy = "ORDERE BY (SELECT 0)";
            }
            sb.Append("SELECT * FROM (SELECT ROW_NUMBER() Over (" + orderBy + ")");
            sb.Append(" AS ROWNUM, * From (" + strSql + ") t ) AS N WHERE ROWNUM > " + startNum + " AND ROWNUM <= " + endNum + "");
            return sb;
        }

        /// <summary>
        /// 获取 MySql 分页脚本
        /// </summary>
        /// <param name="strSql">语句</param>
        /// <param name="dbParameter">参数</param>
        /// <param name="sort">排序</param>
        /// <param name="isAsc">Asc 排序</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="pageIndex">页索引</param>
        /// <returns></returns>
        private static StringBuilder MySqlPageSql(string strSql, DbParameter[] dbParameter, string sort, bool isAsc, int pageSize, int pageIndex)
        {
            CheckSqlParam(sort);

            StringBuilder sb = new StringBuilder();
            if (pageIndex == 0)
            {
                pageIndex = 1;
            }
            int num = (pageIndex - 1) * pageSize;
            string orderBy = string.Empty;

            if (!string.IsNullOrEmpty(sort))
            {
                if (sort.ToUpper().IndexOf("ASC") + sort.ToUpper().IndexOf("DESC") > 0)
                {
                    orderBy = " ORDER BY " + sort;
                }
                else
                {
                    orderBy = " ORDER BY " + sort + " " + (isAsc ? "ASC" : "DESC");
                }
            }
            sb.Append(strSql + orderBy);
            sb.Append(" LIMIT " + num + "," + pageSize + "");
            return sb;
        }

        /// <summary>
        /// 获取 Oracle 分页脚本
        /// </summary>
        /// <param name="strSql">语句</param>
        /// <param name="dbParameter">参数</param>
        /// <param name="sort">排序</param>
        /// <param name="isAsc">Asc 排序</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="pageIndex">页索引</param>
        /// <returns></returns>
        private static StringBuilder OraclePageSql(string strSql, DbParameter[] dbParameter, string sort, bool isAsc, int pageSize, int pageIndex)
        {
            CheckSqlParam(sort);

            StringBuilder sb = new StringBuilder();
            if (pageIndex == 0)
            {
                pageIndex = 1;
            }
            int startNum = (pageIndex - 1) * pageSize;
            int endNum = (pageIndex) * pageSize;
            string orderBy = string.Empty;

            if (!string.IsNullOrEmpty(sort))
            {
                if (sort.ToUpper().IndexOf("ASC") + sort.ToUpper().IndexOf("DESC") > 0)
                {
                    orderBy = " ORDER BY " + sort;
                }
                else
                {
                    orderBy = " ORDER BY " + sort + " " + (isAsc ? "ASC" : "DESC");
                }
            }
            sb.Append("SELECT * From (SELECT ROWNUM AS n,");
            sb.Append(" T.* From (" + strSql + orderBy + ") t )  N WHERE n > " + startNum + " AND n <= " + endNum + "");
            return sb;
        }

        /// <summary>
        /// 获取查询数量脚本
        /// </summary>
        /// <param name="strSql">语句</param>
        /// <returns></returns>
        public static string GetCountSql(string strSql)
        {
            string countSql = string.Empty;
            string strSqlCopy = strSql.ToLower();
            int selectIndex = strSqlCopy.IndexOf("SELECT ");
            int lastFromIndex = strSqlCopy.LastIndexOf(" FROM ");
            if (selectIndex >= 0 && lastFromIndex >= 0)
            {
                int backFromIndex = strSqlCopy.LastIndexOf(" FROM ", lastFromIndex);
                int backSelectIndex = strSqlCopy.LastIndexOf("SELECT ", lastFromIndex);
                if (backSelectIndex == selectIndex)
                {
                    countSql = "SELECT COUNT(*) " + strSql.Substring(lastFromIndex);
                    return countSql;
                }
            }
            countSql = "SELECT COUNT(1) FROM (" + strSql + ") t";
            return countSql;
        }

        /// <summary>
        /// 检查脚本是否有注入
        /// </summary>
        /// <param name="param"></param>
        /// <exception cref="ArgumentException"></exception>
        private static void CheckSqlParam(string param)
        {
            if (!SecurityHelper.IsSafeSqlParam(param))
            {
                throw new ArgumentException("含有Sql注入的参数");
            }
        }
    }
}
