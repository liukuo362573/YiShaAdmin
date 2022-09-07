using YiSha.Entity.Models;
using YiSha.Util;

namespace YiSha.Entity.DefaultData
{
    /// <summary>
    /// 字典类型表数据初始化
    /// </summary>
    internal class SysDataDictDBInitializer
    {
        /// <summary>
        /// 获取数据
        /// </summary>
        public static List<SysDataDict> GetData
        {
            get
            {
                var lists = new List<SysDataDict>();

                lists.Add(new SysDataDict()
                {
                    Id = 16508640061124399,
                    BaseIsDelete = 0,
                    BaseCreateTime = "2022-01-01 00:00:00".ToDate(),
                    BaseModifyTime = "2022-01-01 00:00:00".ToDate(),
                    BaseCreatorId = 0,
                    BaseModifierId = 0,
                    BaseVersion = 0,
                    DictType = "NewsType",
                    DictSort = 0,
                    Remark = "新闻类别",
                });

                return lists;
            }
        }
    }
}
