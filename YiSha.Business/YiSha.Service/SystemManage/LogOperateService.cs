using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using YiSha.Data;
using YiSha.Data.Repository;
using YiSha.Entity.SystemManage;
using YiSha.Model;
using YiSha.Model.Param.SystemManage;
using YiSha.Util;
using YiSha.Util.Extension;
using YiSha.Util.Model;

namespace YiSha.Service.SystemManage
{
    public class LogOperateService : RepositoryFactory
    {
        #region 获取数据
        public async Task<List<LogOperateEntity>> GetList(LogOperateListParam param)
        {
            var strSql = new StringBuilder();
            List<DbParameter> filter = ListFilter(param, strSql);
            var list = await this.BaseRepository().FindList<LogOperateEntity>(strSql.ToString(), filter.ToArray());
            return list.ToList();
        }

        public async Task<List<LogOperateEntity>> GetPageList(LogOperateListParam param, Pagination pagination)
        {
            var strSql = new StringBuilder();
            List<DbParameter> filter = ListFilter(param, strSql);
            var list = await this.BaseRepository().FindList<LogOperateEntity>(strSql.ToString(), filter.ToArray(), pagination);
            return list.ToList();
        }

        public async Task<LogOperateEntity> GetEntity(long id)
        {
            return await this.BaseRepository().FindEntity<LogOperateEntity>(id);
        }
        #endregion

        #region 提交数据
        public async Task SaveForm(LogOperateEntity entity)
        {
            if (entity.Id.IsNullOrZero())
            {
                await entity.Create();
                await this.BaseRepository().Insert(entity);
            }
            else
            {
                await this.BaseRepository().Update(entity);
            }
        }

        public async Task DeleteForm(string ids)
        {
            long[] idArr = TextHelper.SplitToArray<long>(ids, ',');
            await this.BaseRepository().Delete<LogOperateEntity>(idArr);
        }

        public async Task RemoveAllForm()
        {
            await this.BaseRepository().ExecuteBySql("truncate table sys_log_operate");
        }
        #endregion

        #region 私有方法
        private List<DbParameter> ListFilter(LogOperateListParam param, StringBuilder strSql)
        {
            strSql.Append(@"SELECT  a.id as Id,
                                    a.base_create_time as BaseCreateTime,
                                    a.base_creator_id as BaseCreatorId,
                                    a.log_status as LogStatus,
                                    a.ip_address as IpAddress,
                                    a.ip_location as IpLocation,
                                    a.remark as Remark,
                                    a.execute_url as ExecuteUrl,
                                    a.execute_param as ExecuteParam,
                                    a.execute_result as ExecuteResult,
                                    a.execute_time as ExecuteTime,
                                    b.user_name as UserName,
                                    c.department_name as DepartmentName
                            FROM    sys_log_operate a
                                    LEFT JOIN sys_user b ON a.base_creator_id = b.id
                                    LEFT JOIN sys_department c ON b.department_id = c.id
                            WHERE   1 = 1");
            var parameter = new List<DbParameter>();
            if (param != null)
            {
                if (!string.IsNullOrEmpty(param.UserName))
                {
                    strSql.Append(" AND b.user_name like @UserName");
                    parameter.Add(DbParameterExtension.CreateDbParameter("@UserName", '%' + param.UserName + '%'));
                }
                if (param.LogStatus > -1)
                {
                    strSql.Append(" AND a.log_status = @LogStatus");
                    parameter.Add(DbParameterExtension.CreateDbParameter("@LogStatus", param.LogStatus));
                }
                if (!string.IsNullOrEmpty(param.ExecuteUrl))
                {
                    strSql.Append(" AND a.execute_url like @ExecuteUrl");
                    parameter.Add(DbParameterExtension.CreateDbParameter("@ExecuteUrl", '%' + param.ExecuteUrl + '%'));
                }
                if (!string.IsNullOrEmpty(param.StartTime.ParseToString()))
                {
                    strSql.Append(" AND a.base_create_time >= @StartTime");
                    parameter.Add(DbParameterExtension.CreateDbParameter("@StartTime", param.StartTime));
                }
                if (!string.IsNullOrEmpty(param.EndTime.ParseToString()))
                {
                    param.EndTime = (param.EndTime.Value.ToString("yyyy-MM-dd") + " 23:59:59").ParseToDateTime();
                    strSql.Append(" AND a.base_create_time <= @EndTime");
                    parameter.Add(DbParameterExtension.CreateDbParameter("@EndTime", param.EndTime));
                }
            }
            return parameter;
        }
        #endregion
    }
}
