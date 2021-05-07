using NLog;
using System;
using YiSha.Util.Extension;

namespace YiSha.Util.Helper
{
    public static class LogHelper
    {
        private static readonly Logger _log = LogManager.GetLogger(string.Empty);

        public static void Trace(object msg, Exception ex = null)
        {
            if (ex == null)
            {
                _log.Trace(msg.ParseToString());
            }
            else
            {
                _log.Trace(msg + GetExceptionMessage(ex));
            }
        }

        public static void Debug(object msg, Exception ex = null)
        {
            if (ex == null)
            {
                _log.Debug(msg.ParseToString());
            }
            else
            {
                _log.Debug(msg + GetExceptionMessage(ex));
            }
        }

        public static void Info(object msg, Exception ex = null)
        {
            if (ex == null)
            {
                _log.Info(msg.ParseToString());
            }
            else
            {
                _log.Info(msg + GetExceptionMessage(ex));
            }
        }

        public static void Warning(object msg, Exception ex = null)
        {
            if (ex == null)
            {
                _log.Warn(msg.ParseToString());
            }
            else
            {
                _log.Warn(msg + GetExceptionMessage(ex));
            }
        }

        public static void Error(Exception ex)
        {
            if (ex != null)
            {
                _log.Error(GetExceptionMessage(ex));
            }
        }

        public static void Error(object msg, Exception ex = null)
        {
            if (ex == null)
            {
                _log.Error(msg.ParseToString());
            }
            else
            {
                _log.Error(msg + GetExceptionMessage(ex));
            }
        }

        public static void Fatal(Exception ex)
        {
            if (ex != null)
            {
                _log.Fatal(GetExceptionMessage(ex));
            }
        }

        public static void Fatal(object msg, Exception ex = null)
        {
            if (ex == null)
            {
                _log.Fatal(msg.ParseToString());
            }
            else
            {
                _log.Fatal(msg + GetExceptionMessage(ex));
            }
        }

        private static string GetExceptionMessage(Exception ex)
        {
            if (ex == null)
            {
                return string.Empty;
            }
            string message = ex.Message + Environment.NewLine;
            var originalException = ex.GetOriginalException();
            if (originalException != null)
            {
                if (originalException.Message != ex.Message)
                {
                    message += originalException.Message + Environment.NewLine;
                }
            }
            message += ex.StackTrace + Environment.NewLine;
            return message;
        }
    }
}