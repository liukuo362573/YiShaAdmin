﻿using Microsoft.AspNetCore.Http;
using System;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using YiSha.Util;
using YiSha.Util.Extension;
using YiSha.Cache.Factory;

namespace YiSha.Web.Code
{
    public class Operator
    {
        public static Operator Instance
        {
            get { return new Operator(); }
        }

        private string LoginProvider = GlobalContext.Configuration.GetSection("SystemConfig:LoginProvider").Value;
        private string TokenName = "UserToken"; //cookie name or session name

        public async Task AddCurrent(string token)
        {
            switch (LoginProvider)
            {
                case "Cookie":
                    new CookieHelper().WriteCookie(TokenName, token);
                    break;

                case "Session":
                    new SessionHelper().WriteSession(TokenName, token);
                    break;

                case "WebApi":
                    OperatorInfo user = await new DataRepository().GetUserByToken(token);
                    if (user != null)
                    {
                        CacheFactory.Cache().AddCache(token, user);
                    }
                    break;

                default:
                    throw new Exception("未找到LoginProvider配置");
            }
        }

        /// <summary>
        /// Api接口需要传入apiToken
        /// </summary>
        /// <param name="apiToken"></param>
        public void RemoveCurrent(string apiToken = "")
        {
            switch (LoginProvider)
            {
                case "Cookie":
                    new CookieHelper().RemoveCookie(TokenName);
                    break;

                case "Session":
                    new SessionHelper().RemoveSession(TokenName);
                    break;

                case "WebApi":
                    CacheFactory.Cache().RemoveCache(apiToken);
                    break;

                default:
                    throw new Exception("未找到LoginProvider配置");
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
            if (hca == null || hca.HttpContext == null) return null;

            string token = string.Empty;
            switch (LoginProvider)
            {
                case "Cookie":
                    token = new CookieHelper().GetCookie(TokenName);
                    break;

                case "Session":
                    token = new SessionHelper().GetSession(TokenName);
                    break;

                case "WebApi":
                    token = apiToken;
                    break;
            }
            if (string.IsNullOrEmpty(token)) return null;
            else token = token.Trim('"');

            OperatorInfo user = CacheFactory.Cache().GetCache<OperatorInfo>(token);
            if (user == null)
            {
                user = await new DataRepository().GetUserByToken(token);
                if (user != null)
                {
                    CacheFactory.Cache().AddCache(token, user);
                }
            }
            return user;
        }
    }
}
