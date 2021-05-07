using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using YiSha.Cache.Factory;
using YiSha.Util.Model;
using YiSha.Web.Code.State;

namespace YiSha.Web.Code
{
    public class Operator
    {
        public static Operator Instance
        {
            get { return new(); }
        }

        private readonly string _loginProvider = GlobalContext.Configuration.GetSection("SystemConfig:LoginProvider").Value;
        private readonly string _tokenName = "UserToken"; //cookie name or session name

        public async Task AddCurrent(string token)
        {
            switch (_loginProvider)
            {
                case "Cookie":
                    new CookieHelper().WriteCookie(_tokenName, token);
                    break;

                case "Session":
                    new SessionHelper().WriteSession(_tokenName, token);
                    break;

                case "WebApi":
                    OperatorInfo user = await new DataRepository().GetUserByToken(token);
                    if (user != null)
                    {
                        CacheFactory.Cache.SetCache(token, user);
                    }
                    break;

                default: throw new Exception("未找到LoginProvider配置");
            }
        }

        /// <summary>
        /// Api接口需要传入apiToken
        /// </summary>
        /// <param name="apiToken"></param>
        public void RemoveCurrent(string apiToken = "")
        {
            switch (_loginProvider)
            {
                case "Cookie":
                    new CookieHelper().RemoveCookie(_tokenName);
                    break;

                case "Session":
                    new SessionHelper().RemoveSession(_tokenName);
                    break;

                case "WebApi":
                    CacheFactory.Cache.RemoveCache(apiToken);
                    break;

                default: throw new Exception("未找到LoginProvider配置");
            }
        }

        /// <summary>
        /// Api接口需要传入apiToken
        /// </summary>
        /// <param name="apiToken"></param>
        /// <returns></returns>
        public async Task<OperatorInfo> Current(string apiToken = "")
        {
            IHttpContextAccessor hca = GlobalContext.ServiceProvider?.GetService<IHttpContextAccessor>();
            OperatorInfo user = null;
            string token = string.Empty;
            switch (_loginProvider)
            {
                case "Cookie":
                    if (hca.HttpContext != null)
                    {
                        token = new CookieHelper().GetCookie(_tokenName);
                    }
                    break;

                case "Session":
                    if (hca.HttpContext != null)
                    {
                        token = new SessionHelper().GetSession(_tokenName);
                    }
                    break;

                case "WebApi":
                    token = apiToken;
                    break;
            }
            if (string.IsNullOrEmpty(token))
            {
                return user;
            }
            token = token.Trim('"');
            user = CacheFactory.Cache.GetCache<OperatorInfo>(token);
            if (user == null)
            {
                user = await new DataRepository().GetUserByToken(token);
                if (user != null)
                {
                    CacheFactory.Cache.SetCache(token, user);
                }
            }
            return user;
        }
    }
}