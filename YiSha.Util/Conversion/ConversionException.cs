namespace YiSha.Util
{
    /// <summary>
    /// 数据转换拓展包
    /// </summary>
    public static partial class ConversionException
    {
        public static Exception GetOriginalException(this Exception ex)
        {
            if (ex.InnerException == null) return ex;

            return ex.InnerException.GetOriginalException();
        }
    }
}
