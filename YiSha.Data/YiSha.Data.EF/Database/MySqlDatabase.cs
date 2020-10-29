using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Data;
using System.Data.Common;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using YiSha.Util.Extension;

namespace YiSha.Data.EF
{
    public class MySqlDatabase : IDatabase
    {
        #region 构造函数
        /// <summary>
        /// 构造方法
        /// </summary>
        public MySqlDatabase(string connString)
        {
            dbContext = new MySqlDbContext(connString);
        }
        #endregion

        #region 属性
        /// <summary>
        /// 获取 当前使用的数据访问上下文对象
        /// </summary>
        public DbContext dbContext { get; set; }
        /// <summary>
        /// 事务对象
        /// </summary>
        public IDbContextTransaction dbContextTransaction { get; set; }
        #endregion

        #region 事务提交
        /// <summary>
        /// 事务开始
        /// </summary>
        /// <returns></returns>
        public async Task<IDatabase> BeginTrans()
        {
            DbConnection dbConnection = dbContext.Database.GetDbConnection();
            if (dbConnection.State == ConnectionState.Closed)
            {
                await dbConnection.OpenAsync();
            }
            dbContextTransaction = await dbContext.Database.BeginTransactionAsync();
            return this;
        }
        /// <summary>
        /// 提交当前操作的结果
        /// </summary>
        public async Task<int> CommitTrans()
        {
            try
            {
                DbContextExtension.SetEntityDefaultValue(dbContext);

                int returnValue = await dbContext.SaveChangesAsync();
                if (dbContextTransaction != null)
                {
                    await dbContextTransaction.CommitAsync();
                    await this.Close();
                }
                else
                {
                    await this.Close();
                }
                return returnValue;
            }
            catch
            {
                throw;
            }
            finally
            {
                if (dbContextTransaction == null)
                {
                    await this.Close();
                }
            }
        }
        /// <summary>
        /// 把当前操作回滚成未提交状态
        /// </summary>
        public async Task RollbackTrans()
        {
            await this.dbContextTransaction.RollbackAsync();
            await this.dbContextTransaction.DisposeAsync();
            await this.Close();
        }
        /// <summary>
        /// 关闭连接 内存回收
        /// </summary>
        public async Task Close()
        {
            await dbContext.DisposeAsync();
        }
        #endregion

        #region 执行 SQL 语句
        public async Task<int> ExecuteBySql(string strSql)
        {
            if (dbContextTransaction == null)
            {
                return await dbContext.Database.ExecuteSqlRawAsync(strSql);
            }
            else
            {
                await dbContext.Database.ExecuteSqlRawAsync(strSql);
                return dbContextTransaction == null ? await this.CommitTrans() : 0;
            }
        }
        public async Task<int> ExecuteBySql(string strSql, params DbParameter[] dbParameter)
        {
            if (dbContextTransaction == null)
            {
                return await dbContext.Database.ExecuteSqlRawAsync(strSql, dbParameter);
            }
            else
            {
                await dbContext.Database.ExecuteSqlRawAsync(strSql, dbParameter);
                return dbContextTransaction == null ? await this.CommitTrans() : 0;
            }
        }
        public async Task<int> ExecuteByProc(string procName)
        {
            if (dbContextTransaction == null)
            {
                return await dbContext.Database.ExecuteSqlRawAsync(DbContextExtension.BuilderProc(procName));
            }
            else
            {
                await dbContext.Database.ExecuteSqlRawAsync(DbContextExtension.BuilderProc(procName));
                return dbContextTransaction == null ? await this.CommitTrans() : 0;
            }
        }
        public async Task<int> ExecuteByProc(string procName, params DbParameter[] dbParameter)
        {
            if (dbContextTransaction == null)
            {
                return await dbContext.Database.ExecuteSqlRawAsync(DbContextExtension.BuilderProc(procName, dbParameter), dbParameter);
            }
            else
            {
                await dbContext.Database.ExecuteSqlRawAsync(DbContextExtension.BuilderProc(procName, dbParameter), dbParameter);
                return dbContextTransaction == null ? await this.CommitTrans() : 0;
            }
        }
        #endregion

        #region 对象实体 添加、修改、删除
        public async Task<int> Insert<T>(T entity) where T : class
        {
            dbContext.Entry<T>(entity).State = EntityState.Added;
            return dbContextTransaction == null ? await this.CommitTrans() : 0;
        }
        public async Task<int> Insert<T>(IEnumerable<T> entities) where T : class
        {
            foreach (var entity in entities)
            {
                dbContext.Entry<T>(entity).State = EntityState.Added;
            }
            return dbContextTransaction == null ? await this.CommitTrans() : 0;
        }

        public async Task<int> Delete<T>() where T : class
        {
            IEntityType entityType = DbContextExtension.GetEntityType<T>(dbContext);
            if (entityType != null)
            {
                string tableName = entityType.GetTableName();
                return await this.ExecuteBySql(DbContextExtension.DeleteSql(tableName));
            }
            return -1;
        }
        public async Task<int> Delete<T>(T entity) where T : class
        {
            dbContext.Set<T>().Attach(entity);
            dbContext.Set<T>().Remove(entity);
            return dbContextTransaction == null ? await this.CommitTrans() : 0;
        }
        public async Task<int> Delete<T>(IEnumerable<T> entities) where T : class
        {
            foreach (var entity in entities)
            {
                dbContext.Set<T>().Attach(entity);
                dbContext.Set<T>().Remove(entity);
            }
            return dbContextTransaction == null ? await this.CommitTrans() : 0;
        }
        public async Task<int> Delete<T>(Expression<Func<T, bool>> condition) where T : class, new()
        {
            IEnumerable<T> entities = await dbContext.Set<T>().Where(condition).ToListAsync();
            return entities.Count() > 0 ? await Delete(entities) : 0;
        }
        public async Task<int> Delete<T>(long keyValue) where T : class
        {
            IEntityType entityType = DbContextExtension.GetEntityType<T>(dbContext);
            if (entityType != null)
            {
                string tableName = entityType.GetTableName();
                string keyField = "Id";
                return await this.ExecuteBySql(DbContextExtension.DeleteSql(tableName, keyField, keyValue));
            }
            return -1;
        }
        public async Task<int> Delete<T>(long[] keyValue) where T : class
        {
            IEntityType entityType = DbContextExtension.GetEntityType<T>(dbContext);
            if (entityType != null)
            {
                string tableName = entityType.GetTableName();
                string keyField = "Id";
                return await this.ExecuteBySql(DbContextExtension.DeleteSql(tableName, keyField, keyValue));
            }
            return -1;
        }
        public async Task<int> Delete<T>(string propertyName, long propertyValue) where T : class
        {
            IEntityType entityType = DbContextExtension.GetEntityType<T>(dbContext);
            if (entityType != null)
            {
                string tableName = entityType.GetTableName();
                return await this.ExecuteBySql(DbContextExtension.DeleteSql(tableName, propertyName, propertyValue));
            }
            return -1;
        }

        public async Task<int> Update<T>(T entity) where T : class
        {
            dbContext.Set<T>().Attach(entity);
            Hashtable props = DatabasesExtension.GetPropertyInfo<T>(entity);
            foreach (string item in props.Keys)
            {
                if (item == "Id")
                {
                    continue;
                }
                object value = dbContext.Entry(entity).Property(item).CurrentValue;

                if (value != null)
                {
                    dbContext.Entry(entity).Property(item).IsModified = true;
                }
            }
            return dbContextTransaction == null ? await this.CommitTrans() : 0;
        }
        public async Task<int> Update<T>(IEnumerable<T> entities) where T : class
        {
            foreach (var entity in entities)
            {
                dbContext.Entry<T>(entity).State = EntityState.Modified;
            }
            return dbContextTransaction == null ? await this.CommitTrans() : 0;
        }
        public async Task<int> UpdateAllField<T>(T entity) where T : class
        {
            dbContext.Set<T>().Attach(entity);
            dbContext.Entry(entity).State = EntityState.Modified;
            return dbContextTransaction == null ? await this.CommitTrans() : 0;
        }
        public async Task<int> Update<T>(Expression<Func<T, bool>> condition) where T : class, new()
        {
            IEnumerable<T> entities = await dbContext.Set<T>().Where(condition).ToListAsync();
            return entities.Count() > 0 ? await Update(entities) : 0;
        }

        public IQueryable<T> IQueryable<T>(Expression<Func<T, bool>> condition) where T : class, new()
        {
            return dbContext.Set<T>().Where(condition);
        }
        #endregion

        #region 对象实体 查询
        public async Task<T> FindEntity<T>(object keyValue) where T : class
        {
            return await dbContext.Set<T>().FindAsync(keyValue);
        }
        public async Task<T> FindEntity<T>(Expression<Func<T, bool>> condition) where T : class, new()
        {
            return await dbContext.Set<T>().Where(condition).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<T>> FindList<T>() where T : class, new()
        {
            return await dbContext.Set<T>().ToListAsync();
        }
        public async Task<IEnumerable<T>> FindList<T>(Func<T, object> orderby) where T : class, new()
        {
            var list = await dbContext.Set<T>().ToListAsync();
            list = list.OrderBy(orderby).ToList();
            return list;
        }
        public async Task<IEnumerable<T>> FindList<T>(Expression<Func<T, bool>> condition) where T : class, new()
        {
            return await dbContext.Set<T>().Where(condition).ToListAsync();
        }
        public async Task<IEnumerable<T>> FindList<T>(string strSql) where T : class
        {
            return await FindList<T>(strSql, null);
        }
        public async Task<IEnumerable<T>> FindList<T>(string strSql, DbParameter[] dbParameter) where T : class
        {
            using (var dbConnection = dbContext.Database.GetDbConnection())
            {
                var reader = await new DbHelper(dbContext, dbConnection).ExecuteReadeAsync(CommandType.Text, strSql, dbParameter);
                return DatabasesExtension.IDataReaderToList<T>(reader);
            }
        }
        public async Task<(int total, IEnumerable<T> list)> FindList<T>(string sort, bool isAsc, int pageSize, int pageIndex) where T : class, new()
        {
            var tempData = dbContext.Set<T>().AsQueryable();
            return await FindList<T>(tempData, sort, isAsc, pageSize, pageIndex);
        }
        public async Task<(int total, IEnumerable<T> list)> FindList<T>(Expression<Func<T, bool>> condition, string sort, bool isAsc, int pageSize, int pageIndex) where T : class, new()
        {
            var tempData = dbContext.Set<T>().Where(condition);
            return await FindList<T>(tempData, sort, isAsc, pageSize, pageIndex);
        }
        public async Task<(int total, IEnumerable<T>)> FindList<T>(string strSql, string sort, bool isAsc, int pageSize, int pageIndex) where T : class
        {
            return await FindList<T>(strSql, null, sort, isAsc, pageSize, pageIndex);
        }
        public async Task<(int total, IEnumerable<T>)> FindList<T>(string strSql, DbParameter[] dbParameter, string sort, bool isAsc, int pageSize, int pageIndex) where T : class
        {
            using (var dbConnection = dbContext.Database.GetDbConnection())
            {
                DbHelper dbHelper = new DbHelper(dbContext, dbConnection);
                StringBuilder sb = new StringBuilder();
                sb.Append(DatabasePageExtension.MySqlPageSql(strSql, dbParameter, sort, isAsc, pageSize, pageIndex));
                object tempTotal = await dbHelper.ExecuteScalarAsync(CommandType.Text, DatabasePageExtension.GetCountSql(strSql), dbParameter);
                int total = tempTotal.ParseToInt();
                if (total > 0)
                {
                    var reader = await dbHelper.ExecuteReadeAsync(CommandType.Text, sb.ToString(), dbParameter);
                    return (total, DatabasesExtension.IDataReaderToList<T>(reader));
                }
                else
                {
                    return (total, new List<T>());
                }
            }
        }
        private async Task<(int total, IEnumerable<T> list)> FindList<T>(IQueryable<T> tempData, string sort, bool isAsc, int pageSize, int pageIndex)
        {
            tempData = DatabasesExtension.AppendSort<T>(tempData, sort, isAsc);
            var total = tempData.Count();
            if (total > 0)
            {
                tempData = tempData.Skip<T>(pageSize * (pageIndex - 1)).Take<T>(pageSize).AsQueryable();
                var list = await tempData.ToListAsync();
                return (total, list);
            }
            else
            {
                return (total, new List<T>());
            }
        }
        #endregion

        #region 数据源查询
        public async Task<DataTable> FindTable(string strSql)
        {
            return await FindTable(strSql, null);
        }
        public async Task<DataTable> FindTable(string strSql, DbParameter[] dbParameter)
        {
            using (var dbConnection = dbContext.Database.GetDbConnection())
            {
                var reader = await new DbHelper(dbContext, dbConnection).ExecuteReadeAsync(CommandType.Text, strSql, dbParameter);
                return DatabasesExtension.IDataReaderToDataTable(reader);
            }
        }
        public async Task<(int total, DataTable)> FindTable(string strSql, string sort, bool isAsc, int pageSize, int pageIndex)
        {
            return await FindTable(strSql, null, sort, isAsc, pageSize, pageIndex);
        }
        public async Task<(int total, DataTable)> FindTable(string strSql, DbParameter[] dbParameter, string sort, bool isAsc, int pageSize, int pageIndex)
        {
            using (var dbConnection = dbContext.Database.GetDbConnection())
            {
                DbHelper dbHelper = new DbHelper(dbContext, dbConnection);
                StringBuilder sb = new StringBuilder();
                sb.Append(DatabasePageExtension.MySqlPageSql(strSql, dbParameter, sort, isAsc, pageSize, pageIndex));
                object tempTotal = await dbHelper.ExecuteScalarAsync(CommandType.Text, "SELECT COUNT(1) FROM (" + strSql + ") T", dbParameter);
                int total = tempTotal.ParseToInt();
                if (total > 0)
                {
                    var reader = await dbHelper.ExecuteReadeAsync(CommandType.Text, sb.ToString(), dbParameter);
                    DataTable resultTable = DatabasesExtension.IDataReaderToDataTable(reader);
                    return (total, resultTable);
                }
                else
                {
                    return (total, new DataTable());
                }
            }
        }

        public async Task<object> FindObject(string strSql)
        {
            return await FindObject(strSql, null);
        }
        public async Task<object> FindObject(string strSql, DbParameter[] dbParameter)
        {
            using (var dbConnection = dbContext.Database.GetDbConnection())
            {
                return await new DbHelper(dbContext, dbConnection).ExecuteScalarAsync(CommandType.Text, strSql, dbParameter);
            }
        }
        public async Task<T> FindObject<T>(string strSql) where T : class
        {
            var list = await dbContext.SqlQuery<T>(strSql);
            return list.FirstOrDefault();
        }
        #endregion
    }
}
