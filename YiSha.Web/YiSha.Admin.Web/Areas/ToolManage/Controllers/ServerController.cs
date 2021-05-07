using Microsoft.AspNetCore.Mvc;
using System;
using YiSha.Admin.Web.Controllers;
using YiSha.Util.Helper;
using YiSha.Util.Model;

namespace YiSha.Admin.Web.Areas.ToolManage.Controllers
{
    [Area("ToolManage")]
    public class ServerController : BaseController
    {
        #region 视图功能

        public IActionResult ServerIndex()
        {
            return View();
        }

        #endregion

        #region 获取数据

        [HttpGet]
        public IActionResult GetServerJson()
        {
            try
            {
                return Json(new TData<ComputerInfo> { Tag = 1, Data = ComputerHelper.GetComputerInfo() });
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                return Json(new TData<ComputerInfo> { Tag = 0, Message = ex.Message });
            }
        }

        public IActionResult GetServerIpJson()
        {
            string ip = NetHelper.GetWanIp();
            string ipLocation = IpLocationHelper.GetIpLocation(ip);
            return Json(new TData<string> { Data = $"{ip} ({ipLocation})", Tag = 1 });
        }

        #endregion
    }
}