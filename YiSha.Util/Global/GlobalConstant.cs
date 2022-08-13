using Microsoft.Extensions.Hosting;

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
        /// 是否正式环境
        /// </summary>
        public static bool IsProduction
        {
            get
            {
                var isDevelopment = GlobalContext.HostingEnvironment?.IsProduction();
                return isDevelopment.ToBool(false);
            }
        }

        /// <summary>
        /// 是否开发环境
        /// </summary>
        public static bool IsDevelopment
        {
            get
            {
                var isDevelopment = GlobalContext.HostingEnvironment?.IsDevelopment();
                return isDevelopment.ToBool(true);
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
        /// 程序运行路径（获取基目录，它由程序集冲突解决程序用来探测程序集）
        /// </summary>
        public static string GetRunPath2
        {
            get
            {
                string filePath = System.AppDomain.CurrentDomain.BaseDirectory;
                return filePath;
            }
        }

        /// <summary>
        /// 程序运行路径（获取当前工作目录的完全限定路径）
        /// </summary>
        public static string GetRunPath3
        {
            get
            {
                string filePath = System.Environment.CurrentDirectory;
                return filePath;
            }
        }

        /// <summary>
        /// 程序运行路径（获取应用程序的当前工作目录）
        /// </summary>
        public static string GetRunPath4
        {
            get
            {
                string filePath = System.IO.Directory.GetCurrentDirectory();
                return filePath;
            }
        }

        /// <summary>
        /// 当前时间戳
        /// </summary>
        public static string TimeStamp
        {
            get
            {
                return DateTime.Now.ToDateByUnix().ToTruncate(0).ToTrim();
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

        /// <summary>
        /// 获取 GUID
        /// </summary>
        public static string Guid
        {
            get
            {
                Guid guid = System.Guid.NewGuid();
                return guid.ToString();
            }
        }
    }
}
