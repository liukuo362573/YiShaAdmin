using System;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Threading.Tasks;
using YiSha.Util;
using YiSha.Util.Extension;
using YiSha.Util.Model;
using YiSha.Data.Repository;
using YiSha.Entity.SystemManage;
using YiSha.Model.Param.SystemManage;
using System.Text;
using System.Data.Common;
using YiSha.Data;

namespace YiSha.Service.SystemManage
{
    public class LogApiService : RepositoryFactory
    {
        #region 获取数据
        public async Task<List<LogApiEntity>> GetList(LogApiListParam param)
        {
            var strSql = new StringBuilder();
            List<DbParameter> filter = ListFilter(param, strSql);
            var list = await this.BaseRepository().FindList<LogApiEntity>(strSql.ToString(), filter.ToArray());
            return list.ToList();
        }

        public async Task<List<LogApiEntity>> GetPageList(LogApiListParam param, Pagination pagination)
        {
            var strSql = new StringBuilder();
            List<DbParameter> filter = ListFilter(param, strSql);
            var list = await this.BaseRepository().FindList<LogApiEntity>(strSql.ToString(), filter.ToArray(), pagination);
            return list.ToList();
        }

        public async Task<LogApiEntity> GetEntity(long id)
        {
            return await this.BaseRepository().FindEntity<LogApiEntity>(id);
        }
        #endregion

        #region 提交数据
        public async Task SaveForm(LogApiEntity entity)
        {
            if (entity.Id.IsNullOrZero())
            {
                await entity.Create();
                await this.BaseRepository().Insert<LogApiEntity>(entity);
            }
            else
            {
                await this.BaseRepository().Update<LogApiEntity>(entity);
            }
        }

        public async Task DeleteForm(string ids)
        {
            long[] idArr = TextHelper.SplitToArray<long>(ids, ',');
            await this.BaseRepository().Delete<LogApiEntity>(idArr);
        }

        public async Task RemoveAllForm()
        {
            await this.BaseRepository().ExecuteBySql("truncate table SysLogApi");
        }
        #endregion

        #region 私有方法
        private List<DbParameter> ListFilter(LogApiListParam param, StringBuilder strSql)
        {
            strSql.Append(@"SELECT  a.Id,
                                    a.BaseCreateTime,
                                    a.BaseCreatorId,
                                    a.LogStatus,
                                    a.Remark,
                                    a.ExecuteUrl,
                                    a.ExecuteParam,
                                    a.ExecuteResult,
                                    a.ExecuteTime,
                                    b.UserName,
                                    c.DepartmentName
                            FROM    SysLogApi a
                                    LEFT JOIN SysUser b ON a.BaseCreatorId = b.Id
                                    LEFT JOIN SysDepartment c ON b.DepartmentId = c.Id
                            WHERE   1 = 1");
            var parameter = new List<DbParameter>();
            if (param != null)
            {
                if (!string.IsNullOrEmpty(param.UserName))
                {
                    strSql.Append(" AND b.UserName like @UserName");
                    parameter.Add(DbParameterExtension.CreateDbParameter("@UserName", '%' + param.UserName + '%'));
                }
                if (param.LogStatus > -1)
                {
                    strSql.Append(" AND a.LogStatus = @LogStatus");
                    parameter.Add(DbParameterExtension.CreateDbParameter("@LogStatus", param.LogStatus));
                }
                if (!string.IsNullOrEmpty(param.ExecuteUrl))
                {
                    strSql.Append(" AND a.ExecuteUrl like @ExecuteUrl");
                    parameter.Add(DbParameterExtension.CreateDbParameter("@ExecuteUrl", '%' + param.ExecuteUrl + '%'));
                }
                if (!string.IsNullOrEmpty(param.StartTime.ParseToString()))
                {
                    strSql.Append(" AND a.BaseCreateTime >= @StartTime");
                    parameter.Add(DbParameterExtension.CreateDbParameter("@StartTime", param.StartTime));
                }
                if (!string.IsNullOrEmpty(param.EndTime.ParseToString()))
                {
                    param.EndTime = param.EndTime.Value.Date.Add(new TimeSpan(23, 59, 59));
                    strSql.Append(" AND a.BaseCreateTime <= @EndTime");
                    parameter.Add(DbParameterExtension.CreateDbParameter("@EndTime", param.EndTime));
                }
            }
            return parameter;
        }
        #endregion
    }
}
