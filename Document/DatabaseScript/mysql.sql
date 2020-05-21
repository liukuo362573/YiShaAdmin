/* 执行脚本前，请先选择数据库，脚本会先删除表，然后再创建表，请谨慎执行！！！ */;
/* use YiShaAdmin; */;

DROP TABLE IF EXISTS `SysArea`;
CREATE TABLE IF NOT EXISTS `SysArea` (
  `Id`                   bigint(20)     NOT NULL     COMMENT '主键',
  `BaseIsDelete`       int(11)        NOT NULL     COMMENT '删除标记(0正常 1删除)',
  `BaseCreateTime`     datetime       NOT NULL     COMMENT '创建时间',
  `BaseModifyTime`     datetime       NOT NULL     COMMENT '修改时间',
  `BaseCreatorId`      bigint(20)     NOT NULL     COMMENT '创建人',
  `BaseModifierId`     bigint(20)	    NOT NULL     COMMENT '修改人',
  `BaseVersion`         int(11)        NOT NULL     COMMENT '数据版本(每次更新+1)',
  `AreaCode`            varchar(6)     NOT NULL     COMMENT '地区编码',
  `ParentAreaCode`     varchar(6)     NOT NULL     COMMENT '父地区编码',
  `AreaName`            varchar(50)    NOT NULL     COMMENT '地区名称',
  `ZipCode`             varchar(50)    NOT NULL     COMMENT '邮政编码',
  `AreaLevel`           int(11)        NOT NULL     COMMENT '地区层级(1省份 2城市 3区县)',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB COMMENT '中国省市县表';
 
DROP TABLE IF EXISTS `SysAutoJob`;
CREATE TABLE IF NOT EXISTS `SysAutoJob` (
  `Id`                  bigint(20)		NOT NULL,
  `BaseIsDelete`		int(11)			NOT NULL,
  `BaseCreateTime`	datetime		NOT NULL,
  `BaseModifyTime`	datetime		NOT NULL,
  `BaseCreatorId`		bigint(20)		NOT NULL,
  `BaseModifierId`	bigint(20)		NOT NULL,
  `BaseVersion`		int(11)			NOT NULL,
  `JobGroupName`		varchar(50)		NOT NULL       COMMENT '任务组名称',
  `JobName`			varchar(50)		NOT NULL       COMMENT '任务名称',
  `JobStatus`			int(11)			NOT NULL       COMMENT '任务状态(0禁用 1启用)',
  `CronExpression`		varchar(50)		NOT NULL       COMMENT 'cron表达式',
  `StartTime`			datetime		NOT NULL       COMMENT '运行开始时间',
  `EndTime`			datetime		NOT NULL       COMMENT '运行结束时间',
  `NextStartTime`		datetime		NOT NULL       COMMENT '下次执行时间',
  `Remark`				text			NOT NULL       COMMENT '备注',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB COMMENT '定时任务表';

DROP TABLE IF EXISTS `SysDataDict`;
CREATE TABLE IF NOT EXISTS `SysDataDict` (
  `Id`                  bigint(20)      NOT NULL,
  `BaseIsDelete`      int(11)         NOT NULL,     
  `BaseCreateTime`    datetime        NOT NULL,
  `BaseModifyTime`    datetime        NOT NULL,
  `BaseCreatorId`     bigint(20)      NOT NULL,
  `BaseModifierId`    bigint(20)      NOT NULL,
  `BaseVersion`        int(11)         NOT NULL,
  `DictType`           varchar(50)     NOT NULL       COMMENT '字典类型',
  `DictSort`           int(11)         NOT NULL       COMMENT '字典排序',
  `Remark`              varchar(50)     NOT NULL       COMMENT '备注',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB COMMENT '字典类型表';

DROP TABLE IF EXISTS `SysDataDictDetail`;
CREATE TABLE IF NOT EXISTS `SysDataDictDetail` (
  `Id`                  bigint(20)      NOT NULL,
  `BaseIsDelete`      int(11)         NOT NULL,
  `BaseCreateTime`    datetime        NOT NULL,
  `BaseModifyTime`    datetime        NOT NULL,
  `BaseCreatorId`     bigint(20)      NOT NULL,
  `BaseModifierId`    bigint(20)      NOT NULL,
  `BaseVersion`        int(11)         NOT NULL,
  `DictType`           varchar(50)     NOT NULL       COMMENT '字典类型(外键)',
  `DictSort`           int(11)         NOT NULL       COMMENT '字典排序',
  `DictKey`            int(11)         NOT NULL       COMMENT '字典键(一般从1开始)',
  `DictValue`          varchar(50)     NOT NULL       COMMENT '字典值',
  `ListClass`          varchar(50)     NOT NULL       COMMENT '显示样式(default primary success info warning danger)',
  `DictStatus`         int(11)         NOT NULL       COMMENT '字典状态(0禁用 1启用)',
  `IsDefault`          int(11)         NOT NULL       COMMENT '默认选中(0不是 1是)',
  `Remark`              varchar(50)     NOT NULL       COMMENT '备注',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB COMMENT '字典数据表';

DROP TABLE IF EXISTS `SysDepartment`;
CREATE TABLE IF NOT EXISTS `SysDepartment` (
  `Id`                      bigint(20)      NOT NULL,       
  `BaseIsDelete`          int(11)         NOT NULL,      
  `BaseCreateTime`        datetime        NOT NULL,       
  `BaseModifyTime`        datetime        NOT NULL,       
  `BaseCreatorId`         bigint(20)      NOT NULL,       
  `BaseModifierId`        bigint(20)      NOT NULL,
  `BaseVersion`            int(11)         NOT NULL,
  `ParentId`               bigint(20)      NOT NULL       COMMENT '父部门Id(0表示是根部门)',
  `DepartmentName`         varchar(50)     NOT NULL       COMMENT '部门名称',
  `Telephone`               varchar(50)     NOT NULL       COMMENT '部门电话',
  `Fax`                     varchar(50)     NOT NULL       COMMENT '部门传真',
  `Email`                   varchar(50)     NOT NULL       COMMENT '部门Email',
  `PrincipalId`            bigint(20)      NOT NULL       COMMENT '部门负责人Id',
  `DepartmentSort`         int(11)         NOT NULL       COMMENT '部门排序',
  `Remark`                  text            NOT NULL       COMMENT '备注',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB COMMENT '部门表';

DROP TABLE IF EXISTS `SysMenu`;
CREATE TABLE IF NOT EXISTS `SysMenu` (
  `Id`                      bigint(20)      NOT NULL,
  `BaseIsDelete`          int(11)         NOT NULL,
  `BaseCreateTime`        datetime        NOT NULL,
  `BaseModifyTime`        datetime        NOT NULL,
  `BaseCreatorId`         bigint(20)      NOT NULL,
  `BaseModifierId`        bigint(20)      NOT NULL,
  `BaseVersion`            int(11)         NOT NULL,
  `ParentId`               bigint(20)      NOT NULL       COMMENT '父菜单Id(0表示是根菜单)',
  `MenuName`               varchar(50)     NOT NULL       COMMENT '菜单名称',
  `MenuIcon`               varchar(50)     NOT NULL       COMMENT '菜单图标',  
  `MenuUrl`                varchar(100)    NOT NULL       COMMENT '菜单Url',
  `MenuTarget`             varchar(50)     NOT NULL       COMMENT '链接打开方式',
  `MenuSort`               int(11)         NOT NULL       COMMENT '菜单排序',
  `MenuType`               int(11)         NOT NULL       COMMENT '菜单类型(1目录 2页面 3按钮)',
  `MenuStatus`             int(11)         NOT NULL       COMMENT '菜单状态(0禁用 1启用)',     
  `Authorize`               varchar(50)     NOT NULL       COMMENT '菜单权限标识',
  `Remark`                  varchar(50)     NOT NULL       COMMENT '备注',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB COMMENT '菜单表';

DROP TABLE IF EXISTS `SysMenuAuthorize`;
CREATE TABLE IF NOT EXISTS `SysMenuAuthorize` (
  `Id`                      bigint(20)      NOT NULL,
  `BaseCreateTime`        datetime        NOT NULL,
  `BaseCreatorId`         bigint(20)      NOT NULL,
  `MenuId`                 bigint(20)      NOT NULL       COMMENT '菜单Id',
  `AuthorizeId`            bigint(20)      NOT NULL       COMMENT '授权Id(角色Id或者用户Id)',
  `AuthorizeType`          int(11)         NOT NULL       COMMENT '授权类型(1角色 2用户)',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB COMMENT '菜单权限表';

DROP TABLE IF EXISTS `SysNews`;
CREATE TABLE IF NOT EXISTS `SysNews` (
  `Id`                  bigint(20)      NOT NULL,
  `BaseIsDelete`      int(11)         NOT NULL,
  `BaseCreateTime`    datetime        NOT NULL,
  `BaseModifyTime`    datetime        NOT NULL,
  `BaseCreatorId`     bigint(20)      NOT NULL,
  `BaseModifierId`    bigint(20)      NOT NULL,
  `BaseVersion`        int(11)         NOT NULL,
  `NewsTitle`          varchar(300)    NOT NULL       COMMENT '新闻标题',
  `NewsContent`        longtext        NOT NULL       COMMENT '新闻内容',
  `NewsTag`            varchar(200)    NOT NULL       COMMENT '新闻标签',
  `ProvinceId`         bigint(20)      NOT NULL       COMMENT '省份Id',
  `CityId`             bigint(20)      NOT NULL       COMMENT '城市Id',
  `CountyId`           bigint(20)      NOT NULL       COMMENT '区县Id',
  `ThumbImage`         varchar(200)    NOT NULL       COMMENT '缩略图',
  `NewsSort`           int(11)         NOT NULL       COMMENT '新闻排序',
  `NewsAuthor`         varchar(50)     NOT NULL       COMMENT '发布者',
  `NewsDate`           datetime        NOT NULL       COMMENT '发布时间',
  `NewsType`           int(11)         NOT NULL       COMMENT '新闻类型(1产品案例 2行业新闻)',
  `ViewTimes`          int(11)         NOT NULL       COMMENT '查看次数',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB COMMENT '新闻表';

DROP TABLE IF EXISTS `SysPosition`;
CREATE TABLE IF NOT EXISTS `SysPosition` (
  `Id`                  bigint(20)      NOT NULL,
  `BaseIsDelete`      int(11)         NOT NULL,
  `BaseCreateTime`    datetime        NOT NULL,
  `BaseModifyTime`    datetime        NOT NULL,
  `BaseCreatorId`     bigint(20)      NOT NULL,
  `BaseModifierId`    bigint(20)      NOT NULL,
  `BaseVersion`        int(11)         NOT NULL,
  `PositionName`       varchar(50)     NOT NULL       COMMENT '职位名称',
  `PositionSort`       int(11)         NOT NULL       COMMENT '职位排序',
  `PositionStatus`     int(11)         NOT NULL       COMMENT '职位状态(0禁用 1启用)',
  `Remark`              varchar(50)     NOT NULL       COMMENT '备注',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB COMMENT '职位表';

DROP TABLE IF EXISTS `SysRole`;
CREATE TABLE IF NOT EXISTS `SysRole` (
  `Id`                  bigint(20)      NOT NULL,
  `BaseIsDelete`      int(11)         NOT NULL,
  `BaseCreateTime`    datetime        NOT NULL,
  `BaseModifyTime`    datetime        NOT NULL,
  `BaseCreatorId`     bigint(20)      NOT NULL,
  `BaseModifierId`    bigint(20)      NOT NULL,
  `BaseVersion`        int(11)         NOT NULL,
  `RoleName`           varchar(50)     NOT NULL       COMMENT '角色名称',
  `RoleSort`           int(11)         NOT NULL       COMMENT '角色排序',
  `RoleStatus`         int(11)         NOT NULL       COMMENT '角色状态(0禁用 1启用)',
  `Remark`              varchar(50)     NOT NULL       COMMENT '备注',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB COMMENT '角色表';

DROP TABLE IF EXISTS `SysUser`;
CREATE TABLE IF NOT EXISTS `SysUser` (
  `Id`                  bigint(20)          NOT NULL,
  `BaseIsDelete`      int(11)             NOT NULL,
  `BaseCreateTime`    datetime            NOT NULL,
  `BaseModifyTime`    datetime            NOT NULL,
  `BaseCreatorId`     bigint(20)          NOT NULL,
  `BaseModifierId`    bigint(20)          NOT NULL,
  `BaseVersion`        int(11)             NOT NULL,
  `UserName`           varchar(20)         NOT NULL       COMMENT '用户名',
  `Password`            varchar(32)         NOT NULL       COMMENT '密码',
  `Salt`                varchar(5)          NOT NULL       COMMENT '密码盐值',
  `RealName`           varchar(20)         NOT NULL       COMMENT '姓名',
  `DepartmentId`       bigint(20)          NOT NULL       COMMENT '所属部门Id',
  `Gender`              int(11)             NOT NULL       COMMENT '性别(0未知 1男 2女)',
  `Birthday`            varchar(10)         NOT NULL       COMMENT '出生日期',
  `Portrait`            varchar(200)        NOT NULL       COMMENT '头像',
  `Email`               varchar(50)         NOT NULL       COMMENT 'Email',
  `Mobile`              varchar(11)         NOT NULL       COMMENT '手机',
  `QQ`                  varchar(20)         NOT NULL       COMMENT 'QQ',
  `WeChat`              varchar(20)         NOT NULL       COMMENT '微信',
  `LoginCount`         int(11)             NOT NULL       COMMENT '登录次数',
  `UserStatus`         int(11)             NOT NULL       COMMENT '用户状态(0禁用 1启用)',
  `IsSystem`           int(11)             NOT NULL       COMMENT '系统用户(0不是 1是[系统用户拥有所有的权限])',
  `IsOnline`           int(11)             NOT NULL       COMMENT '在线(0不是 1是)',
  `FirstVisit`         datetime            NOT NULL       COMMENT '首次登录时间',
  `PreviousVisit`      datetime            NOT NULL       COMMENT '上一次登录时间',
  `LastVisit`          datetime            NOT NULL       COMMENT '最后一次登录时间',
  `Remark`              varchar(200)        NOT NULL       COMMENT '备注',
  `WebToken`           varchar(32)         NOT NULL       COMMENT '后台Token',
  `ApiToken`           varchar(32)         NOT NULL       COMMENT 'ApiToken',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB COMMENT '用户表';

DROP TABLE IF EXISTS `SysUserBelong`;
CREATE TABLE IF NOT EXISTS `SysUserBelong` (
  `Id`                  bigint(20)      NOT NULL,
  `BaseCreateTime`    datetime        NOT NULL,
  `BaseCreatorId`     bigint(20)      NOT NULL,
  `UserId`             bigint(20)      NOT NULL       COMMENT '用户Id',
  `BelongId`           bigint(20)      NOT NULL       COMMENT '职位Id或者角色Id',
  `BelongType`         int(11)         NOT NULL       COMMENT '所属类型(1职位 2角色)',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB COMMENT '用户所属表';

DROP TABLE IF EXISTS `SysAutoJobLog`;
CREATE TABLE IF NOT EXISTS `SysAutoJobLog` (
  `Id`                  bigint(20)      NOT NULL,
  `BaseCreateTime`    datetime        NOT NULL,
  `BaseCreatorId`     bigint(20)      NOT NULL,
  `JobGroupName`      varchar(50)     NOT NULL       COMMENT '任务组名称',
  `JobName`            varchar(50)     NOT NULL       COMMENT '任务名称',
  `LogStatus`          int(11)         NOT NULL       COMMENT '执行状态(0失败 1成功)',
  `Remark`              text            NOT NULL       COMMENT '备注',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB COMMENT '用户所属表';

DROP TABLE IF EXISTS `SysLogApi`;
CREATE TABLE IF NOT EXISTS `SysLogApi` (
  `Id`                  bigint(20)      NOT NULL,
  `BaseCreateTime`    datetime        NOT NULL,
  `BaseCreatorId`     bigint(20)      NOT NULL,
  `LogStatus`          int(11)         NOT NULL       COMMENT '执行状态(0失败 1成功)',
  `Remark`              varchar(50)     NOT NULL       COMMENT '备注',
  `ExecuteUrl`         varchar(100)    NOT NULL       COMMENT '接口地址',
  `ExecuteParam`       text            NOT NULL       COMMENT '请求参数',
  `ExecuteResult`      text            NOT NULL       COMMENT '请求结果',
  `ExecuteTime`        int(11)         NOT NULL       COMMENT '执行时间',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB COMMENT 'Api日志表';

DROP TABLE IF EXISTS `SysLogLogin`;
CREATE TABLE IF NOT EXISTS `SysLogLogin` (
  `Id`                  bigint(20)      NOT NULL,
  `BaseCreateTime`    datetime        NOT NULL,
  `BaseCreatorId`     bigint(20)      NOT NULL,
  `LogStatus`          int(11)         NOT NULL       COMMENT '执行状态(0失败 1成功)',
  `IpAddress`          varchar(20)     NOT NULL       COMMENT 'ip地址',
  `IpLocation`         varchar(50)     NOT NULL       COMMENT 'ip位置',
  `Browser`             varchar(50)     NOT NULL       COMMENT '浏览器',
  `OS`                  varchar(50)     NOT NULL       COMMENT '操作系统',
  `Remark`              varchar(50)     NOT NULL       COMMENT '备注',
  `ExtraRemark`        text            NOT NULL       COMMENT '额外备注',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB COMMENT '登录日志表';

DROP TABLE IF EXISTS `SysLogOperate`;
CREATE TABLE IF NOT EXISTS `SysLogOperate` (
  `Id`                  bigint(20)      NOT NULL,
  `BaseCreateTime`    datetime        NOT NULL,
  `BaseCreatorId`     bigint(20)      NOT NULL,
  `LogStatus`          int(11)         NOT NULL       COMMENT '执行状态(0失败 1成功)',
  `IpAddress`          varchar(20)     NOT NULL       COMMENT 'ip地址',
  `IpLocation`         varchar(50)     NOT NULL       COMMENT 'ip位置',
  `Remark`              varchar(50)     NOT NULL       COMMENT '备注',
  `LogType`            varchar(50)     NOT NULL       COMMENT '日志类型(暂未用到)',
  `BusinessType`       varchar(50)     NOT NULL       COMMENT '业务类型(暂未用到)',
  `ExecuteUrl`         varchar(100)    NOT NULL       COMMENT '页面地址',
  `ExecuteParam`       text            NOT NULL       COMMENT '请求参数',
  `ExecuteResult`      text            NOT NULL       COMMENT '请求结果',
  `ExecuteTime`        int(11)         NOT NULL       COMMENT '执行时间',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB COMMENT '操作日志表';