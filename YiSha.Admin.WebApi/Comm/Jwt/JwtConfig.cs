using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
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
        /// 凭据有效时长
        /// </summary>
        public static TimeSpan ClockSkew { get; set; } = TimeSpan.FromDays(60);

        /// <summary>
        /// 配置文件数据
        /// </summary>
        internal static JwtConfigData JwtConfigData { get; } = ConfigHelp.Get<JwtConfigData>("JwtConfig");

        /// <summary>
        /// 添加 Jwt 服务并配置
        /// </summary>
        /// <param name="services">服务</param>
        public static void AddJwtConfig(this IServiceCollection services)
        {
            ClockSkew = TimeSpan.FromDays(3);
            services.AddJwtConfig(ClockSkew);
        }

        /// <summary>
        /// 添加 Jwt 服务并配置
        /// </summary>
        /// <param name="services">服务</param>
        /// <param name="clockSkew">有效时间</param>
        public static void AddJwtConfig(this IServiceCollection services, TimeSpan clockSkew)
        {
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
                    ValidIssuer = JwtConfigData.Issuer,

                    ValidateAudience = true,//是否验证Audience
                    //ValidAudience = JwtConfigData.Audience,
                    AudienceValidator = (m, n, z) => ValidatorUser(m, n, z),

                    IssuerSigningKey = GetSymmetric()//拿到SecurityKey
                };
            });
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
        /// 验证用户
        /// </summary>
        /// <param name="audiences">audiences</param>
        /// <param name="securityToken">securityToken</param>
        /// <param name="validationParameters">validationParameters</param>
        /// <returns></returns>
        private static bool ValidatorUser(IEnumerable<string> audiences, SecurityToken securityToken, TokenValidationParameters validationParameters)
        {
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            try
            {
                //安全密钥（外部）
                var securityKeyA = jwtSecurityToken?.SigningKey as SymmetricSecurityKey;
                if (securityKeyA == null) return false;
                var securityKeyAStr = Encoding.Default.GetString(securityKeyA.Key);
                //安全密钥（内部）
                var securityKeyB = GetSymmetric();
                if (securityKeyB == null) return false;
                var securityKeyBStr = Encoding.Default.GetString(securityKeyB.Key);
                //比较
                if (securityKeyAStr != securityKeyBStr) return false;
                //Claims
                var claims = jwtSecurityToken?.Claims.ToList();
                if (claims == null) return false;
                //有效的时间内
                var eClaim = claims.Where(T => T.Type == "exp");
                var expires = eClaim?.FirstOrDefault()?.Value;
                if (expires == null) return false;
                if (expires.ToLong() > GlobalConstant.TimeStamp.ToLong()) return false;
                //解析 Claim
                var jwtValidator = JwtValidator.GetData(claims);
                if (jwtValidator?.Token == null) return false;
                //验证登录
                var isOK = jwtValidator.Validate(jwtValidator.Token);
                return isOK;
            }
            catch (Exception ex)
            {
                var message = ex.Message;
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
            //生成 Claim
            var claims = jwtValidator.SetData();
            //ShowPII
            IdentityModelEventSource.ShowPII = true;
            //签名秘钥
            var authSigningKey = GetSigning();
            //凭据
            var token = new JwtSecurityToken(
                         issuer: JwtConfigData.Issuer,
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
        /// 获取 SymmetricSecurityKey
        /// </summary>
        /// <returns></returns>
        internal static SymmetricSecurityKey GetSymmetric()
        {
            var strResult = MD5Help.GetMd5(JwtConfigData?.SecureKey);//Md5
            var secureKey = strResult.Replace("-", "");
            var secureKeyByte = Encoding.UTF8.GetBytes(secureKey);
            var securityKey = new SymmetricSecurityKey(secureKeyByte);
            return securityKey;
        }

        /// <summary>
        /// 获取 SigningCredentials
        /// </summary>
        /// <returns></returns>
        internal static SigningCredentials GetSigning()
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
