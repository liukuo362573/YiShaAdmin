using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Data.Common;
using YiSha.DataBase.Extension;
using YiSha.Util;

namespace YiSha.DataBase.Interceptor
{
    /// <summary>
    /// Sql 执行拦截器
    /// </summary>
    public class DbInterceptor : DbCommandInterceptor
    {
        /// <summary>
        /// 慢查询记录Sql(秒),保存到文件以便分析
        /// </summary>
        public static int DBSlowSqlLogTime { get; set; } = 5;

        public override async ValueTask<InterceptionResult<int>> NonQueryExecutingAsync(DbCommand command, CommandEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            var obj = await base.NonQueryExecutingAsync(command, eventData, result, cancellationToken);
            return obj;
        }

        public override async ValueTask<int> NonQueryExecutedAsync(DbCommand command, CommandExecutedEventData eventData, int result, CancellationToken cancellationToken = default)
        {
            if (eventData.Duration.TotalMilliseconds >= DBSlowSqlLogTime * 1000)
            {
                LogHelper.Warn("耗时的Sql：" + command.GetCommandText());
            }
            int val = await base.NonQueryExecutedAsync(command, eventData, result, cancellationToken);
            return val;
        }

        public override async ValueTask<InterceptionResult<object>> ScalarExecutingAsync(DbCommand command, CommandEventData eventData, InterceptionResult<object> result, CancellationToken cancellationToken = default)
        {
            var obj = await base.ScalarExecutingAsync(command, eventData, result, cancellationToken);
            return obj;
        }

        public override async ValueTask<object> ScalarExecutedAsync(DbCommand command, CommandExecutedEventData eventData, object result, CancellationToken cancellationToken = default)
        {
            if (eventData.Duration.TotalMilliseconds >= DBSlowSqlLogTime * 1000)
            {
                LogHelper.Warn("耗时的Sql：" + command.GetCommandText());
            }
            var obj = await base.ScalarExecutedAsync(command, eventData, result, cancellationToken);
            return obj;
        }

        public override async ValueTask<InterceptionResult<DbDataReader>> ReaderExecutingAsync(DbCommand command, CommandEventData eventData, InterceptionResult<DbDataReader> result, CancellationToken cancellationToken = default)
        {
            var obj = await base.ReaderExecutingAsync(command, eventData, result, cancellationToken);
            return obj;
        }

        public override async ValueTask<DbDataReader> ReaderExecutedAsync(DbCommand command, CommandExecutedEventData eventData, DbDataReader result, CancellationToken cancellationToken = default)
        {
            if (eventData.Duration.TotalMilliseconds >= DBSlowSqlLogTime * 1000)
            {
                LogHelper.Warn("耗时的Sql：" + command.GetCommandText());
            }
            var reader = await base.ReaderExecutedAsync(command, eventData, result, cancellationToken);
            return reader;
        }
    }
}
