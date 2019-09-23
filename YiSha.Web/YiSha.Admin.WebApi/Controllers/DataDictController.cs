using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YiSha.Business.SystemManage;
using YiSha.Model.Param.SystemManage;
using YiSha.Model.Result.SystemManage;
using YiSha.Util.Model;

namespace YiSha.Admin.WebApi.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    [AuthorizeFilter]
    public class DataDictController : ControllerBase
    {
        private DataDictBLL dataDictBLL = new DataDictBLL();

        #region 获取数据
        /// <summary>
        /// 获取数据字典列表
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<TData<List<DataDictInfo>>> GetList([FromQuery]DataDictListParam param)
        {
            TData<List<DataDictInfo>> obj = await dataDictBLL.GetDataDictList();
            obj.Tag = 1;
            return obj;
        }
        #endregion
    }
}