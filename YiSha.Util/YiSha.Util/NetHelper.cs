using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using YiSha.Util.Extension;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using YiSha.Util.Browser;

namespace YiSha.Util
{
    public class NetHelper
    {
        public static HttpContext HttpContext
        {
            get { return GlobalContext.ServiceProvider?.GetService<IHttpContextAccessor>().HttpContext; }
        }

        public static string Ip
        {
            get
            {
                string result = string.Empty;
                try
                {
                    if (HttpContext != null)
                    {
                        result = GetWebClientIp();
                    }
                    if (string.IsNullOrEmpty(result))
                    {
                        result = GetLanIp();
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.WriteWithTime(ex);
                }
                return result;
            }
        }

        private static string GetWebClientIp()
        {
            try
            {
                string ip = GetWebRemoteIp();
                foreach (var hostAddress in Dns.GetHostAddresses(ip))
                {
                    if (hostAddress.AddressFamily == AddressFamily.InterNetwork)
                    {
                        return hostAddress.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteWithTime(ex);
            }
            return string.Empty;
        }

        public static string GetLanIp()
        {
            try
            {
                foreach (var hostAddress in Dns.GetHostAddresses(Dns.GetHostName()))
                {
                    if (hostAddress.AddressFamily == AddressFamily.InterNetwork)
                    {
                        return hostAddress.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteWithTime(ex);
            }
            return string.Empty;
        }

        public static string GetWanIp()
        {
            string ip = string.Empty;
            try
            {
                string url = "http://www.net.cn/static/customercare/yourip.asp";
                string html = HttpHelper.HttpGet(url);
                if (!string.IsNullOrEmpty(html))
                {
                    ip = HtmlHelper.Resove(html, "<h2>", "</h2>");
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteWithTime(ex);
            }
            return ip;
        }

        private static string GetWebRemoteIp()
        {
            try
            {
                string ip = HttpContext?.Connection?.RemoteIpAddress.ParseToString();
                if (HttpContext != null && HttpContext.Request != null)
                {
                    if (HttpContext.Request.Headers.ContainsKey("X-Real-IP"))
                    {
                        ip = HttpContext.Request.Headers["X-Real-IP"].ToString();
                    }

                    if (HttpContext.Request.Headers.ContainsKey("X-Forwarded-For"))
                    {
                        ip = HttpContext.Request.Headers["X-Forwarded-For"].ToString();
                    }
                }
                return ip;
            }
            catch (Exception ex)
            {
                LogHelper.WriteWithTime(ex);
            }
            return string.Empty;
        }


        private static string GetWebClientHostName()
        {
            string result = string.Empty;
            try
            {
                string ip = GetWebRemoteIp();
                result = Dns.GetHostEntry(IPAddress.Parse(ip)).HostName;
                if (result == "localhost.localdomain")
                {
                    result = Dns.GetHostName();
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteWithTime(ex);
            }
            return result;
        }

        public static string Browser
        {
            get
            {
                try
                {
                    var browser = HttpContext.Request.Headers["User-Agent"];
                    var agent = UserAgent.ParseToString();
                    return BrowserHelper.GetBrwoserInfo(agent);
                }
                catch (Exception ex)
                {
                    LogHelper.WriteWithTime(ex);
                }
                return string.Empty;
            }
        }

        public static string UserAgent
        {
            get
            {
                string userAgent = string.Empty;
                try
                {
                    userAgent = HttpContext?.Request?.Headers["User-Agent"];
                }
                catch (Exception ex)
                {
                    LogHelper.WriteWithTime(ex);
                }
                return userAgent;
            }
        }

        public static string GetOSVersion()
        {
            var osVersion = string.Empty;
            try
            {
                var userAgent = UserAgent;
                if (userAgent.Contains("NT 10"))
                {
                    osVersion = "Windows 10";
                }
                else if (userAgent.Contains("NT 6.3"))
                {
                    osVersion = "Windows 8";
                }
                else if (userAgent.Contains("NT 6.1"))
                {
                    osVersion = "Windows 7";
                }
                else if (userAgent.Contains("NT 6.0"))
                {
                    osVersion = "Windows Vista/Server 2008";
                }
                else if (userAgent.Contains("NT 5.2"))
                {
                    osVersion = "Windows Server 2003";
                }
                else if (userAgent.Contains("NT 5.1"))
                {
                    osVersion = "Windows XP";
                }
                else if (userAgent.Contains("NT 5"))
                {
                    osVersion = "Windows 2000";
                }
                else if (userAgent.Contains("NT 4"))
                {
                    osVersion = "Windows NT4";
                }
                else if (userAgent.Contains("Android"))
                {
                    osVersion = "Android";
                }
                else if (userAgent.Contains("Me"))
                {
                    osVersion = "Windows Me";
                }
                else if (userAgent.Contains("98"))
                {
                    osVersion = "Windows 98";
                }
                else if (userAgent.Contains("95"))
                {
                    osVersion = "Windows 95";
                }
                else if (userAgent.Contains("Mac"))
                {
                    osVersion = "Mac";
                }
                else if (userAgent.Contains("Unix"))
                {
                    osVersion = "UNIX";
                }
                else if (userAgent.Contains("Linux"))
                {
                    osVersion = "Linux";
                }
                else if (userAgent.Contains("SunOS"))
                {
                    osVersion = "SunOS";
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteWithTime(ex);
            }
            return osVersion;
        }
    }
}
