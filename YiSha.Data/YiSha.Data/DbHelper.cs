using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Threading.Tasks;
using YiSha.Util.Model;

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
        public DbHelper(DbConnection dbConnection)
        {
            DbConnection = dbConnection;
            DbCommand = DbConnection.CreateCommand();
        }

        public DbHelper(DbContext dbContext, DbConnection dbConnection)
        {
            DbContext = dbContext;
            DbConnection = dbConnection;
            DbCommand = DbConnection.CreateCommand();
        }

        #endregion 构造函数

        #region 属性

        private DbContext DbContext { get; }

        /// <summary>
        /// 数据库连接对象
        /// </summary>
        private DbConnection DbConnection { get; }

        /// <summary>
        /// 执行命令对象
        /// </summary>
        private DbCommand DbCommand { get; }

        /// <summary>
        /// 执行命令对象的行为
        /// </summary>
        private static CommandBehavior CommandBehavior => CommandBehavior.CloseConnection | CommandBehavior.KeyInfo | CommandBehavior.SequentialAccess | CommandBehavior.SingleResult;

        /// <summary>
        /// 关闭数据库连接
        /// </summary>
        private void Close()
        {
            if (DbConnection != null)
            {
                DbConnection.Close();
                DbConnection.Dispose();
            }
            DbCommand?.Dispose();
        }

        #endregion 属性

        /// <summary>
        /// 执行SQL返回 DataReader
        /// </summary>
        /// <param name="cmdType">命令的类型</param>
        /// <param name="sql">Sql语句</param>
        /// <param name="dbParameter">Sql参数</param>
        public async Task<IDataReader> ExecuteReadeAsync(CommandType cmdType, string sql, params DbParameter[] dbParameter)
        {
            try
            {
                if (DbContext == null)
                {
                    await PrepareCommand(DbConnection, DbCommand, null, cmdType, sql, dbParameter);
                    return await DbCommand.ExecuteReaderAsync(CommandBehavior);
                }

                // 兼容EF Core的DbCommandInterceptor
                var dependencies = ((IDatabaseFacadeDependenciesAccessor)DbContext.Database).Dependencies;
                var relationalDatabaseFacade = (IRelationalDatabaseFacadeDependencies)dependencies;
                var connection = relationalDatabaseFacade.RelationalConnection;
                var logger = relationalDatabaseFacade.CommandLogger;
                var commandId = Guid.NewGuid();

                await PrepareCommand(DbConnection, DbCommand, null, cmdType, sql, dbParameter);

                var startTime = DateTimeOffset.UtcNow;
                var stopwatch = Stopwatch.StartNew();

                var interceptionResult = logger == null
                    ? default
                    : await logger.CommandReaderExecutingAsync(connection,
                                                               DbCommand,
                                                               DbContext,
                                                               Guid.NewGuid(),
                                                               connection.ConnectionId,
                                                               startTime);

                var reader = interceptionResult.HasResult
                    ? interceptionResult.Result
                    : await DbCommand.ExecuteReaderAsync(CommandBehavior);

                if (logger != null)
                {
                    reader = await logger.CommandReaderExecutedAsync(connection,
                                                                     DbCommand,
                                                                     DbContext,
                                                                     commandId,
                                                                     connection.ConnectionId,
                                                                     reader,
                                                                     startTime,
                                                                     stopwatch.Elapsed);
                }
                return reader;
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
        /// <param name="sql">Sql语句</param>
        /// <param name="dbParameter">Sql参数</param>
        public async Task<object> ExecuteScalarAsync(CommandType cmdType, string sql, params DbParameter[] dbParameter)
        {
            try
            {
                if (DbContext == null)
                {
                    await PrepareCommand(DbConnection, DbCommand, null, cmdType, sql, dbParameter);
                    var result = await DbCommand.ExecuteScalarAsync();
                    DbCommand.Parameters.Clear();
                    return result;
                }

                // 兼容EF Core的DbCommandInterceptor
                var dependencies = ((IDatabaseFacadeDependenciesAccessor)DbContext.Database).Dependencies;
                var relationalDatabaseFacade = (IRelationalDatabaseFacadeDependencies)dependencies;
                var connection = relationalDatabaseFacade.RelationalConnection;
                var logger = relationalDatabaseFacade.CommandLogger;
                var commandId = Guid.NewGuid();

                await PrepareCommand(DbConnection, DbCommand, null, cmdType, sql, dbParameter);

                var startTime = DateTimeOffset.UtcNow;
                var stopwatch = Stopwatch.StartNew();

                var interceptionResult = logger == null
                    ? default
                    : await logger.CommandScalarExecutingAsync(connection,
                                                               DbCommand,
                                                               DbContext,
                                                               Guid.NewGuid(),
                                                               connection.ConnectionId,
                                                               startTime);

                var obj = interceptionResult.HasResult
                    ? interceptionResult.Result
                    : await DbCommand.ExecuteScalarAsync();

                if (logger != null)
                {
                    obj = await logger.CommandScalarExecutedAsync(connection,
                                                                  DbCommand,
                                                                  DbContext,
                                                                  commandId,
                                                                  connection.ConnectionId,
                                                                  obj,
                                                                  startTime,
                                                                  stopwatch.Elapsed);
                }

                return obj;
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
        /// <param name="sql">存储过程名称或者T-SQL命令行, e.g. Select * from Products</param>
        /// <param name="dbParameter">执行命令所需的sql语句对应参数</param>
        private static async Task PrepareCommand(DbConnection conn, IDbCommand cmd, IDbTransaction isOpenTrans, CommandType cmdType, string sql, params DbParameter[] dbParameter)
        {
            if (conn.State != ConnectionState.Open)
            {
                await conn.OpenAsync();
            }
            if (isOpenTrans != null)
            {
                cmd.Transaction = isOpenTrans;
            }
            if (dbParameter != null)
            {
                cmd.Parameters.Clear();
                foreach (var parameter in dbParameter)
                {
                    cmd.Parameters.Add(parameter);
                }
            }
            cmd.Connection = conn;
            cmd.CommandText = sql;
            cmd.CommandType = cmdType;
            cmd.CommandTimeout = GlobalContext.SystemConfig.DbCommandTimeout;
        }
    }
}