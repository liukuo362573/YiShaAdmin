using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace YiSha.Admin.Web.Areas.DemoManage.Controllers
{
    [Area("DemoManage")]
    public class IconController : Controller
    {
        public IActionResult FontAwesome()
        {
            return View();
        }
    }
}