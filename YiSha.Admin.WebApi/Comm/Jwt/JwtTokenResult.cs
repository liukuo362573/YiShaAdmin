namespace YiSha.Admin.WebApi.Comm
{
    /// <summary>
    /// 获取 Token 结果
    /// </summary>
    public class JwtTokenResult
    {
        /// <summary>
        /// 状态
        /// </summary>
        public bool Status { get; set; } = false;

        /// <summary>
        /// Token
        /// </summary>
        public string Token { get; set; } = "";

        /// <summary>
        /// 有效时间
        /// </summary>
        public DateTime ValidTo { get; set; } = DateTime.Now;
    }
}
