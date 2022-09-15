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
        /// 登录时验证
        /// </summary>
        /// <returns>结果</returns>
        public bool Validate()
        {
            return true;
        }

        /// <summary>
        /// 验证登录
        /// </summary>
        /// <returns>结果</returns>
        public bool Validate(string Account, string Password)
        {
            return true;
        }
    }
}
