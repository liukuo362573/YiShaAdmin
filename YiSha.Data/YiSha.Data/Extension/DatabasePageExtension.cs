using System;
using System.Text;
using System.Data.Common;
using YiSha.Util;
using System.Linq;

namespace YiSha.Data
{
    public class DatabasePageExtension
    {
        public static StringBuilder SqlServerPageSql(string strSql, DbParameter[] dbParameter, string sort, bool isAsc, int pageSize, int pageIndex)
        {
            CheckSqlParam(sort);

            StringBuilder sb = new StringBuilder();
            if (pageIndex == 0)
            {
                pageIndex = 1;
            }
            int startNum = (pageIndex - 1) * pageSize;
            int endNum = (pageIndex) * pageSize;
            string orderBy = BuildOrderBy(sort, isAsc);

            sb.Append("SELECT * FROM (SELECT ROW_NUMBER() Over (" + orderBy + ")");
            sb.Append(" AS ROWNUM, * From (" + strSql + ") t ) AS N WHERE ROWNUM > " + startNum + " AND ROWNUM <= " + endNum + "");
            return sb;
        }

        public static StringBuilder OraclePageSql(string strSql, DbParameter[] dbParameter, string sort, bool isAsc, int pageSize, int pageIndex)
        {
            CheckSqlParam(sort);

            StringBuilder sb = new StringBuilder();
            if (pageIndex == 0)
            {
                pageIndex = 1;
            }
            int startNum = (pageIndex - 1) * pageSize;
            int endNum = (pageIndex) * pageSize;
            string orderBy = BuildOrderBy(sort, isAsc);

            sb.Append("SELECT * From (SELECT ROWNUM AS n,");
            sb.Append(" T.* From (" + strSql + orderBy + ") t )  N WHERE n > " + startNum + " AND n <= " + endNum + "");
            return sb;
        }

        public static StringBuilder MySqlPageSql(string strSql, DbParameter[] dbParameter, string sort, bool isAsc, int pageSize, int pageIndex)
        {
            CheckSqlParam(sort);

            StringBuilder sb = new StringBuilder();
            if (pageIndex == 0)
            {
                pageIndex = 1;
            }
            int num = (pageIndex - 1) * pageSize;
            string orderBy = BuildOrderBy(sort, isAsc);

            sb.Append(strSql + orderBy);
            sb.Append(" LIMIT " + num + "," + pageSize + "");
            return sb;
        }

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

        private static string BuildOrderBy(string sort, bool isAsc)
        {
            string orderBy = string.Empty;
            if (!string.IsNullOrEmpty(sort))
            {
                string sortUpper = sort.ToUpper();
                CheckSort(sortUpper);

                if (sortUpper.IndexOf("ASC") + sortUpper.IndexOf("DESC") > 0)
                {
                    orderBy = " ORDER BY " + sort;
                }
                else
                {
                    orderBy = " ORDER BY " + sort + " " + (isAsc ? "ASC" : "DESC");
                }
            }
            return orderBy;
        }

        private static void CheckSort(string sort)
        {
            string[] sortArr = TextHelper.SplitToArray<string>(sort, new string[] { ",", " " });
            int sortTypeCount = sortArr.Where(p => p == "ASC" || p == "DESC").Count();
            int sortFieldCount = sortArr.Length - sortTypeCount;
            if (sortFieldCount > 5)
            {
                throw new ArgumentException("排序字段过多");
            }
        }

        private static void CheckSqlParam(string param)
        {
            if (!SecurityHelper.IsSafeSqlParam(param))
            {
                throw new ArgumentException("含有Sql注入的参数");
            }
        }
    }
}
