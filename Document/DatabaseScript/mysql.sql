/* 执行脚本前，请先选择数据库，脚本会先删除表，然后再创建表，请谨慎执行！！！ */;
/* use yisha_admin; */;

DROP TABLE IF EXISTS `sys_area`;
CREATE TABLE IF NOT EXISTS `sys_area` (
  `id`                   bigint(20)     NOT NULL     COMMENT '主键',
  `base_is_delete`       int(11)        NOT NULL     COMMENT '删除标记(0正常 1删除)',
  `base_create_time`     datetime       NOT NULL     COMMENT '创建时间',
  `base_modify_time`     datetime       NOT NULL     COMMENT '修改时间',
  `base_creator_id`      bigint(20)     NOT NULL     COMMENT '创建人',
  `base_modifier_id`     bigint(20)	    NOT NULL     COMMENT '修改人',
  `base_version`         int(11)        NOT NULL     COMMENT '数据版本(每次更新+1)',
  `area_code`            varchar(6)     NOT NULL     COMMENT '地区编码',
  `parent_area_code`     varchar(6)     NOT NULL     COMMENT '父地区编码',
  `area_name`            varchar(50)    NOT NULL     COMMENT '地区名称',
  `zip_code`             varchar(50)    NOT NULL     COMMENT '邮政编码',
  `area_level`           int(11)        NOT NULL     COMMENT '地区层级(1省份 2城市 3区县)',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB COMMENT '中国省市县表';
 
DROP TABLE IF EXISTS `sys_auto_job`;
CREATE TABLE IF NOT EXISTS `sys_auto_job` (
  `id`                  bigint(20)		NOT NULL,
  `base_is_delete`		int(11)			NOT NULL,
  `base_create_time`	datetime		NOT NULL,
  `base_modify_time`	datetime		NOT NULL,
  `base_creator_id`		bigint(20)		NOT NULL,
  `base_modifier_id`	bigint(20)		NOT NULL,
  `base_version`		int(11)			NOT NULL,
  `job_group_name`		varchar(50)		NOT NULL       COMMENT '任务组名称',
  `job_name`			varchar(50)		NOT NULL       COMMENT '任务名称',
  `job_status`			int(11)			NOT NULL       COMMENT '任务状态(0禁用 1启用)',
  `cron_expression`		varchar(50)		NOT NULL       COMMENT 'cron表达式',
  `start_time`			datetime		NOT NULL       COMMENT '运行开始时间',
  `end_time`			datetime		NOT NULL       COMMENT '运行结束时间',
  `next_start_time`		datetime		NOT NULL       COMMENT '下次执行时间',
  `remark`				text			NOT NULL       COMMENT '备注',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB COMMENT '定时任务表';

DROP TABLE IF EXISTS `sys_data_dict`;
CREATE TABLE IF NOT EXISTS `sys_data_dict` (
  `id`                  bigint(20)      NOT NULL,
  `base_is_delete`      int(11)         NOT NULL,     
  `base_create_time`    datetime        NOT NULL,
  `base_modify_time`    datetime        NOT NULL,
  `base_creator_id`     bigint(20)      NOT NULL,
  `base_modifier_id`    bigint(20)      NOT NULL,
  `base_version`        int(11)         NOT NULL,
  `dict_type`           varchar(50)     NOT NULL       COMMENT '字典类型',
  `dict_sort`           int(11)         NOT NULL       COMMENT '字典排序',
  `remark`              varchar(50)     NOT NULL       COMMENT '备注',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB COMMENT '字典类型表';

DROP TABLE IF EXISTS `sys_data_dict_detail`;
CREATE TABLE IF NOT EXISTS `sys_data_dict_detail` (
  `id`                  bigint(20)      NOT NULL,
  `base_is_delete`      int(11)         NOT NULL,
  `base_create_time`    datetime        NOT NULL,
  `base_modify_time`    datetime        NOT NULL,
  `base_creator_id`     bigint(20)      NOT NULL,
  `base_modifier_id`    bigint(20)      NOT NULL,
  `base_version`        int(11)         NOT NULL,
  `dict_type`           varchar(50)     NOT NULL       COMMENT '字典类型(外键)',
  `dict_sort`           int(11)         NOT NULL       COMMENT '字典排序',
  `dict_key`            int(11)         NOT NULL       COMMENT '字典键(一般从1开始)',
  `dict_value`          varchar(50)     NOT NULL       COMMENT '字典值',
  `list_class`          varchar(50)     NOT NULL       COMMENT '显示样式(default primary success info warning danger)',
  `dict_status`         int(11)         NOT NULL       COMMENT '字典状态(0禁用 1启用)',
  `is_default`          int(11)         NOT NULL       COMMENT '默认选中(0不是 1是)',
  `remark`              varchar(50)     NOT NULL       COMMENT '备注',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB COMMENT '字典数据表';

DROP TABLE IF EXISTS `sys_department`;
CREATE TABLE IF NOT EXISTS `sys_department` (
  `id`                      bigint(20)      NOT NULL,       
  `base_is_delete`          int(11)         NOT NULL,      
  `base_create_time`        datetime        NOT NULL,       
  `base_modify_time`        datetime        NOT NULL,       
  `base_creator_id`         bigint(20)      NOT NULL,       
  `base_modifier_id`        bigint(20)      NOT NULL,
  `base_version`            int(11)         NOT NULL,
  `parent_id`               bigint(20)      NOT NULL       COMMENT '父部门Id(0表示是根部门)',
  `department_name`         varchar(50)     NOT NULL       COMMENT '部门名称',
  `telephone`               varchar(50)     NOT NULL       COMMENT '部门电话',
  `fax`                     varchar(50)     NOT NULL       COMMENT '部门传真',
  `email`                   varchar(50)     NOT NULL       COMMENT '部门Email',
  `principal_id`            bigint(20)      NOT NULL       COMMENT '部门负责人Id',
  `department_sort`         int(11)         NOT NULL       COMMENT '部门排序',
  `remark`                  text            NOT NULL       COMMENT '备注',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB COMMENT '部门表';

DROP TABLE IF EXISTS `sys_menu`;
CREATE TABLE IF NOT EXISTS `sys_menu` (
  `id`                      bigint(20)      NOT NULL,
  `base_is_delete`          int(11)         NOT NULL,
  `base_create_time`        datetime        NOT NULL,
  `base_modify_time`        datetime        NOT NULL,
  `base_creator_id`         bigint(20)      NOT NULL,
  `base_modifier_id`        bigint(20)      NOT NULL,
  `base_version`            int(11)         NOT NULL,
  `parent_id`               bigint(20)      NOT NULL       COMMENT '父菜单Id(0表示是根菜单)',
  `menu_name`               varchar(50)     NOT NULL       COMMENT '菜单名称',
  `menu_icon`               varchar(50)     NOT NULL       COMMENT '菜单图标',  
  `menu_url`                varchar(100)    NOT NULL       COMMENT '菜单Url',
  `menu_target`             varchar(50)     NOT NULL       COMMENT '链接打开方式',
  `menu_sort`               int(11)         NOT NULL       COMMENT '菜单排序',
  `menu_type`               int(11)         NOT NULL       COMMENT '菜单类型(1目录 2页面 3按钮)',
  `menu_status`             int(11)         NOT NULL       COMMENT '菜单状态(0禁用 1启用)',     
  `authorize`               varchar(50)     NOT NULL       COMMENT '菜单权限标识',
  `remark`                  varchar(50)     NOT NULL       COMMENT '备注',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB COMMENT '菜单表';

DROP TABLE IF EXISTS `sys_menu_authorize`;
CREATE TABLE IF NOT EXISTS `sys_menu_authorize` (
  `id`                      bigint(20)      NOT NULL,
  `base_create_time`        datetime        NOT NULL,
  `base_creator_id`         bigint(20)      NOT NULL,
  `menu_id`                 bigint(20)      NOT NULL       COMMENT '菜单Id',
  `authorize_id`            bigint(20)      NOT NULL       COMMENT '授权Id(角色Id或者用户Id)',
  `authorize_type`          int(11)         NOT NULL       COMMENT '授权类型(1角色 2用户)',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB COMMENT '菜单权限表';

DROP TABLE IF EXISTS `sys_news`;
CREATE TABLE IF NOT EXISTS `sys_news` (
  `id`                  bigint(20)      NOT NULL,
  `base_is_delete`      int(11)         NOT NULL,
  `base_create_time`    datetime        NOT NULL,
  `base_modify_time`    datetime        NOT NULL,
  `base_creator_id`     bigint(20)      NOT NULL,
  `base_modifier_id`    bigint(20)      NOT NULL,
  `base_version`        int(11)         NOT NULL,
  `news_title`          varchar(300)    NOT NULL       COMMENT '新闻标题',
  `news_content`        longtext        NOT NULL       COMMENT '新闻内容',
  `news_tag`            varchar(200)    NOT NULL       COMMENT '新闻标签',
  `province_id`         bigint(20)      NOT NULL       COMMENT '省份Id',
  `city_id`             bigint(20)      NOT NULL       COMMENT '城市Id',
  `county_id`           bigint(20)      NOT NULL       COMMENT '区县Id',
  `thumb_image`         varchar(200)    NOT NULL       COMMENT '缩略图',
  `news_sort`           int(11)         NOT NULL       COMMENT '新闻排序',
  `news_author`         varchar(50)     NOT NULL       COMMENT '发布者',
  `news_date`           datetime        NOT NULL       COMMENT '发布时间',
  `news_type`           int(11)         NOT NULL       COMMENT '新闻类型(1产品案例 2行业新闻)',
  `view_times`          int(11)         NOT NULL       COMMENT '查看次数',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB COMMENT '新闻表';

DROP TABLE IF EXISTS `sys_position`;
CREATE TABLE IF NOT EXISTS `sys_position` (
  `id`                  bigint(20)      NOT NULL,
  `base_is_delete`      int(11)         NOT NULL,
  `base_create_time`    datetime        NOT NULL,
  `base_modify_time`    datetime        NOT NULL,
  `base_creator_id`     bigint(20)      NOT NULL,
  `base_modifier_id`    bigint(20)      NOT NULL,
  `base_version`        int(11)         NOT NULL,
  `position_name`       varchar(50)     NOT NULL       COMMENT '职位名称',
  `position_sort`       int(11)         NOT NULL       COMMENT '职位排序',
  `position_status`     int(11)         NOT NULL       COMMENT '职位状态(0禁用 1启用)',
  `remark`              varchar(50)     NOT NULL       COMMENT '备注',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB COMMENT '职位表';

DROP TABLE IF EXISTS `sys_role`;
CREATE TABLE IF NOT EXISTS `sys_role` (
  `id`                  bigint(20)      NOT NULL,
  `base_is_delete`      int(11)         NOT NULL,
  `base_create_time`    datetime        NOT NULL,
  `base_modify_time`    datetime        NOT NULL,
  `base_creator_id`     bigint(20)      NOT NULL,
  `base_modifier_id`    bigint(20)      NOT NULL,
  `base_version`        int(11)         NOT NULL,
  `role_name`           varchar(50)     NOT NULL       COMMENT '角色名称',
  `role_sort`           int(11)         NOT NULL       COMMENT '角色排序',
  `role_status`         int(11)         NOT NULL       COMMENT '角色状态(0禁用 1启用)',
  `remark`              varchar(50)     NOT NULL       COMMENT '备注',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB COMMENT '角色表';

DROP TABLE IF EXISTS `sys_user`;
CREATE TABLE IF NOT EXISTS `sys_user` (
  `id`                  bigint(20)          NOT NULL,
  `base_is_delete`      int(11)             NOT NULL,
  `base_create_time`    datetime            NOT NULL,
  `base_modify_time`    datetime            NOT NULL,
  `base_creator_id`     bigint(20)          NOT NULL,
  `base_modifier_id`    bigint(20)          NOT NULL,
  `base_version`        int(11)             NOT NULL,
  `user_name`           varchar(20)         NOT NULL       COMMENT '用户名',
  `password`            varchar(32)         NOT NULL       COMMENT '密码',
  `salt`                varchar(5)          NOT NULL       COMMENT '密码盐值',
  `real_name`           varchar(20)         NOT NULL       COMMENT '姓名',
  `department_id`       bigint(20)          NOT NULL       COMMENT '所属部门Id',
  `gender`              int(11)             NOT NULL       COMMENT '性别(0未知 1男 2女)',
  `birthday`            varchar(10)         NOT NULL       COMMENT '出生日期',
  `portrait`            varchar(200)        NOT NULL       COMMENT '头像',
  `email`               varchar(50)         NOT NULL       COMMENT 'Email',
  `mobile`              varchar(11)         NOT NULL       COMMENT '手机',
  `qq`                  varchar(20)         NOT NULL       COMMENT 'QQ',
  `wechat`              varchar(20)         NOT NULL       COMMENT '微信',
  `login_count`         int(11)             NOT NULL       COMMENT '登录次数',
  `user_status`         int(11)             NOT NULL       COMMENT '用户状态(0禁用 1启用)',
  `is_system`           int(11)             NOT NULL       COMMENT '系统用户(0不是 1是[系统用户拥有所有的权限])',
  `is_online`           int(11)             NOT NULL       COMMENT '在线(0不是 1是)',
  `first_visit`         datetime            NOT NULL       COMMENT '首次登录时间',
  `previous_visit`      datetime            NOT NULL       COMMENT '上一次登录时间',
  `last_visit`          datetime            NOT NULL       COMMENT '最后一次登录时间',
  `remark`              varchar(200)        NOT NULL       COMMENT '备注',
  `web_token`           varchar(32)         NOT NULL       COMMENT '后台Token',
  `api_token`           varchar(32)         NOT NULL       COMMENT 'ApiToken',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB COMMENT '用户表';

DROP TABLE IF EXISTS `sys_user_belong`;
CREATE TABLE IF NOT EXISTS `sys_user_belong` (
  `id`                  bigint(20)      NOT NULL,
  `base_create_time`    datetime        NOT NULL,
  `base_creator_id`     bigint(20)      NOT NULL,
  `user_id`             bigint(20)      NOT NULL       COMMENT '用户Id',
  `belong_id`           bigint(20)      NOT NULL       COMMENT '职位Id或者角色Id',
  `belong_type`         int(11)         NOT NULL       COMMENT '所属类型(1职位 2角色)',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB COMMENT '用户所属表';

DROP TABLE IF EXISTS `sys_auto_job_log`;
CREATE TABLE IF NOT EXISTS `sys_auto_job_log` (
  `id`                  bigint(20)      NOT NULL,
  `base_create_time`    datetime        NOT NULL,
  `base_creator_id`     bigint(20)      NOT NULL,
  `job_group_name`      varchar(50)     NOT NULL       COMMENT '任务组名称',
  `job_name`            varchar(50)     NOT NULL       COMMENT '任务名称',
  `log_status`          int(11)         NOT NULL       COMMENT '执行状态(0失败 1成功)',
  `remark`              text            NOT NULL       COMMENT '备注',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB COMMENT '用户所属表';

DROP TABLE IF EXISTS `sys_log_api`;
CREATE TABLE IF NOT EXISTS `sys_log_api` (
  `id`                  bigint(20)      NOT NULL,
  `base_create_time`    datetime        NOT NULL,
  `base_creator_id`     bigint(20)      NOT NULL,
  `log_status`          int(11)         NOT NULL       COMMENT '执行状态(0失败 1成功)',
  `remark`              varchar(50)     NOT NULL       COMMENT '备注',
  `execute_url`         varchar(100)    NOT NULL       COMMENT '接口地址',
  `execute_param`       text            NOT NULL       COMMENT '请求参数',
  `execute_result`      text            NOT NULL       COMMENT '请求结果',
  `execute_time`        int(11)         NOT NULL       COMMENT '执行时间',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB COMMENT 'Api日志表';

DROP TABLE IF EXISTS `sys_log_login`;
CREATE TABLE IF NOT EXISTS `sys_log_login` (
  `id`                  bigint(20)      NOT NULL,
  `base_create_time`    datetime        NOT NULL,
  `base_creator_id`     bigint(20)      NOT NULL,
  `log_status`          int(11)         NOT NULL       COMMENT '执行状态(0失败 1成功)',
  `ip_address`          varchar(20)     NOT NULL       COMMENT 'ip地址',
  `ip_location`         varchar(50)     NOT NULL       COMMENT 'ip位置',
  `browser`             varchar(50)     NOT NULL       COMMENT '浏览器',
  `os`                  varchar(50)     NOT NULL       COMMENT '操作系统',
  `remark`              varchar(50)     NOT NULL       COMMENT '备注',
  `extra_remark`        text            NOT NULL       COMMENT '额外备注',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB COMMENT '登录日志表';

DROP TABLE IF EXISTS `sys_log_operate`;
CREATE TABLE IF NOT EXISTS `sys_log_operate` (
  `id`                  bigint(20)      NOT NULL,
  `base_create_time`    datetime        NOT NULL,
  `base_creator_id`     bigint(20)      NOT NULL,
  `log_status`          int(11)         NOT NULL       COMMENT '执行状态(0失败 1成功)',
  `ip_address`          varchar(20)     NOT NULL       COMMENT 'ip地址',
  `ip_location`         varchar(50)     NOT NULL       COMMENT 'ip位置',
  `remark`              varchar(50)     NOT NULL       COMMENT '备注',
  `log_type`            varchar(50)     NOT NULL       COMMENT '日志类型(暂未用到)',
  `business_type`       varchar(50)     NOT NULL       COMMENT '业务类型(暂未用到)',
  `execute_url`         varchar(100)    NOT NULL       COMMENT '页面地址',
  `execute_param`       text            NOT NULL       COMMENT '请求参数',
  `execute_result`      text            NOT NULL       COMMENT '请求结果',
  `execute_time`        int(11)         NOT NULL       COMMENT '执行时间',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB COMMENT '操作日志表';