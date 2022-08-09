using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;
using System.Data.Common;
using System.Diagnostics;

namespace YiSha.DataBase.Extension
{
    /// <summary>
    /// 数据查询拓展
    /// </summary>
    public class DbScalarExtension
    {
        /// <summary>
        /// 数据查询拓展
        /// </summary>
        public DbScalarExtension(DbConnection _dbConnection)
        {
            MyDbConnection = _dbConnection;
            MyDbCommand = MyDbConnection.CreateCommand();
        }

        /// <summary>
        /// 数据查询拓展
        /// </summary>
        /// <param name="_dbContext">数据库上下文</param>
        /// <param name="_dbConnection">数据库连接对象</param>
        public DbScalarExtension(DbContext _dbContext, DbConnection _dbConnection)
        {
            MyDbContext = _dbContext;
            MyDbConnection = _dbConnection;
            MyDbCommand = MyDbConnection.CreateCommand();
        }

        /// <summary>
        /// 数据库上下文
        /// </summary>
        private DbContext MyDbContext { get; set; }

        /// <summary>
        /// 数据库连接对象
        /// </summary>
        private DbConnection MyDbConnection { get; set; }

        /// <summary>
        /// 执行命令对象
        /// </summary>
        private DbCommand MyDbCommand { get; set; }

        /// <summary>
        /// 关闭数据库连接
        /// </summary>
        public void Close()
        {
            if (MyDbConnection != null)
            {
                MyDbConnection.Close();
                MyDbConnection.Dispose();
            }
            if (MyDbCommand != null)
            {
                MyDbCommand.Dispose();
            }
        }

        /// <summary>
        /// 执行查询
        /// </summary>
        /// <param name="cmdType">命令的类型</param>
        /// <param name="strSql">Sql语句</param>
        /// <param name="dbParameter">Sql参数</param>
        /// <returns>DataReader</returns>
        public IDataReader ExecuteReade(CommandType cmdType, string strSql, params DbParameter[] dbParameter)
        {
            try
            {
                if (MyDbContext == null)
                {
                    PrepareCommand(MyDbConnection, MyDbCommand, null, cmdType, strSql, dbParameter);
                    var reader = MyDbCommand.ExecuteReader(CommandBehavior.CloseConnection);
                    return reader;
                }
                else
                {
                    //兼容 EF Core 的 DbCommandInterceptor
                    var dependencies = ((IDatabaseFacadeDependenciesAccessor)MyDbContext.Database).Dependencies;
                    var relationalDatabaseFacade = (IRelationalDatabaseFacadeDependencies)dependencies;
                    var connection = relationalDatabaseFacade.RelationalConnection;
                    var logger = relationalDatabaseFacade.CommandLogger;
                    var commandId = Guid.NewGuid();

                    PrepareCommand(MyDbConnection, MyDbCommand, null, cmdType, strSql, dbParameter);

                    var startTime = DateTimeOffset.UtcNow;
                    var stopwatch = Stopwatch.StartNew();

                    var interceptionResult = logger == null
                       ? default
                       : logger.CommandReaderExecuting(
                           connection,
                           MyDbCommand,
                           MyDbContext,
                           Guid.NewGuid(),
                           connection.ConnectionId,
                           startTime,
                           CommandSource.Unknown);

                    var reader = interceptionResult.HasResult ? interceptionResult.Result : MyDbCommand.ExecuteReader(CommandBehavior.CloseConnection);

                    if (logger != null)
                    {
                        reader = logger.CommandReaderExecuted(
                            connection,
                            MyDbCommand,
                            MyDbContext,
                            commandId,
                            connection.ConnectionId,
                            reader,
                            startTime,
                            stopwatch.Elapsed,
                           CommandSource.Unknown);
                    }
                    return reader;
                }
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                throw;
            }
            finally
            {
                Close();
            }
        }

        /// <summary>
        /// 执行查询
        /// </summary>
        /// <param name="cmdType">命令的类型</param>
        /// <param name="strSql">Sql语句</param>
        /// <param name="dbParameter">Sql参数</param>
        /// <returns>DataReader</returns>
        public async Task<IDataReader> ExecuteReadeAsync(CommandType cmdType, string strSql, params DbParameter[] dbParameter)
        {
            try
            {
                if (MyDbContext == null)
                {
                    PrepareCommand(MyDbConnection, MyDbCommand, null, cmdType, strSql, dbParameter);
                    var reader = await MyDbCommand.ExecuteReaderAsync(CommandBehavior.CloseConnection);
                    return reader;
                }
                else
                {
                    //兼容 EF Core 的 DbCommandInterceptor
                    var dependencies = ((IDatabaseFacadeDependenciesAccessor)MyDbContext.Database).Dependencies;
                    var relationalDatabaseFacade = (IRelationalDatabaseFacadeDependencies)dependencies;
                    var connection = relationalDatabaseFacade.RelationalConnection;
                    var logger = relationalDatabaseFacade.CommandLogger;
                    var commandId = Guid.NewGuid();

                    PrepareCommand(MyDbConnection, MyDbCommand, null, cmdType, strSql, dbParameter);

                    var startTime = DateTimeOffset.UtcNow;
                    var stopwatch = Stopwatch.StartNew();

                    var interceptionResult = logger == null
                       ? default
                       : await logger.CommandReaderExecutingAsync(
                           connection,
                           MyDbCommand,
                           MyDbContext,
                           Guid.NewGuid(),
                           connection.ConnectionId,
                           startTime,
                           CommandSource.Unknown);

                    var reader = interceptionResult.HasResult ? interceptionResult.Result : await MyDbCommand.ExecuteReaderAsync(CommandBehavior.CloseConnection);

                    if (logger != null)
                    {
                        reader = await logger.CommandReaderExecutedAsync(
                            connection,
                            MyDbCommand,
                            MyDbContext,
                            commandId,
                            connection.ConnectionId,
                            reader,
                            startTime,
                            stopwatch.Elapsed,
                            CommandSource.Unknown);
                    }
                    return reader;
                }
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                throw;
            }
            finally
            {
                Close();
            }
        }

        /// <summary>
        /// 执行查询首行首列
        /// </summary>
        /// <param name="cmdType">命令的类型</param>
        /// <param name="strSql">Sql语句</param>
        /// <param name="dbParameter">Sql参数</param>
        /// <returns>首行首列</returns>
        public object ExecuteScalar(CommandType cmdType, string strSql, params DbParameter[] dbParameter)
        {
            try
            {
                if (MyDbContext == null)
                {
                    PrepareCommand(MyDbConnection, MyDbCommand, null, cmdType, strSql, dbParameter);
                    var obj = MyDbCommand.ExecuteScalar();
                    MyDbCommand.Parameters.Clear();
                    return obj;
                }
                else
                {
                    //兼容 EF Core 的 DbCommandInterceptor
                    var dependencies = ((IDatabaseFacadeDependenciesAccessor)MyDbContext.Database).Dependencies;
                    var relationalDatabaseFacade = (IRelationalDatabaseFacadeDependencies)dependencies;
                    var connection = relationalDatabaseFacade.RelationalConnection;
                    var logger = relationalDatabaseFacade.CommandLogger;
                    var commandId = Guid.NewGuid();

                    PrepareCommand(MyDbConnection, MyDbCommand, null, cmdType, strSql, dbParameter);

                    var startTime = DateTimeOffset.UtcNow;
                    var stopwatch = Stopwatch.StartNew();

                    var interceptionResult = logger == null
                       ? default
                       : logger.CommandScalarExecuting(
                           connection,
                           MyDbCommand,
                           MyDbContext,
                           Guid.NewGuid(),
                           connection.ConnectionId,
                           startTime,
                           CommandSource.Unknown);

                    var obj = interceptionResult.HasResult ? interceptionResult.Result : MyDbCommand.ExecuteScalar();

                    if (logger != null)
                    {
                        obj = logger.CommandScalarExecuted(
                            connection,
                            MyDbCommand,
                            MyDbContext,
                            commandId,
                            connection.ConnectionId,
                            obj,
                            startTime,
                            stopwatch.Elapsed,
                           CommandSource.Unknown);
                    }
                    return obj;
                }
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                throw;
            }
            finally
            {
                Close();
            }
        }

        /// <summary>
        /// 执行查询首行首列
        /// </summary>
        /// <param name="cmdType">命令的类型</param>
        /// <param name="strSql">Sql语句</param>
        /// <param name="dbParameter">Sql参数</param>
        /// <returns>首行首列</returns>
        public async Task<object> ExecuteScalarAsync(CommandType cmdType, string strSql, params DbParameter[] dbParameter)
        {
            try
            {
                if (MyDbContext == null)
                {
                    PrepareCommand(MyDbConnection, MyDbCommand, null, cmdType, strSql, dbParameter);
                    var obj = await MyDbCommand.ExecuteScalarAsync();
                    MyDbCommand.Parameters.Clear();
                    return obj;
                }
                else
                {
                    //兼容 EF Core 的 DbCommandInterceptor
                    var dependencies = ((IDatabaseFacadeDependenciesAccessor)MyDbContext.Database).Dependencies;
                    var relationalDatabaseFacade = (IRelationalDatabaseFacadeDependencies)dependencies;
                    var connection = relationalDatabaseFacade.RelationalConnection;
                    var logger = relationalDatabaseFacade.CommandLogger;
                    var commandId = Guid.NewGuid();

                    PrepareCommand(MyDbConnection, MyDbCommand, null, cmdType, strSql, dbParameter);

                    var startTime = DateTimeOffset.UtcNow;
                    var stopwatch = Stopwatch.StartNew();

                    var interceptionResult = logger == null
                       ? default
                       : await logger.CommandScalarExecutingAsync(
                           connection,
                           MyDbCommand,
                           MyDbContext,
                           Guid.NewGuid(),
                           connection.ConnectionId,
                           startTime,
                           CommandSource.Unknown);

                    var obj = interceptionResult.HasResult ? interceptionResult.Result : await MyDbCommand.ExecuteScalarAsync();

                    if (logger != null)
                    {
                        obj = await logger.CommandScalarExecutedAsync(
                            connection,
                            MyDbCommand,
                            MyDbContext,
                            commandId,
                            connection.ConnectionId,
                            obj,
                            startTime,
                            stopwatch.Elapsed,
                           CommandSource.Unknown);
                    }
                    return obj;
                }
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                throw;
            }
            finally
            {
                Close();
            }
        }

        /// <summary>
        /// 为即将执行准备一个命令
        /// </summary>
        /// <param name="conn">SqlConnection对象</param>
        /// <param name="cmd">SqlCommand对象</param>
        /// <param name="isOpenTrans">DbTransaction对象</param>
        /// <param name="cmdType">执行命令的类型（存储过程或T-SQL，等等）</param>
        /// <param name="strSql">存储过程名称或者T-SQL命令行（select * from Products）</param>
        /// <param name="dbParameter">执行命令所需的sql语句对应参数</param>
        private void PrepareCommand(DbConnection conn, IDbCommand cmd, DbTransaction isOpenTrans, CommandType cmdType, string strSql, params DbParameter[] dbParameter)
        {
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }
            cmd.Connection = conn;
            cmd.CommandText = strSql;
            cmd.CommandTimeout = DbFactory.Timeout;
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
