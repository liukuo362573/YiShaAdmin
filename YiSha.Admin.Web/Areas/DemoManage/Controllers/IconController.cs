using Microsoft.AspNetCore.Mvc;

namespace YiSha.Admin.Web.Areas.DemoManage.Controllers
{
    public class IconController : DemoManageBaseController
    {
        public IActionResult FontAwesome()
        {
            return View();
        }
    }
}
