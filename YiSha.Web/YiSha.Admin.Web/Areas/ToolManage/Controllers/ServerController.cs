﻿using Microsoft.AspNetCore.Mvc;
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
            TData<ComputerInfo> obj = new TData<ComputerInfo>();
            ComputerInfo computerInfo = null;
            try
            {
                computerInfo = ComputerHelper.GetComputerInfo();
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                obj.Message = ex.Message;
            }
            obj.Data = computerInfo;
            obj.Tag = 1;
            return Json(obj);
        }

        public IActionResult GetServerIpJson()
        {
            TData<string> obj = new TData<string>();
            string ip = NetHelper.GetWanIp();
            string ipLocation = IpLocationHelper.GetIpLocation(ip);
            obj.Data = string.Format("{0} ({1})", ip, ipLocation);
            obj.Tag = 1;
            return Json(obj);
        }

        #endregion
    }
}