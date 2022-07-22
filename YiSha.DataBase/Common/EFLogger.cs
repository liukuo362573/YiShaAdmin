using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using YiSha.Util;

namespace YiSha.DataBase.Common
{
    /// <summary>
    /// 执行的脚本日志
    /// </summary>
    public class EFLogger
    {
        /// <summary>
        /// 输出到 DeBug
        /// </summary>
        public static readonly ILoggerFactory LoggerFactoryDeBug = LoggerFactory.Create(builder => { builder.AddDebug(); });

        /// <summary>
        /// 输出到 Console
        /// </summary>
        public static readonly ILoggerFactory loggerFactoryConsole = LoggerFactory.Create(builder => { builder.AddConsole(); });

        /// <summary>
        /// 输出数据
        /// </summary>
        public static void Add(DbContextOptionsBuilder optionsBuilder)
        {
            //开发模式
            if (GlobalContext.SystemConfig.Debug)
            {
                //控制器
                optionsBuilder.UseLoggerFactory(loggerFactoryConsole);
                ////DeBug
                //optionsBuilder.UseLoggerFactory(LoggerFactoryDeBug);
            }
        }
    }
}
