using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using YiSha.Common;
using YiSha.Util;

namespace YiSha.Admin.WebApi.Comm
{
    /// <summary>
    /// 配置 JWT
    /// </summary>
    public static class JwtConfig
    {
        /// <summary>
        /// 有效时间
        /// </summary>
        public static TimeSpan ClockSkew { get; set; } = TimeSpan.FromDays(3);

        /// <summary>
        /// 添加 Jwt 服务并配置
        /// </summary>
        /// <typeparam name="T">Jwt 验证</typeparam>
        /// <param name="services">服务</param>
        public static void AddJwtConfig<T>(this IServiceCollection services) where T : JwtValidator, new()
        {
            ClockSkew = TimeSpan.FromDays(3);
            services.AddJwtConfig<T>(ClockSkew);
        }

        /// <summary>
        /// 添加 Jwt 服务并配置
        /// </summary>
        /// <typeparam name="T">Jwt 验证</typeparam>
        /// <param name="services">服务</param>
        /// <param name="clockSkew">有效时间</param>
        public static void AddJwtConfig<T>(this IServiceCollection services, TimeSpan clockSkew) where T : JwtValidator, new()
        {
            var tokenConfig = ConfigHelp.Get<JwtConfigData>("JwtConfig");
            services.AddAuthentication(options =>//添加JWT Scheme
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>//添加 Jwt 验证
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateLifetime = true,//是否验证时间
                    ClockSkew = clockSkew,//有效时间

                    ValidateIssuer = true,//是否验证Issuer
                    ValidIssuer = tokenConfig.Issuer,

                    ValidateAudience = true,//是否验证Audience
                    //ValidAudience = tokenConfig.Audience,
                    AudienceValidator = (m, n, z) => ValidatorUser<T>(m, n, z),

                    IssuerSigningKey = GetSymmetric()//拿到SecurityKey
                };
            });
        }

        /// <summary>
        /// 验证用户
        /// </summary>
        /// <typeparam name="T">Jwt 验证</typeparam>
        /// <param name="audiences">audiences</param>
        /// <param name="securityToken">securityToken</param>
        /// <param name="validationParameters">validationParameters</param>
        /// <returns></returns>
        private static bool ValidatorUser<T>(IEnumerable<string> audiences, SecurityToken securityToken, TokenValidationParameters validationParameters) where T : JwtValidator, new()
        {
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            try
            {
                //账号
                var aClaim = jwtSecurityToken?.Claims.Where(T => T.Type == "account");
                var account = aClaim?.FirstOrDefault()?.Value;
                if (account == null) return false;
                //密码
                var pClaim = jwtSecurityToken?.Claims.Where(T => T.Type == "password");
                var password = pClaim?.FirstOrDefault()?.Value;
                if (password == null) return false;
                //调用实现方法
                var jwtValidator = new T();
                return jwtValidator.Validate(account, password);
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 获取 Token
        /// </summary>
        /// <param name="jwtValidator">Jwt 验证</param>
        /// <returns></returns>
        public static JwtTokenResult GetToken(JwtValidator jwtValidator)
        {
            //准备返回的结构
            var jwtToken = new JwtTokenResult();
            //调用实现方法
            if (!jwtValidator.Validate()) return jwtToken;
            //
            IdentityModelEventSource.ShowPII = true;
            var tokenConfig = ConfigHelp.Get<JwtConfigData>("JwtConfig");
            if (jwtValidator?.Account == null) return jwtToken;
            if (jwtValidator?.Password == null) return jwtToken;
            //Claim
            var claims = new List<Claim>();
            claims.Add(new Claim("account", jwtValidator.Account));
            claims.Add(new Claim("password", jwtValidator.Password));
            //签名秘钥
            var authSigningKey = GetSigning();
            //凭据
            var token = new JwtSecurityToken(
                         issuer: tokenConfig.Issuer,
                         claims: claims,
                         expires: DateTime.Now.Add(ClockSkew),
                         signingCredentials: authSigningKey);
            //结构数据
            jwtToken.Status = true;
            jwtToken.Token = new JwtSecurityTokenHandler().WriteToken(token);
            jwtToken.ValidTo = token.ValidTo;
            return jwtToken;
        }

        /// <summary>
        /// 启用 授权验证服务
        /// </summary>
        /// <param name="app">应用</param>
        public static void JwtAuthorize(this IApplicationBuilder app)
        {
            //用户认证
            app.UseAuthentication();
            //用户授权
            app.UseAuthorization();
        }

        /// <summary>
        /// 获取 SymmetricSecurityKey
        /// </summary>
        /// <returns></returns>
        public static SymmetricSecurityKey GetSymmetric()
        {
            var tokenConfig = ConfigHelp.Get<JwtConfigData>("JwtConfig");
            var strResult = MD5Help.GetMd5(tokenConfig?.SecureKey);//Md5
            var secureKey = strResult.Replace("-", "");
            var secureKeyByte = Encoding.UTF8.GetBytes(secureKey);
            var securityKey = new SymmetricSecurityKey(secureKeyByte);
            return securityKey;
        }

        /// <summary>
        /// 获取 SigningCredentials
        /// </summary>
        /// <returns></returns>
        public static SigningCredentials GetSigning()
        {
            var authSigningKey = GetSymmetric();
            var securityAlgorithms = SecurityAlgorithms.HmacSha256;
            var credentials = new SigningCredentials(authSigningKey, securityAlgorithms);
            return credentials;
        }
    }

    /// <summary>
    /// 配置文件数据映射
    /// </summary>
    internal partial class JwtConfigData
    {
        /// <summary>
        /// 安全密钥
        /// </summary>
        public string? SecureKey { get; set; }

        /// <summary>
        /// 签发者
        /// </summary>
        public string? Issuer { get; set; }
    }
}
