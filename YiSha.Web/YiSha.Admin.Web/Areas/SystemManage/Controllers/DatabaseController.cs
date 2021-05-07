using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using YiSha.Admin.Web.Controllers;
using YiSha.Admin.Web.Filter;
using YiSha.Business.SystemManage;
using YiSha.Util.Model;

namespace YiSha.Admin.Web.Areas.SystemManage.Controllers
{
    [Area("SystemManage")]
    public class DatabaseController : BaseController
    {
        private readonly DatabaseTableBLL _databaseTableBll = new();

        #region 视图功能

        [AuthorizeFilter("system:datatable:view")]
        public IActionResult DatatableIndex()
        {
            return View();
        }

        #endregion

        #region 获取数据

        [HttpGet, AuthorizeFilter("system:datatable:search")]
        public async Task<IActionResult> GetTableListJson(string tableName)
        {
            var obj = await _databaseTableBll.GetTableList(tableName);
            return Json(obj);
        }

        [HttpGet, AuthorizeFilter("system:datatable:search")]
        public async Task<IActionResult> GetTablePageListJson(string tableName, Pagination pagination)
        {
            var obj = await _databaseTableBll.GetTablePageList(tableName, pagination);
            return Json(obj);
        }

        [HttpGet]
        public async Task<IActionResult> GetTableFieldListJson(string tableName)
        {
            var obj = await _databaseTableBll.GetTableFieldList(tableName);
            return Json(obj);
        }

        #endregion

        public async Task<IActionResult> SyncDatabaseJson()
        {
            var obj = await _databaseTableBll.SyncDatabase();
            return Json(obj);
        }
    }
}