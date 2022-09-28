using System.Security.Claims;
using YiSha.Common;
using YiSha.Entity;
using YiSha.Entity.Models;
using YiSha.Util;

namespace YiSha.Admin.WebApi.Comm
{
    /// <summary>
    /// Jwt 验证
    /// </summary>
    public class JwtValidator
    {
        /// <summary>
        /// 账号
        /// </summary>
        public string? Account { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string? Password { get; set; }

        /// <summary>
        /// 凭据
        /// </summary>
        public string? Token { get; set; }

        /// <summary>
        /// 登录时验证
        /// </summary>
        /// <returns>结果</returns>
        public bool Validate()
        {
            //查询用户信息
            using var dbCmd = new MyDbContext();
            var sysUser = dbCmd.SysUser.Where(T => T.UserName == Account).FirstOrDefault();
            if (sysUser == null) return false;
            var isEqual = sysUser.Password == MD5Help.GetMd5(Password + sysUser.Salt);
            sysUser.ApiToken = Token;
            dbCmd.SaveChanges();
            return isEqual;
        }

        /// <summary>
        /// 验证登录
        /// </summary>
        /// <returns>结果</returns>
        public bool Validate(string account, string password, string token)
        {
            //查询用户信息
            using var dbCmd = new MyDbContext();
            var sysUser = dbCmd.SysUser.Where(T => T.ApiToken == token).FirstOrDefault();
            if (sysUser == null) return false;
            return true;
        }

        /// <summary>
        /// 验证登录
        /// </summary>
        /// <returns>结果</returns>
        public bool Validate(string token)
        {
            //查询用户信息
            using var dbCmd = new MyDbContext();
            var sysUser = dbCmd.SysUser.Where(T => T.ApiToken == token).FirstOrDefault();
            if (sysUser == null) return false;
            return true;
        }

        /// <summary>
        /// 生成 Claim 数据
        /// </summary>
        /// <returns></returns>
        public List<Claim> SetData()
        {
            var claims = new List<Claim>();
            //判断为空
            if (Account == null) return claims;
            if (Password == null) return claims;
            if (Token == null) return claims;
            //Claim
            claims.Add(new Claim("account", Account));
            claims.Add(new Claim("password", Password));
            claims.Add(new Claim("token", Token));
            //返回
            return claims;
        }

        /// <summary>
        /// 解析 Claim 数据
        /// </summary>
        /// <param name="claims">claims</param>
        /// <returns></returns>
        public static JwtValidator GetData(List<Claim> claims)
        {
            var jwtValidator = new JwtValidator();
            //账号
            var aClaim = claims.Where(T => T.Type == "account");
            var account = aClaim?.FirstOrDefault()?.Value;
            if (account == null) return jwtValidator;
            jwtValidator.Account = account;
            //密码
            var pClaim = claims.Where(T => T.Type == "password");
            var password = pClaim?.FirstOrDefault()?.Value;
            if (password == null) return jwtValidator;
            jwtValidator.Password = password;
            //密码
            var tClaim = claims.Where(T => T.Type == "token");
            var token = tClaim?.FirstOrDefault()?.Value;
            if (token == null) return jwtValidator;
            jwtValidator.Token = token;
            //返回
            return jwtValidator;
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="claimsPrincipal">身份持有者</param>
        /// <returns></returns>
        public static SysUser GetUserInfo(ClaimsPrincipal claimsPrincipal)
        {
            //解析 Claim
            var claims = claimsPrincipal.Claims.ToList();
            var jwtValidator = GetData(claims);
            //查询用户信息
            using var dbCmd = new MyDbContext();
            var sysUser = dbCmd.SysUser.Where(T => T.ApiToken == jwtValidator.Token).FirstOrDefault();
            if (sysUser == null) return new SysUser();
            return sysUser;
        }

        /// <summary>
        /// 退出用户登录
        /// </summary>
        /// <param name="claimsPrincipal">身份持有者</param>
        /// <returns></returns>
        public static bool ExitLogin(ClaimsPrincipal claimsPrincipal)
        {
            //解析 Claim
            var claims = claimsPrincipal.Claims.ToList();
            var jwtValidator = GetData(claims);
            //查询用户信息
            using var dbCmd = new MyDbContext();
            var sysUser = dbCmd.SysUser.Where(T => T.ApiToken == jwtValidator.Token).FirstOrDefault();
            if (sysUser == null) return false;
            sysUser.ApiToken = MD5Help.GetMd5(GlobalConstant.TimeStamp);
            int updCount = dbCmd.SaveChanges();
            return updCount > 0;
        }
    }
}
