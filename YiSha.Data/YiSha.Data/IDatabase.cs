using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace YiSha.Data
{
    public interface IDatabase
    {
        #region 属性

        /// <summary>
        /// 获取 当前使用的数据访问上下文对象
        /// </summary>
        public DbContext dbContext { get; set; }

        /// <summary>
        /// 事务对象
        /// </summary>
        public IDbContextTransaction DbContextTransaction { get; set; }

        #endregion 属性

        #region Transaction

        Task<IDatabase> BeginTrans();

        Task<int> CommitTrans();

        Task RollbackTrans();

        Task Close();

        #endregion Transaction

        #region Execute

        Task<int> ExecuteBySql(string sql, params DbParameter[] dbParameter);

        Task<int> ExecuteByProc(string procName, params DbParameter[] dbParameter);

        #endregion Execute

        #region Insert

        Task<int> Insert<T>(T entity) where T : class;

        Task<int> Insert<T>(IEnumerable<T> entities) where T : class;

        #endregion Insert

        #region Delete

        Task<int> Delete<T>() where T : class;

        Task<int> Delete<T>(T entity) where T : class;

        Task<int> Delete<T>(IEnumerable<T> entities) where T : class;

        Task<int> Delete<T>(Expression<Func<T, bool>> condition) where T : class, new();

        Task<int> Delete<T>(params object[] id) where T : class;

        Task<int> Delete<T>(string propertyName, object propertyValue) where T : class;

        #endregion Delete

        #region Update

        Task<int> Update<T>(T entity) where T : class;

        Task<int> Update<T>(IEnumerable<T> entities) where T : class;

        Task<int> Update<T>(Expression<Func<T, bool>> condition) where T : class, new();

        #endregion Update

        #region Find

        IQueryable<T> AsQueryable<T>(Expression<Func<T, bool>> condition) where T : class, new();

        Task<T> FindEntity<T>(object keyValue) where T : class;

        Task<T> FindEntity<T>(Expression<Func<T, bool>> condition) where T : class, new();

        Task<T> FindEntity<T>(string sort, bool isAsc) where T : class, new();

        Task<T> FindEntity<T>(string sql, params DbParameter[] dbParameter);

        Task<List<T>> FindList<T>() where T : class, new();

        Task<List<T>> FindList<T>(Expression<Func<T, bool>> condition) where T : class, new();

        Task<List<T>> FindList<T>(string sql, params DbParameter[] dbParameter) where T : class;

        Task<(int total, List<T> list)> FindList<T>(string sort, bool isAsc, int pageSize, int pageIndex, Expression<Func<T, bool>> condition) where T : class, new();

        Task<(int total, List<T>)> FindList<T>(string sql, string sort, bool isAsc, int pageSize, int pageIndex, params DbParameter[] dbParameter);

        Task<DataTable> FindTable(string sql, params DbParameter[] dbParameter);

        Task<(int total, DataTable)> FindTable(string sql, string sort, bool isAsc, int pageSize, int pageIndex, params DbParameter[] dbParameter);

        #endregion Find
    }
}