using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YiSha.Data.Extension;
using YiSha.Data.Repository;
using YiSha.Entity.SystemManage;
using YiSha.Model.Param.SystemManage;
using YiSha.Util.Extension;
using YiSha.Util.Helper;
using YiSha.Util.Model;

namespace YiSha.Service.SystemManage
{
    public class LogApiService : RepositoryFactory
    {
        #region 获取数据

        public async Task<List<LogApiEntity>> GetList(LogApiListParam param)
        {
            var strSql = new StringBuilder();
            List<DbParameter> filter = ListFilter(param, strSql);
            var list = await BaseRepository().FindList<LogApiEntity>(strSql.ToString(), filter.ToArray());
            return list.ToList();
        }

        public async Task<List<LogApiEntity>> GetPageList(LogApiListParam param, Pagination pagination)
        {
            var strSql = new StringBuilder();
            List<DbParameter> filter = ListFilter(param, strSql);
            var list = await BaseRepository().FindList<LogApiEntity>(strSql.ToString(), filter.ToArray(), pagination);
            return list.ToList();
        }

        public async Task<LogApiEntity> GetEntity(long id)
        {
            return await BaseRepository().FindEntity<LogApiEntity>(id);
        }

        #endregion

        #region 提交数据

        public async Task SaveForm(LogApiEntity entity)
        {
            if (entity.Id.IsNullOrZero())
            {
                await entity.Create();
                await BaseRepository().Insert(entity);
            }
            else
            {
                await BaseRepository().Update(entity);
            }
        }

        public async Task DeleteForm(string ids)
        {
            long[] idArr = TextHelper.SplitToArray<long>(ids, ',');
            await BaseRepository().Delete<LogApiEntity>(idArr);
        }

        public async Task RemoveAllForm()
        {
            await BaseRepository().ExecuteBySql("truncate table SysLogApi");
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
                    parameter.Add(DbParameterHelper.CreateDbParameter("@UserName", '%' + param.UserName + '%'));
                }
                if (param.LogStatus > -1)
                {
                    strSql.Append(" AND a.LogStatus = @LogStatus");
                    parameter.Add(DbParameterHelper.CreateDbParameter("@LogStatus", param.LogStatus));
                }
                if (!string.IsNullOrEmpty(param.ExecuteUrl))
                {
                    strSql.Append(" AND a.ExecuteUrl like @ExecuteUrl");
                    parameter.Add(DbParameterHelper.CreateDbParameter("@ExecuteUrl", '%' + param.ExecuteUrl + '%'));
                }
                if (!string.IsNullOrEmpty(param.StartTime.ParseToString()))
                {
                    strSql.Append(" AND a.BaseCreateTime >= @StartTime");
                    parameter.Add(DbParameterHelper.CreateDbParameter("@StartTime", param.StartTime));
                }
                if (!string.IsNullOrEmpty(param.EndTime.ParseToString()))
                {
                    param.EndTime = param.EndTime.Value.Date.Add(new TimeSpan(23, 59, 59));
                    strSql.Append(" AND a.BaseCreateTime <= @EndTime");
                    parameter.Add(DbParameterHelper.CreateDbParameter("@EndTime", param.EndTime));
                }
            }
            return parameter;
        }

        #endregion
    }
}