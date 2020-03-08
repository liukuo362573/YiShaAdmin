using System;
using System.Collections.Generic;
using System.Text;

namespace YiSha.Util.Model
{
    public class SystemConfig
    {
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
        public string LoginProvider { get; set; }
        /// <summary>
        ///  数据库超时间（秒）
        /// </summary>
        public int CommandTimeout { get; set; }
        /// <summary>
        /// Snow Flake Worker Id
        /// </summary>
        public int SnowFlakeWorkerId { get; set; }
        /// <summary>
        /// api地址
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

        public string DBProvider { get; set; }
        public string DBConnectionString { get; set; }
        public string DBCommandTimeout { get; set; }
        /// <summary>
        /// 数据库备份路径
        /// </summary>
        public string DBBackup { get; set; }

        public string CacheProvider { get; set; }
        public string RedisConnectionString { get; set; }

        public string AppId { get; set; }
        public string AppSecret { get; set; }
        /// <summary>
        /// 商户Id
        /// </summary>
        public string MerchantId { get; set; }
        public string PayKey { get; set; }
        public string NotifyUrl { get; set; }
    }
}
