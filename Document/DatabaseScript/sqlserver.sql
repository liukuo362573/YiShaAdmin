/* 执行脚本前，请先选择数据库，脚本会先删除表，然后再创建表，请谨慎执行！！！ */;
/* use [yisha_admin] */

IF OBJECT_ID('[dbo].[sys_area]', 'U') IS NOT NULL DROP TABLE [dbo].[sys_area]; 
CREATE TABLE [dbo].[sys_area](
	[id]					[bigint]		 NOT NULL,
	[base_is_delete]		[int]			 NOT NULL,
	[base_create_time]		[datetime]		 NOT NULL,
	[base_modify_time]		[datetime]		 NOT NULL,
	[base_creator_id]		[bigint]		 NOT NULL,
	[base_modifier_id]		[bigint]		 NOT NULL,
	[base_version]			[int]			 NOT NULL,
	[area_code]				[varchar](6)	 NOT NULL,
	[parent_area_code]		[varchar](6)	 NOT NULL,
	[area_name]				[varchar](50)	 NOT NULL,
	[zip_code]				[varchar](50)	 NOT NULL,
	[area_level]			[int]			 NOT NULL,
 CONSTRAINT [PK_sys_area] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'主键',                       @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_area', @level2type=N'COLUMN',@level2name=N'id'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'删除标记(0正常 1删除)',       @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_area', @level2type=N'COLUMN',@level2name=N'base_is_delete'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间',                   @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_area', @level2type=N'COLUMN',@level2name=N'base_create_time'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改时间',                   @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_area', @level2type=N'COLUMN',@level2name=N'base_modify_time'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建人',					  @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_area', @level2type=N'COLUMN',@level2name=N'base_creator_id'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改人',                     @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_area', @level2type=N'COLUMN',@level2name=N'base_modifier_id'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'数据版本(每次更新+1)',        @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_area', @level2type=N'COLUMN',@level2name=N'base_version'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'地区编码',					  @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_area', @level2type=N'COLUMN',@level2name=N'area_code'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'父地区编码',			      @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_area', @level2type=N'COLUMN',@level2name=N'parent_area_code'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'地区名称',				      @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_area', @level2type=N'COLUMN',@level2name=N'area_name'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'邮政编码',                   @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_area', @level2type=N'COLUMN',@level2name=N'zip_code'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'地区层级(1省份 2城市 3区县)', @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_area', @level2type=N'COLUMN',@level2name=N'area_level'

IF OBJECT_ID('[dbo].[sys_auto_job]', 'U') IS NOT NULL DROP TABLE [dbo].sys_auto_job; 
CREATE TABLE [dbo].[sys_auto_job](
	[id]                    [bigint]	     NOT NULL,
	[base_is_delete]		[int]			 NOT NULL,
	[base_create_time]		[datetime]		 NOT NULL,
	[base_modify_time]		[datetime]		 NOT NULL,
	[base_creator_id]		[bigint]		 NOT NULL,
	[base_modifier_id]		[bigint]		 NOT NULL,
	[base_version]		    [int]			 NOT NULL,
	[job_group_name]		[varchar](50)	 NOT NULL,
	[job_name]				[varchar](50)	 NOT NULL,
	[job_status]			[int]			 NOT NULL,
	[cron_expression]		[varchar](50)	 NOT NULL,
	[start_time]			[datetime]		 NOT NULL,
	[end_time]				[datetime]		 NOT NULL,
	[next_start_time]		[datetime]		 NOT NULL,
	[remark]				[varchar](500)   NOT NULL,
 CONSTRAINT [PK_sys_auto_job] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'任务组名称',             @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_auto_job', @level2type=N'COLUMN',@level2name=N'job_group_name'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'任务名称',               @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_auto_job', @level2type=N'COLUMN',@level2name=N'job_name'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'任务状态(0禁用 1启用)',   @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_auto_job', @level2type=N'COLUMN',@level2name=N'job_status'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'cron表达式',             @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_auto_job', @level2type=N'COLUMN',@level2name=N'cron_expression'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'运行开始时间',           @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_auto_job', @level2type=N'COLUMN',@level2name=N'start_time'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'运行结束时间',           @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_auto_job', @level2type=N'COLUMN',@level2name=N'end_time'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'下次执行时间',           @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_auto_job', @level2type=N'COLUMN',@level2name=N'next_start_time'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'备注',                  @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_auto_job', @level2type=N'COLUMN',@level2name=N'remark'

IF OBJECT_ID('[dbo].[sys_auto_job_log]', 'U') IS NOT NULL DROP TABLE [dbo].[sys_auto_job_log]; 
CREATE TABLE [dbo].[sys_auto_job_log](
	[id]					[bigint]         NOT NULL,
	[base_create_time]		[datetime]		 NOT NULL,
	[base_creator_id]		[bigint]         NOT NULL,
	[job_group_name]		[varchar](50)    NOT NULL,
	[job_name]				[varchar](50)    NOT NULL,
	[log_status]			[int]			 NOT NULL,
	[remark]				[varchar](500)   NOT NULL,
 CONSTRAINT [PK_sys_auto_job_log] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'任务组名称',             @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_auto_job_log', @level2type=N'COLUMN',@level2name=N'job_group_name'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'任务名称',               @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_auto_job_log', @level2type=N'COLUMN',@level2name=N'job_name'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'执行状态(0失败 1成功)',  @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_auto_job_log', @level2type=N'COLUMN',@level2name=N'log_status'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'备注',                  @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_auto_job_log', @level2type=N'COLUMN',@level2name=N'remark'

IF OBJECT_ID('[dbo].[sys_data_dict]', 'U') IS NOT NULL DROP TABLE [dbo].[sys_data_dict]; 
CREATE TABLE [dbo].[sys_data_dict](
	[id]					[bigint]			NOT NULL,
	[base_is_delete]		[int]				NOT NULL,
	[base_create_time]		[datetime]			NOT NULL,
	[base_modify_time]		[datetime]			NOT NULL,
	[base_creator_id]		[bigint]			NOT NULL,
	[base_modifier_id]		[bigint]			NOT NULL,
	[base_version]			[int]				NOT NULL,
	[dict_type]				[varchar](50)		NOT NULL,
	[dict_sort]				[int]				NOT NULL,
	[remark]				[varchar](50)		NOT NULL,
 CONSTRAINT [PK_sys_data_dict] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'字典类型',  @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_data_dict', @level2type=N'COLUMN',@level2name=N'dict_type'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'字典排序',  @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_data_dict', @level2type=N'COLUMN',@level2name=N'dict_sort'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'备注',      @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_data_dict', @level2type=N'COLUMN',@level2name=N'remark'

IF OBJECT_ID('[dbo].[sys_data_dict_detail]', 'U') IS NOT NULL DROP TABLE [dbo].[sys_data_dict_detail]; 
CREATE TABLE [dbo].[sys_data_dict_detail](
	[id]				    [bigint]			NOT NULL,
	[base_is_delete]		[int]				NOT NULL,
	[base_create_time]		[datetime]			NOT NULL,
	[base_modify_time]		[datetime]			NOT NULL,
	[base_creator_id]		[bigint]			NOT NULL,
	[base_modifier_id]		[bigint]			NOT NULL,
	[base_version]			[int]				NOT NULL,
	[dict_type]				[varchar](50)		NOT NULL,
	[dict_sort]				[int]				NOT NULL,
	[dict_key]				[int]				NOT NULL,
	[dict_value]			[varchar](50)		NOT NULL,
	[list_class]			[varchar](50)		NOT NULL,
	[dict_status]			[int]				NOT NULL,
	[is_default]			[int]				NOT NULL,
	[remark]				[varchar](50)		NOT NULL,
 CONSTRAINT [PK_sys_data_dict_detail] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'字典类型(外键)',          @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_data_dict_detail', @level2type=N'COLUMN',@level2name=N'dict_type'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'字典排序',                @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_data_dict_detail', @level2type=N'COLUMN',@level2name=N'dict_sort'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'字典键(一般从1开始)',     @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_data_dict_detail', @level2type=N'COLUMN',@level2name=N'dict_key'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'字典值',			       @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_data_dict_detail', @level2type=N'COLUMN',@level2name=N'dict_value'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'显示样式(default primary success info warning danger)',  @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_data_dict_detail', @level2type=N'COLUMN',@level2name=N'list_class'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'字典状态(0禁用 1启用)',   @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_data_dict_detail', @level2type=N'COLUMN',@level2name=N'dict_status'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'默认选中(0不是 1是)',     @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_data_dict_detail', @level2type=N'COLUMN',@level2name=N'is_default'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'备注',                   @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_data_dict_detail', @level2type=N'COLUMN',@level2name=N'remark'

IF OBJECT_ID('[dbo].[sys_department]', 'U') IS NOT NULL DROP TABLE [dbo].[sys_department]; 
CREATE TABLE [dbo].[sys_department](
	[id]				    [bigint]			NOT NULL,
	[base_is_delete]		[int]				NOT NULL,
	[base_create_time]		[datetime]			NOT NULL,
	[base_modify_time]		[datetime]			NOT NULL,
	[base_creator_id]		[bigint]			NOT NULL,
	[base_modifier_id]		[bigint]			NOT NULL,
	[base_version]			[int]				NOT NULL,
	[parent_id]				[bigint]			NOT NULL,
	[department_name]		[varchar](50)		NOT NULL,
	[telephone]				[varchar](50)		NOT NULL,
	[fax]					[varchar](50)		NOT NULL,
	[email]					[varchar](50)		NOT NULL,
	[principal_id]			[bigint]			NOT NULL,
	[department_sort]		[int]				NOT NULL,
	[remark]				[varchar](500)		NOT NULL,
 CONSTRAINT [PK_sys_department] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'父部门Id(0表示是根部门)',   @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_department', @level2type=N'COLUMN',@level2name=N'parent_id'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'部门名称',                 @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_department', @level2type=N'COLUMN',@level2name=N'department_name'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'部门电话',                 @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_department', @level2type=N'COLUMN',@level2name=N'telephone'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'部门传真',                 @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_department', @level2type=N'COLUMN',@level2name=N'fax'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'部门Email',				@level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_department', @level2type=N'COLUMN',@level2name=N'email'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'部门负责人Id',             @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_department', @level2type=N'COLUMN',@level2name=N'principal_id'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'部门排序',                 @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_department', @level2type=N'COLUMN',@level2name=N'department_sort'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'备注',                     @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_department', @level2type=N'COLUMN',@level2name=N'remark'

IF OBJECT_ID('[dbo].[sys_log_api]', 'U') IS NOT NULL DROP TABLE [dbo].[sys_log_api]; 
CREATE TABLE [dbo].[sys_log_api](
	[id]                    [bigint]			NOT NULL,
	[base_create_time]		[datetime]			NOT NULL,
	[base_creator_id]		[bigint]			NOT NULL,
	[log_status]			[int]				NOT NULL,
	[remark]				[varchar](50)		NOT NULL,
	[execute_url]			[varchar](100)		NOT NULL,
	[execute_param]			[varchar](8000)		NOT NULL,
	[execute_result]		[varchar](8000)		NOT NULL,
	[execute_time]			[int]				NOT NULL,
 CONSTRAINT [PK_sys_log_api] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'执行状态(0失败 1成功)',  @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_log_api', @level2type=N'COLUMN',@level2name=N'log_status'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'备注',                  @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_log_api', @level2type=N'COLUMN',@level2name=N'remark'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'接口地址',				 @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_log_api', @level2type=N'COLUMN',@level2name=N'execute_url'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'请求参数',              @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_log_api', @level2type=N'COLUMN',@level2name=N'execute_param'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'请求结果',              @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_log_api', @level2type=N'COLUMN',@level2name=N'execute_result'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'执行时间',              @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_log_api', @level2type=N'COLUMN',@level2name=N'execute_time'

IF OBJECT_ID('[dbo].[sys_log_login]', 'U') IS NOT NULL DROP TABLE [dbo].[sys_log_login]; 
CREATE TABLE [dbo].[sys_log_login](
	[id]					[bigint]			NOT NULL,
	[base_create_time]		[datetime]			NOT NULL,
	[base_creator_id]		[bigint]			NOT NULL,
	[log_status]			[int]				NOT NULL,
	[ip_address]			[varchar](20)		NOT NULL,
	[ip_location]			[varchar](50)		NOT NULL,
	[browser]				[varchar](50)		NOT NULL,
	[os]					[varchar](50)		NOT NULL,
	[remark]			    [varchar](50)		NOT NULL,
	[extra_remark]			[varchar](500)		NOT NULL,
 CONSTRAINT [PK_sys_log_login] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'执行状态(0失败 1成功)',  @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_log_login', @level2type=N'COLUMN',@level2name=N'log_status'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ip地址',                @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_log_login', @level2type=N'COLUMN',@level2name=N'ip_address'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ip位置',				  @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_log_login', @level2type=N'COLUMN',@level2name=N'ip_location'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'浏览器',                 @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_log_login', @level2type=N'COLUMN',@level2name=N'browser'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'操作系统',               @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_log_login', @level2type=N'COLUMN',@level2name=N'os'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'备注',                  @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_log_login', @level2type=N'COLUMN',@level2name=N'remark'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'额外备注',               @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_log_login', @level2type=N'COLUMN',@level2name=N'extra_remark'

IF OBJECT_ID('[dbo].[sys_log_operate]', 'U') IS NOT NULL DROP TABLE [dbo].[sys_log_operate]; 
CREATE TABLE [dbo].[sys_log_operate](
	[id]					[bigint]			NOT NULL,
	[base_create_time]		[datetime]			NOT NULL,
	[base_creator_id]		[bigint]			NOT NULL,
	[log_status]			[int]				NOT NULL,
	[ip_address]			[varchar](20)		NOT NULL,
	[ip_location]			[varchar](50)		NOT NULL,
	[remark]				[varchar](50)		NOT NULL,
	[log_type]				[varchar](50)		NOT NULL,
	[business_type]			[varchar](50)		NOT NULL,
	[execute_url]			[varchar](100)		NOT NULL,
	[execute_param]			[varchar](8000)		NOT NULL,
	[execute_result]		[varchar](8000)		NOT NULL,
	[execute_time]			[int]				NOT NULL,
 CONSTRAINT [PK_sys_log_operate] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'执行状态(0失败 1成功)',  @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_log_operate', @level2type=N'COLUMN',@level2name=N'log_status'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ip地址',                @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_log_operate', @level2type=N'COLUMN',@level2name=N'ip_address'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ip位置',                @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_log_operate', @level2type=N'COLUMN',@level2name=N'ip_location'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'备注',                  @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_log_operate', @level2type=N'COLUMN',@level2name=N'remark'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'日志类型(暂未用到)',     @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_log_operate', @level2type=N'COLUMN',@level2name=N'log_type'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'业务类型(暂未用到)',     @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_log_operate', @level2type=N'COLUMN',@level2name=N'business_type'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'页面地址',              @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_log_operate', @level2type=N'COLUMN',@level2name=N'execute_url'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'请求参数',              @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_log_operate', @level2type=N'COLUMN',@level2name=N'execute_param'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'请求结果',              @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_log_operate', @level2type=N'COLUMN',@level2name=N'execute_result'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'执行时间',              @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_log_operate', @level2type=N'COLUMN',@level2name=N'execute_time'

IF OBJECT_ID('[dbo].[sys_menu]', 'U') IS NOT NULL DROP TABLE [dbo].[sys_menu]; 
CREATE TABLE [dbo].[sys_menu](
	[id]				    [bigint]			NOT NULL,
	[base_is_delete]		[int]				NOT NULL,
	[base_create_time]		[datetime]			NOT NULL,
	[base_modify_time]		[datetime]			NOT NULL,
	[base_creator_id]		[bigint]			NOT NULL,
	[base_modifier_id]		[bigint]			NOT NULL,
	[base_version]			[int]				NOT NULL,
	[parent_id]				[bigint]			NOT NULL,
	[menu_name]				[varchar](50)		NOT NULL,
	[menu_icon]				[varchar](50)		NOT NULL,
	[menu_url]				[varchar](100)		NOT NULL,
	[menu_target]			[varchar](50)		NOT NULL,
	[menu_sort]				[int]				NOT NULL,
	[menu_type]				[int]				NOT NULL,
	[menu_status]			[int]				NOT NULL,
	[authorize]				[varchar](50)		NOT NULL,
	[remark]				[varchar](50)		NOT NULL,
 CONSTRAINT [PK_sys_menu] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'父菜单Id(0表示是根菜单)',     @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_menu', @level2type=N'COLUMN',@level2name=N'parent_id'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'菜单名称',                   @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_menu', @level2type=N'COLUMN',@level2name=N'menu_name'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'菜单图标',                   @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_menu', @level2type=N'COLUMN',@level2name=N'menu_icon'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'菜单Url',                   @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_menu', @level2type=N'COLUMN',@level2name=N'menu_url'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'链接打开方式',               @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_menu', @level2type=N'COLUMN',@level2name=N'menu_target'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'菜单排序',                  @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_menu', @level2type=N'COLUMN',@level2name=N'menu_sort'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'菜单类型(1目录 2页面 3按钮)',@level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_menu', @level2type=N'COLUMN',@level2name=N'menu_type'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'菜单状态(0禁用 1启用)',      @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_menu', @level2type=N'COLUMN',@level2name=N'menu_status'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'菜单权限标识',              @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_menu', @level2type=N'COLUMN',@level2name=N'authorize'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'备注',                     @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_menu', @level2type=N'COLUMN',@level2name=N'remark'

IF OBJECT_ID('[dbo].[sys_menu_authorize]', 'U') IS NOT NULL DROP TABLE [dbo].[sys_menu_authorize]; 
CREATE TABLE [dbo].[sys_menu_authorize](
	[id]				    [bigint]			NOT NULL,
	[base_create_time]		[datetime]			NOT NULL,
	[base_creator_id]		[bigint]			NOT NULL,
	[menu_id]				[bigint]			NOT NULL,
	[authorize_id]			[bigint]			NOT NULL,
	[authorize_type]		[int]				NOT NULL,
 CONSTRAINT [PK_sys_menu_authorize] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'菜单Id',                  @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_menu_authorize', @level2type=N'COLUMN',@level2name=N'menu_id'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'授权Id(角色Id或者用户Id)', @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_menu_authorize', @level2type=N'COLUMN',@level2name=N'authorize_id'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'授权类型(1角色 2用户)',    @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_menu_authorize', @level2type=N'COLUMN',@level2name=N'authorize_type'

IF OBJECT_ID('[dbo].[sys_news]', 'U') IS NOT NULL DROP TABLE [dbo].[sys_news]; 
CREATE TABLE [dbo].[sys_news](
	[id]					[bigint]			NOT NULL,
	[base_is_delete]		[int]				NOT NULL,
	[base_create_time]		[datetime]			NOT NULL,
	[base_modify_time]		[datetime]			NOT NULL,
	[base_creator_id]		[bigint]			NOT NULL,
	[base_modifier_id]		[bigint]			NOT NULL,
	[base_version]			[int]				NOT NULL,
	[news_title]			[varchar](300)		NOT NULL,
	[news_content]			[text]				NOT NULL,
	[news_tag]				[varchar](200)		NOT NULL,
	[thumb_image]			[varchar](200)		NOT NULL,
	[news_author]			[varchar](50)		NOT NULL,
	[news_sort]				[int]				NOT NULL,
	[news_date]				[datetime]			NOT NULL,
	[news_type]				[int]				NOT NULL,
	[view_times]			[int]				NOT NULL,
	[province_id]			[bigint]			NOT NULL,
	[city_id]				[bigint]			NOT NULL,
	[county_id]				[bigint]			NOT NULL,
 CONSTRAINT [PK_sys_news] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'新闻标题',                     @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_news', @level2type=N'COLUMN',@level2name=N'news_title'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'新闻内容',                     @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_news', @level2type=N'COLUMN',@level2name=N'news_content'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'新闻标签',                     @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_news', @level2type=N'COLUMN',@level2name=N'news_tag'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'省份Id',                       @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_news', @level2type=N'COLUMN',@level2name=N'province_id'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'城市Id',                       @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_news', @level2type=N'COLUMN',@level2name=N'city_id'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'区县Id',                       @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_news', @level2type=N'COLUMN',@level2name=N'county_id'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'缩略图',                       @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_news', @level2type=N'COLUMN',@level2name=N'thumb_image'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'新闻排序',                     @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_news', @level2type=N'COLUMN',@level2name=N'news_sort'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'发布者',                       @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_news', @level2type=N'COLUMN',@level2name=N'news_author'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'发布时间',                     @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_news', @level2type=N'COLUMN',@level2name=N'news_date'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'新闻类型(1产品案例 2行业新闻)', @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_news', @level2type=N'COLUMN',@level2name=N'news_type'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'查看次数',                     @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_news', @level2type=N'COLUMN',@level2name=N'view_times'

IF OBJECT_ID('[dbo].[sys_position]', 'U') IS NOT NULL DROP TABLE [dbo].[sys_position]; 
CREATE TABLE [dbo].[sys_position](
	[id]					[bigint]			NOT NULL,
	[base_is_delete]		[int]				NOT NULL,
	[base_create_time]		[datetime]			NOT NULL,
	[base_modify_time]		[datetime]			NOT NULL,
	[base_creator_id]		[bigint]			NOT NULL,
	[base_modifier_id]		[bigint]			NOT NULL,
	[base_version]			[int]				NOT NULL,
	[position_name]			[varchar](50)		NOT NULL,
	[position_sort]			[int]				NOT NULL,
	[position_status]		[int]				NOT NULL,
	[remark]				[varchar](50)		NOT NULL,
 CONSTRAINT [PK_sys_position] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'职位名称',             @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_position', @level2type=N'COLUMN',@level2name=N'position_name'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'职位排序',             @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_position', @level2type=N'COLUMN',@level2name=N'position_sort'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'职位状态(0禁用 1启用)', @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_position', @level2type=N'COLUMN',@level2name=N'position_status'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'备注',                 @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_position', @level2type=N'COLUMN',@level2name=N'remark'

IF OBJECT_ID('[dbo].[sys_role]', 'U') IS NOT NULL DROP TABLE [dbo].[sys_role]; 
CREATE TABLE [dbo].[sys_role](
	[id]					[bigint]			NOT NULL,
	[base_is_delete]		[int]				NOT NULL,
	[base_create_time]		[datetime]			NOT NULL,
	[base_modify_time]		[datetime]			NOT NULL,
	[base_creator_id]		[bigint]			NOT NULL,
	[base_modifier_id]		[bigint]			NOT NULL,
	[base_version]			[int]				NOT NULL,
	[role_name]				[varchar](50)		NOT NULL,
	[role_sort]				[int]				NOT NULL,
	[role_status]			[int]				NOT NULL,
	[remark]				[varchar](50)		NOT NULL,
 CONSTRAINT [PK_sys_role] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'角色名称',              @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_role', @level2type=N'COLUMN',@level2name=N'role_name'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'角色排序',              @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_role', @level2type=N'COLUMN',@level2name=N'role_sort'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'角色状态(0禁用 1启用)', @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_role', @level2type=N'COLUMN',@level2name=N'role_status'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'备注',                 @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_role', @level2type=N'COLUMN',@level2name=N'remark'

IF OBJECT_ID('[dbo].[sys_user]', 'U') IS NOT NULL DROP TABLE [dbo].[sys_user]; 
CREATE TABLE [dbo].[sys_user](
	[id]					[bigint]			NOT NULL,
	[base_is_delete]		[int]				NOT NULL,
	[base_create_time]		[datetime]			NOT NULL,
	[base_modify_time]		[datetime]			NOT NULL,
	[base_creator_id]		[bigint]			NOT NULL,
	[base_modifier_id]		[bigint]			NOT NULL,
	[base_version]			[int]				NOT NULL,
	[user_name]				[varchar](20)		NOT NULL,
	[password]				[varchar](32)		NOT NULL,
	[salt]					[varchar](5)		NOT NULL,
	[real_name]				[varchar](20)		NOT NULL,
	[department_id]			[bigint]			NOT NULL,
	[gender]				[int]				NOT NULL,
	[birthday]				[varchar](10)		NOT NULL,
	[portrait]				[varchar](200)		NOT NULL,
	[email]					[varchar](50)		NOT NULL,
	[mobile]				[varchar](11)		NOT NULL,
	[qq]					[varchar](20)		NOT NULL,
	[wechat]				[varchar](20)		NOT NULL,
	[login_count]			[int]				NOT NULL,
	[user_status]			[int]				NOT NULL,
	[is_system]				[int]				NOT NULL,
	[is_online]				[int]				NOT NULL,
	[first_visit]			[datetime]			NOT NULL,
	[previous_visit]		[datetime]			NOT NULL,
	[last_visit]			[datetime]			NOT NULL,
	[remark]				[varchar](200)		NOT NULL,
	[web_token]				[varchar](32)		NOT NULL,
	[api_token]				[varchar](32)		NOT NULL,
 CONSTRAINT [PK_sys_user] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户名',									@level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_user', @level2type=N'COLUMN',@level2name=N'user_name'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'密码',										@level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_user', @level2type=N'COLUMN',@level2name=N'password'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'密码盐值',									@level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_user', @level2type=N'COLUMN',@level2name=N'salt'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'姓名',									    @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_user', @level2type=N'COLUMN',@level2name=N'real_name'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'所属部门Id',								@level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_user', @level2type=N'COLUMN',@level2name=N'department_id'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'性别(0未知 1男 2女)',						@level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_user', @level2type=N'COLUMN',@level2name=N'gender'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'出生日期',									@level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_user', @level2type=N'COLUMN',@level2name=N'birthday'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'头像',										@level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_user', @level2type=N'COLUMN',@level2name=N'portrait'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Email',									@level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_user', @level2type=N'COLUMN',@level2name=N'email'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'手机',										@level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_user', @level2type=N'COLUMN',@level2name=N'mobile'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'QQ',                                      @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_user', @level2type=N'COLUMN',@level2name=N'qq'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'微信',                                     @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_user', @level2type=N'COLUMN',@level2name=N'wechat'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登录次数',                                 @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_user', @level2type=N'COLUMN',@level2name=N'login_count'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户状态(0禁用 1启用)',                     @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_user', @level2type=N'COLUMN',@level2name=N'user_status'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'系统用户(0不是 1是[系统用户拥有所有的权限])', @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_user', @level2type=N'COLUMN',@level2name=N'is_system'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'在线(0不是 1是)',							 @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_user', @level2type=N'COLUMN',@level2name=N'is_online'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'首次登录时间',								 @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_user', @level2type=N'COLUMN',@level2name=N'first_visit'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'上一次登录时间',						     @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_user', @level2type=N'COLUMN',@level2name=N'previous_visit'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最后一次登录时间',                          @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_user', @level2type=N'COLUMN',@level2name=N'last_visit'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'备注',                                     @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_user', @level2type=N'COLUMN',@level2name=N'remark'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'后台Token',                                @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_user', @level2type=N'COLUMN',@level2name=N'web_token'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ApiToken',                                @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_user', @level2type=N'COLUMN',@level2name=N'api_token'

IF OBJECT_ID('[dbo].[sys_user_belong]', 'U') IS NOT NULL DROP TABLE [dbo].[sys_user_belong]; 
CREATE TABLE [dbo].[sys_user_belong](
	[id]					[bigint]			NOT NULL,
	[base_create_time]		[datetime]			NOT NULL,
	[base_creator_id]		[bigint]			NOT NULL,
	[user_id]				[bigint]			NOT NULL,
	[belong_id]				[bigint]			NOT NULL,
	[belong_type]			[int]				NOT NULL,
 CONSTRAINT [PK_sys_user_belong] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户Id',               @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_user_belong', @level2type=N'COLUMN',@level2name=N'user_id'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'职位Id或者角色Id',      @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_user_belong', @level2type=N'COLUMN',@level2name=N'belong_id'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'所属类型(1职位 2角色)', @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'sys_user_belong', @level2type=N'COLUMN',@level2name=N'belong_type'
