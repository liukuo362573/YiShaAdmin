using System.Text;
using System.Data.Common;

namespace YiSha.Data
{
    public class DatabasePageExtension
    {
        public StringBuilder SqlPageSql(string strSql, DbParameter[] dbParameter, string orderField, bool isAsc, int pageSize, int pageIndex)
        {
            StringBuilder sb = new StringBuilder();
            if (pageIndex == 0)
            {
                pageIndex = 1;
            }
            int num = (pageIndex - 1) * pageSize;
            int num1 = (pageIndex) * pageSize;
            string OrderBy = "";

            if (!string.IsNullOrEmpty(orderField))
            {
                if (orderField.ToUpper().IndexOf("ASC") + orderField.ToUpper().IndexOf("DESC") > 0)
                {
                    OrderBy = " ORDER BY " + orderField;
                }
                else
                {
                    OrderBy = " ORDER BY " + orderField + " " + (isAsc ? "ASC" : "DESC");
                }
            }
            else
            {
                OrderBy = "ORDERE BY (SELECT 0)";
            }
            sb.Append("SELECT * FROM (SELECT ROW_NUMBER() Over (" + OrderBy + ")");
            sb.Append(" AS ROWNUM, * From (" + strSql + ") t ) AS N WHERE ROWNUM > " + num + " AND ROWNUM <= " + num1 + "");
            return sb;
        }

        public StringBuilder OraclePageSql(string strSql, DbParameter[] dbParameter, string orderField, bool isAsc, int pageSize, int pageIndex)
        {
            StringBuilder sb = new StringBuilder();
            if (pageIndex == 0)
            {
                pageIndex = 1;
            }
            int num = (pageIndex - 1) * pageSize;
            int num1 = (pageIndex) * pageSize;
            string OrderBy = "";

            if (!string.IsNullOrEmpty(orderField))
            {
                if (orderField.ToUpper().IndexOf("ASC") + orderField.ToUpper().IndexOf("DESC") > 0)
                {
                    OrderBy = " ORDER BY " + orderField;
                }
                else
                {
                    OrderBy = " ORDER BY " + orderField + " " + (isAsc ? "ASC" : "DESC");
                }
            }
            sb.Append("SELECT * From (SELECT ROWNUM AS n,");
            sb.Append(" T.* From (" + strSql + OrderBy + ") t )  N WHERE n > " + num + " AND n <= " + num1 + "");
            return sb;
        }

        public StringBuilder MySqlPageSql(string strSql, DbParameter[] dbParameter, string orderField, bool isAsc, int pageSize, int pageIndex)
        {
            StringBuilder sb = new StringBuilder();
            if (pageIndex == 0)
            {
                pageIndex = 1;
            }
            int num = (pageIndex - 1) * pageSize;
            string OrderBy = "";

            if (!string.IsNullOrEmpty(orderField))
            {
                if (orderField.ToUpper().IndexOf("ASC") + orderField.ToUpper().IndexOf("DESC") > 0)
                {
                    OrderBy = " ORDER BY " + orderField;
                }
                else
                {
                    OrderBy = " ORDER BY " + orderField + " " + (isAsc ? "ASC" : "DESC");
                }
            }
            sb.Append(strSql + OrderBy);
            sb.Append(" LIMIT " + num + "," + pageSize + "");
            return sb;
        }

        public string GetCountSql(string strSql)
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
    }
}
