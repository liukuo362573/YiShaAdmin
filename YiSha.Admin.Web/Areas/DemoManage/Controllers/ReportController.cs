using Microsoft.AspNetCore.Mvc;

namespace YiSha.Admin.Web.Areas.DemoManage.Controllers
{
    public class ReportController : DemoManageBaseController
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
