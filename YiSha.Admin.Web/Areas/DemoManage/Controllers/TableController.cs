using Microsoft.AspNetCore.Mvc;
using YiSha.Model.Entity.OrganizationManage;
using YiSha.Model.Param.OrganizationManage;
using YiSha.Util.Model;

namespace YiSha.Admin.Web.Areas.DemoManage.Controllers
{
    public class TableController : DemoManageBaseController
    {
        public IActionResult Editable()
        {
            return View();
        }

        public IActionResult Image()
        {
            return View();
        }

        public IActionResult Footer()
        {
            return View();
        }

        public IActionResult GroupHeader()
        {
            return View();
        }

        public IActionResult MultiToolbar()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetPageListJson(UserListParam param, Pagination pagination)
        {
            // 测试总共23条数据
            int total = 23;
            TData<List<UserEntity>> obj = new TData<List<UserEntity>>();
            obj.Total = total;
            obj.Data = new List<UserEntity>();
            int id = (pagination.PageIndex - 1) * pagination.PageSize + 1;
            for (int i = id; i <= pagination.PageIndex * pagination.PageSize; i++)
            {
                if (i > total)
                {
                    break;
                }
                obj.Data.Add(new UserEntity
                {
                    Id = i,
                    RealName = "用户" + i,
                    Mobile = "15612345678",
                    Email = "test@163.com",
                    Birthday = DateTime.Now.ToString("yyyy-MM-dd"),
                    LoginCount = new Random().Next(1, 100),
                    UserStatus = i % 2
                });
            }
            obj.Tag = 1;
            return Json(obj);
        }
    }
}
