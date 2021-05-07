using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YiSha.Data.Extension;
using YiSha.Data.Repository;
using YiSha.Model.Result.SystemManage;
using YiSha.Util.Extension;
using YiSha.Util.Model;

namespace YiSha.Service.SystemManage
{
    public class DatabaseTableSqlServerService : RepositoryFactory, IDatabaseTableService
    {
        #region 获取数据

        public async Task<List<TableInfo>> GetTableList(string tableName)
        {
            var builder = new StringBuilder();
            builder.Append(@"SELECT id Id,name TableName FROM sysobjects WHERE xtype = 'u' order by name");
            IEnumerable<TableInfo> list = await BaseRepository().FindList<TableInfo>(builder.ToString());
            if (!tableName.IsEmpty())
            {
                list = list.Where(p => p.TableName.Contains(tableName));
            }
            await SetTableDetail(list);
            return list.ToList();
        }

        public async Task<List<TableInfo>> GetTablePageList(string tableName, Pagination pagination)
        {
            var builder = new StringBuilder();
            var parameter = new List<DbParameter>();
            builder.Append("SELECT id Id,name TableName FROM sysobjects WHERE xtype = 'u'");

            if (!tableName.IsEmpty())
            {
                builder.Append(" AND name like @TableName ");
                parameter.Add(DbParameterHelper.CreateDbParameter("@TableName", '%' + tableName + '%'));
            }

            IEnumerable<TableInfo> list = await BaseRepository().FindList<TableInfo>(builder.ToString(), pagination, parameter.ToArray());
            await SetTableDetail(list);
            return list.ToList();
        }

        public async Task<List<TableFieldInfo>> GetTableFieldList(string tableName)
        {
            var builder = new StringBuilder();
            builder.Append(@"
            SELECT
                TableColumn = rtrim(b.name),
                TableIdentity = CASE
                    WHEN h.id IS NOT NULL THEN 'PK'
                    ELSE ''
                END,
                Datatype = type_name(b.xusertype) +CASE
                    WHEN b.colstat & 1 = 1 THEN '[ID(' + CONVERT(varchar, ident_seed(a.name)) + ',' + CONVERT(varchar, ident_incr(a.name)) + ')]'
                    ELSE ''
                END,
                FieldLength = b.length,
                IsNullable = CASE
                    b.isnullable
                    WHEN 0 THEN 'N'
                    ELSE 'Y'
                END,
                FieldDefault = ISNULL(e.text, ''),
                Remark = (
                    SELECT
                        ep.value
                    FROM
                        sys.columns sc
                        LEFT JOIN sys.extended_properties ep ON ep.major_id = sc.object_id
                        AND ep.minor_id = sc.column_id
                    WHERE
                        sc.object_id = a.id
                        AND sc.name = b.name
                )
            FROM
                sysobjects a,
                syscolumns b
                LEFT OUTER JOIN syscomments e ON b.cdefault = e.id
                LEFT OUTER JOIN (
                    Select
                        g.id,
                        g.colid
                    FROM
                        sysindexes f,
                        sysindexkeys g
                    Where
                        (f.id = g.id)
                        AND(f.indid = g.indid)
                        AND(f.indid > 0)
                        AND(f.indid < 255)
                        AND(f.status & 2048) <> 0
                ) h ON (b.id = h.id)
                AND(b.colid = h.colid)
            Where
                (a.id = b.id)
                AND(a.id = object_id(@TableName))
            ORDER BY
                b.colid");
            return await BaseRepository().FindList<TableFieldInfo>(builder.ToString(), DbParameterHelper.CreateDbParameter("@TableName", tableName));
        }

        #endregion

        #region 公有方法

        public async Task<bool> DatabaseBackup(string database, string backupPath)
        {
            string backupFile = $"{backupPath}\\{database}_{DateTime.Now:yyyyMMddHHmmss}.bak";
            string strSql = $" backup database [{database}] to disk = '{backupFile}'";
            return await BaseRepository().ExecuteBySql(strSql) > 0;
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 获取所有表的主键、主键名称、记录数
        /// </summary>
        private async Task<List<TableInfo>> GetTableDetailList()
        {
            string sql = @"
            SELECT
                (
                    SELECT
                        name
                    FROM
                        sysobjects as t
                    WHERE
                        xtype = 'U'
                        and t.id = sc.id
                ) TableName,
                sc.id Id,
                sc.name TableKey,
                sysobjects.name TableKeyName,
                sysindexes.rows TableCount
            FROM
                syscolumns sc,
                sysobjects,
                sysindexes,
                sysindexkeys
            WHERE
                sysobjects.xtype = 'PK'
                AND sysobjects.parent_obj = sc.id
                AND sysindexes.id = sc.id
                AND sysobjects.name = sysindexes.name
                AND sysindexkeys.id = sc.id
                AND sysindexkeys.indid = sysindexes.indid
                AND sc.colid = sysindexkeys.colid";
            return await BaseRepository().FindList<TableInfo>(sql);
        }

        /// <summary>
        /// 赋值表的主键、主键名称、记录数
        /// </summary>
        private async Task SetTableDetail(IEnumerable<TableInfo> list)
        {
            var detailList = await GetTableDetailList();
            foreach (var table in list)
            {
                table.TableKey = string.Join(",", detailList.Where(p => p.Id == table.Id).Select(p => p.TableKey));
                var tableInfo = detailList.FirstOrDefault(p => p.TableName == table.TableName);
                if (tableInfo != null)
                {
                    table.TableKeyName = tableInfo.TableKeyName;
                    table.TableCount = tableInfo.TableCount;
                    table.Remark = tableInfo.Remark;
                }
            }
        }

        #endregion
    }
}