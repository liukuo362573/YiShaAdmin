namespace YiSha.CodeGenerator.Model
{
    /// <summary>
    /// 根配置
    /// </summary>
    public class BaseConfigModel
    {
        /// <summary>
        /// 数据库表名sys_menu
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// 表名首字母大写SysMenu
        /// </summary>
        public string TableNameUpper { get; set; }

        public FileConfigModel FileConfig { get; set; }

        public OutputConfigModel OutputConfig { get; set; }

        public PageIndexModel PageIndex { get; set; }

        public PageFormModel PageForm { get; set; }
    }

    /// <summary>
    /// 文件配置
    /// </summary>
    public class FileConfigModel
    {
        public string ClassPrefix { get; set; }

        public string ClassDescription { get; set; }

        public string CreateName { get; set; }

        public string CreateDate { get; set; }
        
        public string EntityName { get; set; }

        public string EntityMapName { get; set; }

        public string EntityParamName { get; set; }

        public string BusinessName { get; set; }

        public string ServiceName { get; set; }

        public string ControllerName { get; set; }

        public string PageIndexName { get; set; }

        public string PageFormName { get; set; }
    }

    /// <summary>
    /// 输出配置
    /// </summary>
    public class OutputConfigModel
    {
        /// <summary>
        /// 模块列表
        /// </summary>
        public List<string> ModuleList { get; set; }

        /// <summary>
        /// 输出模块路径
        /// </summary>
        public string OutputModule { get; set; }

        /// <summary>
        /// 输出 Entity 路径
        /// </summary>
        public string OutputEntity { get; set; }

        /// <summary>
        /// 输出 Model 路径
        /// </summary>
        public string OutputModel { get; set; }

        /// <summary>
        /// 输出 Business 路径
        /// </summary>
        public string OutputBusiness { get; set; }

        /// <summary>
        /// 输出 Web 路径
        /// </summary>
        public string OutputWeb { get; set; }
    }

    /// <summary>
    /// 分页配置
    /// </summary>
    public class PageIndexModel
    {
        /// <summary>
        /// 是否有搜索
        /// </summary>
        public int IsSearch { get; set; }

        /// <summary>
        /// 工具栏按钮（新增 修改 删除）
        /// </summary>
        public List<string> ButtonList { get; set; }

        /// <summary>
        /// 是否有分页
        /// </summary>
        public int IsPagination { get; set; }

        public List<string> ColumnList { get; set; }
    }

    /// <summary>
    /// 分页表单
    /// </summary>
    public class PageFormModel
    {
        /// <summary>
        /// 1表示显示成1列，2表示显示成2列
        /// </summary>
        public int ShowMode { get; set; }

        public List<string> FieldList { get; set; }
    }
}
