using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YiSha.Entity;
using YiSha.Model;
using YiSha.Model.Result.SystemManage;

namespace YiSha.Admin.WebApi.Controllers
{
    /// <summary>
    /// 字典数据控制器
    /// </summary>
    [Authorize]
    [Route("[controller]/[action]")]
    public class DataDictController : Controller
    {
        /// <summary>
        /// 数据库上下文
        /// </summary>
        private MyDbContext DbCmd { get; }

        /// <summary>
        /// 字典数据控制器
        /// </summary>
        public DataDictController(MyDbContext myDbContext)
        {
            this.DbCmd = myDbContext;
        }

        /// <summary>
        /// 获取数据字典列表
        /// </summary>
        /// <param name="dictType">字典类型</param>
        /// <param name="remark">备注</param>
        /// <returns></returns>
        [HttpGet]
        public TData<List<DataDictInfo>> GetList(string? dictType = default, string? remark = default)
        {
            var dataDictInfoList = (from dic in DbCmd.SysDataDict
                                    where (dictType == default || dic.DictType == dictType)
                                    orderby dic.DictSort
                                    select new DataDictInfo
                                    {
                                        Detail = (from dicDetail in DbCmd.SysDataDictDetail
                                                  where dic.DictType == dicDetail.DictType
                                                        && (remark == default || dicDetail.Remark.Contains(remark))
                                                  orderby dicDetail.DictSort
                                                  select new DataDictDetailInfo
                                                  {
                                                      DictKey = dicDetail.DictKey,
                                                      DictValue = dicDetail.DictValue,
                                                      ListClass = dicDetail.ListClass,
                                                      IsDefault = dicDetail.IsDefault,
                                                      DictStatus = dicDetail.DictStatus,
                                                      Remark = dicDetail.Remark,
                                                  }).ToList(),
                                        DictType = dic.DictType,
                                    }).ToList();

            var obj = new TData<List<DataDictInfo>>();
            obj.Data = dataDictInfoList;
            obj.Tag = 1;
            return obj;
        }
    }
}
