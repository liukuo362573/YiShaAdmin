using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using YiSha.Business.Cache;
using YiSha.CodeGenerator.Model;
using YiSha.Data.Repository;
using YiSha.Entity;
using YiSha.Entity.SystemManage;
using YiSha.Enum.SystemManage;
using YiSha.Util.Extension;
using YiSha.Util.Helper;
using YiSha.Util.Model;

namespace YiSha.CodeGenerator.Template
{
    public class SingleTableTemplate
    {
        private readonly string _projectName = "YiSha";
        private string _keyType;

        public BaseConfigModel GetBaseConfig(string path, string userName, string tableName, string tableDescription, List<string> tableFieldList)
        {
            path = GetProjectRootPath(path);
            int defaultField = 2; // 默认显示2个字段
            string classPrefix = TableMappingHelper.GetClassNamePrefix(tableName);
            string outputWeb = Path.Combine(path, $"{_projectName}.Web", $"{_projectName}.Admin.Web");
            string areasModule = Path.Combine(outputWeb, "Areas");
            var moduleList = new List<string> { "TestManage" };
            if (Directory.Exists(areasModule))
            {
                moduleList = Directory.GetDirectories(areasModule).Select(Path.GetFileName).Where(p => p != "DemoManage").ToList();
            }

            var baseConfigModel = new BaseConfigModel 
            {
                TableName = tableName,
                TableNameUpper = tableName,
                FileConfig = new FileConfigModel
                {
                    ClassPrefix = classPrefix,
                    ClassDescription = tableDescription,
                    CreateName = userName,
                    CreateDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm"),
                    EntityName = $"{classPrefix}Entity",
                    EntityMapName = $"{classPrefix}Map",
                    EntityParamName = $"{classPrefix}Param",
                    BusinessName = $"{classPrefix}BLL",
                    ServiceName = $"{classPrefix}Service",
                    ControllerName = $"{classPrefix}Controller",
                    PageIndexName = $"{classPrefix}Index",
                    PageFormName = $"{classPrefix}Form"
                },
                OutputConfig = new OutputConfigModel
                {
                    OutputModule = string.Empty,
                    OutputEntity = Path.Combine(path, $"{_projectName}.Entity"),
                    OutputBusiness = Path.Combine(path, $"{_projectName}.Business"),
                    OutputWeb = outputWeb,
                    ModuleList = moduleList
                },
                PageIndex = new PageIndexModel
                {
                    IsSearch = 1,
                    IsPagination = 1,
                    ButtonList = new List<string>(),
                    ColumnList = tableFieldList.Take(defaultField).ToList()
                },
                PageForm = new PageFormModel
                {
                    ShowMode = 1,
                    FieldList = tableFieldList.Take(defaultField).ToList()
                }
            };
            return baseConfigModel;
        }

        public string BuildEntity(BaseConfigModel baseConfigModel, DataTable dt)
        {
            var builder = new StringBuilder();
            builder.AppendLine("using Newtonsoft.Json;");
            builder.AppendLine("using System;");
            builder.AppendLine("using System.ComponentModel.DataAnnotations.Schema;");
            builder.AppendLine($"using {_projectName}.Util.Helper;");
            builder.AppendLine();
            builder.AppendLine($"namespace {_projectName}.Entity.{baseConfigModel.OutputConfig.OutputModule}");
            builder.AppendLine("{");
            builder.Append(GetClassDescription("实体类", baseConfigModel));
            builder.AppendLine($"    [Table(\"{baseConfigModel.TableName}\")]");
            builder.AppendLine($"    public class {baseConfigModel.FileConfig.EntityName} : {GetBaseEntity(dt)}");
            builder.AppendLine("    {");

            string column;
            int breakLine = 0;
            foreach (DataRow dr in dt.Rows)
            {
                column = dr["TableColumn"].ToString();
                // 基础字段不需要生成，继承合适的BaseEntity即可。
                if (BaseField.BaseFieldList.Any(p => p == column)) continue;

                var remark = dr["Remark"].ToString();
                var datatype = dr["Datatype"].ToString();
                datatype = TableMappingHelper.GetPropertyDatatype(datatype);
                
                if (breakLine++ > 0) builder.AppendLine();
                builder.AppendLine("        /// <summary>");
                builder.AppendLine($"        /// {remark}");
                builder.AppendLine("        /// </summary>");

                switch (datatype)
                {
                    case "long?":
                        builder.AppendLine("        [JsonConverter(typeof(StringJsonConverter))]");
                        break;

                    case "DateTime?":
                        builder.AppendLine("        [JsonConverter(typeof(DateTimeJsonConverter))]");
                        break;
                }
                builder.AppendLine($"        public {datatype} {column} {{ get; set; }}");
            }
            builder.AppendLine("    }");
            builder.AppendLine("}");
            return builder.ToString();
        }

        public string BuildEntityParam(BaseConfigModel baseConfigModel)
        {
            var builder = new StringBuilder();
            builder.AppendLine($"namespace {_projectName}.Model.Param.{baseConfigModel.OutputConfig.OutputModule}");
            builder.AppendLine("{");
            builder.Append(GetClassDescription("实体查询参数类", baseConfigModel));
            builder.AppendLine($"    public class {baseConfigModel.FileConfig.EntityParamName.Replace("Param", "ListParam")}");
            builder.AppendLine("    {");
            builder.AppendLine("    }");
            builder.AppendLine("}");
            return builder.ToString();
        }

        public string BuildService(BaseConfigModel baseConfigModel, DataTable dt)
        {
            string baseEntity = GetBaseEntity(dt);
            string entityParamName = GetEntityParamName(baseConfigModel);
            var builder = new StringBuilder();
            builder.AppendLine($"using {_projectName}.Data;");
            builder.AppendLine($"using {_projectName}.Data.Repository;");
            builder.AppendLine($"using {_projectName}.Entity.{baseConfigModel.OutputConfig.OutputModule};");
            builder.AppendLine($"using {_projectName}.Model.Param.{baseConfigModel.OutputConfig.OutputModule};");
            builder.AppendLine($"using {_projectName}.Util;");
            builder.AppendLine($"using {_projectName}.Util.Extension;");
            builder.AppendLine($"using {_projectName}.Util.Model;");
            builder.AppendLine($"using {_projectName}.Util.Helper;");
            builder.AppendLine("using System;");
            builder.AppendLine("using System.Collections.Generic;");
            builder.AppendLine("using System.Linq;");
            builder.AppendLine("using System.Linq.Expressions;");
            builder.AppendLine("using System.Threading.Tasks;");
            builder.AppendLine();
            builder.AppendLine($"namespace {_projectName}.Service.{baseConfigModel.OutputConfig.OutputModule}");
            builder.AppendLine("{");
            builder.Append(GetClassDescription("服务类", baseConfigModel));
            builder.AppendLine($"    public class {baseConfigModel.FileConfig.ServiceName} : RepositoryFactory");
            builder.AppendLine("    {");
            builder.AppendLine("        #region 获取数据");
            builder.AppendLine();
            builder.AppendLine($"        public async Task<List<{baseConfigModel.FileConfig.EntityName}>> GetList({entityParamName} param)");
            builder.AppendLine("        {");
            builder.AppendLine("            var expression = ListFilter(param);");
            builder.AppendLine("            return await BaseRepository().FindList(expression);");
            builder.AppendLine("        }");
            builder.AppendLine();
            builder.AppendLine($"        public async Task<List<{baseConfigModel.FileConfig.EntityName}>> GetPageList({entityParamName} param, Pagination pagination)");
            builder.AppendLine("        {");
            builder.AppendLine("            var expression = ListFilter(param);");
            builder.AppendLine("            return await BaseRepository().FindList(expression, pagination);");
            builder.AppendLine("        }");
            builder.AppendLine();
            builder.AppendLine($"        public async Task<{baseConfigModel.FileConfig.EntityName}> GetEntity({_keyType} id)");
            builder.AppendLine("        {");
            builder.AppendLine($"            return await BaseRepository().FindEntity<{baseConfigModel.FileConfig.EntityName}>(id);");
            builder.AppendLine("        }");
            builder.AppendLine();
            builder.AppendLine("        #endregion");
            builder.AppendLine();
            builder.AppendLine("        #region 提交数据");
            builder.AppendLine();
            builder.AppendLine($"        public async Task SaveForm({baseConfigModel.FileConfig.EntityName} entity)");
            builder.AppendLine("        {");
            builder.AppendLine("            if (entity.Id.IsNullOrZero())");
            builder.AppendLine("            {");
            builder.AppendLine($"                {GetSaveFormCreate(baseEntity)}");
            builder.AppendLine("                await BaseRepository().Insert(entity);");
            builder.AppendLine("            }");
            builder.AppendLine("            else");
            builder.AppendLine("            {");
            string saveFormModify = GetSaveFormModify(baseEntity);
            if (saveFormModify?.Length > 0)
            {
                builder.AppendLine($"                {GetSaveFormModify(baseEntity)}");
            }
            builder.AppendLine("                await BaseRepository().Update(entity);");
            builder.AppendLine("            }");
            builder.AppendLine("        }");
            builder.AppendLine();
            builder.AppendLine("        public async Task DeleteForm(string ids)");
            builder.AppendLine("        {");
            builder.AppendLine("            var idArr = TextHelper.SplitToArray<object>(ids, ',');");
            builder.AppendLine($"            await BaseRepository().Delete<{baseConfigModel.FileConfig.EntityName}>(idArr);");
            builder.AppendLine("        }");
            builder.AppendLine();
            builder.AppendLine("        #endregion");
            builder.AppendLine();
            builder.AppendLine("        #region 私有方法");
            builder.AppendLine();
            builder.AppendLine($"        private Expression<Func<{baseConfigModel.FileConfig.EntityName}, bool>> ListFilter({entityParamName} param)");
            builder.AppendLine("        {");
            builder.AppendLine($"            var expression = LinqExtensions.True<{baseConfigModel.FileConfig.EntityName}>();");
            builder.AppendLine("            if (param != null) { }");
            builder.AppendLine("            return expression;");
            builder.AppendLine("        }");
            builder.AppendLine();
            builder.AppendLine("        #endregion");
            builder.AppendLine("    }");
            builder.AppendLine("}");
            return builder.ToString();
        }

        public string BuildBusiness(BaseConfigModel baseConfigModel)
        {
            var builder = new StringBuilder();
            string entityParamName = GetEntityParamName(baseConfigModel);
            builder.AppendLine($"using {_projectName}.Entity.{baseConfigModel.OutputConfig.OutputModule};");
            builder.AppendLine($"using {_projectName}.Model.Param.{baseConfigModel.OutputConfig.OutputModule};");
            builder.AppendLine($"using {_projectName}.Service.{baseConfigModel.OutputConfig.OutputModule};");
            builder.AppendLine($"using {_projectName}.Util.Extension;");
            builder.AppendLine($"using {_projectName}.Util.Model;");
            builder.AppendLine("using System.Collections.Generic;");
            builder.AppendLine("using System.Threading.Tasks;");
            builder.AppendLine();
            builder.AppendLine($"namespace {_projectName}.Business.{baseConfigModel.OutputConfig.OutputModule}");
            builder.AppendLine("{");
            builder.Append(GetClassDescription("业务类", baseConfigModel));
            builder.AppendLine($"    public class {baseConfigModel.FileConfig.BusinessName}");
            builder.AppendLine("    {");
            builder.AppendLine($"        private readonly {baseConfigModel.FileConfig.ServiceName} _{TableMappingHelper.FirstLetterLowercase(baseConfigModel.FileConfig.ServiceName)} = new();");
            builder.AppendLine();
            builder.AppendLine("        #region 获取数据");
            builder.AppendLine();
            builder.AppendLine($"        public async Task<TData<List<{baseConfigModel.FileConfig.EntityName}>>> GetList({entityParamName} param)");
            builder.AppendLine("        {");
            builder.AppendLine($"            var list = await _{TableMappingHelper.FirstLetterLowercase(baseConfigModel.FileConfig.ServiceName)}.GetList(param);");
            builder.AppendLine("            return new() { Tag = 1, Total = list.Count, Data = list };");
            builder.AppendLine("        }");
            builder.AppendLine();
            builder.AppendLine($"        public async Task<TData<List<{baseConfigModel.FileConfig.EntityName}>>> GetPageList({entityParamName} param, Pagination pagination)");
            builder.AppendLine("        {");
            builder.AppendLine($"            var list = await _{TableMappingHelper.FirstLetterLowercase(baseConfigModel.FileConfig.ServiceName)}.GetPageList(param, pagination);");
            builder.AppendLine("            return new() { Tag = 1, Total = pagination.TotalCount, Data = list };");
            builder.AppendLine("        }");
            builder.AppendLine();
            builder.AppendLine($"        public async Task<TData<{baseConfigModel.FileConfig.EntityName}>> GetEntity({_keyType} id)");
            builder.AppendLine("        {");
            builder.AppendLine($"            var result = await _{TableMappingHelper.FirstLetterLowercase(baseConfigModel.FileConfig.ServiceName)}.GetEntity(id);");
            builder.AppendLine("            return new() { Tag = result != null ? 1 : 0, Data = result };");
            builder.AppendLine("        }");
            builder.AppendLine();
            builder.AppendLine("        #endregion");
            builder.AppendLine();
            builder.AppendLine("        #region 提交数据");
            builder.AppendLine();
            builder.AppendLine($"        public async Task<TData<string>> SaveForm({baseConfigModel.FileConfig.EntityName} entity)");
            builder.AppendLine("        {");
            builder.AppendLine($"            await _{TableMappingHelper.FirstLetterLowercase(baseConfigModel.FileConfig.ServiceName)}.SaveForm(entity);");
            builder.AppendLine("            return new() { Tag = 1, Data = entity.Id.ParseToString() };");
            builder.AppendLine("        }");
            builder.AppendLine();
            builder.AppendLine("        public async Task<TData> DeleteForm(string ids)");
            builder.AppendLine("        {");
            builder.AppendLine($"            await _{TableMappingHelper.FirstLetterLowercase(baseConfigModel.FileConfig.ServiceName)}.DeleteForm(ids);");
            builder.AppendLine("            return new() { Tag = 1 };");
            builder.AppendLine("        }");
            builder.AppendLine();
            builder.AppendLine("        #endregion");
            builder.AppendLine();
            builder.AppendLine("        #region 私有方法");
            builder.AppendLine();
            builder.AppendLine("        #endregion");
            builder.AppendLine("    }");
            builder.AppendLine("}");
            return builder.ToString();
        }

        public string BuildController(BaseConfigModel baseConfigModel)
        {
            string modulePrefix = GetModulePrefix(baseConfigModel);
            string entityParamName = GetEntityParamName(baseConfigModel);
            string classPrefix = baseConfigModel.FileConfig.ClassPrefix.ToLower();

            var builder = new StringBuilder();
            builder.AppendLine($"using {_projectName}.Admin.Web.Controllers;");
            builder.AppendLine($"using {_projectName}.Business.{baseConfigModel.OutputConfig.OutputModule};");
            builder.AppendLine($"using {_projectName}.Entity.{baseConfigModel.OutputConfig.OutputModule};");
            builder.AppendLine($"using {_projectName}.Model.Param.{baseConfigModel.OutputConfig.OutputModule};");
            builder.AppendLine($"using {_projectName}.Util.Model;");
            builder.AppendLine($"using {_projectName}.Admin.Web.Filter;");
            builder.AppendLine("using Microsoft.AspNetCore.Mvc;");
            builder.AppendLine("using System.Threading.Tasks;");
            builder.AppendLine();
            builder.AppendLine($"namespace {_projectName}.Admin.Web.Areas.{baseConfigModel.OutputConfig.OutputModule}.Controllers");
            builder.AppendLine("{");
            builder.Append(GetClassDescription("控制器类", baseConfigModel));
            builder.AppendLine($"    [Area(\"{baseConfigModel.OutputConfig.OutputModule}\")]");
            builder.AppendLine($"    public class {baseConfigModel.FileConfig.ControllerName} : BaseController");
            builder.AppendLine("    {");
            builder.AppendLine($"        private readonly {baseConfigModel.FileConfig.BusinessName} _{TableMappingHelper.FirstLetterLowercase(baseConfigModel.FileConfig.BusinessName)} = new();");
            builder.AppendLine();
            builder.AppendLine("        #region 视图功能");
            builder.AppendLine();
            builder.AppendLine($"        [AuthorizeFilter(\"{modulePrefix}:{classPrefix}:view\")]");
            builder.AppendLine($"        public IActionResult {baseConfigModel.FileConfig.PageIndexName}()");
            builder.AppendLine("        {");
            builder.AppendLine("            return View();");
            builder.AppendLine("        }");
            builder.AppendLine();
            builder.AppendLine($"        public IActionResult {baseConfigModel.FileConfig.PageFormName}()");
            builder.AppendLine("        {");
            builder.AppendLine("            return View();");
            builder.AppendLine("        }");
            builder.AppendLine();
            builder.AppendLine("        #endregion");
            builder.AppendLine();
            builder.AppendLine("        #region 获取数据");
            builder.AppendLine();
            builder.AppendLine($"        [HttpGet, AuthorizeFilter(\"{modulePrefix}:{classPrefix}:search\")]");
            builder.AppendLine($"        public async Task<IActionResult> GetListJson({entityParamName} param)");
            builder.AppendLine("        {");
            builder.AppendLine($"            var result = await _{TableMappingHelper.FirstLetterLowercase(baseConfigModel.FileConfig.BusinessName)}.GetList(param);");
            builder.AppendLine("            return Json(result);");
            builder.AppendLine("        }");
            builder.AppendLine();
            builder.AppendLine($"        [HttpGet, AuthorizeFilter(\"{modulePrefix}:{classPrefix}:search\")]");
            builder.AppendLine($"        public async Task<IActionResult> GetPageListJson({entityParamName} param, Pagination pagination)");
            builder.AppendLine("        {");
            builder.AppendLine($"            var result = await _{TableMappingHelper.FirstLetterLowercase(baseConfigModel.FileConfig.BusinessName)}.GetPageList(param, pagination);");
            builder.AppendLine("            return Json(result);");
            builder.AppendLine("        }");
            builder.AppendLine();
            builder.AppendLine("        [HttpGet]");
            builder.AppendLine($"        public async Task<IActionResult> GetFormJson({_keyType} id)");
            builder.AppendLine("        {");
            builder.AppendLine($"            var result = await _{TableMappingHelper.FirstLetterLowercase(baseConfigModel.FileConfig.BusinessName)}.GetEntity(id);");
            builder.AppendLine("            return Json(result);");
            builder.AppendLine("        }");
            builder.AppendLine();
            builder.AppendLine("        #endregion");
            builder.AppendLine();
            builder.AppendLine("        #region 提交数据");
            builder.AppendLine();
            builder.AppendLine($"        [HttpPost, AuthorizeFilter(\"{modulePrefix}:{classPrefix}:add,{modulePrefix}:{classPrefix}:edit\")]");
            builder.AppendLine($"        public async Task<IActionResult> SaveFormJson({baseConfigModel.FileConfig.EntityName} entity)");
            builder.AppendLine("        {");
            builder.AppendLine($"            var result = await _{TableMappingHelper.FirstLetterLowercase(baseConfigModel.FileConfig.BusinessName)}.SaveForm(entity);");
            builder.AppendLine("            return Json(result);");
            builder.AppendLine("        }");
            builder.AppendLine();
            builder.AppendLine($"        [HttpPost, AuthorizeFilter(\"{modulePrefix}:{classPrefix}:delete\")]");
            builder.AppendLine("        public async Task<IActionResult> DeleteFormJson(string ids)");
            builder.AppendLine("        {");
            builder.AppendLine($"            var result = await _{TableMappingHelper.FirstLetterLowercase(baseConfigModel.FileConfig.BusinessName)}.DeleteForm(ids);");
            builder.AppendLine("            return Json(result);");
            builder.AppendLine("        }");
            builder.AppendLine();
            builder.AppendLine("        #endregion");
            builder.AppendLine("    }");
            builder.AppendLine("}");
            return builder.ToString();
        }

        public string BuildIndex(BaseConfigModel baseConfigModel)
        {
            #region 初始化集合

            baseConfigModel.PageIndex.ButtonList ??= new List<string>();
            baseConfigModel.PageIndex.ColumnList ??= new List<string>();

            #endregion

            var builder = new StringBuilder();
            builder.AppendLine("@{");
            builder.AppendLine("    Layout = \"~/Views/Shared/_Index.cshtml\";");
            builder.AppendLine("}");
            builder.AppendLine();
            builder.AppendLine("<div class=\"container-div\">");
            builder.AppendLine("    <div class=\"row\">");

            #region 是否显示搜索

            if (baseConfigModel.PageIndex.IsSearch == 1)
            {
                string fieldName = TextHelper.GetCustomValue(baseConfigModel.PageIndex.ColumnList.FirstOrDefault(), "fieldName");
                string fieldNameLower = TableMappingHelper.FirstLetterLowercase(fieldName);
                builder.AppendLine("        <div id=\"searchDiv\" class=\"col-sm-12 search-collapse\">");
                builder.AppendLine("            <div class=\"select-list\">");
                builder.AppendLine("                <ul>");
                builder.AppendLine("                    <li>");
                builder.AppendLine($"                        {fieldName}：<input id=\"{fieldNameLower}\" col=\"{fieldName}\" type=\"text\" />");
                builder.AppendLine("                    </li>");
                builder.AppendLine("                    <li>");
                builder.AppendLine("                        <a id=\"btnSearch\" class=\"btn btn-primary btn-sm\" onclick=\"searchGrid()\"><i class=\"fa fa-search\"></i>&nbsp;搜索</a>");
                builder.AppendLine("                    </li>");
                builder.AppendLine("                </ul>");
                builder.AppendLine("            </div>");
                builder.AppendLine("        </div>");
            }

            #endregion

            #region 是否显示工具栏

            if (baseConfigModel.PageIndex.ButtonList.Any(p => p != "btnSearch"))
            {
                builder.AppendLine("        <div class=\"btn-group-sm hidden-xs\" id=\"toolbar\">");
                if (baseConfigModel.PageIndex.ButtonList.Any(p => p == "btnAdd"))
                {
                    builder.AppendLine("            <a id=\"btnAdd\" class=\"btn btn-success\" onclick=\"showSaveForm(true)\"><i class=\"fa fa-plus\"></i> 新增</a>");
                }
                if (baseConfigModel.PageIndex.ButtonList.Any(p => p == "btnEdit"))
                {
                    builder.AppendLine("            <a id=\"btnEdit\" class=\"btn btn-primary disabled\" onclick=\"showSaveForm(false)\"><i class=\"fa fa-edit\"></i> 修改</a>");
                }
                if (baseConfigModel.PageIndex.ButtonList.Any(p => p == "btnDelete"))
                {
                    builder.AppendLine("            <a id=\"btnDelete\" class=\"btn btn-danger disabled\" onclick=\"deleteForm()\"><i class=\"fa fa-remove\"></i> 删除</a>");
                }
                builder.AppendLine("        </div>");
            }

            #endregion

            builder.AppendLine("        <div class=\"col-sm-12 select-table table-striped\">");
            builder.AppendLine("            <table id=\"gridTable\" data-mobile-responsive=\"true\"></table>");
            builder.AppendLine("        </div>");
            builder.AppendLine("    </div>");
            builder.AppendLine("</div>");
            builder.AppendLine();
            builder.AppendLine("<script type=\"text/javascript\">");
            builder.AppendLine("    $(function () {");
            builder.AppendLine("        initGrid();");
            builder.AppendLine("    });");
            builder.AppendLine();
            builder.AppendLine("    function initGrid() {");
            builder.AppendLine("        var queryUrl = '@Url.Action(\"GetPageListJson\")';");
            builder.AppendLine("        $('#gridTable').ysTable({");
            builder.AppendLine("            url: queryUrl,");
            builder.AppendLine("            columns: [");
            builder.AppendLine("                { checkbox: true, visible: true },");
            builder.AppendLine("                { field: 'Id', title: 'Id', visible: false },");

            foreach (string column in baseConfigModel.PageIndex.ColumnList)
            {
                builder.AppendLine($"                {{ field: '{column}', title: '{column}' }},");
            }

            builder.AppendLine("            ],");
            if (baseConfigModel.PageIndex.IsPagination == 0)
            {
                builder.AppendLine("            pagination: false,");
            }
            builder.AppendLine("            queryParams: function (params) {");
            builder.AppendLine("                var pagination = $('#gridTable').ysTable('getPagination', params);");
            builder.AppendLine("                var queryString = $('#searchDiv').getWebControls(pagination);");
            builder.AppendLine("                return queryString;");
            builder.AppendLine("            }");
            builder.AppendLine("        });");
            builder.AppendLine("    }");
            builder.AppendLine();
            builder.AppendLine("    function searchGrid() {");
            builder.AppendLine("        $('#gridTable').ysTable('search');");
            builder.AppendLine("        resetToolbarStatus();");
            builder.AppendLine("    }");
            builder.AppendLine();

            #region 新增和修改方法

            if (baseConfigModel.PageIndex.ButtonList.Any(p => p is "btnAdd" or "btnEdit"))
            {
                builder.AppendLine("    function showSaveForm(bAdd) {");
                builder.AppendLine("        var id = 0;");
                builder.AppendLine("        if (!bAdd) {");
                builder.AppendLine("            var selectedRow = $('#gridTable').bootstrapTable('getSelections');");
                builder.AppendLine("            if (!ys.checkRowEdit(selectedRow)) {");
                builder.AppendLine("                return;");
                builder.AppendLine("            }");
                builder.AppendLine("            else {");
                builder.AppendLine("                id = selectedRow[0].Id;");
                builder.AppendLine("            }");
                builder.AppendLine("        }");
                builder.AppendLine("        ys.openDialog({");
                builder.AppendLine("            title: id > 0 ? '编辑' : '添加',");
                builder.AppendLine($"            content: '@Url.Action(\"{baseConfigModel.FileConfig.PageFormName}\")?id=' + id,");
                builder.AppendLine("            width: '768px',");
                builder.AppendLine("            height: '550px',");
                builder.AppendLine("            callback: function (index, layero) {");
                builder.AppendLine("                var iframeWin = window[layero.find('iframe')[0]['name']];");
                builder.AppendLine("                iframeWin.saveForm(index);");
                builder.AppendLine("            }");
                builder.AppendLine("        });");
                builder.AppendLine("    }");
            }

            #endregion

            builder.AppendLine();

            #region 删除方法

            if (baseConfigModel.PageIndex.ButtonList.Any(p => p == "btnDelete"))
            {
                builder.AppendLine("    function deleteForm() {");
                builder.AppendLine("        var selectedRow = $('#gridTable').bootstrapTable('getSelections');");
                builder.AppendLine("        if (ys.checkRowDelete(selectedRow)) {");
                builder.AppendLine("            ys.confirm('确认要删除选中的' + selectedRow.length + '条数据吗？', function () {");
                builder.AppendLine("                var ids = ys.getIds(selectedRow);");
                builder.AppendLine("                ys.ajax({");
                builder.AppendLine("                    url: '@Url.Action(\"DeleteFormJson\")?ids=' + ids,");
                builder.AppendLine("                    type: 'post',");
                builder.AppendLine("                    success: function (res) {");
                builder.AppendLine("                        if (res.Tag == 1) {");
                builder.AppendLine("                            ys.msgSuccess(res.Message);");
                builder.AppendLine("                            searchGrid();");
                builder.AppendLine("                        }");
                builder.AppendLine("                        else {");
                builder.AppendLine("                            ys.msgError(res.Message);");
                builder.AppendLine("                        }");
                builder.AppendLine("                    }");
                builder.AppendLine("                });");
                builder.AppendLine("            });");
                builder.AppendLine("        }");
                builder.AppendLine("    }");
            }

            #endregion

            builder.AppendLine("</script>");
            return builder.ToString();
        }

        public string BuildForm(BaseConfigModel baseConfigModel)
        {
            #region 初始化集合

            baseConfigModel.PageForm.FieldList ??= new List<string>();

            #endregion

            var builder = new StringBuilder();
            builder.AppendLine("@{");
            builder.AppendLine("    Layout = \"~/Views/Shared/_FormWhite.cshtml\";");
            builder.AppendLine("}");
            builder.AppendLine();
            builder.AppendLine("<div class=\"wrapper animated fadeInRight\">");
            builder.AppendLine("    <form id=\"form\" class=\"form-horizontal m\">");

            #region 表单控件

            if (baseConfigModel.PageForm.FieldList.Count > 0)
            {
                string field;
                string fieldLower;
                switch (baseConfigModel.PageForm.ShowMode)
                {
                    case 1:
                        for (int i = 0; i < baseConfigModel.PageForm.FieldList.Count; i++)
                        {
                            field = baseConfigModel.PageForm.FieldList[i];
                            fieldLower = TableMappingHelper.FirstLetterLowercase(field);

                            builder.AppendLine("        <div class=\"form-group\">");
                            builder.AppendLine($"            <label class=\"col-sm-3 control-label\">{field}{(i == 0 ? "<span class=\"red\"> *</span>" : string.Empty)}</label>");
                            builder.AppendLine("            <div class=\"col-sm-8\">");
                            builder.AppendLine($"                <input id=\"{fieldLower}\" col=\"{field}\" type=\"text\" class=\"form-control\" />");
                            builder.AppendLine("            </div>");
                            builder.AppendLine("        </div>");
                        }
                        break;

                    case 2:
                        for (int i = 0; i < baseConfigModel.PageForm.FieldList.Count; i++)
                        {
                            field = baseConfigModel.PageForm.FieldList[i];
                            fieldLower = TableMappingHelper.FirstLetterLowercase(field);

                            if (i % 2 == 0)
                            {
                                builder.AppendLine("        <div class=\"form-group\">");
                            }

                            builder.AppendLine($"            <label class=\"col-sm-2 control-label\">{field}<span class=\"red\"> *</span></label>");
                            builder.AppendLine("            <div class=\"col-sm-4\">");
                            builder.AppendLine($"                <input id=\"{fieldLower}\" col=\"{field}\" type=\"text\" class=\"form-control\" />");
                            builder.AppendLine("            </div>");

                            if (i % 2 == 1)
                            {
                                builder.AppendLine("        </div>");
                            }
                        }
                        break;
                }
            }

            #endregion

            builder.AppendLine("    </form>");
            builder.AppendLine("</div>");
            builder.AppendLine();
            builder.AppendLine("<script type=\"text/javascript\">");
            builder.AppendLine("    var id = ys.request(\"id\");");
            builder.AppendLine("    $(function () {");
            builder.AppendLine("        getForm();");
            builder.AppendLine();
            builder.AppendLine("        $('#form').validate({");
            builder.AppendLine("            rules: {");
            builder.AppendLine($"                {TextHelper.GetCustomValue(TableMappingHelper.FirstLetterLowercase(baseConfigModel.PageForm.FieldList.FirstOrDefault()), "fieldName")}: {{ required: true }}");
            builder.AppendLine("            }");
            builder.AppendLine("        });");
            builder.AppendLine("    });");
            builder.AppendLine();
            builder.AppendLine("    function getForm() {");
            builder.AppendLine("        if (id > 0) {");
            builder.AppendLine("            ys.ajax({");
            builder.AppendLine("                url: '@Url.Action(\"GetFormJson\")?id=' + id,");
            builder.AppendLine("                type: 'get',");
            builder.AppendLine("                success: function (res) {");
            builder.AppendLine("                    if (res.Tag == 1) {");
            builder.AppendLine("                        $('#form').setWebControls(res.Data);");
            builder.AppendLine("                    }");
            builder.AppendLine("                }");
            builder.AppendLine("            });");
            builder.AppendLine("        }");
            builder.AppendLine("        else {");
            builder.AppendLine("            var defaultData = {};");
            builder.AppendLine("            $('#form').setWebControls(defaultData);");
            builder.AppendLine("        }");
            builder.AppendLine("    }");
            builder.AppendLine();
            builder.AppendLine("    function saveForm(index) {");
            builder.AppendLine("        if ($('#form').validate().form()) {");
            builder.AppendLine("            var postData = $('#form').getWebControls({ Id: id });");
            builder.AppendLine("            ys.ajax({");
            builder.AppendLine("                url: '@Url.Action(\"SaveFormJson\")',");
            builder.AppendLine("                type: 'post',");
            builder.AppendLine("                data: postData,");
            builder.AppendLine("                success: function (res) {");
            builder.AppendLine("                    if (res.Tag == 1) {");
            builder.AppendLine("                        ys.msgSuccess(res.Message);");
            builder.AppendLine("                        parent.searchGrid();");
            builder.AppendLine("                        parent.layer.close(index);");
            builder.AppendLine("                    }");
            builder.AppendLine("                    else {");
            builder.AppendLine("                        ys.msgError(res.Message);");
            builder.AppendLine("                    }");
            builder.AppendLine("                }");
            builder.AppendLine("            });");
            builder.AppendLine("        }");
            builder.AppendLine("    }");
            builder.AppendLine("</script>");
            return builder.ToString();
        }

        public string BuildMenu(BaseConfigModel baseConfigModel)
        {
            var builder = new StringBuilder();
            builder.AppendLine();
            builder.AppendLine($"  菜单路径:{baseConfigModel.OutputConfig.OutputModule}/{baseConfigModel.FileConfig.ClassPrefix}/{baseConfigModel.FileConfig.PageIndexName}");
            builder.AppendLine();
            string modulePrefix = GetModulePrefix(baseConfigModel);
            string classPrefix = baseConfigModel.FileConfig.ClassPrefix.ToLower();
            builder.AppendLine($"  页面显示权限：{modulePrefix}:{classPrefix}:view");
            builder.AppendLine();
            List<KeyValue> list = GetButtonAuthorizeList();

            if (baseConfigModel.PageIndex.IsSearch == 1)
            {
                KeyValue button = list.FirstOrDefault(p => p.Key == "btnSearch");
                builder.AppendLine($"  按钮{button?.Description}权限：{modulePrefix}:{classPrefix}:{button?.Value}");
            }
            foreach (string btn in baseConfigModel.PageIndex.ButtonList)
            {
                KeyValue button = list.FirstOrDefault(p => p.Key == btn);
                builder.AppendLine($"  按钮{button?.Description}权限：{modulePrefix}:{classPrefix}:{button?.Value}");
            }
            builder.AppendLine();
            return builder.ToString();
        }

        #region CreateCode

        public async Task<List<KeyValue>> CreateCode(BaseConfigModel baseConfigModel, string code)
        {
            var result = new List<KeyValue>();
            var param = code.ToJObject();

            #region 实体类

            if (!string.IsNullOrEmpty(param["CodeEntity"].ParseToString()))
            {
                string codeEntity = HttpUtility.HtmlDecode(param["CodeEntity"].ToString());
                string codePath = Path.Combine(baseConfigModel.OutputConfig.OutputEntity, $"{_projectName}.Entity", baseConfigModel.OutputConfig.OutputModule, $"{baseConfigModel.FileConfig.EntityName}.cs");
                if (!File.Exists(codePath))
                {
                    FileHelper.CreateFile(codePath, codeEntity);
                    result.Add(new KeyValue { Key = "实体类", Value = codePath });
                }
            }

            #endregion

            #region 查询类

            if (!param["CodeEntityParam"].IsEmpty())
            {
                string codeListEntity = HttpUtility.HtmlDecode(param["CodeEntityParam"].ToString());
                string codePath = Path.Combine(baseConfigModel.OutputConfig.OutputEntity, $"{_projectName}.Model", "Param", baseConfigModel.OutputConfig.OutputModule, $"{baseConfigModel.FileConfig.EntityParamName}.cs");
                if (!File.Exists(codePath))
                {
                    FileHelper.CreateFile(codePath, codeListEntity);
                    result.Add(new KeyValue { Key = "实体查询类", Value = codePath });
                }
            }

            #endregion

            #region 服务类

            if (!param["CodeService"].IsEmpty())
            {
                string codeService = HttpUtility.HtmlDecode(param["CodeService"].ToString());
                string codePath = Path.Combine(baseConfigModel.OutputConfig.OutputBusiness, $"{_projectName}.Service", baseConfigModel.OutputConfig.OutputModule, $"{baseConfigModel.FileConfig.ServiceName}.cs");
                if (!File.Exists(codePath))
                {
                    FileHelper.CreateFile(codePath, codeService);
                    result.Add(new KeyValue { Key = "服务类", Value = codePath });
                }
            }

            #endregion

            #region 业务类

            if (!param["CodeBusiness"].IsEmpty())
            {
                string codeBusiness = HttpUtility.HtmlDecode(param["CodeBusiness"].ToString());
                string codePath = Path.Combine(baseConfigModel.OutputConfig.OutputBusiness, $"{_projectName}.Business", baseConfigModel.OutputConfig.OutputModule, $"{baseConfigModel.FileConfig.BusinessName}.cs");
                if (!File.Exists(codePath))
                {
                    FileHelper.CreateFile(codePath, codeBusiness);
                    result.Add(new KeyValue { Key = "业务类", Value = codePath });
                }
            }

            #endregion

            #region 控制器

            if (!param["CodeController"].IsEmpty())
            {
                string codeController = HttpUtility.HtmlDecode(param["CodeController"].ToString());
                string codePath = Path.Combine(baseConfigModel.OutputConfig.OutputWeb, "Areas", baseConfigModel.OutputConfig.OutputModule, "Controllers", $"{baseConfigModel.FileConfig.ControllerName}.cs");
                if (!File.Exists(codePath))
                {
                    FileHelper.CreateFile(codePath, codeController);
                    result.Add(new KeyValue { Key = "控制器", Value = codePath });
                }
            }

            #endregion

            #region 列表页

            if (!param["CodeIndex"].IsEmpty())
            {
                string codeIndex = HttpUtility.HtmlDecode(param["CodeIndex"].ToString());
                string codePath = Path.Combine(baseConfigModel.OutputConfig.OutputWeb, "Areas", baseConfigModel.OutputConfig.OutputModule, "Views", baseConfigModel.FileConfig.ClassPrefix, $"{baseConfigModel.FileConfig.PageIndexName}.cshtml");
                if (!File.Exists(codePath))
                {
                    FileHelper.CreateFile(codePath, codeIndex);
                    result.Add(new KeyValue { Key = "列表页", Value = codePath });
                }

                // 生成菜单
                RepositoryFactory repositoryFactory = new RepositoryFactory();
                List<KeyValue> buttonAuthorizeList = GetButtonAuthorizeList();
                string menuUrl = $"{baseConfigModel.OutputConfig.OutputModule}/{baseConfigModel.FileConfig.ClassPrefix}/{baseConfigModel.FileConfig.PageIndexName}";
                string modulePrefix = GetModulePrefix(baseConfigModel);
                string classPrefix = baseConfigModel.FileConfig.ClassPrefix.ToLower();
                MenuEntity menuEntity = new MenuEntity
                {
                    MenuName = baseConfigModel.FileConfig.ClassDescription,
                    MenuUrl = menuUrl,
                    MenuType = (int)MenuTypeEnum.Menu,
                    Authorize = $"{modulePrefix}:{classPrefix}:view"
                };
                TData obj = await AddMenu(repositoryFactory, menuEntity);
                if (obj.Tag == 1)
                {
                    result.Add(new KeyValue { Key = "菜单(刷新页面可见)", Value = menuUrl });
                    if (baseConfigModel.PageIndex.IsSearch == 1)
                    {
                        // 按钮搜索权限
                        var button = buttonAuthorizeList.FirstOrDefault(p => p.Key == "btnSearch");
                        var buttonEntity = new MenuEntity
                        {
                            ParentId = menuEntity.Id,
                            MenuName = baseConfigModel.FileConfig.ClassDescription + button?.Description,
                            MenuType = (int)MenuTypeEnum.Button,
                            Authorize = $"{modulePrefix}:{classPrefix}:{button?.Value}"
                        };
                        await AddMenu(repositoryFactory, buttonEntity);
                    }
                    foreach (string btn in baseConfigModel.PageIndex.ButtonList)
                    {
                        var button = buttonAuthorizeList.FirstOrDefault(p => p.Key == btn);
                        var buttonEntity = new MenuEntity
                        {
                            ParentId = menuEntity.Id,
                            MenuName = baseConfigModel.FileConfig.ClassDescription + button?.Description,
                            MenuType = (int)MenuTypeEnum.Button,
                            Authorize = $"{modulePrefix}:{classPrefix}:{button?.Value}"
                        };
                        await AddMenu(repositoryFactory, buttonEntity);
                    }
                    new MenuCache().Remove();
                }
            }

            #endregion

            #region 表单页

            if (!param["CodeForm"].IsEmpty())
            {
                string codeSave = HttpUtility.HtmlDecode(param["CodeForm"].ToString());
                string codePath = Path.Combine(baseConfigModel.OutputConfig.OutputWeb, "Areas", baseConfigModel.OutputConfig.OutputModule, "Views", baseConfigModel.FileConfig.ClassPrefix, $"{baseConfigModel.FileConfig.PageFormName}.cshtml");
                if (!File.Exists(codePath))
                {
                    FileHelper.CreateFile(codePath, codeSave);
                    result.Add(new KeyValue { Key = "表单页", Value = codePath });
                }
            }

            #endregion

            return result;
        }

        private async Task<TData> AddMenu(RepositoryFactory repositoryFactory, MenuEntity menuEntity)
        {
            var menuList = await repositoryFactory.BaseRepository().FindList<MenuEntity>();
            if (menuList.Any(p => p.MenuName == menuEntity.MenuName && p.Authorize == menuEntity.Authorize))
            {
                return new();
            }
            menuEntity.MenuSort = menuList.Max(p => p.MenuSort) + 1;
            menuEntity.MenuStatus = 1;
            await menuEntity.Create();
            await repositoryFactory.BaseRepository().Insert(menuEntity);
            return new() { Tag = 1 };
        }

        #endregion

        #region 私有方法

        private string GetProjectRootPath(string path)
        {
            path = path.ParseToString();
            path = path.Trim('\\');
            if (GlobalContext.SystemConfig.Debug)
            {
                // 向上找二级
                path = Directory.GetParent(path).FullName;
                path = Directory.GetParent(path).FullName;
            }
            return path;
        }

        private string GetClassDescription(string type, BaseConfigModel baseConfigModel)
        {
            var builder = new StringBuilder();
            builder.AppendLine("    /// <summary>");
            builder.AppendLine($"    /// 创 建：{baseConfigModel.FileConfig.CreateName}");
            builder.AppendLine($"    /// 日 期：{baseConfigModel.FileConfig.CreateDate}");
            builder.AppendLine($"    /// 描 述：{baseConfigModel.FileConfig.ClassDescription}{type}");
            builder.AppendLine("    /// </summary>");
            return builder.ToString();
        }

        private List<KeyValue> GetButtonAuthorizeList()
        {
            return new()
            {
                new KeyValue { Key = "btnSearch", Value = "search", Description = "搜索" },
                new KeyValue { Key = "btnAdd", Value = "add", Description = "新增" },
                new KeyValue { Key = "btnEdit", Value = "edit", Description = "修改" },
                new KeyValue { Key = "btnDelete", Value = "delete", Description = "删除" }
            };
        }

        private string GetModulePrefix(BaseConfigModel baseConfigModel)
        {
            return baseConfigModel.OutputConfig.OutputModule.Replace("Manage", string.Empty).ToLower();
        }

        private string GetEntityParamName(BaseConfigModel baseConfigModel)
        {
            return baseConfigModel.FileConfig.EntityParamName.Replace("Param", "ListParam");
        }

        private string GetBaseEntity(DataTable dt)
        {
            var columnList = dt.AsEnumerable().Select(p => p["TableColumn"].ParseToString()).ToList();

            bool id = columnList.Any(p => p == "Id");
            bool baseIsDelete = columnList.Any(p => p == "BaseIsDelete");
            bool baseVersion = columnList.Any(p => p == "BaseVersion");
            bool baseModifyTime = columnList.Any(p => p == "BaseModifyTime");
            bool baseModifierId = columnList.Any(p => p == "BaseModifierId");
            bool baseCreateTime = columnList.Any(p => p == "BaseCreateTime");
            bool baseCreatorId = columnList.Any(p => p == "BaseCreatorId");

            if (!id)
            {
                throw new Exception("数据库表必须有主键Id字段");
            }

            var dataType = dt.AsEnumerable()
                             .Where(x => x["TableColumn"].ParseToString() == "Id")
                             .Select(x => x["Datatype"])
                             .FirstOrDefault();
            _keyType = TableMappingHelper.GetPropertyDatatype(dataType.ParseToString());
            if (_keyType == "long?")
            {
                if (baseIsDelete && baseVersion && baseModifyTime && baseModifierId && baseCreateTime && baseCreatorId)
                {
                    return "BaseExtensionEntity";
                }
                if (baseVersion && baseModifyTime && baseModifierId && baseCreateTime && baseCreatorId)
                {
                    return "BaseModifyEntity";
                }
                if (baseCreateTime && baseCreatorId)
                {
                    return "BaseCreateEntity";
                }
                return "BaseEntity";
            }
            return $"AbstractEntity<{_keyType}>";
        }

        private string GetSaveFormCreate(string entity)
        {
            return entity switch
            {
                "BaseEntity" => "entity.Create();",
                "BaseCreateEntity" => "await entity.Create();",
                "BaseModifyEntity" => "await entity.Create();",
                "BaseExtensionEntity" => "await entity.Create();",
                _ => string.Empty
            };
        }

        private string GetSaveFormModify(string entity)
        {
            return entity switch
            {
                "BaseEntity" => string.Empty,
                "BaseCreateEntity" => string.Empty,
                "BaseModifyEntity" => "await entity.Modify();",
                "BaseExtensionEntity" => "await entity.Modify();",
                _ => string.Empty
            };
        }

        #endregion
    }
}