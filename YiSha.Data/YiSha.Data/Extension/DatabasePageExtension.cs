using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                    OrderBy = " Order By " + orderField;
                }
                else
                {
                    OrderBy = " Order By " + orderField + " " + (isAsc ? "ASC" : "DESC");
                }
            }
            else
            {
                OrderBy = "order by (select 0)";
            }
            sb.Append("Select * From (Select ROW_NUMBER() Over (" + OrderBy + ")");
            sb.Append(" As rowNum, * From (" + strSql + ")  T ) As N Where rowNum > " + num + " And rowNum <= " + num1 + "");
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
                    OrderBy = " Order By " + orderField;
                }
                else
                {
                    OrderBy = " Order By " + orderField + " " + (isAsc ? "ASC" : "DESC");
                }
            }
            sb.Append("Select * From (Select ROWNUM as n,");
            sb.Append(" T.* From (" + strSql + OrderBy + ")  T )  N Where n > " + num + " And n <= " + num1 + "");
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
                    OrderBy = " Order By " + orderField;
                }
                else
                {
                    OrderBy = " Order By " + orderField + " " + (isAsc ? "ASC" : "DESC");
                }
            }
            sb.Append(strSql + OrderBy);
            sb.Append(" limit " + num + "," + pageSize + "");
            return sb;
        }
        public string GetCountSql(string strSql)
        {
            string countSql = string.Empty;
            string strSqlCopy = strSql.ToLower();
            int selectIndex = strSqlCopy.IndexOf("select ");
            int lastFromIndex = strSqlCopy.LastIndexOf(" from ");
            if (selectIndex >= 0 && lastFromIndex >= 0)
            {
                int backFromIndex = strSqlCopy.LastIndexOf(" from ", lastFromIndex);
                int backSelectIndex = strSqlCopy.LastIndexOf("select ", lastFromIndex);
                if (backSelectIndex == selectIndex)
                {
                    countSql = "select count(*) " + strSql.Substring(lastFromIndex);
                    return countSql;
                }
            }
            countSql = "Select Count(1) From (" + strSql + ") t";
            return countSql;
        }
    }
}
