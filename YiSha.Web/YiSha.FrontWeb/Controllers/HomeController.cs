using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace YiSha.FrontWeb.Controllers
{
    public class HomeController : Controller
    {
        #region 视图功能
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }

        public IActionResult Product()
        {
            return View();
        }

        public IActionResult Question()
        {
            return View();
        }
        #endregion
    }
}
