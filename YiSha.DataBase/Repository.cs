using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;
using System.Data.Common;
using System.Linq.Expressions;
using System.Text;
using YiSha.DataBase.Extension;
using YiSha.Util.Extension;
using YiSha.Util.Model;

namespace YiSha.DataBase
{
    /// <summary>
    /// 存储库
    /// </summary>
    public class Repository
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        public Repository()
        {
            this.dbContext = new DbCommon(DbFactory.Connect);
        }

        /// <summary>
        /// 数据访问对象
        /// </summary>
        public DbContext dbContext { get; set; }

        /// <summary>
        /// 事务对象
        /// </summary>
        public IDbContextTransaction dbContextTransaction { get; set; }

        /// <summary>
        /// 事务开始
        /// </summary>
        /// <returns></returns>
        public async Task<Repository> BeginTrans()
        {
            var dbConnection = dbContext.Database.GetDbConnection();
            if (dbConnection.State == ConnectionState.Closed)
            {
                await dbConnection.OpenAsync();
            }
            dbContextTransaction = await dbContext.Database.BeginTransactionAsync();
            return this;
        }

        /// <summary>
        /// 提交事务
        /// </summary>
        public async Task<int> CommitTrans()
        {
            try
            {
                DbContextExtension.SetEntityDefaultValue(dbContext);

                var returnValue = await dbContext.SaveChangesAsync();
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
            catch (Exception ex)
            {
                var msg = ex.Message;
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
        /// 关闭连接，内存回收
        /// </summary>
        public async Task Close()
        {
            await dbContext.DisposeAsync();
        }

        #region 执行 SQL 语句

        /// <summary>
        /// 执行 SQL 语句
        /// </summary>
        /// <param name="strSql">SQL</param>
        /// <returns>影响数量</returns>
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

        /// <summary>
        /// 执行 SQL 语句
        /// </summary>
        /// <param name="strSql">SQL</param>
        /// <param name="dbParameter">参数</param>
        /// <returns>影响数量</returns>
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

        /// <summary>
        /// 执行 存储过程
        /// </summary>
        /// <param name="procName">过程名称</param>
        /// <param name="dbParameter">参数</param>
        /// <returns>影响数量</returns>
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

        /// <summary>
        /// 执行 存储过程
        /// </summary>
        /// <param name="procName">过程名称</param>
        /// <param name="dbParameter">参数</param>
        /// <returns>影响数量</returns>
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

        /// <summary>
        /// 新增数据
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entity">实体对象</param>
        /// <returns>影响行数</returns>
        public async Task<int> Insert<T>(T entity) where T : class
        {
            dbContext.Entry<T>(entity).State = EntityState.Added;
            return dbContextTransaction == null ? await this.CommitTrans() : 0;
        }

        /// <summary>
        /// 新增数据
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entities">多个实体对象</param>
        /// <returns>影响行数</returns>
        public async Task<int> Insert<T>(IEnumerable<T> entities) where T : class
        {
            foreach (var entity in entities)
            {
                dbContext.Entry<T>(entity).State = EntityState.Added;
            }
            return dbContextTransaction == null ? await this.CommitTrans() : 0;
        }

        /// <summary>
        /// 删除表数据
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <returns>删除数量</returns>
        public async Task<int> Delete<T>() where T : class
        {
            var entityType = DbContextExtension.GetEntityType<T>(dbContext);
            if (entityType != null)
            {
                var tableName = entityType.GetTableName();
                return await this.ExecuteBySql(DbContextExtension.DeleteSql(tableName));
            }
            return -1;
        }

        /// <summary>
        /// 删除表数据
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entity">实体对象</param>
        /// <returns>删除数量</returns>
        public async Task<int> Delete<T>(T entity) where T : class
        {
            dbContext.Set<T>().Attach(entity);
            dbContext.Set<T>().Remove(entity);
            return dbContextTransaction == null ? await this.CommitTrans() : 0;
        }

        /// <summary>
        /// 删除表数据
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entities">多个实体对象</param>
        /// <returns>删除数量</returns>
        public async Task<int> Delete<T>(IEnumerable<T> entities) where T : class
        {
            foreach (var entity in entities)
            {
                dbContext.Set<T>().Attach(entity);
                dbContext.Set<T>().Remove(entity);
            }
            return dbContextTransaction == null ? await this.CommitTrans() : 0;
        }

        /// <summary>
        /// 删除表数据
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="condition">Linq</param>
        /// <returns>删除数量</returns>
        public async Task<int> Delete<T>(Expression<Func<T, bool>> condition) where T : class, new()
        {
            var entities = await dbContext.Set<T>().Where(condition).ToListAsync();
            return entities.Count() > 0 ? await Delete(entities) : 0;
        }

        /// <summary>
        /// 删除表数据
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="keyValue">ID</param>
        /// <returns>删除数量</returns>
        public async Task<int> Delete<T>(long keyValue) where T : class
        {
            var entityType = DbContextExtension.GetEntityType<T>(dbContext);
            if (entityType != null)
            {
                var tableName = entityType.GetTableName();
                var keyField = "Id";
                return await this.ExecuteBySql(DbContextExtension.DeleteSql(tableName, keyField, keyValue));
            }
            return -1;
        }

        /// <summary>
        /// 删除表数据
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="keyValue">ID集合</param>
        /// <returns>删除数量</returns>
        public async Task<int> Delete<T>(long[] keyValue) where T : class
        {
            var entityType = DbContextExtension.GetEntityType<T>(dbContext);
            if (entityType != null)
            {
                var tableName = entityType.GetTableName();
                var keyField = "Id";
                return await this.ExecuteBySql(DbContextExtension.DeleteSql(tableName, keyField, keyValue));
            }
            return -1;
        }

        /// <summary>
        /// 删除表数据
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="propertyName">属性名</param>
        /// <param name="propertyValue">属性值</param>
        /// <returns>删除数量</returns>
        public async Task<int> Delete<T>(string propertyName, long propertyValue) where T : class
        {
            var entityType = DbContextExtension.GetEntityType<T>(dbContext);
            if (entityType != null)
            {
                var tableName = entityType.GetTableName();
                return await this.ExecuteBySql(DbContextExtension.DeleteSql(tableName, propertyName, propertyValue));
            }
            return -1;
        }

        /// <summary>
        /// 修改表数据
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entity">实体对象</param>
        /// <returns>修改数量</returns>
        public async Task<int> Update<T>(T entity) where T : class
        {
            dbContext.Set<T>().Attach(entity);
            var props = DbExtension.GetPropertyInfo<T>(entity);
            foreach (string item in props.Keys)
            {
                if (item == "Id") continue;

                var value = dbContext.Entry(entity).Property(item).CurrentValue;

                if (value != null)
                {
                    dbContext.Entry(entity).Property(item).IsModified = true;
                }
            }
            return dbContextTransaction == null ? await this.CommitTrans() : 0;
        }

        /// <summary>
        /// 修改表数据
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entities">多个实体对象</param>
        /// <returns>修改数量</returns>
        public async Task<int> Update<T>(IEnumerable<T> entities) where T : class
        {
            foreach (var entity in entities)
            {
                dbContext.Entry<T>(entity).State = EntityState.Modified;
            }
            return dbContextTransaction == null ? await this.CommitTrans() : 0;
        }

        /// <summary>
        /// 修改表所有字段
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public async Task<int> UpdateAllField<T>(T entity) where T : class
        {
            dbContext.Set<T>().Attach(entity);
            dbContext.Entry(entity).State = EntityState.Modified;
            return dbContextTransaction == null ? await this.CommitTrans() : 0;
        }

        /// <summary>
        /// 修改表数据
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="condition">Linq</param>
        /// <returns>修改数量</returns>
        public async Task<int> Update<T>(Expression<Func<T, bool>> condition) where T : class, new()
        {
            var entities = await dbContext.Set<T>().Where(condition).ToListAsync();
            return entities.Count() > 0 ? await Update(entities) : 0;
        }

        /// <summary>
        /// 查询数据
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="condition">Linq</param>
        /// <returns>实体数据</returns>
        public IQueryable<T> IQueryable<T>(Expression<Func<T, bool>> condition) where T : class, new()
        {
            return dbContext.Set<T>().Where(condition);
        }

        #endregion

        #region 对象实体 查询

        public async Task<T> FindEntity<T>(long id) where T : class
        {
            return await FindEntity<T>(keyValue: id);
        }

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
            return await FindList<T>(strSql, dbParameter: null);
        }

        public async Task<IEnumerable<T>> FindList<T>(string strSql, DbParameter[] dbParameter) where T : class
        {
            var dbConnection = dbContext.Database.GetDbConnection();
            var reader = await new DbScalarExtension(dbContext, dbConnection).ExecuteReadeAsync(CommandType.Text, strSql, dbParameter);
            return DbExtension.IDataReaderToList<T>(reader);
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
            var dbConnection = dbContext.Database.GetDbConnection();
            var DbScalarExtension = new DbScalarExtension(dbContext, dbConnection);
            var sb = new StringBuilder();
            sb.Append(DbPageExtension.GetPageSql(strSql, dbParameter, sort, isAsc, pageSize, pageIndex));
            var tempTotal = await DbScalarExtension.ExecuteScalarAsync(CommandType.Text, DbPageExtension.GetCountSql(strSql), dbParameter);
            var total = tempTotal.ParseToInt();
            if (total > 0)
            {
                var reader = await DbScalarExtension.ExecuteReadeAsync(CommandType.Text, sb.ToString(), dbParameter);
                return (total, DbExtension.IDataReaderToList<T>(reader));
            }
            else
            {
                return (total, new List<T>());
            }
        }

        private async Task<(int total, IEnumerable<T> list)> FindList<T>(IQueryable<T> tempData, string sort, bool isAsc, int pageSize, int pageIndex)
        {
            tempData = DbExtension.AppendSort<T>(tempData, sort, isAsc);
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

        public async Task<(int total, IEnumerable<T> list)> FindList<T>(Pagination pagination) where T : class, new()
        {
            var total = pagination.TotalCount;
            var data = await FindList<T>(pagination.Sort, pagination.SortType.ToLower() == "asc" ? true : false, pagination.PageSize, pagination.PageIndex);
            pagination.TotalCount = total;
            return data;
        }
        public async Task<IEnumerable<T>> FindList<T>(Expression<Func<T, bool>> condition, Pagination pagination) where T : class, new()
        {
            var data = await FindList<T>(condition, pagination.Sort, pagination.SortType.ToLower() == "asc" ? true : false, pagination.PageSize, pagination.PageIndex);
            pagination.TotalCount = data.total;
            return data.list;
        }
        public async Task<(int total, IEnumerable<T> list)> FindList<T>(string strSql, Pagination pagination) where T : class
        {
            var total = pagination.TotalCount;
            var data = await FindList<T>(strSql, pagination.Sort, pagination.SortType.ToLower() == "asc" ? true : false, pagination.PageSize, pagination.PageIndex);
            pagination.TotalCount = total;
            return data;
        }
        public async Task<IEnumerable<T>> FindList<T>(string strSql, DbParameter[] dbParameter, Pagination pagination) where T : class
        {
            var data = await FindList<T>(strSql, dbParameter, pagination.Sort, pagination.SortType.ToLower() == "asc" ? true : false, pagination.PageSize, pagination.PageIndex);
            pagination.TotalCount = data.total;
            return data.Item2;
        }

        #endregion

        #region 数据源查询

        /// <summary>
        /// 查询表
        /// </summary>
        /// <param name="strSql">语句</param>
        /// <returns>表数据</returns>
        public async Task<DataTable> FindTable(string strSql)
        {
            return await FindTable(strSql, null);
        }

        /// <summary>
        /// 查询表
        /// </summary>
        /// <param name="strSql">语句</param>
        /// <param name="dbParameter">参数</param>
        /// <returns>表数据</returns>
        public async Task<DataTable> FindTable(string strSql, DbParameter[] dbParameter)
        {
            var dbConnection = dbContext.Database.GetDbConnection();
            var reader = await new DbScalarExtension(dbContext, dbConnection).ExecuteReadeAsync(CommandType.Text, strSql, dbParameter);
            return DbExtension.IDataReaderToDataTable(reader);
        }

        /// <summary>
        /// 查询表（分页）
        /// </summary>
        /// <param name="strSql">语句</param>
        /// <param name="sort">排序</param>
        /// <param name="isAsc">Asc 排序</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="pageIndex">页索引</param>
        /// <returns>总行数，表数据</returns>
        public async Task<(int total, DataTable)> FindTable(string strSql, string sort, bool isAsc, int pageSize, int pageIndex)
        {
            return await FindTable(strSql, null, sort, isAsc, pageSize, pageIndex);
        }

        /// <summary>
        /// 查询表（分页）
        /// </summary>
        /// <param name="strSql">语句</param>
        /// <param name="dbParameter">参数</param>
        /// <param name="sort">排序</param>
        /// <param name="isAsc">Asc 排序</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="pageIndex">页索引</param>
        /// <returns>总行数，表数据</returns>
        public async Task<(int total, DataTable)> FindTable(string strSql, DbParameter[] dbParameter, string sort, bool isAsc, int pageSize, int pageIndex)
        {
            var dbConnection = dbContext.Database.GetDbConnection();
            var DbScalarExtension = new DbScalarExtension(dbContext, dbConnection);
            var sb = new StringBuilder();
            sb.Append(DbPageExtension.GetPageSql(strSql, dbParameter, sort, isAsc, pageSize, pageIndex));
            var tempTotal = await DbScalarExtension.ExecuteScalarAsync(CommandType.Text, "SELECT COUNT(1) FROM (" + strSql + ") T", dbParameter);
            var total = tempTotal.ParseToInt();
            if (total > 0)
            {
                var reader = await DbScalarExtension.ExecuteReadeAsync(CommandType.Text, sb.ToString(), dbParameter);
                var resultTable = DbExtension.IDataReaderToDataTable(reader);
                return (total, resultTable);
            }
            else
            {
                return (total, new DataTable());
            }
        }

        /// <summary>
        /// 查询首行首列
        /// </summary>
        /// <param name="strSql">语句</param>
        /// <returns>首行首列</returns>
        public async Task<object> FindObject(string strSql)
        {
            return await FindObject(strSql, null);
        }

        /// <summary>
        /// 查询首行首列
        /// </summary>
        /// <param name="strSql">语句</param>
        /// <param name="dbParameter">参数</param>
        /// <returns>首行首列</returns>
        public async Task<object> FindObject(string strSql, DbParameter[] dbParameter)
        {
            var dbConnection = dbContext.Database.GetDbConnection();
            return await new DbScalarExtension(dbContext, dbConnection).ExecuteScalarAsync(CommandType.Text, strSql, dbParameter);
        }

        /// <summary>
        /// 查询首行数据，并转对象
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="strSql">语句</param>
        /// <returns>对象</returns>
        public async Task<T> FindObject<T>(string strSql) where T : class
        {
            var dbConnection = dbContext.Database.GetDbConnection();
            var dbScalar = new DbScalarExtension(dbContext, dbConnection);
            var dataReader = await dbScalar.ExecuteReadeAsync(CommandType.Text, strSql);
            var dataList = DbExtension.IDataReaderToList<T>(dataReader);
            return dataList.FirstOrDefault();
        }

        #endregion
    }
}
