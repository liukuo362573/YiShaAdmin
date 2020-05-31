using System;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Diagnostics;
using YiSha.Util;

namespace YiSha.Data
{
    public class DbHelper
    {
        /// <summary>
        /// 数据库类型
        /// </summary>
        public static DatabaseType DbType { get; set; }

        #region 构造函数
        /// <summary>
        /// 构造方法
        /// </summary>
        public DbHelper(DbConnection _dbConnection)
        {
            dbConnection = _dbConnection;
            dbCommand = dbConnection.CreateCommand();
        }

        public DbHelper(DbContext _dbContext, DbConnection _dbConnection)
        {
            dbContext = _dbContext;
            dbConnection = _dbConnection;
            dbCommand = dbConnection.CreateCommand();
        }
        #endregion

        #region 属性
        private DbContext dbContext { get; set; }
        /// <summary>
        /// 数据库连接对象
        /// </summary>
        private DbConnection dbConnection { get; set; }
        /// <summary>
        /// 执行命令对象
        /// </summary>
        private DbCommand dbCommand { get; set; }
        /// <summary>
        /// 关闭数据库连接
        /// </summary>
        public void Close()
        {
            if (dbConnection != null)
            {
                dbConnection.Close();
                dbConnection.Dispose();
            }
            if (dbCommand != null)
            {
                dbCommand.Dispose();
            }
        }
        #endregion

        /// <summary>
        /// 执行SQL返回 DataReader
        /// </summary>
        /// <param name="cmdType">命令的类型</param>
        /// <param name="strSql">Sql语句</param>
        /// <param name="dbParameter">Sql参数</param>
        /// <returns></returns>
        public async Task<IDataReader> ExecuteReadeAsync(CommandType cmdType, string strSql, params DbParameter[] dbParameter)
        {
            try
            {
                if (dbContext == null)
                {
                    PrepareCommand(dbConnection, dbCommand, null, cmdType, strSql, dbParameter);
                    var reader = await dbCommand.ExecuteReaderAsync(CommandBehavior.CloseConnection);
                    return reader;
                }
                else
                {
                    // 兼容EF Core的DbCommandInterceptor
                    var dependencies = ((IDatabaseFacadeDependenciesAccessor)dbContext.Database).Dependencies;
                    var relationalDatabaseFacade = (IRelationalDatabaseFacadeDependencies)dependencies;
                    var connection = relationalDatabaseFacade.RelationalConnection;
                    var logger = relationalDatabaseFacade.CommandLogger;
                    var commandId = Guid.NewGuid();

                    PrepareCommand(dbConnection, dbCommand, null, cmdType, strSql, dbParameter);

                    var startTime = DateTimeOffset.UtcNow;
                    var stopwatch = Stopwatch.StartNew();

                    var interceptionResult = logger == null
                       ? default
                       : await logger.CommandReaderExecutingAsync(
                           connection,
                           dbCommand,
                           dbContext,
                           Guid.NewGuid(),
                           connection.ConnectionId,
                           startTime);

                    var reader = interceptionResult.HasResult
                        ? interceptionResult.Result
                        : await dbCommand.ExecuteReaderAsync(CommandBehavior.CloseConnection);

                    if (logger != null)
                    {
                        reader = await logger.CommandReaderExecutedAsync(
                            connection,
                            dbCommand,
                            dbContext,
                            commandId,
                            connection.ConnectionId,
                            reader,
                            startTime,
                            stopwatch.Elapsed);
                    }
                    return reader;
                }
            }
            catch (Exception)
            {
                Close();
                throw;
            }
        }

        /// <summary>
        /// 执行查询，并返回查询所返回的结果集
        /// </summary>
        /// <param name="cmdType">命令的类型</param>
        /// <param name="strSql">Sql语句</param>
        /// <param name="dbParameter">Sql参数</param>
        /// <returns></returns>
        public async Task<object> ExecuteScalarAsync(CommandType cmdType, string strSql, params DbParameter[] dbParameter)
        {
            try
            {
                if (dbContext == null)
                {
                    PrepareCommand(dbConnection, dbCommand, null, cmdType, strSql, dbParameter);
                    var obj = await dbCommand.ExecuteScalarAsync();
                    dbCommand.Parameters.Clear();
                    return obj;
                }
                else
                {
                    // 兼容EF Core的DbCommandInterceptor
                    var dependencies = ((IDatabaseFacadeDependenciesAccessor)dbContext.Database).Dependencies;
                    var relationalDatabaseFacade = (IRelationalDatabaseFacadeDependencies)dependencies;
                    var connection = relationalDatabaseFacade.RelationalConnection;
                    var logger = relationalDatabaseFacade.CommandLogger;
                    var commandId = Guid.NewGuid();

                    PrepareCommand(dbConnection, dbCommand, null, cmdType, strSql, dbParameter);

                    var startTime = DateTimeOffset.UtcNow;
                    var stopwatch = Stopwatch.StartNew();

                    var interceptionResult = logger == null
                       ? default
                       : await logger.CommandScalarExecutingAsync(
                           connection,
                           dbCommand,
                           dbContext,
                           Guid.NewGuid(),
                           connection.ConnectionId,
                           startTime);

                    var obj = interceptionResult.HasResult
                        ? interceptionResult.Result
                        : await dbCommand.ExecuteScalarAsync();

                    if (logger != null)
                    {
                        obj = await logger.CommandScalarExecutedAsync(
                            connection,
                            dbCommand,
                            dbContext,
                            commandId,
                            connection.ConnectionId,
                            obj,
                            startTime,
                            stopwatch.Elapsed);
                    }
                    return obj;
                }
            }
            catch (Exception)
            {
                Close();
                throw;
            }
        }

        /// <summary>
        /// 为即将执行准备一个命令
        /// </summary>
        /// <param name="conn">SqlConnection对象</param>
        /// <param name="cmd">SqlCommand对象</param>
        /// <param name="isOpenTrans">DbTransaction对象</param>
        /// <param name="cmdType">执行命令的类型（存储过程或T-SQL，等等）</param>
        /// <param name="strSql">存储过程名称或者T-SQL命令行, e.g. Select * from Products</param>
        /// <param name="dbParameter">执行命令所需的sql语句对应参数</param>
        private void PrepareCommand(DbConnection conn, IDbCommand cmd, DbTransaction isOpenTrans, CommandType cmdType, string strSql, params DbParameter[] dbParameter)
        {
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }
            cmd.Connection = conn;
            cmd.CommandText = strSql;
            cmd.CommandTimeout = GlobalContext.SystemConfig.DBCommandTimeout;
            if (isOpenTrans != null)
            {
                cmd.Transaction = isOpenTrans;
            }
            cmd.CommandType = cmdType;
            if (dbParameter != null)
            {
                cmd.Parameters.Clear();
                dbParameter = DbParameterExtension.ToDbParameter(dbParameter);
                foreach (var parameter in dbParameter)
                {
                    cmd.Parameters.Add(parameter);
                }
            }
        }
    }
}
