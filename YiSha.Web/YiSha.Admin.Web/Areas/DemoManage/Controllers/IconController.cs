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