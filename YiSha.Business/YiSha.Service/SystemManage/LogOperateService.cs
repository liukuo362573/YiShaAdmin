using System.Collections.Generic;
using System.Data.Common;
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
    public class LogOperateService : RepositoryFactory
    {
        #region 获取数据

        public async Task<List<LogOperateEntity>> GetList(LogOperateListParam param)
        {
            var builder = new StringBuilder();
            var filter = ListFilter(param, builder).ToArray();
            return await BaseRepository().FindList<LogOperateEntity>(builder.ToString(), filter);
        }

        public async Task<List<LogOperateEntity>> GetPageList(LogOperateListParam param, Pagination pagination)
        {
            var builder = new StringBuilder();
            var filter = ListFilter(param, builder).ToArray();
            return await BaseRepository().FindList<LogOperateEntity>(builder.ToString(), pagination, filter);
        }

        public async Task<LogOperateEntity> GetEntity(long id)
        {
            return await BaseRepository().FindEntity<LogOperateEntity>(id);
        }

        #endregion

        #region 提交数据

        public async Task SaveForm(LogOperateEntity entity)
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
            var idArr = TextHelper.SplitToArray<object>(ids, ',');
            await BaseRepository().Delete<LogOperateEntity>(idArr);
        }

        public async Task RemoveAllForm()
        {
            await BaseRepository().ExecuteBySql("truncate table SysLogOperate");
        }

        #endregion

        #region 私有方法

        private List<DbParameter> ListFilter(LogOperateListParam param, StringBuilder builder)
        {
            builder.Append(@"SELECT  a.Id,
                                    a.BaseCreateTime,
                                    a.BaseCreatorId,
                                    a.LogStatus,
                                    a.IpAddress,
                                    a.IpLocation,
                                    a.Remark,
                                    a.ExecuteUrl,
                                    a.ExecuteParam,
                                    a.ExecuteResult,
                                    a.ExecuteTime,
                                    b.UserName,
                                    c.DepartmentName
                            FROM    SysLogOperate a
                                    LEFT JOIN SysUser b ON a.BaseCreatorId = b.Id
                                    LEFT JOIN SysDepartment c ON b.DepartmentId = c.Id
                            WHERE   1 = 1");
            var parameter = new List<DbParameter>();
            if (param != null)
            {
                if (param.UserName?.Length > 0)
                {
                    builder.Append(" AND b.UserName like @UserName");
                    parameter.Add(DbParameterHelper.CreateDbParameter("@UserName", '%' + param.UserName + '%'));
                }
                if (param.LogStatus > -1)
                {
                    builder.Append(" AND a.LogStatus = @LogStatus");
                    parameter.Add(DbParameterHelper.CreateDbParameter("@LogStatus", param.LogStatus));
                }
                if (param.ExecuteUrl?.Length > 0)
                {
                    builder.Append(" AND a.ExecuteUrl like @ExecuteUrl");
                    parameter.Add(DbParameterHelper.CreateDbParameter("@ExecuteUrl", '%' + param.ExecuteUrl + '%'));
                }
                if (param.StartTime.ParseToString()?.Length > 0)
                {
                    builder.Append(" AND a.BaseCreateTime >= @StartTime");
                    parameter.Add(DbParameterHelper.CreateDbParameter("@StartTime", param.StartTime));
                }
                if (param.EndTime.ParseToString()?.Length > 0)
                {
                    param.EndTime = (param.EndTime?.ToString("yyyy-MM-dd") + " 23:59:59").ParseToDateTime();
                    builder.Append(" AND a.BaseCreateTime <= @EndTime");
                    parameter.Add(DbParameterHelper.CreateDbParameter("@EndTime", param.EndTime));
                }
            }
            return parameter;
        }

        #endregion
    }
}