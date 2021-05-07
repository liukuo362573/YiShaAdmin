﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YiSha.Data;
using YiSha.Data.Repository;
using YiSha.Model.Result.SystemManage;
using YiSha.Util;
using YiSha.Util.Extension;
using YiSha.Util.Model;

namespace YiSha.Service.SystemManage
{
    public class DatabaseTableSqlServerService : RepositoryFactory, IDatabaseTableService
    {
        #region 获取数据
        public async Task<List<TableInfo>> GetTableList(string tableName)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT id Id,name TableName FROM sysobjects WHERE xtype = 'u' order by name");
            IEnumerable<TableInfo> list = await this.BaseRepository().FindList<TableInfo>(strSql.ToString());
            if (!tableName.IsEmpty())
            {
                list = list.Where(p => p.TableName.Contains(tableName));
            }
            await SetTableDetail(list);
            return list.ToList();
        }

        public async Task<List<TableInfo>> GetTablePageList(string tableName, Pagination pagination)
        {
            StringBuilder strSql = new StringBuilder();
            var parameter = new List<DbParameter>();
            strSql.Append(@"SELECT id Id,name TableName FROM sysobjects WHERE xtype = 'u'");

            if (!tableName.IsEmpty())
            {
                strSql.Append(" AND name like @TableName ");
                parameter.Add(DbParameterHelper.CreateDbParameter("@TableName", '%' + tableName + '%'));
            }

            IEnumerable<TableInfo> list = await this.BaseRepository().FindList<TableInfo>(strSql.ToString(), parameter.ToArray(), pagination);
            await SetTableDetail(list);
            return list.ToList();
        }

        public async Task<List<TableFieldInfo>> GetTableFieldList(string tableName)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT  
                                  TableColumn = rtrim(b.name),  
                                  TableIdentity = CASE WHEN h.id IS NOT NULL  THEN 'PK' ELSE '' END,  
                                  Datatype = type_name(b.xusertype)+CASE WHEN b.colstat&1=1 THEN '[ID(' + CONVERT(varchar, ident_seed(a.name))+','+CONVERT(varchar,ident_incr(a.name))+')]' ELSE '' END,  
                                  FieldLength = b.length,   
                                  IsNullable = CASE b.isnullable WHEN 0 THEN 'N' ELSE 'Y' END,   
                                  FieldDefault = ISNULL(e.text, ''),
                                  Remark = (SELECT ep.value FROM sys.columns sc LEFT JOIN sys.extended_properties ep ON ep.major_id = sc.object_id AND ep.minor_id = sc.column_id
										                    WHERE sc.object_id = a.id AND sc.name = b.name)
                            FROM sysobjects a, syscolumns b  
                            LEFT OUTER JOIN syscomments e ON b.cdefault = e.id  
                            LEFT OUTER JOIN (Select g.id, g.colid FROM sysindexes f, sysindexkeys g Where (f.id=g.id)AND(f.indid=g.indid)AND(f.indid>0)AND(f.indid<255)AND(f.status&2048)<>0) h ON (b.id=h.id)AND(b.colid=h.colid)  
                            Where (a.id=b.id)AND(a.id=object_id(@TableName))   
                                  ORDER BY b.colid");
            var parameter = new List<DbParameter>();
            parameter.Add(DbParameterHelper.CreateDbParameter("@TableName", tableName));
            var list = await this.BaseRepository().FindList<TableFieldInfo>(strSql.ToString(), parameter.ToArray());
            return list.ToList();
        }
        #endregion

        #region 公有方法
        public async Task<bool> DatabaseBackup(string database, string backupPath)
        {
            string backupFile = string.Format("{0}\\{1}_{2}.bak", backupPath, database, DateTime.Now.ToString("yyyyMMddHHmmss"));
            string strSql = string.Format(" backup database [{0}] to disk = '{1}'", database, backupFile);
            var result = await this.BaseRepository().ExecuteBySql(strSql);
            return result > 0 ? true : false;
        }
        #endregion

        #region 私有方法
        /// <summary>
        /// 获取所有表的主键、主键名称、记录数
        /// </summary>
        /// <returns></returns>
        private async Task<List<TableInfo>> GetTableDetailList()
        {
            string strSql = @"SELECT (SELECT name FROM sysobjects as t WHERE xtype = 'U' and t.id = sc.id) TableName,
                                     sc.id Id,sc.name TableKey,sysobjects.name TableKeyName,sysindexes.rows TableCount
                                     FROM syscolumns sc ,sysobjects,sysindexes,sysindexkeys 
                                     WHERE sysobjects.xtype = 'PK' 
                                           AND sysobjects.parent_obj = sc.id 
                                           AND sysindexes.id = sc.id 
                                           AND sysobjects.name = sysindexes.name AND sysindexkeys.id = sc.id 
                                           AND sysindexkeys.indid = sysindexes.indid 
                                           AND sc.colid = sysindexkeys.colid;";

            IEnumerable<TableInfo> list = await this.BaseRepository().FindList<TableInfo>(strSql.ToString());
            return list.ToList();
        }

        /// <summary>
        /// 赋值表的主键、主键名称、记录数
        /// </summary>
        /// <param name="list"></param>
        private async Task SetTableDetail(IEnumerable<TableInfo> list)
        {
            List<TableInfo> detailList = await GetTableDetailList();
            foreach (TableInfo table in list)
            {
                table.TableKey = string.Join(",", detailList.Where(p => p.Id == table.Id).Select(p => p.TableKey));
                var tableInfo = detailList.Where(p => p.TableName == table.TableName).FirstOrDefault();
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
