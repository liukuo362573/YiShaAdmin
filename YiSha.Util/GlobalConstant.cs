namespace YiSha.Util
{
    /// <summary>
    /// 全局常数
    /// </summary>
    public class GlobalConstant
    {
        /// <summary>
        /// 系统类型
        /// </summary>
        public static PlatformID SystemType
        {
            get
            {
                return Environment.OSVersion.Platform;
            }
        }

        /// <summary>
        /// 系统 32Or64
        /// </summary>
        public static int System32Or64
        {
            get
            {
                if (IntPtr.Size == 8)
                {
                    return 64;//64 bit
                }
                else if (IntPtr.Size == 4)
                {
                    return 32;//32 bit
                }
                else
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// 程序运行路径
        /// </summary>
        public static string GetRunPath
        {
            get
            {
                string filePath = Path.GetDirectoryName(typeof(GlobalConstant).Assembly.Location);
                return filePath;
            }
        }

        /// <summary>
        /// 默认时间
        /// </summary>
        public static DateTime DefaultTime
        {
            get
            {
                return DateTime.Parse("1970-01-01 00:00:00");
            }
        }
    }
}
