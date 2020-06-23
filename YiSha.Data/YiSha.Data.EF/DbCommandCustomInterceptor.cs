using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore.Diagnostics;
using YiSha.Util;

namespace YiSha.Data.EF
{
    /// <summary>
    /// Sql执行拦截器
    /// </summary>
    public class DbCommandCustomInterceptor : DbCommandInterceptor
    {
        public async override Task<InterceptionResult<int>> NonQueryExecutingAsync(DbCommand command, CommandEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            var obj = await base.NonQueryExecutingAsync(command, eventData, result, cancellationToken);
            return obj;
        }

        public async override Task<int> NonQueryExecutedAsync(DbCommand command, CommandExecutedEventData eventData, int result, CancellationToken cancellationToken = default)
        {
            if (eventData.Duration.TotalMilliseconds >= GlobalContext.SystemConfig.DBSlowSqlLogTime * 1000)
            {
                LogHelper.Warn("耗时的Sql：" + command.GetCommandText());
            }
            int val = await base.NonQueryExecutedAsync(command, eventData, result, cancellationToken);
            return val;
        }

        public async override Task<InterceptionResult<object>> ScalarExecutingAsync(DbCommand command, CommandEventData eventData, InterceptionResult<object> result, CancellationToken cancellationToken = default)
        {
            var obj = await base.ScalarExecutingAsync(command, eventData, result, cancellationToken);
            return obj;
        }

        public async override Task<object> ScalarExecutedAsync(DbCommand command, CommandExecutedEventData eventData, object result, CancellationToken cancellationToken = default)
        {
            if (eventData.Duration.TotalMilliseconds >= GlobalContext.SystemConfig.DBSlowSqlLogTime * 1000)
            {
                LogHelper.Warn("耗时的Sql：" + command.GetCommandText());
            }
            var obj = await base.ScalarExecutedAsync(command, eventData, result, cancellationToken);
            return obj;
        }

        public async override Task<InterceptionResult<DbDataReader>> ReaderExecutingAsync(DbCommand command, CommandEventData eventData, InterceptionResult<DbDataReader> result, CancellationToken cancellationToken = default)
        {
            var obj = await base.ReaderExecutingAsync(command, eventData, result, cancellationToken);
            return obj;
        }

        public async override Task<DbDataReader> ReaderExecutedAsync(DbCommand command, CommandExecutedEventData eventData, DbDataReader result, CancellationToken cancellationToken = default)
        {
            if (eventData.Duration.TotalMilliseconds >= GlobalContext.SystemConfig.DBSlowSqlLogTime * 1000)
            {
                LogHelper.Warn("耗时的Sql：" + command.GetCommandText());
            }
            var reader = await base.ReaderExecutedAsync(command, eventData, result, cancellationToken);
            return reader;
        }
    }
}
