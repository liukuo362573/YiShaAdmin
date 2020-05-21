using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Web;
using Newtonsoft.Json.Linq;
using YiSha.Data.Repository;
using YiSha.CodeGenerator.Model;
using YiSha.Util;
using YiSha.Util.Extension;
using YiSha.Util.Model;
using YiSha.Entity.SystemManage;
using YiSha.Enum.SystemManage;
using YiSha.Business.Cache;
using YiSha.Entity;

namespace YiSha.CodeGenerator.Template
{
    public class SingleTableTemplate
    {
        #region GetBaseConfig
        public BaseConfigModel GetBaseConfig(string path, string userName, string tableName, string tableDescription, List<string> tableFieldList)
        {
            path = GetProjectRootPath(path);

            int defaultField = 2; // 默认显示2个字段

            BaseConfigModel baseConfigModel = new BaseConfigModel();
            baseConfigModel.TableName = tableName;
            baseConfigModel.TableNameUpper = tableName;

            #region FileConfigModel
            baseConfigModel.FileConfig = new FileConfigModel();
            baseConfigModel.FileConfig.ClassPrefix = TableMappingHelper.GetClassNamePrefix(tableName);
            baseConfigModel.FileConfig.ClassDescription = tableDescription;
            baseConfigModel.FileConfig.CreateName = userName;
            baseConfigModel.FileConfig.CreateDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm");

            baseConfigModel.FileConfig.EntityName = string.Format("{0}Entity", baseConfigModel.FileConfig.ClassPrefix);
            baseConfigModel.FileConfig.EntityMapName = string.Format("{0}Map", baseConfigModel.FileConfig.ClassPrefix);
            baseConfigModel.FileConfig.EntityParamName = string.Format("{0}Param", baseConfigModel.FileConfig.ClassPrefix);
            baseConfigModel.FileConfig.BusinessName = string.Format("{0}BLL", baseConfigModel.FileConfig.ClassPrefix);
            baseConfigModel.FileConfig.ServiceName = string.Format("{0}Service", baseConfigModel.FileConfig.ClassPrefix);
            baseConfigModel.FileConfig.ControllerName = string.Format("{0}Controller", baseConfigModel.FileConfig.ClassPrefix);
            baseConfigModel.FileConfig.PageIndexName = string.Format("{0}Index", baseConfigModel.FileConfig.ClassPrefix);
            baseConfigModel.FileConfig.PageFormName = string.Format("{0}Form", baseConfigModel.FileConfig.ClassPrefix);
            #endregion

            #region OutputConfigModel          
            baseConfigModel.OutputConfig = new OutputConfigModel();
            baseConfigModel.OutputConfig.OutputModule = string.Empty;
            baseConfigModel.OutputConfig.OutputEntity = Path.Combine(path, "YiSha.Entity");
            baseConfigModel.OutputConfig.OutputBusiness = Path.Combine(path, "YiSha.Business");
            baseConfigModel.OutputConfig.OutputWeb = Path.Combine(path, "YiSha.Web", "YiSha.Admin.Web");
            string areasModule = Path.Combine(baseConfigModel.OutputConfig.OutputWeb, "Areas");
            if (Directory.Exists(areasModule))
            {
                baseConfigModel.OutputConfig.ModuleList = Directory.GetDirectories(areasModule).Select(p => Path.GetFileName(p)).Where(p => p != "DemoManage").ToList();
            }
            else
            {
                baseConfigModel.OutputConfig.ModuleList = new List<string> { "TestManage" };
            }
            #endregion

            #region PageIndexModel
            baseConfigModel.PageIndex = new PageIndexModel();
            baseConfigModel.PageIndex.IsSearch = 1;
            baseConfigModel.PageIndex.IsPagination = 1;
            baseConfigModel.PageIndex.ButtonList = new List<string>();
            baseConfigModel.PageIndex.ColumnList = new List<string>();
            baseConfigModel.PageIndex.ColumnList.AddRange(tableFieldList.Take(defaultField));
            #endregion

            #region PageFormModel
            baseConfigModel.PageForm = new PageFormModel();
            baseConfigModel.PageForm.ShowMode = 1;
            baseConfigModel.PageForm.FieldList = new List<string>();
            baseConfigModel.PageForm.FieldList.AddRange(tableFieldList.Take(defaultField));
            #endregion

            return baseConfigModel;
        }
        #endregion

        #region BuildEntity
        public string BuildEntity(BaseConfigModel baseConfigModel, DataTable dt)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("using System;");
            sb.AppendLine("using Newtonsoft.Json;");
            sb.AppendLine("using System.ComponentModel.DataAnnotations.Schema;");
            sb.AppendLine("using YiSha.Util;");
            sb.AppendLine();

            sb.AppendLine("namespace YiSha.Entity." + baseConfigModel.OutputConfig.OutputModule);
            sb.AppendLine("{");

            SetClassDescription("实体类", baseConfigModel, sb);

            sb.AppendLine("    [Table(\"" + baseConfigModel.TableName + "\")]");
            sb.AppendLine("    public class " + baseConfigModel.FileConfig.EntityName + " : " + GetBaseEntity(dt));
            sb.AppendLine("    {");

            string column = string.Empty;
            string remark = string.Empty;
            string datatype = string.Empty;
            foreach (DataRow dr in dt.Rows)
            {
                column = dr["TableColumn"].ToString();
                if (BaseField.BaseFieldList.Where(p => p == column).Any())
                {
                    // 基础字段不需要生成，继承合适的BaseEntity即可。
                    continue;
                }

                remark = dr["Remark"].ToString();
                datatype = dr["Datatype"].ToString();

                datatype = TableMappingHelper.GetPropertyDatatype(datatype);

                sb.AppendLine("        /// <summary>");
                sb.AppendLine("        /// " + remark);
                sb.AppendLine("        /// </summary>");
                sb.AppendLine("        /// <returns></returns>");

                switch (datatype)
                {
                    case "long?":
                        sb.AppendLine("        [JsonConverter(typeof(StringJsonConverter))]");
                        break;

                    case "DateTime?":
                        sb.AppendLine("        [JsonConverter(typeof(DateTimeJsonConverter))]");
                        break;
                }
                sb.AppendLine("        public " + datatype + " " + column + " { get; set; }");
            }
            sb.AppendLine("    }");
            sb.AppendLine("}");

            return sb.ToString();
        }
        #endregion

        #region BuildEntityParam
        public string BuildEntityParam(BaseConfigModel baseConfigModel)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("using System;");
            sb.AppendLine("using System.Collections.Generic;");
            sb.AppendLine("using Newtonsoft.Json;");
            sb.AppendLine("using YiSha.Util;");
            sb.AppendLine();

            sb.AppendLine("namespace YiSha.Model.Param." + baseConfigModel.OutputConfig.OutputModule);
            sb.AppendLine("{");

            SetClassDescription("实体查询类", baseConfigModel, sb);

            sb.AppendLine("    public class " + baseConfigModel.FileConfig.EntityParamName.Replace("Param", "ListParam"));
            sb.AppendLine("    {");

            sb.AppendLine("    }");
            sb.AppendLine("}");

            return sb.ToString();
        }
        #endregion

        #region BuildService
        public string BuildService(BaseConfigModel baseConfigModel, DataTable dt)
        {
            string baseEntity = GetBaseEntity(dt);

            StringBuilder sb = new StringBuilder();
            string method = string.Empty;
            sb.AppendLine("using System;");
            sb.AppendLine("using System.Linq;");
            sb.AppendLine("using System.Text;");
            sb.AppendLine("using System.Data.Common;");
            sb.AppendLine("using System.Linq.Expressions;");
            sb.AppendLine("using System.Collections.Generic;");
            sb.AppendLine("using System.Threading.Tasks;");
            sb.AppendLine("using YiSha.Util;");
            sb.AppendLine("using YiSha.Util.Extension;");
            sb.AppendLine("using YiSha.Util.Model;");
            sb.AppendLine("using YiSha.Data;");
            sb.AppendLine("using YiSha.Data.Repository;");
            sb.AppendLine("using YiSha.Entity." + baseConfigModel.OutputConfig.OutputModule + ";");
            sb.AppendLine("using YiSha.Model.Param." + baseConfigModel.OutputConfig.OutputModule + ";");

            sb.AppendLine();

            sb.AppendLine("namespace YiSha.Service." + baseConfigModel.OutputConfig.OutputModule);
            sb.AppendLine("{");

            SetClassDescription("服务类", baseConfigModel, sb);

            sb.AppendLine("    public class " + baseConfigModel.FileConfig.ServiceName + " :  RepositoryFactory");
            sb.AppendLine("    {");
            sb.AppendLine("        #region 获取数据");
            sb.AppendLine("        public async Task<List<" + baseConfigModel.FileConfig.EntityName + ">> GetList(" + baseConfigModel.FileConfig.EntityParamName.Replace("Param", "ListParam") + " param)");
            sb.AppendLine("        {");
            sb.AppendLine("            var expression = ListFilter(param);");
            sb.AppendLine("            var list = await this.BaseRepository().FindList(expression);");
            sb.AppendLine("            return list.ToList();");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        public async Task<List<" + baseConfigModel.FileConfig.EntityName + ">> GetPageList(" + baseConfigModel.FileConfig.EntityParamName.Replace("Param", "ListParam") + " param, Pagination pagination)");
            sb.AppendLine("        {");
            sb.AppendLine("            var expression = ListFilter(param);");
            sb.AppendLine("            var list= await this.BaseRepository().FindList(expression, pagination);");
            sb.AppendLine("            return list.ToList();");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        public async Task<" + baseConfigModel.FileConfig.EntityName + "> GetEntity(long id)");
            sb.AppendLine("        {");
            sb.AppendLine("            return await this.BaseRepository().FindEntity<" + baseConfigModel.FileConfig.EntityName + ">(id);");
            sb.AppendLine("        }");
            sb.AppendLine("        #endregion");
            sb.AppendLine();
            sb.AppendLine("        #region 提交数据");
            sb.AppendLine("        public async Task SaveForm(" + baseConfigModel.FileConfig.EntityName + " entity)");
            sb.AppendLine("        {");
            sb.AppendLine("            if (entity.Id.IsNullOrZero())");
            sb.AppendLine("            {");
            sb.AppendLine("                " + GetSaveFormCreate(baseEntity));
            sb.AppendLine("                await this.BaseRepository().Insert(entity);");
            sb.AppendLine("            }");
            sb.AppendLine("            else");
            sb.AppendLine("            {");
            sb.AppendLine("                " + GetSaveFormModify(baseEntity));
            sb.AppendLine("                await this.BaseRepository().Update(entity);");
            sb.AppendLine("            }");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        public async Task DeleteForm(string ids)");
            sb.AppendLine("        {");
            sb.AppendLine("            long[] idArr = TextHelper.SplitToArray<long>(ids, ',');");
            sb.AppendLine("            await this.BaseRepository().Delete<" + baseConfigModel.FileConfig.EntityName + ">(idArr);");
            sb.AppendLine("        }");
            sb.AppendLine("        #endregion");
            sb.AppendLine();
            sb.AppendLine("        #region 私有方法");
            sb.AppendLine("        private Expression<Func<" + baseConfigModel.FileConfig.EntityName + ", bool>> ListFilter(" + baseConfigModel.FileConfig.EntityParamName.Replace("Param", "ListParam") + " param)");
            sb.AppendLine("        {");
            sb.AppendLine("            var expression = LinqExtensions.True<" + baseConfigModel.FileConfig.EntityName + ">();");
            sb.AppendLine("            if (param != null)");
            sb.AppendLine("            {");
            sb.AppendLine("            }");
            sb.AppendLine("            return expression;");
            sb.AppendLine("        }");
            sb.AppendLine("        #endregion");

            sb.AppendLine("    }");
            sb.AppendLine("}");
            return sb.ToString();
        }
        #endregion

        #region BuildBusiness
        public string BuildBusiness(BaseConfigModel baseConfigModel)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("using System;");
            sb.AppendLine("using System.Linq;");
            sb.AppendLine("using System.Collections.Generic;");
            sb.AppendLine("using System.Threading.Tasks;");
            sb.AppendLine("using YiSha.Util;");
            sb.AppendLine("using YiSha.Util.Extension;");
            sb.AppendLine("using YiSha.Util.Model;");
            sb.AppendLine("using YiSha.Entity." + baseConfigModel.OutputConfig.OutputModule + ";");
            sb.AppendLine("using YiSha.Model.Param." + baseConfigModel.OutputConfig.OutputModule + ";");
            sb.AppendLine("using YiSha.Service." + baseConfigModel.OutputConfig.OutputModule + ";");
            sb.AppendLine();

            sb.AppendLine("namespace YiSha.Business." + baseConfigModel.OutputConfig.OutputModule);
            sb.AppendLine("{");

            SetClassDescription("业务类", baseConfigModel, sb);

            sb.AppendLine("    public class " + baseConfigModel.FileConfig.BusinessName);
            sb.AppendLine("    {");

            sb.AppendLine("        private " + baseConfigModel.FileConfig.ServiceName + " " + TableMappingHelper.FirstLetterLowercase(baseConfigModel.FileConfig.ServiceName) + " = new " + baseConfigModel.FileConfig.ServiceName + "();");
            sb.AppendLine();
            sb.AppendLine("        #region 获取数据");
            sb.AppendLine("        public async Task<TData<List<" + baseConfigModel.FileConfig.EntityName + ">>> GetList(" + baseConfigModel.FileConfig.EntityParamName.Replace("Param", "ListParam") + " param)");
            sb.AppendLine("        {");
            sb.AppendLine("            TData<List<" + baseConfigModel.FileConfig.EntityName + ">> obj = new TData<List<" + baseConfigModel.FileConfig.EntityName + ">>();");
            sb.AppendLine("            obj.Data = await " + TableMappingHelper.FirstLetterLowercase(baseConfigModel.FileConfig.ServiceName) + ".GetList(param);");
            sb.AppendLine("            obj.Total = obj.Data.Count;");
            sb.AppendLine("            obj.Tag = 1;");
            sb.AppendLine("            return obj;");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        public async Task<TData<List<" + baseConfigModel.FileConfig.EntityName + ">>> GetPageList(" + baseConfigModel.FileConfig.EntityParamName.Replace("Param", "ListParam") + " param, Pagination pagination)");
            sb.AppendLine("        {");
            sb.AppendLine("            TData<List<" + baseConfigModel.FileConfig.EntityName + ">> obj = new TData<List<" + baseConfigModel.FileConfig.EntityName + ">>();");
            sb.AppendLine("            obj.Data = await " + TableMappingHelper.FirstLetterLowercase(baseConfigModel.FileConfig.ServiceName) + ".GetPageList(param, pagination);");
            sb.AppendLine("            obj.Total = pagination.TotalCount;");
            sb.AppendLine("            obj.Tag = 1;");
            sb.AppendLine("            return obj;");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        public async Task<TData<" + baseConfigModel.FileConfig.EntityName + ">> GetEntity(long id)");
            sb.AppendLine("        {");
            sb.AppendLine("            TData<" + baseConfigModel.FileConfig.EntityName + "> obj = new TData<" + baseConfigModel.FileConfig.EntityName + ">();");
            sb.AppendLine("            obj.Data = await " + TableMappingHelper.FirstLetterLowercase(baseConfigModel.FileConfig.ServiceName) + ".GetEntity(id);");
            sb.AppendLine("            if (obj.Data != null)");
            sb.AppendLine("            {");
            sb.AppendLine("                obj.Tag = 1;");
            sb.AppendLine("            }");
            sb.AppendLine("            return obj;");
            sb.AppendLine("        }");
            sb.AppendLine("        #endregion");
            sb.AppendLine();
            sb.AppendLine("        #region 提交数据");
            sb.AppendLine("        public async Task<TData<string>> SaveForm(" + baseConfigModel.FileConfig.EntityName + " entity)");
            sb.AppendLine("        {");
            sb.AppendLine("            TData<string> obj = new TData<string>();");
            sb.AppendLine("            await " + TableMappingHelper.FirstLetterLowercase(baseConfigModel.FileConfig.ServiceName) + ".SaveForm(entity);");
            sb.AppendLine("            obj.Data = entity.Id.ParseToString();");
            sb.AppendLine("            obj.Tag = 1;");
            sb.AppendLine("            return obj;");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        public async Task<TData> DeleteForm(string ids)");
            sb.AppendLine("        {");
            sb.AppendLine("            TData obj = new TData();");
            sb.AppendLine("            await " + TableMappingHelper.FirstLetterLowercase(baseConfigModel.FileConfig.ServiceName) + ".DeleteForm(ids);");
            sb.AppendLine("            obj.Tag = 1;");
            sb.AppendLine("            return obj;");
            sb.AppendLine("        }");
            sb.AppendLine("        #endregion");
            sb.AppendLine();
            sb.AppendLine("        #region 私有方法");
            sb.AppendLine("        #endregion");

            sb.AppendLine("    }");
            sb.AppendLine("}");
            return sb.ToString();
        }
        #endregion

        #region BuildController
        public string BuildController(BaseConfigModel baseConfigModel)
        {
            string modulePrefix = GetModulePrefix(baseConfigModel);
            string classPrefix = baseConfigModel.FileConfig.ClassPrefix.ToLower();

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("using System;");
            sb.AppendLine("using System.Collections.Generic;");
            sb.AppendLine("using System.Threading.Tasks;");
            sb.AppendLine("using System.Linq;");
            sb.AppendLine("using System.Web;");
            sb.AppendLine("using Microsoft.AspNetCore.Mvc;");
            sb.AppendLine("using YiSha.Util;");
            sb.AppendLine("using YiSha.Util.Model;");
            sb.AppendLine("using YiSha.Entity;");
            sb.AppendLine("using YiSha.Model;");
            sb.AppendLine("using YiSha.Admin.Web.Controllers;");
            sb.AppendLine("using YiSha.Entity." + baseConfigModel.OutputConfig.OutputModule + ";");
            sb.AppendLine("using YiSha.Business." + baseConfigModel.OutputConfig.OutputModule + ";");
            sb.AppendLine("using YiSha.Model.Param." + baseConfigModel.OutputConfig.OutputModule + ";");
            sb.AppendLine();

            sb.AppendLine("namespace YiSha.Admin.Web.Areas." + baseConfigModel.OutputConfig.OutputModule + ".Controllers");
            sb.AppendLine("{");

            SetClassDescription("控制器类", baseConfigModel, sb);

            sb.AppendLine("    [Area(\"" + baseConfigModel.OutputConfig.OutputModule + "\")]");
            sb.AppendLine("    public class " + baseConfigModel.FileConfig.ControllerName + " :  BaseController");
            sb.AppendLine("    {");
            sb.AppendLine("        private " + baseConfigModel.FileConfig.BusinessName + " " + TableMappingHelper.FirstLetterLowercase(baseConfigModel.FileConfig.BusinessName) + " = new " + baseConfigModel.FileConfig.BusinessName + "();");
            sb.AppendLine();
            sb.AppendLine("        #region 视图功能");
            sb.AppendLine("        [AuthorizeFilter(\"" + string.Format("{0}:{1}:{2}", modulePrefix, classPrefix, "view") + "\")]");
            sb.AppendLine("        public ActionResult " + baseConfigModel.FileConfig.PageIndexName + "()");
            sb.AppendLine("        {");
            sb.AppendLine("            return View();");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        public ActionResult " + baseConfigModel.FileConfig.PageFormName + "()");
            sb.AppendLine("        {");
            sb.AppendLine("            return View();");
            sb.AppendLine("        }");
            sb.AppendLine("        #endregion");
            sb.AppendLine();
            sb.AppendLine("        #region 获取数据");
            sb.AppendLine("        [HttpGet]");
            sb.AppendLine("        [AuthorizeFilter(\"" + string.Format("{0}:{1}:{2}", modulePrefix, classPrefix, "search") + "\")]");
            sb.AppendLine("        public async Task<ActionResult> GetListJson(" + baseConfigModel.FileConfig.EntityParamName.Replace("Param", "ListParam") + " param)");
            sb.AppendLine("        {");
            sb.AppendLine("            TData<List<" + baseConfigModel.FileConfig.EntityName + ">> obj = await " + TableMappingHelper.FirstLetterLowercase(baseConfigModel.FileConfig.BusinessName) + ".GetList(param);");
            sb.AppendLine("            return Json(obj);");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        [HttpGet]");
            sb.AppendLine("        [AuthorizeFilter(\"" + string.Format("{0}:{1}:{2}", modulePrefix, classPrefix, "search") + "\")]");
            sb.AppendLine("        public async Task<ActionResult> GetPageListJson(" + baseConfigModel.FileConfig.EntityParamName.Replace("Param", "ListParam") + " param, Pagination pagination)");
            sb.AppendLine("        {");
            sb.AppendLine("            TData<List<" + baseConfigModel.FileConfig.EntityName + ">> obj = await " + TableMappingHelper.FirstLetterLowercase(baseConfigModel.FileConfig.BusinessName) + ".GetPageList(param, pagination);");
            sb.AppendLine("            return Json(obj);");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        [HttpGet]");
            sb.AppendLine("        public async Task<ActionResult> GetFormJson(long id)");
            sb.AppendLine("        {");
            sb.AppendLine("            TData<" + baseConfigModel.FileConfig.EntityName + "> obj = await " + TableMappingHelper.FirstLetterLowercase(baseConfigModel.FileConfig.BusinessName) + ".GetEntity(id);");
            sb.AppendLine("            return Json(obj);");
            sb.AppendLine("        }");
            sb.AppendLine("        #endregion");
            sb.AppendLine();
            sb.AppendLine("        #region 提交数据");
            sb.AppendLine("        [HttpPost]");
            sb.AppendLine("        [AuthorizeFilter(\"" + string.Format("{0}:{1}:{2}", modulePrefix, classPrefix, "add") + "," + string.Format("{0}:{1}:{2}", modulePrefix, classPrefix, "edit") + "\")]");
            sb.AppendLine("        public async Task<ActionResult> SaveFormJson(" + baseConfigModel.FileConfig.EntityName + " entity)");
            sb.AppendLine("        {");
            sb.AppendLine("            TData<string> obj = await " + TableMappingHelper.FirstLetterLowercase(baseConfigModel.FileConfig.BusinessName) + ".SaveForm(entity);");
            sb.AppendLine("            return Json(obj);");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        [HttpPost]");
            sb.AppendLine("        [AuthorizeFilter(\"" + string.Format("{0}:{1}:{2}", modulePrefix, classPrefix, "delete") + "\")]");
            sb.AppendLine("        public async Task<ActionResult> DeleteFormJson(string ids)");
            sb.AppendLine("        {");
            sb.AppendLine("            TData obj = await " + TableMappingHelper.FirstLetterLowercase(baseConfigModel.FileConfig.BusinessName) + ".DeleteForm(ids);");
            sb.AppendLine("            return Json(obj);");
            sb.AppendLine("        }");
            sb.AppendLine("        #endregion");
            sb.AppendLine("    }");
            sb.AppendLine("}");
            return sb.ToString();
        }
        #endregion

        #region BuildIndex
        public string BuildIndex(BaseConfigModel baseConfigModel)
        {
            #region 初始化集合
            if (baseConfigModel.PageIndex.ButtonList == null)
            {
                baseConfigModel.PageIndex.ButtonList = new List<string>();
            }
            if (baseConfigModel.PageIndex.ColumnList == null)
            {
                baseConfigModel.PageIndex.ColumnList = new List<string>();
            }
            #endregion

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("@{");
            sb.AppendLine("    Layout = \"~/Views/Shared/_Index.cshtml\";");
            sb.AppendLine(" }");
            sb.AppendLine("<div class=\"container-div\">");
            sb.AppendLine("    <div class=\"row\">");

            #region 是否显示搜索
            if (baseConfigModel.PageIndex.IsSearch == 1)
            {
                string fieldName = TextHelper.GetCustomValue(baseConfigModel.PageIndex.ColumnList.FirstOrDefault(), "fieldName");
                string fieldNameLower = TableMappingHelper.FirstLetterLowercase(fieldName);
                sb.AppendLine("        <div id=\"searchDiv\" class=\"col-sm-12 search-collapse\">");
                sb.AppendLine("            <div class=\"select-list\">");
                sb.AppendLine("                <ul>");
                sb.AppendLine("                    <li>");
                sb.AppendLine("                        " + fieldName + "：<input id=\"" + fieldNameLower + "\" col=\"" + fieldName + "\" type=\"text\" />");
                sb.AppendLine("                    </li>");
                sb.AppendLine("                    <li>");
                sb.AppendLine("                        <a id=\"btnSearch\" class=\"btn btn-primary btn-sm\" onclick=\"searchGrid()\"><i class=\"fa fa-search\"></i>&nbsp;搜索</a>");
                sb.AppendLine("                    </li>");
                sb.AppendLine("                </ul>");
                sb.AppendLine("            </div>");
                sb.AppendLine("        </div>");
            }
            #endregion

            #region 是否显示工具栏
            if (baseConfigModel.PageIndex.ButtonList.Where(p => p != "btnSearch").Any())
            {
                sb.AppendLine("        <div class=\"btn-group-sm hidden-xs\" id=\"toolbar\">");
                if (baseConfigModel.PageIndex.ButtonList.Where(p => p == "btnAdd").Any())
                {
                    sb.AppendLine("            <a id=\"btnAdd\" class=\"btn btn-success\" onclick=\"showSaveForm(true)\"><i class=\"fa fa-plus\"></i> 新增</a>");
                }
                if (baseConfigModel.PageIndex.ButtonList.Where(p => p == "btnEdit").Any())
                {
                    sb.AppendLine("            <a id=\"btnEdit\" class=\"btn btn-primary disabled\" onclick=\"showSaveForm(false)\"><i class=\"fa fa-edit\"></i> 修改</a>");
                }
                if (baseConfigModel.PageIndex.ButtonList.Where(p => p == "btnDelete").Any())
                {
                    sb.AppendLine("            <a id=\"btnDelete\" class=\"btn btn-danger disabled\" onclick=\"deleteForm()\"><i class=\"fa fa-remove\"></i> 删除</a>");
                }
                sb.AppendLine("        </div>");
            }
            #endregion

            sb.AppendLine("        <div class=\"col-sm-12 select-table table-striped\">");
            sb.AppendLine("            <table id=\"gridTable\" data-mobile-responsive=\"true\"></table>");
            sb.AppendLine("        </div>");
            sb.AppendLine("    </div>");
            sb.AppendLine("</div>");
            sb.AppendLine("");
            sb.AppendLine("<script type=\"text/javascript\">");
            sb.AppendLine("    $(function () {");
            sb.AppendLine("        initGrid();");
            sb.AppendLine("    });");
            sb.AppendLine("");
            sb.AppendLine("    function initGrid() {");
            sb.AppendLine("        var queryUrl = '@Url.Content(\"~/" + baseConfigModel.OutputConfig.OutputModule + "/" + baseConfigModel.FileConfig.ClassPrefix + "/GetPageListJson\")';");
            sb.AppendLine("        $('#gridTable').ysTable({");
            sb.AppendLine("            url: queryUrl,");
            sb.AppendLine("            columns: [");
            sb.AppendLine("                { checkbox: true, visible: true },");
            sb.AppendLine("                { field: 'Id', title: 'Id', visible: false },");

            foreach (string column in baseConfigModel.PageIndex.ColumnList)
            {
                sb.AppendLine("                { field: '" + column + "', title: '" + column + "' },");
            }

            sb.AppendLine("            ],");
            if (baseConfigModel.PageIndex.IsPagination == 0)
            {
                sb.AppendLine("            pagination: false,");
            }
            sb.AppendLine("            queryParams: function (params) {");
            sb.AppendLine("                var pagination = $('#gridTable').ysTable('getPagination', params);");
            sb.AppendLine("                var queryString = $('#searchDiv').getWebControls(pagination);");
            sb.AppendLine("                return queryString;");
            sb.AppendLine("            }");
            sb.AppendLine("        });");
            sb.AppendLine("    }");
            sb.AppendLine("");
            sb.AppendLine("    function searchGrid() {");
            sb.AppendLine("        $('#gridTable').ysTable('search');");
            sb.AppendLine("        resetToolbarStatus();");
            sb.AppendLine("    }");
            sb.AppendLine("");

            #region 新增和修改方法
            if (baseConfigModel.PageIndex.ButtonList.Where(p => p == "btnAdd" || p == "btnEdit").Any())
            {
                sb.AppendLine("    function showSaveForm(bAdd) {");
                sb.AppendLine("        var id = 0;");
                sb.AppendLine("        if (!bAdd) {");
                sb.AppendLine("            var selectedRow = $('#gridTable').bootstrapTable('getSelections');");
                sb.AppendLine("            if (!ys.checkRowEdit(selectedRow)) {");
                sb.AppendLine("                return;");
                sb.AppendLine("            }");
                sb.AppendLine("            else {");
                sb.AppendLine("                id = selectedRow[0].Id;");
                sb.AppendLine("            }");
                sb.AppendLine("        }");
                sb.AppendLine("        ys.openDialog({");
                sb.AppendLine("            title: id > 0 ? '编辑' : '添加',");
                sb.AppendLine("            content: '@Url.Content(\"~/" + baseConfigModel.OutputConfig.OutputModule + "/" + baseConfigModel.FileConfig.ClassPrefix + "/" + baseConfigModel.FileConfig.ClassPrefix + "Form\")' + '?id=\' + id,");
                sb.AppendLine("            width: '768px',");
                sb.AppendLine("            height: '550px',");
                sb.AppendLine("            callback: function (index, layero) {");
                sb.AppendLine("                var iframeWin = window[layero.find('iframe')[0]['name']];");
                sb.AppendLine("                iframeWin.saveForm(index);");
                sb.AppendLine("            }");
                sb.AppendLine("        });");
                sb.AppendLine("    }");
            }
            #endregion

            sb.AppendLine("");

            #region 删除方法
            if (baseConfigModel.PageIndex.ButtonList.Where(p => p == "btnDelete").Any())
            {
                sb.AppendLine("    function deleteForm() {");
                sb.AppendLine("        var selectedRow = $('#gridTable').bootstrapTable('getSelections');");
                sb.AppendLine("        if (ys.checkRowDelete(selectedRow)) {");
                sb.AppendLine("            ys.confirm('确认要删除选中的' + selectedRow.length + '条数据吗？', function () {");
                sb.AppendLine("                var ids = ys.getIds(selectedRow);");
                sb.AppendLine("                ys.ajax({");
                sb.AppendLine("                    url: '@Url.Content(\"~/" + baseConfigModel.OutputConfig.OutputModule + "/" + baseConfigModel.FileConfig.ClassPrefix + "/DeleteFormJson\")' + '?ids=' + ids,");
                sb.AppendLine("                    type: 'post',");
                sb.AppendLine("                    success: function (obj) {");
                sb.AppendLine("                        if (obj.Tag == 1) {");
                sb.AppendLine("                            ys.msgSuccess(obj.Message);");
                sb.AppendLine("                            searchGrid();");
                sb.AppendLine("                        }");
                sb.AppendLine("                        else {");
                sb.AppendLine("                            ys.msgError(obj.Message);");
                sb.AppendLine("                        }");
                sb.AppendLine("                    }");
                sb.AppendLine("                });");
                sb.AppendLine("            });");
                sb.AppendLine("        }");
                sb.AppendLine("    }");
            }
            #endregion

            sb.AppendLine("</script>");
            return sb.ToString();
        }
        #endregion

        #region BuildForm
        public string BuildForm(BaseConfigModel baseConfigModel)
        {
            #region 初始化集合
            if (baseConfigModel.PageForm.FieldList == null)
            {
                baseConfigModel.PageForm.FieldList = new List<string>();
            }
            #endregion
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("@{");
            sb.AppendLine("    Layout = \"~/Views/Shared/_FormWhite.cshtml\";");
            sb.AppendLine("}");
            sb.AppendLine("");
            sb.AppendLine("<div class=\"wrapper animated fadeInRight\">");
            sb.AppendLine("    <form id=\"form\" class=\"form-horizontal m\">");

            #region 表单控件
            if (baseConfigModel.PageForm.FieldList.Count > 0)
            {
                string field = string.Empty;
                string fieldLower = string.Empty;
                switch (baseConfigModel.PageForm.ShowMode)
                {
                    case 1:
                        for (int i = 0; i < baseConfigModel.PageForm.FieldList.Count; i++)
                        {
                            field = baseConfigModel.PageForm.FieldList[i];
                            fieldLower = TableMappingHelper.FirstLetterLowercase(field);

                            sb.AppendLine("        <div class=\"form-group\">");
                            sb.AppendLine("            <label class=\"col-sm-3 control-label \">" + field + (i == 0 ? "<font class=\"red\"> *</font>" : string.Empty) + "</label>");
                            sb.AppendLine("            <div class=\"col-sm-8\">");
                            sb.AppendLine("                <input id=\"" + fieldLower + "\" col=\"" + field + "\" type=\"text\" class=\"form-control\" />");
                            sb.AppendLine("            </div>");
                            sb.AppendLine("        </div>");
                        }
                        break;

                    case 2:
                        for (int i = 0; i < baseConfigModel.PageForm.FieldList.Count; i++)
                        {
                            field = baseConfigModel.PageForm.FieldList[i];
                            fieldLower = TableMappingHelper.FirstLetterLowercase(field);

                            if (i % 2 == 0)
                            {
                                sb.AppendLine("        <div class=\"form-group\">");
                            }

                            sb.AppendLine("            <label class=\"col-sm-2 control-label \">" + field + "<font class=\"red\"> *</font></label>");
                            sb.AppendLine("            <div class=\"col-sm-4\">");
                            sb.AppendLine("                <input id=\"" + fieldLower + "\" col=\"" + field + "\" type=\"text\" class=\"form-control\" />");
                            sb.AppendLine("            </div>");

                            if (i % 2 == 1)
                            {
                                sb.AppendLine("        </div>");
                            }
                        }
                        break;
                }
            }
            #endregion

            sb.AppendLine("    </form>");
            sb.AppendLine("</div>");
            sb.AppendLine("");
            sb.AppendLine("<script type=\"text/javascript\">");
            sb.AppendLine("    var id = ys.request(\"id\");");
            sb.AppendLine("    $(function () {");
            sb.AppendLine("        getForm();");
            sb.AppendLine("");
            sb.AppendLine("        $('#form').validate({");
            sb.AppendLine("            rules: {");
            sb.AppendLine("                " + TextHelper.GetCustomValue(TableMappingHelper.FirstLetterLowercase(baseConfigModel.PageForm.FieldList.FirstOrDefault()), "fieldName") + ": { required: true }");
            sb.AppendLine("            }");
            sb.AppendLine("        });");
            sb.AppendLine("    });");
            sb.AppendLine("");
            sb.AppendLine("    function getForm() {");
            sb.AppendLine("        if (id > 0) {");
            sb.AppendLine("            ys.ajax({");
            sb.AppendLine("                url: '@Url.Content(\"~/" + baseConfigModel.OutputConfig.OutputModule + "/" + baseConfigModel.FileConfig.ClassPrefix + "/GetFormJson\")' + '?id=' + id,");
            sb.AppendLine("                type: 'get',");
            sb.AppendLine("                success: function (obj) {");
            sb.AppendLine("                    if (obj.Tag == 1) {");
            sb.AppendLine("                        $('#form').setWebControls(obj.Data);");
            sb.AppendLine("                    }");
            sb.AppendLine("                }");
            sb.AppendLine("            });");
            sb.AppendLine("        }");
            sb.AppendLine("        else {");
            sb.AppendLine("            var defaultData = {};");
            sb.AppendLine("            $('#form').setWebControls(defaultData);");
            sb.AppendLine("        }");
            sb.AppendLine("    }");
            sb.AppendLine("");
            sb.AppendLine("    function saveForm(index) {");
            sb.AppendLine("        if ($('#form').validate().form()) {");
            sb.AppendLine("            var postData = $('#form').getWebControls({ Id: id });");
            sb.AppendLine("            ys.ajax({");
            sb.AppendLine("                url: '@Url.Content(\"~/" + baseConfigModel.OutputConfig.OutputModule + "/" + baseConfigModel.FileConfig.ClassPrefix + "/SaveFormJson\")',");
            sb.AppendLine("                type: 'post',");
            sb.AppendLine("                data: postData,");
            sb.AppendLine("                success: function (obj) {");
            sb.AppendLine("                    if (obj.Tag == 1) {");
            sb.AppendLine("                        ys.msgSuccess(obj.Message);");
            sb.AppendLine("                        parent.searchGrid();");
            sb.AppendLine("                        parent.layer.close(index);");
            sb.AppendLine("                    }");
            sb.AppendLine("                    else {");
            sb.AppendLine("                        ys.msgError(obj.Message);");
            sb.AppendLine("                    }");
            sb.AppendLine("                }");
            sb.AppendLine("            });");
            sb.AppendLine("        }");
            sb.AppendLine("    }");
            sb.AppendLine("</script>");
            sb.AppendLine("");
            return sb.ToString();
        }
        #endregion

        #region BuildMenu
        public string BuildMenu(BaseConfigModel baseConfigModel)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine();
            sb.AppendLine("  菜单路径:" + baseConfigModel.OutputConfig.OutputModule + "/" + baseConfigModel.FileConfig.ClassPrefix + "/" + baseConfigModel.FileConfig.PageIndexName);
            sb.AppendLine();
            string modulePrefix = GetModulePrefix(baseConfigModel);
            string classPrefix = baseConfigModel.FileConfig.ClassPrefix.ToLower();
            sb.AppendLine("  页面显示权限：" + string.Format("{0}:{1}:{2}", modulePrefix, classPrefix, "view"));
            sb.AppendLine();
            List<KeyValue> list = GetButtonAuthorizeList();

            if (baseConfigModel.PageIndex.IsSearch == 1)
            {
                KeyValue button = list.Where(p => p.Key == "btnSearch").FirstOrDefault();
                sb.AppendLine("  按钮" + button.Description + "权限：" + string.Format("{0}:{1}:{2}", modulePrefix, classPrefix, button.Value));
            }
            foreach (string btn in baseConfigModel.PageIndex.ButtonList)
            {
                KeyValue button = list.Where(p => p.Key == btn).FirstOrDefault();
                sb.AppendLine("  按钮" + button.Description + "权限：" + string.Format("{0}:{1}:{2}", modulePrefix, classPrefix, button.Value));
            }
            sb.AppendLine();
            return sb.ToString();
        }
        #endregion

        #region CreateCode
        public async Task<List<KeyValue>> CreateCode(BaseConfigModel baseConfigModel, string code)
        {
            List<KeyValue> result = new List<KeyValue>();
            JObject param = code.ToJObject();

            #region 实体类
            if (!string.IsNullOrEmpty(param["CodeEntity"].ParseToString()))
            {
                string codeEntity = HttpUtility.HtmlDecode(param["CodeEntity"].ToString());
                string codePath = Path.Combine(baseConfigModel.OutputConfig.OutputEntity, "YiSha.Entity", baseConfigModel.OutputConfig.OutputModule, baseConfigModel.FileConfig.EntityName + ".cs");
                if (!File.Exists(codePath))
                {
                    FileHelper.CreateFile(codePath, codeEntity);
                    result.Add(new KeyValue { Key = "实体类", Value = codePath });
                }
            }
            #endregion

            #region 实体查询类
            if (!param["CodeEntityParam"].IsEmpty())
            {
                string codeListEntity = HttpUtility.HtmlDecode(param["CodeEntityParam"].ToString());
                string codePath = Path.Combine(baseConfigModel.OutputConfig.OutputEntity, "YiSha.Model", "Param", baseConfigModel.OutputConfig.OutputModule, baseConfigModel.FileConfig.EntityParamName + ".cs");
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
                string codePath = Path.Combine(baseConfigModel.OutputConfig.OutputBusiness, "YiSha.Service", baseConfigModel.OutputConfig.OutputModule, baseConfigModel.FileConfig.ServiceName + ".cs");
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
                string codePath = Path.Combine(baseConfigModel.OutputConfig.OutputBusiness, "YiSha.Business", baseConfigModel.OutputConfig.OutputModule, baseConfigModel.FileConfig.BusinessName + ".cs");
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
                string codePath = Path.Combine(baseConfigModel.OutputConfig.OutputWeb, "Areas", baseConfigModel.OutputConfig.OutputModule, "Controllers", baseConfigModel.FileConfig.ControllerName + ".cs");
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
                string codePath = Path.Combine(baseConfigModel.OutputConfig.OutputWeb, "Areas", baseConfigModel.OutputConfig.OutputModule, "Views", baseConfigModel.FileConfig.ClassPrefix, baseConfigModel.FileConfig.PageIndexName + ".cshtml");
                if (!File.Exists(codePath))
                {
                    FileHelper.CreateFile(codePath, codeIndex);
                    result.Add(new KeyValue { Key = "列表页", Value = codePath });
                }

                // 生成菜单
                RepositoryFactory repositoryFactory = new RepositoryFactory();
                List<KeyValue> buttonAuthorizeList = GetButtonAuthorizeList();
                string menuUrl = baseConfigModel.OutputConfig.OutputModule + "/" + baseConfigModel.FileConfig.ClassPrefix + "/" + baseConfigModel.FileConfig.PageIndexName;
                string modulePrefix = GetModulePrefix(baseConfigModel);
                string classPrefix = baseConfigModel.FileConfig.ClassPrefix.ToLower();
                MenuEntity menuEntity = new MenuEntity
                {
                    MenuName = baseConfigModel.FileConfig.ClassDescription,
                    MenuUrl = menuUrl,
                    MenuType = (int)MenuTypeEnum.Menu,
                    Authorize = string.Format("{0}:{1}:{2}", modulePrefix, classPrefix, "view")
                };
                TData obj = await AddMenu(repositoryFactory, menuEntity);
                if (obj.Tag == 1)
                {
                    result.Add(new KeyValue { Key = "菜单(刷新页面可见)", Value = menuUrl });
                    if (baseConfigModel.PageIndex.IsSearch == 1)
                    {
                        // 按钮搜索权限
                        KeyValue button = buttonAuthorizeList.Where(p => p.Key == "btnSearch").FirstOrDefault();
                        MenuEntity buttonEntity = new MenuEntity
                        {
                            ParentId = menuEntity.Id,
                            MenuName = baseConfigModel.FileConfig.ClassDescription + button.Description,
                            MenuType = (int)MenuTypeEnum.Button,
                            Authorize = string.Format("{0}:{1}:{2}", modulePrefix, classPrefix, button.Value)
                        };
                        await AddMenu(repositoryFactory, buttonEntity);
                    }
                    foreach (string btn in baseConfigModel.PageIndex.ButtonList)
                    {
                        KeyValue button = buttonAuthorizeList.Where(p => p.Key == btn).FirstOrDefault();
                        MenuEntity buttonEntity = new MenuEntity
                        {
                            ParentId = menuEntity.Id,
                            MenuName = baseConfigModel.FileConfig.ClassDescription + button.Description,
                            MenuType = (int)MenuTypeEnum.Button,
                            Authorize = string.Format("{0}:{1}:{2}", modulePrefix, classPrefix, button.Value)
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
                string codePath = Path.Combine(baseConfigModel.OutputConfig.OutputWeb, "Areas", baseConfigModel.OutputConfig.OutputModule, "Views", baseConfigModel.FileConfig.ClassPrefix, baseConfigModel.FileConfig.PageFormName + ".cshtml");
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
            TData obj = new TData();
            IEnumerable<MenuEntity> menuList = await repositoryFactory.BaseRepository().FindList<MenuEntity>();
            if (!menuList.Where(p => p.MenuName == menuEntity.MenuName && p.Authorize == menuEntity.Authorize).Any())
            {
                menuEntity.MenuSort = menuList.Max(p => p.MenuSort) + 1;
                menuEntity.MenuStatus = 1;
                await menuEntity.Create();
                await repositoryFactory.BaseRepository().Insert(menuEntity);
                obj.Tag = 1;
            }
            return obj;
        }
        #endregion

        #region 私有方法
        #region GetProjectRootPath
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
        #endregion

        #region SetClassDescription
        private void SetClassDescription(string type, BaseConfigModel baseConfigModel, StringBuilder sb)
        {
            sb.AppendLine("    /// <summary>");
            sb.AppendLine("    /// 创 建：" + baseConfigModel.FileConfig.CreateName);
            sb.AppendLine("    /// 日 期：" + baseConfigModel.FileConfig.CreateDate);
            sb.AppendLine("    /// 描 述：" + baseConfigModel.FileConfig.ClassDescription + type);
            sb.AppendLine("    /// </summary>");
        }
        #endregion

        #region GetButtonAuthorizeList
        private List<KeyValue> GetButtonAuthorizeList()
        {
            var list = new List<KeyValue>();
            list.Add(new KeyValue { Key = "btnSearch", Value = "search", Description = "搜索" });
            list.Add(new KeyValue { Key = "btnAdd", Value = "add", Description = "新增" });
            list.Add(new KeyValue { Key = "btnEdit", Value = "edit", Description = "修改" });
            list.Add(new KeyValue { Key = "btnDelete", Value = "delete", Description = "删除" });
            return list;
        }
        #endregion 

        private string GetModulePrefix(BaseConfigModel baseConfigModel)
        {
            return baseConfigModel.OutputConfig.OutputModule.Replace("Manage", string.Empty).ToLower();
        }

        private string GetBaseEntity(DataTable dt)
        {
            string entity = string.Empty;
            var columnList = dt.AsEnumerable().Select(p => p["TableColumn"].ParseToString()).ToList();

            bool id = columnList.Where(p => p == "Id").Any();
            bool baseIsDelete = columnList.Where(p => p == "BaseIsDelete").Any();
            bool baseVersion = columnList.Where(p => p == "BaseVersion").Any();
            bool baseModifyTime = columnList.Where(p => p == "BaseModifyTime").Any();
            bool baseModifierId = columnList.Where(p => p == "BaseModifierId").Any();
            bool baseCreateTime = columnList.Where(p => p == "BaseCreateTime").Any();
            bool baseCreatorId = columnList.Where(p => p == "BaseCreatorId").Any();

            if (!id)
            {
                throw new Exception("数据库表必须有主键Id字段");
            }
            if (baseIsDelete && baseVersion && baseModifyTime && baseModifierId && baseCreateTime && baseCreatorId)
            {
                entity = "BaseExtensionEntity";
            }
            else if (baseVersion && baseModifyTime && baseModifierId && baseCreateTime && baseCreatorId)
            {
                entity = "BaseModifyEntity";
            }
            else if (baseCreateTime && baseCreatorId)
            {
                entity = "BaseCreateEntity";
            }
            else
            {
                entity = "BaseEntity";
            }
            return entity;
        }

        private string GetSaveFormCreate(string entity)
        {
            string line = string.Empty;
            switch (entity)
            {
                case "BaseEntity":
                    line = "entity.Create();";
                    break;

                case "BaseCreateEntity":
                    line = "await entity.Create();";
                    break;

                case "BaseModifyEntity":
                    line = "await entity.Create();";
                    break;

                case "BaseExtensionEntity":
                    line = "await entity.Create();";
                    break;
            }
            return line;
        }

        private string GetSaveFormModify(string entity)
        {
            string line = string.Empty;
            switch (entity)
            {
                case "BaseEntity":
                    line = string.Empty;
                    break;

                case "BaseCreateEntity":
                    line = string.Empty;
                    break;

                case "BaseModifyEntity":
                    line = "await entity.Modify();";
                    break;

                case "BaseExtensionEntity":
                    line = "await entity.Modify();";
                    break;
            }
            return line;
        }
        #endregion
    }
}
