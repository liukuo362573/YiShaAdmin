using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using YiSha.Admin.Web.Controllers;
using YiSha.Admin.Web.Filter;
using YiSha.Business.SystemManage;
using YiSha.CodeGenerator.Model;
using YiSha.CodeGenerator.Template;
using YiSha.Entity;
using YiSha.Util.Extension;
using YiSha.Util.Model;
using YiSha.Web.Code;

namespace YiSha.Admin.Web.Areas.ToolManage.Controllers
{
    [Area("ToolManage")]
    public class CodeGeneratorController : BaseController
    {
        private readonly DatabaseTableBLL _databaseTableBll = new();

        #region 视图功能

        [AuthorizeFilter("tool:codegenerator:view")]
        public IActionResult CodeGeneratorIndex()
        {
            return View();
        }

        public IActionResult CodeGeneratorForm(string outputModule)
        {
            ViewBag.OutputModule = outputModule;
            return View();
        }

        public IActionResult CodeGeneratorEditSearch()
        {
            return View();
        }

        public IActionResult CodeGeneratorEditToolbar()
        {
            return View();
        }

        public IActionResult CodeGeneratorEditList()
        {
            return View();
        }

        #endregion

        #region 获取数据

        [HttpGet, AuthorizeFilter("tool:codegenerator:search")]
        public async Task<IActionResult> GetTableFieldTreeListJson(string tableName)
        {
            var obj = await _databaseTableBll.GetTableFieldZtreeList(tableName);
            return Json(obj);
        }

        [HttpGet, AuthorizeFilter("tool:codegenerator:search")]
        public async Task<IActionResult> GetTableFieldTreePartListJson(string tableName, int upper = 0)
        {
            var obj = await _databaseTableBll.GetTableFieldZtreeList(tableName);
            // 基础字段不显示出来
            obj.Data?.RemoveAll(p => BaseField.BaseFieldList.Contains(p.name));
            return Json(obj);
        }

        [HttpGet, AuthorizeFilter("tool:codegenerator:search")]
        public async Task<IActionResult> GetBaseConfigJson(string tableName)
        {
            string tableDescription = string.Empty;
            var tDataTableField = await _databaseTableBll.GetTableFieldList(tableName);
            List<string> columnList = tDataTableField.Data.Where(p => !BaseField.BaseFieldList.Contains(p.TableColumn)).Select(p => p.TableColumn).ToList();

            OperatorInfo operatorInfo = await Operator.Instance.Current();
            string serverPath = GlobalContext.HostingEnvironment.ContentRootPath;
            return base.Json(new TData<BaseConfigModel>
            {
                Data = new SingleTableTemplate().GetBaseConfig(serverPath, operatorInfo.UserName, tableName, tableDescription, columnList),
                Tag = 1
            });
        }

        #endregion

        #region 提交数据

        [HttpPost, AuthorizeFilter("tool:codegenerator:add")]
        public async Task<IActionResult> CodePreviewJson(BaseConfigModel baseConfig)
        {
            if (string.IsNullOrEmpty(baseConfig.OutputConfig.OutputModule))
            {
                return Json(new TData { Tag = 0, Message = "请选择输出到的模块" });
            }

            var template = new SingleTableTemplate();
            var objTable = await _databaseTableBll.GetTableFieldList(baseConfig.TableName);
            var dt = objTable.Data.ToDataTable(); // 用DataTable类型，避免依赖

            var codeEntity = template.BuildEntity(baseConfig, dt);
            var codeEntityParam = template.BuildEntityParam(baseConfig);
            var codeService = template.BuildService(baseConfig, dt);
            var codeBusiness = template.BuildBusiness(baseConfig);
            var codeController = template.BuildController(baseConfig);
            var codeIndex = template.BuildIndex(baseConfig);
            var codeForm = template.BuildForm(baseConfig);
            var codeMenu = template.BuildMenu(baseConfig);

            return base.Json(new TData<object>
            {
                Data = new
                {
                    CodeEntity = HttpUtility.HtmlEncode(codeEntity),
                    CodeEntityParam = HttpUtility.HtmlEncode(codeEntityParam),
                    CodeService = HttpUtility.HtmlEncode(codeService),
                    CodeBusiness = HttpUtility.HtmlEncode(codeBusiness),
                    CodeController = HttpUtility.HtmlEncode(codeController),
                    CodeIndex = HttpUtility.HtmlEncode(codeIndex),
                    CodeForm = HttpUtility.HtmlEncode(codeForm),
                    CodeMenu = HttpUtility.HtmlEncode(codeMenu)
                },
                Tag = 1
            });
        }

        [HttpPost, AuthorizeFilter("tool:codegenerator:add")]
        public async Task<IActionResult> CodeGenerateJson(BaseConfigModel baseConfig, string code)
        {
            if (!GlobalContext.SystemConfig.Debug)
            {
                return Json(new TData { Tag = 0, Message = "请在本地开发模式时使用代码生成" });
            }

            var template = new SingleTableTemplate();
            var data = await template.CreateCode(baseConfig, HttpUtility.UrlDecode(code));
            return base.Json(new TData<List<KeyValue>> { Data = data, Tag = 1 });
        }

        #endregion
    }
}