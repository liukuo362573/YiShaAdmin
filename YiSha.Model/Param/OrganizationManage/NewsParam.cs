using YiSha.Model.Param.SystemManage;

namespace YiSha.Model.Param.OrganizationManage
{
    public class NewsListParam : BaseAreaParam
    {
        public string NewsTitle { get; set; } = "";

        public int? NewsType { get; set; } = default;

        public string NewsTag { get; set; } = "";
    }
}
