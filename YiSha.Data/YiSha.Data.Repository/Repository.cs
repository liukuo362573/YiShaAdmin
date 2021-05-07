using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using YiSha.Util.Model;

namespace YiSha.Data.Repository
{
    /// <summary>
    /// 创建人：admin
    /// 日 期：2018.10.18
    /// 描 述：定义仓储模型中的数据标准操作接口
    /// </summary>
    public class Repository
    {
        #region Constructor

        public readonly IDatabase Db;

        public Repository(IDatabase iDb)
        {
            Db = iDb;
        }

        #endregion Constructor

        #region Transaction

        public async Task<Repository> BeginTrans()
        {
            await Db.BeginTrans();
            return this;
        }

        public async Task<int> CommitTrans()
        {
            return await Db.CommitTrans();
        }

        public async Task RollbackTrans()
        {
            await Db.RollbackTrans();
        }

        #endregion Transaction

        #region Execute

        public async Task<int> ExecuteBySql(string sql, params DbParameter[] dbParameter)
        {
            return await Db.ExecuteBySql(sql, dbParameter);
        }

        public async Task<int> ExecuteByProc(string procName, params DbParameter[] dbParameter)
        {
            return await Db.ExecuteByProc(procName, dbParameter);
        }

        #endregion Execute

        #region Insert

        public async Task<int> Insert<T>(T entity) where T : class
        {
            return await Db.Insert(entity);
        }

        public async Task<int> Insert<T>(IEnumerable<T> entity) where T : class
        {
            return await Db.Insert(entity);
        }

        #endregion Insert

        #region Delete

        public async Task<int> Delete<T>() where T : class
        {
            return await Db.Delete<T>();
        }

        public async Task<int> Delete<T>(T entity) where T : class
        {
            return await Db.Delete(entity);
        }

        public async Task<int> Delete<T>(IEnumerable<T> entity) where T : class
        {
            return await Db.Delete(entity);
        }

        public async Task<int> Delete<T>(Expression<Func<T, bool>> condition) where T : class, new()
        {
            return await Db.Delete(condition);
        }

        public async Task<int> Delete<T>(params object[] id) where T : class
        {
            return await Db.Delete<T>(id);
        }

        public async Task<int> Delete<T>(string propertyName, object propertyValue) where T : class
        {
            return await Db.Delete<T>(propertyName, propertyValue);
        }

        #endregion Delete

        #region Update

        public async Task<int> Update<T>(T entity) where T : class
        {
            return await Db.Update(entity);
        }

        public async Task<int> Update<T>(IEnumerable<T> entity) where T : class
        {
            return await Db.Update(entity);
        }

        public async Task<int> Update<T>(Expression<Func<T, bool>> condition) where T : class, new()
        {
            return await Db.Update(condition);
        }

        #endregion Update

        #region Find

        public IQueryable<T> AsQueryable<T>(Expression<Func<T, bool>> condition) where T : class, new()
        {
            return Db.AsQueryable(condition);
        }

        public async Task<T> FindEntity<T>(long id) where T : class
        {
            return await Db.FindEntity<T>(id);
        }

        public async Task<T> FindEntity<T>(Expression<Func<T, bool>> condition) where T : class, new()
        {
            return await Db.FindEntity(condition);
        }

        public async Task<T> FindEntity<T>(string sql, params DbParameter[] dbParameter)
        {
            return await Db.FindEntity<T>(sql, dbParameter);
        }

        public async Task<IEnumerable<T>> FindList<T>() where T : class, new()
        {
            return await Db.FindList<T>();
        }

        public async Task<IEnumerable<T>> FindList<T>(Expression<Func<T, bool>> condition) where T : class, new()
        {
            return await Db.FindList(condition);
        }

        public async Task<IEnumerable<T>> FindList<T>(string strSql) where T : class
        {
            return await Db.FindList<T>(strSql);
        }

        public async Task<IEnumerable<T>> FindList<T>(string strSql, DbParameter[] dbParameter) where T : class
        {
            return await Db.FindList<T>(strSql, dbParameter);
        }

        public async Task<(int total, IEnumerable<T> list)> FindList<T>(Pagination pagination) where T : class, new()
        {
            int total = pagination.TotalCount;
            var data = await Db.FindList<T>(pagination.Sort, pagination.SortType.ToLower() == "asc", pagination.PageSize, pagination.PageIndex, null);
            pagination.TotalCount = total;
            return data;
        }

        public async Task<IEnumerable<T>> FindList<T>(Expression<Func<T, bool>> condition, Pagination pagination) where T : class, new()
        {
            var data = await Db.FindList(pagination.Sort, pagination.SortType.ToLower() == "asc", pagination.PageSize, pagination.PageIndex, condition);
            pagination.TotalCount = data.total;
            return data.list;
        }

        public async Task<(int total, IEnumerable<T> list)> FindList<T>(string strSql, Pagination pagination) where T : class
        {
            int total = pagination.TotalCount;
            var data = await Db.FindList<T>(strSql, pagination.Sort, pagination.SortType.ToLower() == "asc", pagination.PageSize, pagination.PageIndex);
            pagination.TotalCount = total;
            return data;
        }

        public async Task<IEnumerable<T>> FindList<T>(string strSql, DbParameter[] dbParameter, Pagination pagination) where T : class
        {
            var data = await Db.FindList<T>(strSql, pagination.Sort, pagination.SortType.ToLower() == "asc", pagination.PageSize, pagination.PageIndex, dbParameter);
            pagination.TotalCount = data.total;
            return data.Item2;
        }

        public async Task<DataTable> FindTable(string sql, params DbParameter[] dbParameter)
        {
            return await Db.FindTable(sql, dbParameter);
        }

        public async Task<DataTable> FindTable(string sql, Pagination pagination, params DbParameter[] dbParameter)
        {
            var (total, dataTable) = await Db.FindTable(sql, pagination.Sort, pagination.SortType.ToLower() == "asc", pagination.PageSize, pagination.PageIndex, dbParameter);
            pagination.TotalCount = total;
            return dataTable;
        }

        #endregion
    }
}