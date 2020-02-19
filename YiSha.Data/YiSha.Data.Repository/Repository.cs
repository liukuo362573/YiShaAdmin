using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using YiSha.Util;
using YiSha.Util.Model;

namespace YiSha.Data.Repository
{
    /// <summary>
    /// Copyright (c) 2018 合肥一沙软件
    /// 创建人：刘括
    /// 日 期：2018.10.18
    /// 描 述：定义仓储模型中的数据标准操作接口
    /// </summary>
    public class Repository
    {
        #region 构造
        public IDatabase db;
        public Repository(IDatabase iDatabase)
        {
            this.db = iDatabase;
        }
        #endregion

        #region 事务提交
        public async Task<Repository> BeginTrans()
        {
            await db.BeginTrans();
            return this;
        }
        public async Task<int> CommitTrans()
        {
            return await db.CommitTrans();
        }
        public async Task RollbackTrans()
        {
            await db.RollbackTrans();
        }
        #endregion

        #region 执行 SQL 语句
        public async Task<int> ExecuteBySql(string strSql)
        {
            return await db.ExecuteBySql(strSql);
        }
        public async Task<int> ExecuteBySql(string strSql, params DbParameter[] dbParameter)
        {
            return await db.ExecuteBySql(strSql, dbParameter);
        }
        public async Task<int> ExecuteByProc(string procName)
        {
            return await db.ExecuteByProc(procName);
        }
        public async Task<int> ExecuteByProc(string procName, params DbParameter[] dbParameter)
        {
            return await db.ExecuteByProc(procName, dbParameter);
        }
        #endregion

        #region 对象实体 添加、修改、删除
        public async Task<int> Insert<T>(T entity) where T : class
        {
            return await db.Insert<T>(entity);
        }
        public async Task<int> Insert<T>(List<T> entity) where T : class
        {
            return await db.Insert<T>(entity);
        }

        public async Task<int> Delete<T>() where T : class
        {
            return await db.Delete<T>();
        }
        public async Task<int> Delete<T>(T entity) where T : class
        {
            return await db.Delete<T>(entity);
        }
        public async Task<int> Delete<T>(List<T> entity) where T : class
        {
            return await db.Delete<T>(entity);
        }
        public async Task<int> Delete<T>(Expression<Func<T, bool>> condition) where T : class, new()
        {
            return await db.Delete<T>(condition);
        }
        public async Task<int> Delete<T>(long id) where T : class
        {
            return await db.Delete<T>(id);
        }
        public async Task<int> Delete<T>(long[] id) where T : class
        {
            return await db.Delete<T>(id);
        }
        public async Task<int> Delete<T>(string propertyName, long propertyValue) where T : class
        {
            return await db.Delete<T>(propertyName, propertyValue);
        }

        public async Task<int> Update<T>(T entity) where T : class
        {
            return await db.Update<T>(entity);
        }
        public async Task<int> Update<T>(List<T> entity) where T : class
        {
            return await db.Update<T>(entity);
        }
        public async Task<int> UpdateAllField<T>(T entity) where T : class
        {
            return await db.UpdateAllField<T>(entity);
        }
        public async Task<int> Update<T>(Expression<Func<T, bool>> condition) where T : class, new()
        {
            return await db.Update<T>(condition);
        }

        public IQueryable<T> IQueryable<T>(Expression<Func<T, bool>> condition) where T : class, new()
        {
            return db.IQueryable<T>(condition);
        }
        #endregion

        #region 对象实体 查询
        public async Task<T> FindEntity<T>(long id) where T : class
        {
            return await db.FindEntity<T>(id);
        }
        public async Task<T> FindEntity<T>(Expression<Func<T, bool>> condition) where T : class, new()
        {
            return await db.FindEntity<T>(condition);
        }

        public async Task<IEnumerable<T>> FindList<T>() where T : class, new()
        {
            return await db.FindList<T>();
        }
        public async Task<IEnumerable<T>> FindList<T>(Expression<Func<T, bool>> condition) where T : class, new()
        {
            return await db.FindList<T>(condition);
        }
        public async Task<IEnumerable<T>> FindList<T>(string strSql) where T : class
        {
            return await db.FindList<T>(strSql);
        }
        public async Task<IEnumerable<T>> FindList<T>(string strSql, DbParameter[] dbParameter) where T : class
        {
            return await db.FindList<T>(strSql, dbParameter);
        }
        public async Task<(int total, IEnumerable<T> list)> FindList<T>(Pagination pagination) where T : class, new()
        {
            int total = pagination.TotalCount;
            var data = await db.FindList<T>(pagination.Sort, pagination.SortType.ToLower() == "asc" ? true : false, pagination.PageSize, pagination.PageIndex);
            pagination.TotalCount = total;
            return data;
        }
        public async Task<IEnumerable<T>> FindList<T>(Expression<Func<T, bool>> condition, Pagination pagination) where T : class, new()
        {
            var data = await db.FindList<T>(condition, pagination.Sort, pagination.SortType.ToLower() == "asc" ? true : false, pagination.PageSize, pagination.PageIndex);
            pagination.TotalCount = data.total;
            return data.list;
        }
        public async Task<(int total, IEnumerable<T> list)> FindList<T>(string strSql, Pagination pagination) where T : class
        {
            int total = pagination.TotalCount;
            var data = await db.FindList<T>(strSql, pagination.Sort, pagination.SortType.ToLower() == "asc" ? true : false, pagination.PageSize, pagination.PageIndex);
            pagination.TotalCount = total;
            return data;
        }
        public async Task<IEnumerable<T>> FindList<T>(string strSql, DbParameter[] dbParameter, Pagination pagination) where T : class
        {
            var data = await db.FindList<T>(strSql, dbParameter, pagination.Sort, pagination.SortType.ToLower() == "asc" ? true : false, pagination.PageSize, pagination.PageIndex);
            pagination.TotalCount = data.total;
            return data.Item2;
        }
        private async Task<(int total, IEnumerable<T> list)> FindList<T>(IQueryable<T> tempData, string orderField, bool isAsc, int pageSize, int pageIndex)
        {
            string[] _order = orderField.Split(',');
            MethodCallExpression resultExp = null;
            foreach (string item in _order)
            {
                string _orderPart = item;
                _orderPart = Regex.Replace(_orderPart, @"\s+", " ");
                string[] _orderArry = _orderPart.Split(' ');
                string _orderField = _orderArry[0];
                bool sort = isAsc;
                if (_orderArry.Length == 2)
                {
                    isAsc = _orderArry[1].ToUpper() == "ASC" ? true : false;
                }
                var parameter = Expression.Parameter(typeof(T), "t");
                var property = typeof(T).GetProperty(_orderField);
                var propertyAccess = Expression.MakeMemberAccess(parameter, property);
                var orderByExp = Expression.Lambda(propertyAccess, parameter);
                resultExp = Expression.Call(typeof(Queryable), isAsc ? "OrderBy" : "OrderByDescending", new Type[] { typeof(T), property.PropertyType }, tempData.Expression, Expression.Quote(orderByExp));
            }
            tempData = tempData.Provider.CreateQuery<T>(resultExp);
            var total = tempData.Count();
            tempData = tempData.Skip<T>(pageSize * (pageIndex - 1)).Take<T>(pageSize).AsQueryable();
            var list = await tempData.ToListAsync();
            return (total, list);
        }
        #endregion

        #region 数据源 查询
        public async Task<DataTable> FindTable(string strSql)
        {
            return await db.FindTable(strSql);
        }
        public async Task<DataTable> FindTable(string strSql, DbParameter[] dbParameter)
        {
            return await db.FindTable(strSql, dbParameter);
        }
        public async Task<DataTable> FindTable(string strSql, Pagination pagination)
        {
            var data = await db.FindTable(strSql, pagination.Sort, pagination.SortType.ToLower() == "asc" ? true : false, pagination.PageSize, pagination.PageIndex);
            pagination.TotalCount = data.total;
            return data.Item2;
        }
        public async Task<DataTable> FindTable(string strSql, DbParameter[] dbParameter, Pagination pagination)
        {
            var data = await db.FindTable(strSql, dbParameter, pagination.Sort, pagination.SortType.ToLower() == "asc" ? true : false, pagination.PageSize, pagination.PageIndex);
            pagination.TotalCount = data.total;
            return data.Item2;
        }

        public async Task<object> FindObject(string strSql)
        {
            return await db.FindObject(strSql);
        }
        public async Task<object> FindObject(string strSql, DbParameter[] dbParameter)
        {
            return await db.FindObject(strSql, dbParameter);
        }
        public async Task<T> FindObject<T>(string strSql) where T : class
        {
            return await db.FindObject<T>(strSql);
        }
        #endregion
    }
}
