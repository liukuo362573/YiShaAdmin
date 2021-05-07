using System;
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
            _databaseTableService = GlobalContext.SystemConfig.DbProvider switch
            {
                "MySql" => new DatabaseTableMySqlService(),
                "Oracle" => new DatabaseTableOracleService(),
                "SqlServer" => new DatabaseTableSqlServerService(),
                _ => throw new Exception("未找到数据库配置")
            };
        }

        #endregion

        #region 获取数据

        public async Task<TData<List<TableInfo>>> GetTableList(string tableName)
        {
            var list = await _databaseTableService.GetTableList(tableName);
            return new() { Data = list, Total = list.Count, Tag = 1 };
        }

        public async Task<TData<List<TableInfo>>> GetTablePageList(string tableName, Pagination pagination)
        {
            return new()
            {
                Data = await _databaseTableService.GetTablePageList(tableName, pagination),
                Total = pagination.TotalCount,
                Tag = 1
            };
        }

        /// <summary>
        /// 获取表字段
        /// </summary>
        public async Task<TData<List<TableFieldInfo>>> GetTableFieldList(string tableName)
        {
            var list = await _databaseTableService.GetTableFieldList(tableName);
            return new() { Data = list, Total = list.Count, Tag = 1 };
        }

        /// <summary>
        /// 获取表字段，去掉基础字段
        /// </summary>
        public async Task<TData<List<TableFieldInfo>>> GetTableFieldPartList(string tableName)
        {
            var list = await _databaseTableService.GetTableFieldList(tableName);
            list.RemoveAll(p => BaseField.BaseFieldList.Contains(p.TableColumn));
            return new() { Data = list, Tag = 1 };
        }

        public async Task<TData<List<ZtreeInfo>>> GetTableFieldZtreeList(string tableName)
        {
            if (string.IsNullOrEmpty(tableName))
            {
                return new();
            }
            var list = await _databaseTableService.GetTableFieldList(tableName);
            var treeList = new List<ZtreeInfo> { new() { id = 1, pId = 0, name = tableName } };
            treeList.AddRange(list.Select(t => t.TableColumn).Select((s, i) => new ZtreeInfo
            {
                id = i + 2,
                pId = 1,
                name = s
            }));
            return new() { Data = treeList, Tag = 1 };
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
            await new DatabaseTableMySqlService().SyncDatabase();
            return new() { Tag = 1 };
        }

        #endregion
    }
}