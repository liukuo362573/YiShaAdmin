﻿using Microsoft.AspNetCore.Mvc;

namespace YiSha.Admin.Web.Areas.DemoManage.Controllers
{
    [Area("DemoManage")]
    public class ReportController : Controller
    {
        public IActionResult ECharts()
        {
            return View();
        }

        public IActionResult Peity()
        {
            return View();
        }
    }
}