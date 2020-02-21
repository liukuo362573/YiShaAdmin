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
        Task<IDatabase> BeginTrans();
        Task<int> CommitTrans();
        Task RollbackTrans();
        Task Close();

        Task<int> ExecuteBySql(string strSql);
        Task<int> ExecuteBySql(string strSql, params DbParameter[] dbParameter);
        Task<int> ExecuteByProc(string procName);
        Task<int> ExecuteByProc(string procName, DbParameter[] dbParameter);

        Task<int> Insert<T>(T entity) where T : class;
        Task<int> Insert<T>(IEnumerable<T> entities) where T : class;

        Task<int> Delete<T>() where T : class;
        Task<int> Delete<T>(T entity) where T : class;
        Task<int> Delete<T>(IEnumerable<T> entities) where T : class;
        Task<int> Delete<T>(Expression<Func<T, bool>> condition) where T : class, new();
        Task<int> Delete<T>(long id) where T : class;
        Task<int> Delete<T>(long[] id) where T : class;
        Task<int> Delete<T>(string propertyName, long propertyValue) where T : class;

        Task<int> Update<T>(T entity) where T : class;
        Task<int> Update<T>(IEnumerable<T> entities) where T : class;
        Task<int> UpdateAllField<T>(T entity) where T : class;
        Task<int> Update<T>(Expression<Func<T, bool>> condition) where T : class, new();

        IQueryable<T> IQueryable<T>(Expression<Func<T, bool>> condition) where T : class, new();

        Task<T> FindEntity<T>(object KeyValue) where T : class;
        Task<T> FindEntity<T>(Expression<Func<T, bool>> condition) where T : class, new();

        Task<IEnumerable<T>> FindList<T>() where T : class, new();
        Task<IEnumerable<T>> FindList<T>(Func<T, object> orderby) where T : class, new();
        Task<IEnumerable<T>> FindList<T>(Expression<Func<T, bool>> condition) where T : class, new();
        Task<IEnumerable<T>> FindList<T>(string strSql) where T : class;
        Task<IEnumerable<T>> FindList<T>(string strSql, DbParameter[] dbParameter) where T : class;
        Task<(int total, IEnumerable<T> list)> FindList<T>(string orderField, bool isAsc, int pageSize, int pageIndex) where T : class, new();
        Task<(int total, IEnumerable<T> list)> FindList<T>(Expression<Func<T, bool>> condition, string orderField, bool isAsc, int pageSize, int pageIndex) where T : class, new();
        Task<(int total, IEnumerable<T>)> FindList<T>(string strSql, string orderField, bool isAsc, int pageSize, int pageIndex) where T : class;
        Task<(int total, IEnumerable<T>)> FindList<T>(string strSql, DbParameter[] dbParameter, string orderField, bool isAsc, int pageSize, int pageIndex) where T : class;

        Task<DataTable> FindTable(string strSql);
        Task<DataTable> FindTable(string strSql, DbParameter[] dbParameter);
        Task<(int total, DataTable)> FindTable(string strSql, string orderField, bool isAsc, int pageSize, int pageIndex);
        Task<(int total, DataTable)> FindTable(string strSql, DbParameter[] dbParameter, string orderField, bool isAsc, int pageSize, int pageIndex);

        Task<object> FindObject(string strSql);
        Task<object> FindObject(string strSql, DbParameter[] dbParameter);
        Task<T> FindObject<T>(string strSql) where T : class;
    }
}
