﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YiSha.Entity;
using YiSha.Model.Result;
using YiSha.Model.Result.SystemManage;
using YiSha.Service.SystemManage;
using YiSha.Util.Helper;
using YiSha.Util.Model;

namespace YiSha.Business.SystemManage
{
    public class DatabaseTableBLL
    {
        #region 构造函数

        private readonly IDatabaseTableService _databaseTableService;

        public DatabaseTableBLL()
        {
            string dbType = GlobalContext.SystemConfig.DbProvider;
            switch (dbType)
            {
                case "SqlServer":
                    _databaseTableService = new DatabaseTableSqlServerService();
                    break;
                case "MySql":
                    _databaseTableService = new DatabaseTableMySqlService();
                    break;
                case "Oracle":
                    _databaseTableService = new DatabaseTableOracleService();
                    break;
                default: throw new Exception("未找到数据库配置");
            }
        }

        #endregion

        #region 获取数据

        public async Task<TData<List<TableInfo>>> GetTableList(string tableName)
        {
            TData<List<TableInfo>> obj = new TData<List<TableInfo>>();
            List<TableInfo> list = await _databaseTableService.GetTableList(tableName);
            obj.Data = list;
            obj.Total = list.Count;
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<List<TableInfo>>> GetTablePageList(string tableName, Pagination pagination)
        {
            TData<List<TableInfo>> obj = new TData<List<TableInfo>>();
            List<TableInfo> list = await _databaseTableService.GetTablePageList(tableName, pagination);
            obj.Data = list;
            obj.Total = pagination.TotalCount;
            obj.Tag = 1;
            return obj;
        }

        /// <summary>
        /// 获取表字段
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public async Task<TData<List<TableFieldInfo>>> GetTableFieldList(string tableName)
        {
            TData<List<TableFieldInfo>> obj = new TData<List<TableFieldInfo>>();
            List<TableFieldInfo> list = await _databaseTableService.GetTableFieldList(tableName);
            obj.Data = list;
            obj.Total = list.Count;
            obj.Tag = 1;
            return obj;
        }

        /// <summary>
        /// 获取表字段，去掉基础字段
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public async Task<TData<List<TableFieldInfo>>> GetTableFieldPartList(string tableName)
        {
            TData<List<TableFieldInfo>> obj = new TData<List<TableFieldInfo>>();
            List<TableFieldInfo> list = await _databaseTableService.GetTableFieldList(tableName);
            obj.Data = list;
            obj.Data.RemoveAll(p => BaseField.BaseFieldList.Contains(p.TableColumn));
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<List<ZtreeInfo>>> GetTableFieldZtreeList(string tableName)
        {
            var obj = new TData<List<ZtreeInfo>>();
            obj.Data = new List<ZtreeInfo>();
            if (string.IsNullOrEmpty(tableName))
            {
                return obj;
            }
            List<TableFieldInfo> list = await _databaseTableService.GetTableFieldList(tableName);
            obj.Data.Add(new ZtreeInfo { id = 1, pId = 0, name = tableName });
            string sName = string.Empty;
            for (int i = 0; i < list.Count; i++)
            {
                sName = list[i].TableColumn;
                obj.Data.Add(new ZtreeInfo
                {
                    id = (i + 2),
                    pId = 1,
                    name = sName
                });
            }
            obj.Tag = 1;
            return obj;
        }

        #endregion

        #region 提交数据

        public async Task<string> DatabaseBackup(string backupPath)
        {
            string database = HtmlHelper.Resove(GlobalContext.SystemConfig.DbConnectionString.ToLower(), "database=", ";");
            await _databaseTableService.DatabaseBackup(database, backupPath);
            return backupPath;
        }

        public async Task<TData> SyncDatabase()
        {
            TData obj = new TData();
            await new DatabaseTableMySqlService().SyncDatabase();
            obj.Tag = 1;
            return obj;
        }

        #endregion
    }
}