using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using YiSha.Util.Extension;

namespace YiSha.Util
{
    public class LogHelper
    {
        private static readonly Logger log = LogManager.GetLogger(string.Empty);

        public static void Trace(object msg, Exception ex = null)
        {
            if (ex == null)
            {
                log.Trace(msg.ParseToString());
            }
            else
            {
                log.Trace(msg + GetExceptionMessage(ex));
            }
        }

        public static void Debug(object msg, Exception ex = null)
        {
            if (ex == null)
            {
                log.Debug(msg.ParseToString());
            }
            else
            {
                log.Debug(msg + GetExceptionMessage(ex));
            }
        }

        public static void Info(object msg, Exception ex = null)
        {
            if (ex == null)
            {
                log.Info(msg.ParseToString());
            }
            else
            {
                log.Info(msg + GetExceptionMessage(ex));
            }
        }

        public static void Warn(object msg, Exception ex = null)
        {
            if (ex == null)
            {
                log.Warn(msg.ParseToString());
            }
            else
            {
                log.Warn(msg + GetExceptionMessage(ex));
            }
        }

        public static void Error(object msg, Exception ex = null)
        {
            if (ex == null)
            {
                log.Error(msg.ParseToString());
            }
            else
            {
                log.Error(msg + GetExceptionMessage(ex));
            }
        }

        public static void Error(Exception ex)
        {
            if (ex != null)
            {
                log.Error(GetExceptionMessage(ex));
            }
        }

        public static void Fatal(object msg, Exception ex = null)
        {
            if (ex == null)
            {
                log.Fatal(msg.ParseToString());
            }
            else
            {
                log.Fatal(msg + GetExceptionMessage(ex));
            }
        }

        public static void Fatal(Exception ex)
        {
            if (ex != null)
            {
                log.Fatal(GetExceptionMessage(ex));
            }
        }

        private static string GetExceptionMessage(Exception ex)
        {
            string message = string.Empty;
            if (ex != null)
            {
                message += ex.Message;
                message += Environment.NewLine;
                Exception originalException = ex.GetOriginalException();
                if (originalException != null)
                {
                    if (originalException.Message != ex.Message)
                    {
                        message += originalException.Message;
                        message += Environment.NewLine;
                    }
                }
                message += ex.StackTrace;
                message += Environment.NewLine;
            }
            return message;
        }
    }
}
