using YiSha.Util.Model;

namespace YiSha.Model.Param.SystemManage
{
    public class AreaListParam
    {
        public string AreaName { get; set; } = "";
    }

    public class BaseAreaParam
    {
        public int? ProvinceId { get; set; } = 0;
        public int? CityId { get; set; } = 0;
        public int? CountyId { get; set; } = 0;

        /// <summary>
        /// 逗号分隔的Id
        /// </summary>
        public string AreaId { get; set; } = "";
    }
}
