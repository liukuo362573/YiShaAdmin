using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace YiSha.Admin.Web.Areas.DemoManage.Controllers
{
    [Area("DemoManage")]
    public class FormController : Controller
    {
        public IActionResult Button()
        {
            return View();
        }

        public IActionResult Grid()
        {
            return View();
        }

        public IActionResult Select()
        {
            return View();
        }

        public IActionResult Timeline()
        {
            return View();
        }

        public IActionResult Basic()
        {
            return View();
        }

        public IActionResult Card()
        {
            return View();
        }

        public IActionResult Tab()
        {
            return View();
        }

        public IActionResult Panel()
        {
            return View();
        }

        public IActionResult Upload()
        {
            return View();
        }

        public IActionResult Datetime()
        {
            return View();
        }

        public IActionResult Editor()
        {
            return View();
        }
    }
}