using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YiSha.Data.EF.Database;
using YiSha.Data.Extension;
using YiSha.Data.Repository;
using YiSha.Entity.OrganizationManage;
using YiSha.Entity.SystemManage;
using YiSha.Model.Result.SystemManage;
using YiSha.Util.Extension;
using YiSha.Util.Helper;
using YiSha.Util.Model;

namespace YiSha.Service.SystemManage
{
    public class DatabaseTableMySqlService : RepositoryFactory, IDatabaseTableService
    {
        #region 获取数据

        public async Task<List<TableInfo>> GetTableList(string tableName)
        {
            var builder = new StringBuilder("SELECT table_name TableName FROM information_schema.tables ");
            builder.Append($"WHERE table_schema='{GetDatabase()}' AND table_type='base table'");
            IEnumerable<TableInfo> list = await BaseRepository().FindList<TableInfo>(builder.ToString());
            if (tableName.TryAny())
            {
                list = list.Where(p => p.TableName.Contains(tableName));
            }
            await SetTableDetail(list);
            return list.ToList();
        }

        public async Task<List<TableInfo>> GetTablePageList(string tableName, Pagination pagination)
        {
            var builder = new StringBuilder("SELECT table_name TableName FROM information_schema.tables ");
            builder.Append($"WHERE table_schema='{GetDatabase()}' AND (table_type='base table' OR table_type='BASE TABLE')");

            var parameter = new List<DbParameter>();
            if (tableName?.Length > 0)
            {
                builder.Append(" AND table_name like @TableName ");
                parameter.Add(DbParameterHelper.CreateDbParameter("@TableName", '%' + tableName + '%'));
            }

            IEnumerable<TableInfo> list = await BaseRepository().FindList<TableInfo>(builder.ToString(), pagination, parameter.ToArray());
            await SetTableDetail(list);
            return list.ToList();
        }

        public async Task<List<TableFieldInfo>> GetTableFieldList(string tableName)
        {
            string sql = $@"
            SELECT
                COLUMN_NAME TableColumn,
                DATA_TYPE Datatype,
                (
                    CASE COLUMN_KEY
                        WHEN 'PRI' THEN COLUMN_NAME
                        ELSE ''
                    END
                ) TableIdentity,
                REPLACE(
                    REPLACE(
                        SUBSTRING(COLUMN_TYPE, LOCATE('(', COLUMN_TYPE)),
                        '(', ''
                    ), ')', ''
                ) FieldLength,
                (
                    CASE IS_NULLABLE
                        WHEN 'NO' THEN 'N'
                        ELSE 'Y'
                    END
                ) IsNullable,
                COALESCE(COLUMN_DEFAULT, '') FieldDefault,
                COLUMN_COMMENT Remark
            FROM
                information_schema.columns
            WHERE
                table_schema = '{GetDatabase()}'
                AND table_name = @TableName";
            var dbParameter = DbParameterHelper.CreateDbParameter("@TableName", tableName);
            return await BaseRepository().FindList<TableFieldInfo>(sql, dbParameter);
        }

        #endregion

        #region 公有方法

        public async Task<bool> DatabaseBackup(string database, string backupPath)
        {
            string backupFile = $"{backupPath}\\{database}_{DateTime.Now:yyyyMMddHHmmss}.bak";
            string sql = $" backup database [{database}] to disk = '{backupFile}'";
            return await BaseRepository().ExecuteBySql(sql) > 0;
        }

        /// <summary>
        /// 仅用在YiShaAdmin框架里面，同步不同数据库之间的数据，以 MySql 为主库，同步 MySql 的数据到SqlServer和Oracle，保证各个数据库的数据是一样的
        /// </summary>
        public async Task SyncDatabase()
        {
            #region 同步SqlServer数据库

            await SyncSqlServerTable<AreaEntity>();
            await SyncSqlServerTable<AutoJobEntity>();
            await SyncSqlServerTable<AutoJobLogEntity>();
            await SyncSqlServerTable<DataDictEntity>();
            await SyncSqlServerTable<DataDictDetailEntity>();
            await SyncSqlServerTable<DepartmentEntity>();
            await SyncSqlServerTable<LogLoginEntity>();
            await SyncSqlServerTable<MenuEntity>();
            await SyncSqlServerTable<MenuAuthorizeEntity>();
            await SyncSqlServerTable<NewsEntity>();
            await SyncSqlServerTable<PositionEntity>();
            await SyncSqlServerTable<RoleEntity>();
            await SyncSqlServerTable<UserEntity>();
            await SyncSqlServerTable<UserBelongEntity>();

            #endregion
        }

        private async Task SyncSqlServerTable<T>() where T : class, new()
        {
            string sqlServerConnectionString = "Server=localhost;Database=YiShaAdmin;User Id=sa;Password=123456;";
            var list = await BaseRepository().FindList<T>();
            await new SqlServerDatabase(sqlServerConnectionString).Delete<T>(p => true);
            await new SqlServerDatabase(sqlServerConnectionString).Insert(list);
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 获取所有表的主键、主键名称、记录数
        /// </summary>
        private async Task<List<TableInfo>> GetTableDetailList()
        {
            string sql = $@"
            SELECT
                t1.TABLE_NAME TableName,
                t1.TABLE_COMMENT Remark,
                t1.TABLE_ROWS TableCount,
                t2.CONSTRAINT_NAME TableKeyName,
                t2.column_name TableKey
            FROM
                information_schema.TABLES t1
                LEFT JOIN INFORMATION_SCHEMA.`KEY_COLUMN_USAGE` t2 ON t1.TABLE_NAME = t2.TABLE_NAME
            WHERE
                t1.TABLE_SCHEMA = '{GetDatabase()}'
                AND t2.TABLE_SCHEMA = '{GetDatabase()}'";

            IEnumerable<TableInfo> list = await BaseRepository().FindList<TableInfo>(sql);
            return list.ToList();
        }

        /// <summary>
        /// 赋值表的主键、主键名称、记录数
        /// </summary>
        private async Task SetTableDetail(IEnumerable<TableInfo> list)
        {
            var detailList = await GetTableDetailList();
            foreach (var table in list)
            {
                table.TableKey = string.Join(",", detailList.Where(p => p.TableName == table.TableName).Select(p => p.TableKey));
                var tableInfo = detailList.FirstOrDefault(p => p.TableName == table.TableName);
                if (tableInfo != null)
                {
                    table.TableKeyName = tableInfo.TableKeyName;
                    table.TableCount = tableInfo.TableCount;
                    table.Remark = tableInfo.Remark;
                }
            }
        }

        private string GetDatabase()
        {
            return HtmlHelper.Resove(GlobalContext.SystemConfig.DbConnectionString, "database=", ";");
        }

        #endregion
    }
}