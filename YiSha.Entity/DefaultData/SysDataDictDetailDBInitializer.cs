using YiSha.Entity.Models;
using YiSha.Util;

namespace YiSha.Entity.DefaultData
{
    /// <summary>
    /// 字典数据表数据初始化
    /// </summary>
    internal class SysDataDictDetailDBInitializer
    {
        /// <summary>
        /// 获取数据
        /// </summary>
        public static List<SysDataDictDetail> GetData
        {
            get
            {
                var lists = new List<SysDataDictDetail>();

                lists.Add(new SysDataDictDetail()
                {
                    Id = 16508640061124400,
                    BaseIsDelete = 0,
                    BaseCreateTime = "2022-01-01 00:00:00".ToDate(),
                    BaseModifyTime = "2022-01-01 00:00:00".ToDate(),
                    BaseCreatorId = 0,
                    BaseModifierId = 16508640061130151,
                    BaseVersion = 0,
                    DictType = "NewsType",
                    DictSort = 1,
                    DictKey = 1,
                    DictValue = "产品案例",
                    ListClass = "primary",
                    DictStatus = 1,
                    IsDefault = 1,
                    Remark = "",
                });

                lists.Add(new SysDataDictDetail()
                {
                    Id = 16508640061124401,
                    BaseIsDelete = 0,
                    BaseCreateTime = "2022-01-01 00:00:00".ToDate(),
                    BaseModifyTime = "2022-01-01 00:00:00".ToDate(),
                    BaseCreatorId = 0,
                    BaseModifierId = 0,
                    BaseVersion = 0,
                    DictType = "NewsType",
                    DictSort = 2,
                    DictKey = 2,
                    DictValue = "行业新闻",
                    ListClass = "warning",
                    DictStatus = 1,
                    IsDefault = 0,
                    Remark = "",
                });

                return lists;
            }
        }
    }
}
