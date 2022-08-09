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

        /// <summary>
        /// 文件配置
        /// </summary>
        public FileConfigModel FileConfig { get; set; }

        /// <summary>
        /// 输出配置
        /// </summary>
        public OutputConfigModel OutputConfig { get; set; }

        /// <summary>
        /// 分页配置
        /// </summary>
        public PageIndexModel PageIndex { get; set; }

        /// <summary>
        /// 分页表单
        /// </summary>
        public PageFormModel PageForm { get; set; }
    }

    /// <summary>
    /// 文件配置
    /// </summary>
    public class FileConfigModel
    {
        /// <summary>
        /// 类前缀
        /// </summary>
        public string ClassPrefix { get; set; }

        /// <summary>
        /// 类描述
        /// </summary>
        public string ClassDescription { get; set; }

        /// <summary>
        /// 创建名称
        /// </summary>
        public string CreateName { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>
        public string CreateDate { get; set; }

        /// <summary>
        /// 实体名称
        /// </summary>
        public string EntityName { get; set; }

        /// <summary>
        /// 实体映射名
        /// </summary>
        public string EntityMapName { get; set; }

        /// <summary>
        /// 实体参数名称
        /// </summary>
        public string EntityParamName { get; set; }

        /// <summary>
        /// 业务名称
        /// </summary>
        public string BusinessName { get; set; }

        /// <summary>
        /// 服务名称
        /// </summary>
        public string ServiceName { get; set; }

        /// <summary>
        /// 控制器名称
        /// </summary>
        public string ControllerName { get; set; }

        /// <summary>
        /// 页面索引名称
        /// </summary>
        public string PageIndexName { get; set; }

        /// <summary>
        /// 页面表单名称
        /// </summary>
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

        /// <summary>
        /// 列列表
        /// </summary>
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

        /// <summary>
        /// 字段列表
        /// </summary>
        public List<string> FieldList { get; set; }
    }
}
