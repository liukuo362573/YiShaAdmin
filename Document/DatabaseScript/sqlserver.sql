/* 执行脚本前，请先选择数据库，脚本会先删除表，然后再创建表，请谨慎执行！！！ */;
/* use [YiShaAdmin] */

IF OBJECT_ID('[dbo].[SysArea]', 'U') IS NOT NULL DROP TABLE [dbo].[SysArea]; 
CREATE TABLE [dbo].[SysArea](
	[Id]					[bigint]		 NOT NULL,
	[BaseIsDelete]			[int]			 NOT NULL,
	[BaseCreateTime]		[datetime]		 NOT NULL,
	[BaseModifyTime]		[datetime]		 NOT NULL,
	[BaseCreatorId]			[bigint]		 NOT NULL,
	[BaseModifierId]		[bigint]		 NOT NULL,
	[BaseVersion]			[int]			 NOT NULL,
	[AreaCode]				[varchar](6)	 NOT NULL,
	[ParentAreaCode]		[varchar](6)	 NOT NULL,
	[AreaName]				[nvarchar](50)	 NOT NULL,
	[ZipCode]				[varchar](50)	 NOT NULL,
	[AreaLevel]				[int]			 NOT NULL,
 CONSTRAINT [PK_SysArea] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'主键',                       @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysArea', @level2type=N'COLUMN',@level2name=N'Id'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'删除标记(0正常 1删除)',       @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysArea', @level2type=N'COLUMN',@level2name=N'BaseIsDelete'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间',                   @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysArea', @level2type=N'COLUMN',@level2name=N'BaseCreateTime'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改时间',                   @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysArea', @level2type=N'COLUMN',@level2name=N'BaseModifyTime'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建人',					  @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysArea', @level2type=N'COLUMN',@level2name=N'BaseCreatorId'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改人',                     @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysArea', @level2type=N'COLUMN',@level2name=N'BaseModifierId'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'数据版本(每次更新+1)',        @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysArea', @level2type=N'COLUMN',@level2name=N'BaseVersion'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'地区编码',					  @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysArea', @level2type=N'COLUMN',@level2name=N'AreaCode'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'父地区编码',			      @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysArea', @level2type=N'COLUMN',@level2name=N'ParentAreaCode'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'地区名称',				      @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysArea', @level2type=N'COLUMN',@level2name=N'AreaName'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'邮政编码',                   @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysArea', @level2type=N'COLUMN',@level2name=N'ZipCode'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'地区层级(1省份 2城市 3区县)', @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysArea', @level2type=N'COLUMN',@level2name=N'AreaLevel'

IF OBJECT_ID('[dbo].[SysAutoJob]', 'U') IS NOT NULL DROP TABLE [dbo].SysAutoJob; 
CREATE TABLE [dbo].[SysAutoJob](
	[Id]                    [bigint]	     NOT NULL,
	[BaseIsDelete]		    [int]			 NOT NULL,
	[BaseCreateTime]		[datetime]		 NOT NULL,
	[BaseModifyTime]		[datetime]		 NOT NULL,
	[BaseCreatorId]		    [bigint]		 NOT NULL,
	[BaseModifierId]		[bigint]		 NOT NULL,
	[BaseVersion]		    [int]			 NOT NULL,
	[JobGroupName]		    [nvarchar](50)	 NOT NULL,
	[JobName]				[nvarchar](50)	 NOT NULL,
	[JobStatus]				[int]			 NOT NULL,
	[CronExpression]		[varchar](50)	 NOT NULL,
	[StartTime]				[datetime]		 NOT NULL,
	[EndTime]				[datetime]		 NOT NULL,
	[NextStartTime]			[datetime]		 NOT NULL,
	[Remark]				[nvarchar](500)   NOT NULL,
 CONSTRAINT [PK_SysAutoJob] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'任务组名称',             @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysAutoJob', @level2type=N'COLUMN',@level2name=N'JobGroupName'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'任务名称',               @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysAutoJob', @level2type=N'COLUMN',@level2name=N'JobName'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'任务状态(0禁用 1启用)',   @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysAutoJob', @level2type=N'COLUMN',@level2name=N'JobStatus'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'cron表达式',             @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysAutoJob', @level2type=N'COLUMN',@level2name=N'CronExpression'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'运行开始时间',           @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysAutoJob', @level2type=N'COLUMN',@level2name=N'StartTime'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'运行结束时间',           @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysAutoJob', @level2type=N'COLUMN',@level2name=N'EndTime'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'下次执行时间',           @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysAutoJob', @level2type=N'COLUMN',@level2name=N'NextStartTime'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'备注',                  @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysAutoJob', @level2type=N'COLUMN',@level2name=N'Remark'

IF OBJECT_ID('[dbo].[SysAutoJobLog]', 'U') IS NOT NULL DROP TABLE [dbo].[SysAutoJobLog]; 
CREATE TABLE [dbo].[SysAutoJobLog](
	[Id]					[bigint]         NOT NULL,
	[BaseCreateTime]		[datetime]		 NOT NULL,
	[BaseCreatorId]			[bigint]         NOT NULL,
	[JobGroupName]			[nvarchar](50)    NOT NULL,
	[JobName]				[nvarchar](50)    NOT NULL,
	[LogStatus]				[int]			 NOT NULL,
	[Remark]				[nvarchar](500)   NOT NULL,
 CONSTRAINT [PK_SysAutoJobLog] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'任务组名称',             @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysAutoJobLog', @level2type=N'COLUMN',@level2name=N'JobGroupName'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'任务名称',               @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysAutoJobLog', @level2type=N'COLUMN',@level2name=N'JobName'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'执行状态(0失败 1成功)',  @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysAutoJobLog', @level2type=N'COLUMN',@level2name=N'LogStatus'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'备注',                  @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysAutoJobLog', @level2type=N'COLUMN',@level2name=N'Remark'

IF OBJECT_ID('[dbo].[SysDataDict]', 'U') IS NOT NULL DROP TABLE [dbo].[SysDataDict]; 
CREATE TABLE [dbo].[SysDataDict](
	[Id]					[bigint]			NOT NULL,
	[BaseIsDelete]			[int]				NOT NULL,
	[BaseCreateTime]		[datetime]			NOT NULL,
	[BaseModifyTime]		[datetime]			NOT NULL,
	[BaseCreatorId]			[bigint]			NOT NULL,
	[BaseModifierId]		[bigint]			NOT NULL,
	[BaseVersion]			[int]				NOT NULL,
	[DictType]				[varchar](50)		NOT NULL,
	[DictSort]				[int]				NOT NULL,
	[Remark]				[nvarchar](50)		NOT NULL,
 CONSTRAINT [PK_SysDataDict] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'字典类型',  @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysDataDict', @level2type=N'COLUMN',@level2name=N'DictType'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'字典排序',  @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysDataDict', @level2type=N'COLUMN',@level2name=N'DictSort'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'备注',      @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysDataDict', @level2type=N'COLUMN',@level2name=N'Remark'

IF OBJECT_ID('[dbo].[SysDataDictDetail]', 'U') IS NOT NULL DROP TABLE [dbo].[SysDataDictDetail]; 
CREATE TABLE [dbo].[SysDataDictDetail](
	[Id]				    [bigint]			NOT NULL,
	[BaseIsDelete]			[int]				NOT NULL,
	[BaseCreateTime]		[datetime]			NOT NULL,
	[BaseModifyTime]		[datetime]			NOT NULL,
	[BaseCreatorId]			[bigint]			NOT NULL,
	[BaseModifierId]		[bigint]			NOT NULL,
	[BaseVersion]			[int]				NOT NULL,
	[DictType]				[varchar](50)		NOT NULL,
	[DictSort]				[int]				NOT NULL,
	[DictKey]				[int]				NOT NULL,
	[DictValue]				[varchar](50)		NOT NULL,
	[ListClass]				[varchar](50)		NOT NULL,
	[DictStatus]			[int]				NOT NULL,
	[IsDefault]				[int]				NOT NULL,
	[Remark]				[nvarchar](50)		NOT NULL,
 CONSTRAINT [PK_SysDataDictDetail] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'字典类型(外键)',          @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysDataDictDetail', @level2type=N'COLUMN',@level2name=N'DictType'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'字典排序',                @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysDataDictDetail', @level2type=N'COLUMN',@level2name=N'DictSort'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'字典键(一般从1开始)',     @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysDataDictDetail', @level2type=N'COLUMN',@level2name=N'DictKey'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'字典值',			       @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysDataDictDetail', @level2type=N'COLUMN',@level2name=N'DictValue'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'显示样式(default primary success info warning danger)',  @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysDataDictDetail', @level2type=N'COLUMN',@level2name=N'ListClass'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'字典状态(0禁用 1启用)',   @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysDataDictDetail', @level2type=N'COLUMN',@level2name=N'DictStatus'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'默认选中(0不是 1是)',     @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysDataDictDetail', @level2type=N'COLUMN',@level2name=N'IsDefault'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'备注',                   @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysDataDictDetail', @level2type=N'COLUMN',@level2name=N'Remark'

IF OBJECT_ID('[dbo].[SysDepartment]', 'U') IS NOT NULL DROP TABLE [dbo].[SysDepartment]; 
CREATE TABLE [dbo].[SysDepartment](
	[Id]				    [bigint]			NOT NULL,
	[BaseIsDelete]			[int]				NOT NULL,
	[BaseCreateTime]		[datetime]			NOT NULL,
	[BaseModifyTime]		[datetime]			NOT NULL,
	[BaseCreatorId]			[bigint]			NOT NULL,
	[BaseModifierId]		[bigint]			NOT NULL,
	[BaseVersion]			[int]				NOT NULL,
	[ParentId]				[bigint]			NOT NULL,
	[DepartmentName]		[nvarchar](50)		NOT NULL,
	[Telephone]				[varchar](50)		NOT NULL,
	[Fax]					[varchar](50)		NOT NULL,
	[Email]					[varchar](50)		NOT NULL,
	[PrincipalId]			[bigint]			NOT NULL,
	[DepartmentSort]		[int]				NOT NULL,
	[Remark]				[nvarchar](500)		NOT NULL,
 CONSTRAINT [PK_SysDepartment] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'父部门Id(0表示是根部门)',   @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysDepartment', @level2type=N'COLUMN',@level2name=N'ParentId'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'部门名称',                 @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysDepartment', @level2type=N'COLUMN',@level2name=N'DepartmentName'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'部门电话',                 @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysDepartment', @level2type=N'COLUMN',@level2name=N'Telephone'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'部门传真',                 @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysDepartment', @level2type=N'COLUMN',@level2name=N'Fax'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'部门Email',				@level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysDepartment', @level2type=N'COLUMN',@level2name=N'Email'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'部门负责人Id',             @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysDepartment', @level2type=N'COLUMN',@level2name=N'PrincipalId'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'部门排序',                 @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysDepartment', @level2type=N'COLUMN',@level2name=N'DepartmentSort'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'备注',                     @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysDepartment', @level2type=N'COLUMN',@level2name=N'Remark'

IF OBJECT_ID('[dbo].[SysLogApi]', 'U') IS NOT NULL DROP TABLE [dbo].[SysLogApi]; 
CREATE TABLE [dbo].[SysLogApi](
	[Id]                    [bigint]			NOT NULL,
	[BaseCreateTime]		[datetime]			NOT NULL,
	[BaseCreatorId]			[bigint]			NOT NULL,
	[LogStatus]				[int]				NOT NULL,
	[Remark]				[nvarchar](50)		NOT NULL,
	[ExecuteUrl]			[varchar](100)		NOT NULL,
	[ExecuteParam]			[nvarchar](4000)		NOT NULL,
	[ExecuteResult]			[nvarchar](4000)		NOT NULL,
	[ExecuteTime]			[int]				NOT NULL,
 CONSTRAINT [PK_SysLogApi] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'执行状态(0失败 1成功)',  @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysLogApi', @level2type=N'COLUMN',@level2name=N'LogStatus'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'备注',                  @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysLogApi', @level2type=N'COLUMN',@level2name=N'Remark'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'接口地址',				 @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysLogApi', @level2type=N'COLUMN',@level2name=N'ExecuteUrl'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'请求参数',              @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysLogApi', @level2type=N'COLUMN',@level2name=N'ExecuteParam'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'请求结果',              @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysLogApi', @level2type=N'COLUMN',@level2name=N'ExecuteResult'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'执行时间',              @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysLogApi', @level2type=N'COLUMN',@level2name=N'ExecuteTime'

IF OBJECT_ID('[dbo].[SysLogLogin]', 'U') IS NOT NULL DROP TABLE [dbo].[SysLogLogin]; 
CREATE TABLE [dbo].[SysLogLogin](
	[Id]					[bigint]			NOT NULL,
	[BaseCreateTime]		[datetime]			NOT NULL,
	[BaseCreatorId]			[bigint]			NOT NULL,
	[LogStatus]				[int]				NOT NULL,
	[IpAddress]				[varchar](20)		NOT NULL,
	[IpLocation]			[nvarchar](50)		NOT NULL,
	[Browser]				[nvarchar](50)		NOT NULL,
	[OS]					[nvarchar](50)		NOT NULL,
	[Remark]			    [nvarchar](50)		NOT NULL,
	[ExtraRemark]			[nvarchar](500)		NOT NULL,
 CONSTRAINT [PK_SysLogLogin] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'执行状态(0失败 1成功)',  @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysLogLogin', @level2type=N'COLUMN',@level2name=N'LogStatus'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ip地址',                @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysLogLogin', @level2type=N'COLUMN',@level2name=N'IpAddress'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ip位置',				  @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysLogLogin', @level2type=N'COLUMN',@level2name=N'IpLocation'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'浏览器',                 @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysLogLogin', @level2type=N'COLUMN',@level2name=N'Browser'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'操作系统',               @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysLogLogin', @level2type=N'COLUMN',@level2name=N'OS'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'备注',                  @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysLogLogin', @level2type=N'COLUMN',@level2name=N'Remark'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'额外备注',               @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysLogLogin', @level2type=N'COLUMN',@level2name=N'ExtraRemark'

IF OBJECT_ID('[dbo].[SysLogOperate]', 'U') IS NOT NULL DROP TABLE [dbo].[SysLogOperate]; 
CREATE TABLE [dbo].[SysLogOperate](
	[Id]					[bigint]			NOT NULL,
	[BaseCreateTime]		[datetime]			NOT NULL,
	[BaseCreatorId]			[bigint]			NOT NULL,
	[LogStatus]				[int]				NOT NULL,
	[IpAddress]				[varchar](20)		NOT NULL,
	[IpLocation]			[nvarchar](50)		NOT NULL,
	[Remark]				[nvarchar](50)		NOT NULL,
	[LogType]				[varchar](50)		NOT NULL,
	[BusinessType]			[varchar](50)		NOT NULL,
	[ExecuteUrl]			[nvarchar](100)		NOT NULL,
	[ExecuteParam]			[nvarchar](4000)		NOT NULL,
	[ExecuteResult]			[nvarchar](4000)		NOT NULL,
	[ExecuteTime]			[int]				NOT NULL,
 CONSTRAINT [PK_SysLogOperate] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'执行状态(0失败 1成功)',  @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysLogOperate', @level2type=N'COLUMN',@level2name=N'LogStatus'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ip地址',                @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysLogOperate', @level2type=N'COLUMN',@level2name=N'IpAddress'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ip位置',                @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysLogOperate', @level2type=N'COLUMN',@level2name=N'IpLocation'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'备注',                  @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysLogOperate', @level2type=N'COLUMN',@level2name=N'Remark'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'日志类型(暂未用到)',     @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysLogOperate', @level2type=N'COLUMN',@level2name=N'LogType'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'业务类型(暂未用到)',     @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysLogOperate', @level2type=N'COLUMN',@level2name=N'BusinessType'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'页面地址',              @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysLogOperate', @level2type=N'COLUMN',@level2name=N'ExecuteUrl'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'请求参数',              @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysLogOperate', @level2type=N'COLUMN',@level2name=N'ExecuteParam'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'请求结果',              @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysLogOperate', @level2type=N'COLUMN',@level2name=N'ExecuteResult'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'执行时间',              @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysLogOperate', @level2type=N'COLUMN',@level2name=N'ExecuteTime'

IF OBJECT_ID('[dbo].[SysMenu]', 'U') IS NOT NULL DROP TABLE [dbo].[SysMenu]; 
CREATE TABLE [dbo].[SysMenu](
	[Id]				    [bigint]			NOT NULL,
	[BaseIsDelete]			[int]				NOT NULL,
	[BaseCreateTime]		[datetime]			NOT NULL,
	[BaseModifyTime]		[datetime]			NOT NULL,
	[BaseCreatorId]			[bigint]			NOT NULL,
	[BaseModifierId]		[bigint]			NOT NULL,
	[BaseVersion]			[int]				NOT NULL,
	[ParentId]				[bigint]			NOT NULL,
	[MenuName]				[nvarchar](50)		NOT NULL,
	[MenuIcon]				[varchar](50)		NOT NULL,
	[MenuUrl]				[varchar](100)		NOT NULL,
	[MenuTarget]			[varchar](50)		NOT NULL,
	[MenuSort]				[int]				NOT NULL,
	[MenuType]				[int]				NOT NULL,
	[MenuStatus]			[int]				NOT NULL,
	[Authorize]				[varchar](50)		NOT NULL,
	[Remark]				[nvarchar](50)		NOT NULL,
 CONSTRAINT [PK_SysMenu] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'父菜单Id(0表示是根菜单)',     @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysMenu', @level2type=N'COLUMN',@level2name=N'ParentId'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'菜单名称',                   @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysMenu', @level2type=N'COLUMN',@level2name=N'MenuName'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'菜单图标',                   @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysMenu', @level2type=N'COLUMN',@level2name=N'MenuIcon'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'菜单Url',                   @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysMenu', @level2type=N'COLUMN',@level2name=N'MenuUrl'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'链接打开方式',               @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysMenu', @level2type=N'COLUMN',@level2name=N'MenuTarget'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'菜单排序',                  @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysMenu', @level2type=N'COLUMN',@level2name=N'MenuSort'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'菜单类型(1目录 2页面 3按钮)',@level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysMenu', @level2type=N'COLUMN',@level2name=N'MenuType'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'菜单状态(0禁用 1启用)',      @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysMenu', @level2type=N'COLUMN',@level2name=N'MenuStatus'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'菜单权限标识',              @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysMenu', @level2type=N'COLUMN',@level2name=N'Authorize'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'备注',                     @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysMenu', @level2type=N'COLUMN',@level2name=N'Remark'

IF OBJECT_ID('[dbo].[SysMenuAuthorize]', 'U') IS NOT NULL DROP TABLE [dbo].[SysMenuAuthorize]; 
CREATE TABLE [dbo].[SysMenuAuthorize](
	[Id]				    [bigint]			NOT NULL,
	[BaseCreateTime]		[datetime]			NOT NULL,
	[BaseCreatorId]			[bigint]			NOT NULL,
	[MenuId]				[bigint]			NOT NULL,
	[AuthorizeId]			[bigint]			NOT NULL,
	[AuthorizeType]			[int]				NOT NULL,
 CONSTRAINT [PK_SysMenuAuthorize] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'菜单Id',                  @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysMenuAuthorize', @level2type=N'COLUMN',@level2name=N'MenuId'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'授权Id(角色Id或者用户Id)', @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysMenuAuthorize', @level2type=N'COLUMN',@level2name=N'AuthorizeId'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'授权类型(1角色 2用户)',    @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysMenuAuthorize', @level2type=N'COLUMN',@level2name=N'AuthorizeType'

IF OBJECT_ID('[dbo].[SysNews]', 'U') IS NOT NULL DROP TABLE [dbo].[SysNews]; 
CREATE TABLE [dbo].[SysNews](
	[Id]					[bigint]			NOT NULL,
	[BaseIsDelete]			[int]				NOT NULL,
	[BaseCreateTime]		[datetime]			NOT NULL,
	[BaseModifyTime]		[datetime]			NOT NULL,
	[BaseCreatorId]			[bigint]			NOT NULL,
	[BaseModifierId]		[bigint]			NOT NULL,
	[BaseVersion]			[int]				NOT NULL,
	[NewsTitle]				[nvarchar](300)		NOT NULL,
	[NewsContent]			[ntext]				NOT NULL,
	[NewsTag]				[nvarchar](200)		NOT NULL,
	[ThumbImage]			[varchar](200)		NOT NULL,
	[NewsAuthor]			[nvarchar](50)		NOT NULL,
	[NewsSort]				[int]				NOT NULL,
	[NewsDate]				[datetime]			NOT NULL,
	[NewsType]				[int]				NOT NULL,
	[ViewTimes]				[int]				NOT NULL,
	[ProvinceId]			[bigint]			NOT NULL,
	[CityId]				[bigint]			NOT NULL,
	[CountyId]				[bigint]			NOT NULL,
 CONSTRAINT [PK_SysNews] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'新闻标题',                     @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysNews', @level2type=N'COLUMN',@level2name=N'NewsTitle'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'新闻内容',                     @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysNews', @level2type=N'COLUMN',@level2name=N'NewsContent'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'新闻标签',                     @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysNews', @level2type=N'COLUMN',@level2name=N'NewsTag'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'省份Id',                       @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysNews', @level2type=N'COLUMN',@level2name=N'ProvinceId'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'城市Id',                       @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysNews', @level2type=N'COLUMN',@level2name=N'CityId'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'区县Id',                       @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysNews', @level2type=N'COLUMN',@level2name=N'CountyId'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'缩略图',                       @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysNews', @level2type=N'COLUMN',@level2name=N'ThumbImage'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'新闻排序',                     @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysNews', @level2type=N'COLUMN',@level2name=N'NewsSort'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'发布者',                       @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysNews', @level2type=N'COLUMN',@level2name=N'NewsAuthor'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'发布时间',                     @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysNews', @level2type=N'COLUMN',@level2name=N'NewsDate'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'新闻类型(1产品案例 2行业新闻)', @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysNews', @level2type=N'COLUMN',@level2name=N'NewsType'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'查看次数',                     @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysNews', @level2type=N'COLUMN',@level2name=N'ViewTimes'

IF OBJECT_ID('[dbo].[SysPosition]', 'U') IS NOT NULL DROP TABLE [dbo].[SysPosition]; 
CREATE TABLE [dbo].[SysPosition](
	[Id]					[bigint]			NOT NULL,
	[BaseIsDelete]			[int]				NOT NULL,
	[BaseCreateTime]		[datetime]			NOT NULL,
	[BaseModifyTime]		[datetime]			NOT NULL,
	[BaseCreatorId]			[bigint]			NOT NULL,
	[BaseModifierId]		[bigint]			NOT NULL,
	[BaseVersion]			[int]				NOT NULL,
	[PositionName]			[nvarchar](50)		NOT NULL,
	[PositionSort]			[int]				NOT NULL,
	[PositionStatus]		[int]				NOT NULL,
	[Remark]				[nvarchar](50)		NOT NULL,
 CONSTRAINT [PK_SysPosition] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'职位名称',             @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysPosition', @level2type=N'COLUMN',@level2name=N'PositionName'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'职位排序',             @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysPosition', @level2type=N'COLUMN',@level2name=N'PositionSort'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'职位状态(0禁用 1启用)', @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysPosition', @level2type=N'COLUMN',@level2name=N'PositionStatus'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'备注',                 @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysPosition', @level2type=N'COLUMN',@level2name=N'Remark'

IF OBJECT_ID('[dbo].[SysRole]', 'U') IS NOT NULL DROP TABLE [dbo].[SysRole]; 
CREATE TABLE [dbo].[SysRole](
	[Id]					[bigint]			NOT NULL,
	[BaseIsDelete]			[int]				NOT NULL,
	[BaseCreateTime]		[datetime]			NOT NULL,
	[BaseModifyTime]		[datetime]			NOT NULL,
	[BaseCreatorId]			[bigint]			NOT NULL,
	[BaseModifierId]		[bigint]			NOT NULL,
	[BaseVersion]			[int]				NOT NULL,
	[RoleName]				[nvarchar](50)		NOT NULL,
	[RoleSort]				[int]				NOT NULL,
	[RoleStatus]			[int]				NOT NULL,
	[Remark]				[nvarchar](50)		NOT NULL,
 CONSTRAINT [PK_SysRole] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'角色名称',              @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysRole', @level2type=N'COLUMN',@level2name=N'RoleName'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'角色排序',              @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysRole', @level2type=N'COLUMN',@level2name=N'RoleSort'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'角色状态(0禁用 1启用)', @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysRole', @level2type=N'COLUMN',@level2name=N'RoleStatus'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'备注',                 @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysRole', @level2type=N'COLUMN',@level2name=N'Remark'

IF OBJECT_ID('[dbo].[SysUser]', 'U') IS NOT NULL DROP TABLE [dbo].[SysUser]; 
CREATE TABLE [dbo].[SysUser](
	[Id]					[bigint]			NOT NULL,
	[BaseIsDelete]			[int]				NOT NULL,
	[BaseCreateTime]		[datetime]			NOT NULL,
	[BaseModifyTime]		[datetime]			NOT NULL,
	[BaseCreatorId]			[bigint]			NOT NULL,
	[BaseModifierId]		[bigint]			NOT NULL,
	[BaseVersion]			[int]				NOT NULL,
	[UserName]				[nvarchar](20)		NOT NULL,
	[Password]				[varchar](32)		NOT NULL,
	[Salt]					[varchar](5)		NOT NULL,
	[RealName]				[nvarchar](20)		NOT NULL,
	[DepartmentId]			[bigint]			NOT NULL,
	[Gender]				[int]				NOT NULL,
	[Birthday]				[varchar](10)		NOT NULL,
	[Portrait]				[varchar](200)		NOT NULL,
	[Email]					[varchar](50)		NOT NULL,
	[Mobile]				[varchar](11)		NOT NULL,
	[QQ]					[varchar](20)		NOT NULL,
	[WeChat]				[varchar](20)		NOT NULL,
	[LoginCount]			[int]				NOT NULL,
	[UserStatus]			[int]				NOT NULL,
	[IsSystem]				[int]				NOT NULL,
	[IsOnline]				[int]				NOT NULL,
	[FirstVisit]			[datetime]			NOT NULL,
	[PreviousVisit]			[datetime]			NOT NULL,
	[LastVisit]				[datetime]			NOT NULL,
	[Remark]				[nvarchar](200)		NOT NULL,
	[WebToken]				[varchar](32)		NOT NULL,
	[ApiToken]				[varchar](32)		NOT NULL,
 CONSTRAINT [PK_SysUser] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户名',									@level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysUser', @level2type=N'COLUMN',@level2name=N'UserName'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'密码',										@level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysUser', @level2type=N'COLUMN',@level2name=N'Password'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'密码盐值',									@level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysUser', @level2type=N'COLUMN',@level2name=N'Salt'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'姓名',									    @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysUser', @level2type=N'COLUMN',@level2name=N'RealName'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'所属部门Id',								@level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysUser', @level2type=N'COLUMN',@level2name=N'DepartmentId'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'性别(0未知 1男 2女)',						@level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysUser', @level2type=N'COLUMN',@level2name=N'Gender'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'出生日期',									@level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysUser', @level2type=N'COLUMN',@level2name=N'Birthday'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'头像',										@level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysUser', @level2type=N'COLUMN',@level2name=N'Portrait'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Email',									@level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysUser', @level2type=N'COLUMN',@level2name=N'Email'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'手机',										@level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysUser', @level2type=N'COLUMN',@level2name=N'Mobile'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'QQ',                                      @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysUser', @level2type=N'COLUMN',@level2name=N'QQ'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'微信',                                     @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysUser', @level2type=N'COLUMN',@level2name=N'WeChat'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登录次数',                                 @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysUser', @level2type=N'COLUMN',@level2name=N'LoginCount'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户状态(0禁用 1启用)',                     @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysUser', @level2type=N'COLUMN',@level2name=N'UserStatus'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'系统用户(0不是 1是[系统用户拥有所有的权限])', @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysUser', @level2type=N'COLUMN',@level2name=N'IsSystem'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'在线(0不是 1是)',							 @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysUser', @level2type=N'COLUMN',@level2name=N'IsOnline'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'首次登录时间',								 @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysUser', @level2type=N'COLUMN',@level2name=N'FirstVisit'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'上一次登录时间',						     @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysUser', @level2type=N'COLUMN',@level2name=N'PreviousVisit'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最后一次登录时间',                          @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysUser', @level2type=N'COLUMN',@level2name=N'LastVisit'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'备注',                                     @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysUser', @level2type=N'COLUMN',@level2name=N'Remark'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'后台Token',                                @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysUser', @level2type=N'COLUMN',@level2name=N'WebToken'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ApiToken',                                @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysUser', @level2type=N'COLUMN',@level2name=N'ApiToken'

IF OBJECT_ID('[dbo].[SysUserBelong]', 'U') IS NOT NULL DROP TABLE [dbo].[SysUserBelong]; 
CREATE TABLE [dbo].[SysUserBelong](
	[Id]					[bigint]			NOT NULL,
	[BaseCreateTime]		[datetime]			NOT NULL,
	[BaseCreatorId]			[bigint]			NOT NULL,
	[UserId]				[bigint]			NOT NULL,
	[BelongId]				[bigint]			NOT NULL,
	[BelongType]			[int]				NOT NULL,
 CONSTRAINT [PK_SysUserBelong] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户Id',               @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysUserBelong', @level2type=N'COLUMN',@level2name=N'UserId'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'职位Id或者角色Id',      @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysUserBelong', @level2type=N'COLUMN',@level2name=N'BelongId'
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'所属类型(1职位 2角色)', @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SysUserBelong', @level2type=N'COLUMN',@level2name=N'BelongType'
