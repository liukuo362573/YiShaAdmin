namespace YiSha.Util.Model
{
    /// <summary>
    /// 系统配置
    /// </summary>
    public class SystemConfig
    {
        /// <summary>
        /// 系统配置
        /// </summary>
        public SystemConfig()
        {
            DBSlowSqlLogTime = 5;
        }

        /// <summary>
        /// 是否是Demo模式
        /// </summary>
        public bool Demo { get; set; }

        /// <summary>
        /// 是否是调试模式
        /// </summary>
        public bool Debug { get; set; }

        /// <summary>
        /// 允许一个用户在多个电脑同时登录
        /// </summary>
        public bool LoginMultiple { get; set; }

        /// <summary>
        /// 登录提供者
        /// </summary>
        public string LoginProvider { get; set; }

        /// <summary>
        /// 雪花 ID
        /// </summary>
        public int SnowFlakeWorkerId { get; set; }

        /// <summary>
        /// Api地址
        /// </summary>
        public string ApiSite { get; set; }

        /// <summary>
        /// 允许跨域的站点
        /// </summary>
        public string AllowCorsSite { get; set; }

        /// <summary>
        /// 网站虚拟目录
        /// </summary>
        public string VirtualDirectory { get; set; }

        /// <summary>
        /// 数据库类型
        /// </summary>
        public string DBProvider { get; set; }

        /// <summary>
        /// 数据库字符串
        /// </summary>
        public string DBConnectionString { get; set; }

        /// <summary>
        ///  数据库超时间（秒）
        /// </summary>
        public int DBCommandTimeout { get; set; }

        /// <summary>
        /// 慢查询记录Sql(秒),保存到文件以便分析
        /// </summary>
        public int DBSlowSqlLogTime { get; set; }

        /// <summary>
        /// 数据库备份路径
        /// </summary>
        public string DBBackup { get; set; }

        /// <summary>
        /// 缓存类型
        /// </summary>
        public string CacheProvider { get; set; }

        /// <summary>
        /// 缓存字符串
        /// </summary>
        public string RedisConnectionString { get; set; }
    }
}