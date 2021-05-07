using System;

namespace YiSha.Util.Extension
{
    public static partial class Extensions
    {
        public static Exception GetOriginalException(this Exception ex)
        {
            while (true)
            {
                if (ex?.InnerException == null) return ex;
                ex = ex.InnerException;
            }
        }
    }
}