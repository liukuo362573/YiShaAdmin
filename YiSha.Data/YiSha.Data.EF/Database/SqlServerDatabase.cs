using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using YiSha.Util;

namespace YiSha.Data.EF
{
    public class SqlServerDatabase : IDatabase
    {
        #region 构造函数
        /// <summary>
        /// 构造方法
        /// </summary>
        public SqlServerDatabase(string connString)
        {
            dbcontext = new SqlServerDbContext(connString);
        }
        #endregion

        #region 属性
        /// <summary>
        /// 获取 当前使用的数据访问上下文对象
        /// </summary>
        public DbContext dbcontext { get; set; }
        /// <summary>
        /// 事务对象
        /// </summary>
        public IDbContextTransaction dbTransaction { get; set; }
        #endregion

        #region 事务提交
        /// <summary>
        /// 事务开始
        /// </summary>
        /// <returns></returns>
        public async Task<IDatabase> BeginTrans()
        {
            DbConnection dbConnection = dbcontext.Database.GetDbConnection();
            if (dbConnection.State == ConnectionState.Closed)
            {
                dbConnection.Open();
            }
            dbTransaction = await dbcontext.Database.BeginTransactionAsync();
            return this;
        }
        /// <summary>
        /// 提交当前操作的结果
        /// </summary>
        public async Task<int> Commit()
        {
            try
            {
                DbContextExtension.SetEntityDefaultValue(dbcontext);

                int returnValue = await dbcontext.SaveChangesAsync();
                if (dbTransaction != null)
                {
                    dbTransaction.Commit();
                    this.Close();
                }
                else
                {
                    this.Close();
                }
                return returnValue;
            }
            catch
            {
                throw;
            }
            finally
            {
                if (dbTransaction == null)
                {
                    this.Close();
                }
            }
        }
        /// <summary>
        /// 把当前操作回滚成未提交状态
        /// </summary>
        public void Rollback()
        {
            this.dbTransaction.Rollback();
            this.dbTransaction.Dispose();
            this.Close();
        }
        /// <summary>
        /// 关闭连接 内存回收
        /// </summary>
        public void Close()
        {
            dbcontext.Dispose();
        }
        #endregion

        #region 执行 SQL 语句
        public async Task<int> ExecuteBySql(string strSql)
        {
            if (dbTransaction == null)
            {
                return await dbcontext.Database.ExecuteSqlCommandAsync(strSql);
            }
            else
            {
                await dbcontext.Database.ExecuteSqlCommandAsync(strSql);
                return dbTransaction == null ? await this.Commit() : 0;
            }
        }
        public async Task<int> ExecuteBySql(string strSql, params DbParameter[] dbParameter)
        {
            if (dbTransaction == null)
            {
                return await dbcontext.Database.ExecuteSqlCommandAsync(strSql, dbParameter);
            }
            else
            {
                await dbcontext.Database.ExecuteSqlCommandAsync(strSql, dbParameter);
                return dbTransaction == null ? await this.Commit() : 0;
            }
        }
        public async Task<int> ExecuteByProc(string procName)
        {
            if (dbTransaction == null)
            {
                return await dbcontext.Database.ExecuteSqlCommandAsync(DbContextExtension.BuilderProc(procName));
            }
            else
            {
                await dbcontext.Database.ExecuteSqlCommandAsync(DbContextExtension.BuilderProc(procName));
                return dbTransaction == null ? await this.Commit() : 0;
            }
        }
        public async Task<int> ExecuteByProc(string procName, params DbParameter[] dbParameter)
        {
            if (dbTransaction == null)
            {
                return await dbcontext.Database.ExecuteSqlCommandAsync(DbContextExtension.BuilderProc(procName, dbParameter), dbParameter);
            }
            else
            {
                await dbcontext.Database.ExecuteSqlCommandAsync(DbContextExtension.BuilderProc(procName, dbParameter), dbParameter);
                return dbTransaction == null ? await this.Commit() : 0;
            }
        }
        #endregion

        #region 对象实体 添加、修改、删除
        public async Task<int> Insert<T>(T entity) where T : class
        {
            dbcontext.Entry<T>(entity).State = EntityState.Added;
            return dbTransaction == null ? await this.Commit() : 0;
        }
        public async Task<int> Insert<T>(IEnumerable<T> entities) where T : class
        {
            foreach (var entity in entities)
            {
                dbcontext.Entry<T>(entity).State = EntityState.Added;
            }
            return dbTransaction == null ? await this.Commit() : 0;
        }

        public async Task<int> Delete<T>() where T : class
        {
            IEntityType entityType = DbContextExtension.GetEntityType<T>(dbcontext);
            if (entityType != null)
            {
                string tableName = entityType.Relational().TableName;
                return await this.ExecuteBySql(DbContextExtension.DeleteSql(tableName));
            }
            return -1;
        }
        public async Task<int> Delete<T>(T entity) where T : class
        {
            dbcontext.Set<T>().Attach(entity);
            dbcontext.Set<T>().Remove(entity);
            return dbTransaction == null ? await this.Commit() : 0;
        }
        public async Task<int> Delete<T>(IEnumerable<T> entities) where T : class
        {
            foreach (var entity in entities)
            {
                dbcontext.Set<T>().Attach(entity);
                dbcontext.Set<T>().Remove(entity);
            }
            return dbTransaction == null ? await this.Commit() : 0;
        }
        public async Task<int> Delete<T>(Expression<Func<T, bool>> condition) where T : class, new()
        {
            IEnumerable<T> entities = await dbcontext.Set<T>().Where(condition).ToListAsync();
            return entities.Count() > 0 ? await Delete(entities) : 0;
        }
        public async Task<int> Delete<T>(long keyValue) where T : class
        {
            IEntityType entityType = DbContextExtension.GetEntityType<T>(dbcontext);
            if (entityType != null)
            {
                string tableName = entityType.Relational().TableName;
                string keyField = "Id";
                return await this.ExecuteBySql(DbContextExtension.DeleteSql(tableName, keyField, keyValue));
            }
            return -1;
        }
        public async Task<int> Delete<T>(long[] keyValue) where T : class
        {
            IEntityType entityType = DbContextExtension.GetEntityType<T>(dbcontext);
            if (entityType != null)
            {
                string tableName = entityType.Relational().TableName;
                string keyField = "Id";
                return await this.ExecuteBySql(DbContextExtension.DeleteSql(tableName, keyField, keyValue));
            }
            return -1;
        }
        public async Task<int> Delete<T>(string propertyName, long propertyValue) where T : class
        {
            IEntityType entityType = DbContextExtension.GetEntityType<T>(dbcontext);
            if (entityType != null)
            {
                string tableName = entityType.Relational().TableName;
                return await this.ExecuteBySql(DbContextExtension.DeleteSql(tableName, propertyName, propertyValue));
            }
            return -1;
        }

        public async Task<int> Update<T>(T entity) where T : class
        {
            dbcontext.Set<T>().Attach(entity);
            Hashtable props = ConvertExtension.GetPropertyInfo<T>(entity);
            foreach (string item in props.Keys)
            {
                if (item == "Id")
                {
                    continue;
                }
                object value = dbcontext.Entry(entity).Property(item).CurrentValue;

                if (value != null)
                {
                    if (value.ToString() == "&nbsp;")
                        dbcontext.Entry(entity).Property(item).CurrentValue = null;
                    dbcontext.Entry(entity).Property(item).IsModified = true;
                }
            }
            return dbTransaction == null ? await this.Commit() : 0;
        }
        public async Task<int> Update<T>(IEnumerable<T> entities) where T : class
        {
            foreach (var entity in entities)
            {
                await this.Update(entity);
            }
            return dbTransaction == null ? await this.Commit() : 0;
        }
        public async Task<int> UpdateAllField<T>(T entity) where T : class
        {
            dbcontext.Set<T>().Attach(entity);
            dbcontext.Entry(entity).State = EntityState.Modified;
            return dbTransaction == null ? await this.Commit() : 0;
        }
        public async Task<int> Update<T>(Expression<Func<T, bool>> condition) where T : class, new()
        {
            IEnumerable<T> entities = await dbcontext.Set<T>().Where(condition).ToListAsync();
            return entities.Count() > 0 ? await Update(entities) : 0;
        }

        public IQueryable<T> IQueryable<T>(Expression<Func<T, bool>> condition) where T : class, new()
        {
            return dbcontext.Set<T>().Where(condition);
        }
        #endregion

        #region 对象实体 查询
        public async Task<T> FindEntity<T>(object keyValue) where T : class
        {
            return await dbcontext.Set<T>().FindAsync(keyValue);
        }
        public async Task<T> FindEntity<T>(Expression<Func<T, bool>> condition) where T : class, new()
        {
            return await dbcontext.Set<T>().Where(condition).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<T>> FindList<T>() where T : class, new()
        {
            return await dbcontext.Set<T>().ToListAsync();
        }
        public async Task<IEnumerable<T>> FindList<T>(Func<T, object> orderby) where T : class, new()
        {
            var list = await dbcontext.Set<T>().ToListAsync();
            list = list.OrderBy(orderby).ToList();
            return list;
        }
        public async Task<IEnumerable<T>> FindList<T>(Expression<Func<T, bool>> condition) where T : class, new()
        {
            return await dbcontext.Set<T>().Where(condition).ToListAsync();
        }
        public async Task<IEnumerable<T>> FindList<T>(string strSql) where T : class
        {
            return await FindList<T>(strSql, null);
        }
        public async Task<IEnumerable<T>> FindList<T>(string strSql, DbParameter[] dbParameter) where T : class
        {
            using (var dbConnection = dbcontext.Database.GetDbConnection())
            {
                var IDataReader = await new DbHelper(dbConnection).ExecuteReadeAsync(CommandType.Text, strSql, dbParameter);
                return ConvertExtension.IDataReaderToList<T>(IDataReader);
            }
        }
        public async Task<(int total, IEnumerable<T> list)> FindList<T>(string orderField, bool isAsc, int pageSize, int pageIndex) where T : class, new()
        {
            string[] _order = orderField.Split(',');
            var tempData = dbcontext.Set<T>().AsQueryable();
            return await FindList<T>(tempData, orderField, isAsc, pageSize, pageIndex);
        }
        public async Task<(int total, IEnumerable<T> list)> FindList<T>(Expression<Func<T, bool>> condition, string orderField, bool isAsc, int pageSize, int pageIndex) where T : class, new()
        {
            string[] _order = orderField.Split(',');
            var tempData = dbcontext.Set<T>().Where(condition);
            return await FindList<T>(tempData, orderField, isAsc, pageSize, pageIndex);
        }
        public async Task<(int total, IEnumerable<T>)> FindList<T>(string strSql, string orderField, bool isAsc, int pageSize, int pageIndex) where T : class
        {
            return await FindList<T>(strSql, null, orderField, isAsc, pageSize, pageIndex);
        }
        public async Task<(int total, IEnumerable<T>)> FindList<T>(string strSql, DbParameter[] dbParameter, string orderField, bool isAsc, int pageSize, int pageIndex) where T : class
        {
            using (var dbConnection = dbcontext.Database.GetDbConnection())
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(new DatabasePageExtension().SqlPageSql(strSql, dbParameter, orderField, isAsc, pageSize, pageIndex));
                object tempTotal = await new DbHelper(dbConnection).ExecuteScalarAsync(CommandType.Text, "Select Count(1) From (" + strSql + ")  t", dbParameter);
                int total = Convert.ToInt32(tempTotal);
                if (total > 0)
                {
                    var IDataReader = await new DbHelper(dbConnection).ExecuteReadeAsync(CommandType.Text, sb.ToString(), dbParameter);
                    return (total, ConvertExtension.IDataReaderToList<T>(IDataReader));
                }
                else
                {
                    return (total, new List<T>());
                }
            }
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

        #region 数据源查询
        public async Task<DataTable> FindTable(string strSql)
        {
            return await FindTable(strSql, null);
        }
        public async Task<DataTable> FindTable(string strSql, DbParameter[] dbParameter)
        {
            using (var dbConnection = dbcontext.Database.GetDbConnection())
            {
                var IDataReader = await new DbHelper(dbConnection).ExecuteReadeAsync(CommandType.Text, strSql, dbParameter);
                return ConvertExtension.IDataReaderToDataTable(IDataReader);
            }
        }
        public async Task<(int total, DataTable)> FindTable(string strSql, string orderField, bool isAsc, int pageSize, int pageIndex)
        {
            return await FindTable(strSql, null, orderField, isAsc, pageSize, pageIndex);
        }
        public async Task<(int total, DataTable)> FindTable(string strSql, DbParameter[] dbParameter, string orderField, bool isAsc, int pageSize, int pageIndex)
        {
            using (var dbConnection = dbcontext.Database.GetDbConnection())
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(new DatabasePageExtension().SqlPageSql(strSql, dbParameter, orderField, isAsc, pageSize, pageIndex));
                object tempTotal = await new DbHelper(dbConnection).ExecuteScalarAsync(CommandType.Text, "Select Count(1) From (" + strSql + ") t ", dbParameter);
                int total = Convert.ToInt32(tempTotal);
                var IDataReader = await new DbHelper(dbConnection).ExecuteReadeAsync(CommandType.Text, sb.ToString(), dbParameter);
                DataTable resultTable = ConvertExtension.IDataReaderToDataTable(IDataReader);
                return (total, resultTable);
            }
        }

        public async Task<object> FindObject(string strSql)
        {
            return await FindObject(strSql, null);
        }
        public async Task<object> FindObject(string strSql, DbParameter[] dbParameter)
        {
            using (var dbConnection = dbcontext.Database.GetDbConnection())
            {
                return await new DbHelper(dbConnection).ExecuteScalarAsync(CommandType.Text, strSql, dbParameter);
            }
        }
        public async Task<T> FindObject<T>(string strSql) where T : class
        {
            var list = await dbcontext.SqlQuery<T>(strSql);
            return list.FirstOrDefault();
        }
        #endregion
    }
}
