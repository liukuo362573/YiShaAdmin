using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using YiSha.Data.Extension;
using YiSha.Util.Extension;
using YiSha.Util.Helper;

namespace YiSha.Data.EF.Database
{
    public abstract class AbstractDatabase : IDatabase
    {
        #region 属性

        /// <summary>
        /// 获取 当前使用的数据访问上下文对象
        /// </summary>
        public virtual Microsoft.EntityFrameworkCore.DbContext DbContext { get; set; }

        /// <summary>
        /// 事务对象
        /// </summary>
        public virtual IDbContextTransaction DbContextTransaction { get; set; }

        #endregion 属性

        #region Transaction

        /// <summary>
        /// 事务开始
        /// </summary>
        public virtual async Task<IDatabase> BeginTrans()
        {
            var dbConnection = DbContext.Database.GetDbConnection();
            if (dbConnection.State == ConnectionState.Closed)
            {
                await dbConnection.OpenAsync();
            }
            DbContextTransaction = await DbContext.Database.BeginTransactionAsync();
            return this;
        }

        /// <summary>
        /// 提交当前操作的结果
        /// </summary>
        public virtual async Task<int> CommitTrans()
        {
            try
            {
                DbContextExtension.SetEntityDefaultValue(DbContext);
                int result = await DbContext.SaveChangesAsync();
                if (DbContextTransaction != null)
                {
                    await DbContextTransaction?.CommitAsync();
                }
                return result;
            }
            finally
            {
                await Close();
            }
        }

        /// <summary>
        /// 把当前操作回滚成未提交状态
        /// </summary>
        public virtual async Task RollbackTrans()
        {
            await DbContextTransaction.RollbackAsync();
            await Close();
        }

        /// <summary>
        /// 关闭连接 内存回收
        /// </summary>
        public virtual async Task Close()
        {
            await DbContext.DisposeAsync();
        }

        #endregion Transaction

        #region Execute

        public virtual async Task<int> ExecuteBySql(string sql, params DbParameter[] dbParameter)
        {
            await DbContext.Database.ExecuteSqlRawAsync(sql, dbParameter);
            return DbContextTransaction == null ? await CommitTrans() : 0;
        }

        public virtual async Task<int> ExecuteByProc(string procName, params DbParameter[] dbParameter)
        {
            await DbContext.Database.ExecuteSqlRawAsync(DbContextExtension.BuilderProc(procName, dbParameter), dbParameter);
            return DbContextTransaction == null ? await CommitTrans() : 0;
        }

        #endregion Execute

        #region Insert

        public virtual async Task<int> Insert<T>(T entity) where T : class
        {
            await DbContext.AddAsync(entity);
            return DbContextTransaction == null ? await CommitTrans() : 0;
        }

        public virtual async Task<int> Insert<T>(IEnumerable<T> entities) where T : class
        {
            await DbContext.AddRangeAsync(entities);
            return DbContextTransaction == null ? await CommitTrans() : 0;
        }

        #endregion Insert

        #region Delete

        public virtual async Task<int> Delete<T>() where T : class
        {
            var entityType = DbContextExtension.GetEntityType<T>(DbContext);
            if (entityType != null)
            {
                var tableName = entityType.GetTableName();
                return await ExecuteBySql(DbContextExtension.DeleteSql(tableName));
            }
            return -1;
        }

        public virtual async Task<int> Delete<T>(T entity) where T : class
        {
            DbContext.Remove(entity);
            return DbContextTransaction == null ? await CommitTrans() : 0;
        }

        public virtual async Task<int> Delete<T>(IEnumerable<T> entities) where T : class
        {
            DbContext.RemoveRange(entities);
            return DbContextTransaction == null ? await CommitTrans() : 0;
        }

        public virtual async Task<int> Delete<T>(Expression<Func<T, bool>> condition) where T : class, new()
        {
            var list = await DbContext.Set<T>().Where(condition).ToListAsync();
            return list.Any() ? await Delete(list.AsEnumerable()) : 0;
        }

        public virtual async Task<int> Delete<T>(object[] keyValue) where T : class
        {
            return await ExecuteDelete<T>("Id", keyValue);
        }

        public virtual async Task<int> Delete<T>(string propertyName, object propertyValue) where T : class
        {
            return await ExecuteDelete<T>(propertyName, propertyValue);
        }

        private async Task<int> ExecuteDelete<T>(string propertyName, params object[] propertyValue) where T : class
        {
            var entityType = DbContextExtension.GetEntityType<T>(DbContext);
            if (entityType != null)
            {
                var tableName = entityType.GetTableName();
                var (sql, parameter) = DbContextExtension.DeleteSql(tableName, propertyName, propertyValue);
                return await ExecuteBySql(sql, parameter);
            }
            return -1;
        }

        #endregion Delete

        #region Update

        public virtual async Task<int> Update<T>(T entity) where T : class
        {
            SetModified(entity);
            return DbContextTransaction == null ? await CommitTrans() : 0;
        }

        public virtual async Task<int> Update<T>(IEnumerable<T> entities) where T : class
        {
            foreach (var t in entities)
            {
                SetModified(t);
            }
            return DbContextTransaction == null ? await CommitTrans() : 0;
        }

        private void SetModified<T>(T entity) where T : class
        {
            DbContext.Attach(entity);
            var entityEntry = DbContext.Entry(entity);
            var hashtable = ReflectionHelper.GetPropertyInfo(entity);
            foreach (string item in hashtable.Keys)
            {
                var propertyEntry = entityEntry.Property(item);
                if (item != "Id" && propertyEntry.CurrentValue != null)
                {
                    propertyEntry.IsModified = true;
                }
            }
        }

        public virtual async Task<int> Update<T>(Expression<Func<T, bool>> condition) where T : class, new()
        {
            var list = await DbContext.Set<T>().Where(condition).ToListAsync();
            return list.Any() ? await Update(list) : 0;
        }

        #endregion Update

        #region Find

        public virtual IQueryable<T> AsQueryable<T>(Expression<Func<T, bool>> condition) where T : class, new()
        {
            return DbContext.Set<T>().Where(condition);
        }

        public virtual async Task<T> FindEntity<T>(object keyValue) where T : class
        {
            return await DbContext.Set<T>().FindAsync(keyValue);
        }

        public virtual async Task<T> FindEntity<T>(Expression<Func<T, bool>> condition) where T : class, new()
        {
            return await DbContext.Set<T>().Where(condition).FirstOrDefaultAsync();
        }

        public virtual async Task<T> FindEntity<T>(string sort, bool isAsc) where T : class, new()
        {
            return await DbContext.Set<T>().AsQueryable().AppendSort(sort, isAsc).FirstOrDefaultAsync();
        }

        public virtual async Task<T> FindEntity<T>(string sql, params DbParameter[] dbParameter)
        {
            await using var dbConnection = DbContext.Database.GetDbConnection();
            using var reader = await new DbHelper(DbContext, dbConnection).ExecuteReadeAsync(CommandType.Text, sql, dbParameter);
            return reader.ToInstance<T>();
        }

        public virtual async Task<List<T>> FindList<T>() where T : class, new()
        {
            return await DbContext.Set<T>().ToListAsync();
        }

        public virtual async Task<List<T>> FindList<T>(Expression<Func<T, bool>> condition) where T : class, new()
        {
            return await DbContext.Set<T>().Where(condition).ToListAsync();
        }

        public virtual async Task<List<T>> FindList<T>(string sql, params DbParameter[] dbParameter) where T : class
        {
            await using var dbConnection = DbContext.Database.GetDbConnection();
            using var reader = await new DbHelper(DbContext, dbConnection).ExecuteReadeAsync(CommandType.Text, sql, dbParameter);
            return reader.ToList<T>();
        }

        public virtual async Task<(int total, List<T> list)> FindList<T>(string sort, bool isAsc, int pageSize, int pageIndex, Expression<Func<T, bool>> condition) where T : class, new()
        {
            var query = DbContext.Set<T>().Where(condition ?? (s => true)).AppendSort(sort, isAsc);
            var total = query.Count();
            if (total > 0)
            {
                var list = await query.Skip(pageSize * (pageIndex - 1))
                                      .Take(pageSize)
                                      .ToListAsync();
                return (total, list);
            }
            return (total, new List<T>());
        }

        public virtual async Task<(int total, List<T>)> FindList<T>(string sql, string sort, bool isAsc, int pageSize, int pageIndex, params DbParameter[] dbParameter)
        {
            await using var dbConnection = DbContext.Database.GetDbConnection();
            var dbHelper = new DbHelper(DbContext, dbConnection);
            int total = (await dbHelper.ExecuteScalarAsync(CommandType.Text, DbPagingHelper.GetCountSql(sql), dbParameter)).ParseToInt();
            if (total > 0)
            {
                var pagingSql = DbPagingHelper.GetPagingSql(sql, sort, isAsc, pageSize, pageIndex);
                using var reader = await dbHelper.ExecuteReadeAsync(CommandType.Text, pagingSql, dbParameter);
                return (total, reader.ToList<T>(sql));
            }
            return (total, new List<T>());
        }

        public virtual async Task<DataTable> FindTable(string sql, params DbParameter[] dbParameter)
        {
            await using var dbConnection = DbContext.Database.GetDbConnection();
            using var reader = await new DbHelper(DbContext, dbConnection).ExecuteReadeAsync(CommandType.Text, sql, dbParameter);
            return reader.ToDataTable();
        }

        public virtual async Task<(int total, DataTable)> FindTable(string sql, string sort, bool isAsc, int pageSize, int pageIndex, params DbParameter[] dbParameter)
        {
            await using var dbConnection = DbContext.Database.GetDbConnection();
            var dbHelper = new DbHelper(DbContext, dbConnection);
            int total = (await dbHelper.ExecuteScalarAsync(CommandType.Text, DbPagingHelper.GetCountSql(sql), dbParameter)).ParseToInt();
            if (total > 0)
            {
                var pagingSql = DbPagingHelper.GetPagingSql(sql, sort, isAsc, pageSize, pageIndex);
                using var reader = await dbHelper.ExecuteReadeAsync(CommandType.Text, pagingSql, dbParameter);
                return (total, reader.ToDataTable());
            }
            return (total, new DataTable());
        }

        #endregion Find
    }
}