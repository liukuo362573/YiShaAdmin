using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace YiSha.Util.Model
{
    /// <summary>
    /// 这个是移动端Api用的
    /// </summary>
    public class BaseApiToken
    {
        [NotMapped]
        [Description("WebApi没有Cookie和Session，所以需要传入Token来标识用户身份，请加在Url后面")]
        public string Token { get; set; }
    }
}
