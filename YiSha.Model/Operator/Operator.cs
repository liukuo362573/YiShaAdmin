using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using YiSha.Cache;
using YiSha.Entity;
using YiSha.Enum.OrganizationManage;
using YiSha.Util;

namespace YiSha.Model.Operator
{
    public class Operator
    {
        public static Operator Instance
        {
            get { return new Operator(); }
        }

        /// <summary>
        /// LoginProvider
        /// </summary>
        private string LoginProvider = ConfigHelp.Get("SystemConfig:LoginProvider");

        /// <summary>
        /// TokenName
        /// </summary>
        private string TokenName = "UserToken"; //cookie name or session name

        /// <summary>
        /// AddCurrent
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public void AddCurrent(string token)
        {
            switch (LoginProvider)
            {
                case "Cookie":
                    CookieHelper.Set(TokenName, token);
                    break;

                case "Session":
                    SessionHelper.Set(TokenName, token);
                    break;

                case "WebApi":
                    OperatorInfo user = GetUserByToken(token);
                    if (user != null)
                    {
                        CacheFactory.Cache.SetCache(token, user);
                    }
                    break;

                default:
                    throw new Exception("未找到LoginProvider配置");
            }
        }

        /// <summary>
        /// RemoveCurrent
        /// </summary>
        /// <param name="apiToken"></param>
        /// <exception cref="Exception"></exception>
        public void RemoveCurrent(string apiToken = "")
        {
            switch (LoginProvider)
            {
                case "Cookie":
                    CookieHelper.Remove(TokenName);
                    break;

                case "Session":
                    SessionHelper.Remove(TokenName);
                    break;

                case "WebApi":
                    CacheFactory.Cache.RemoveCache(apiToken);
                    break;

                default:
                    throw new Exception("未找到LoginProvider配置");
            }
        }

        /// <summary>
        /// Current apiToken
        /// </summary>
        /// <param name="apiToken"></param>
        /// <returns></returns>
        public OperatorInfo Current(string apiToken = "")
        {
            IHttpContextAccessor hca = GlobalContext.ServiceProvider?.GetService<IHttpContextAccessor>();
            OperatorInfo user = null;
            string token = string.Empty;
            switch (LoginProvider)
            {
                case "Cookie":
                    if (hca.HttpContext != null)
                    {
                        token = CookieHelper.Get(TokenName);
                    }
                    break;

                case "Session":
                    if (hca.HttpContext != null)
                    {
                        token = SessionHelper.Get(TokenName);
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
                user = GetUserByToken(token);
                if (user != null)
                {
                    CacheFactory.Cache.SetCache(token, user);
                }
            }
            return user;
        }

        /// <summary>
        /// 获取用户 Token
        /// </summary>
        /// <param name="token">token</param>
        /// <returns></returns>
        public OperatorInfo GetUserByToken(string token)
        {
            using var DbCmd = new MyDbContext();
            //
            if (!SecurityHelper.IsSafeSqlParam(token)) return null;
            //
            token = token.ToStr().Trim();
            //
            var operatorInfo = DbCmd.SysUser.Where(W => W.WebToken == token || W.ApiToken == token).Select(T => new OperatorInfo
            {
                UserId = T.Id,
                UserStatus = T.UserStatus,
                IsOnline = T.IsOnline,
                UserName = T.UserName,
                RealName = T.RealName,
                Portrait = T.Portrait,
                DepartmentId = T.DepartmentId,
                WebToken = T.WebToken,
                ApiToken = T.ApiToken,
                IsSystem = T.IsSystem,
            }).FirstOrDefault();
            //
            if (operatorInfo != null)
            {
                //角色
                var roleEnum = UserBelongTypeEnum.Role.ToInt();
                var roleList = DbCmd.SysUserBelong.Where(W => W.UserId == operatorInfo.UserId && W.BelongType == roleEnum).Select(T => T.BelongId).ToList();
                operatorInfo.RoleIds = string.Join(",", roleList);
                //部门名称
                var departmentName = DbCmd.SysDepartment.Where(W => W.Id == operatorInfo.DepartmentId).Select(T => T.DepartmentName).FirstOrDefault();
                if (departmentName != null) operatorInfo.DepartmentName = departmentName.ToStr();
            }
            return operatorInfo;
        }
    }
}
