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
    public class LogLoginService : RepositoryFactory
    {
        #region 获取数据

        public async Task<List<LogLoginEntity>> GetList(LogLoginListParam param)
        {
            var builder = new StringBuilder();
            var filter = ListFilter(param, builder).ToArray();
            return await BaseRepository().FindList<LogLoginEntity>(builder.ToString(), filter);
        }

        public async Task<List<LogLoginEntity>> GetPageList(LogLoginListParam param, Pagination pagination)
        {
            var builder = new StringBuilder();
            var filter = ListFilter(param, builder).ToArray();
            return await BaseRepository().FindList<LogLoginEntity>(builder.ToString(), pagination, filter);
        }

        public async Task<LogLoginEntity> GetEntity(long id)
        {
            return await BaseRepository().FindEntity<LogLoginEntity>(id);
        }

        #endregion

        #region 提交数据

        public async Task SaveForm(LogLoginEntity entity)
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
            var idArr = TextHelper.SplitToArray<long>(ids, ',');
            await BaseRepository().Delete<LogLoginEntity>(idArr);
        }

        public async Task RemoveAllForm()
        {
            await BaseRepository().ExecuteBySql("truncate table SysLogLogin");
        }

        #endregion

        #region 私有方法

        private List<DbParameter> ListFilter(LogLoginListParam param, StringBuilder builder)
        {
            builder.Append(@"SELECT  a.Id,
                                    a.BaseCreateTime,
                                    a.BaseCreatorId,
                                    a.LogStatus,
                                    a.IpAddress,
                                    a.IpLocation,
                                    a.Browser,
                                    a.OS,
                                    a.Remark,
                                    b.UserName
                            FROM    SysLogLogin a
                                    LEFT JOIN SysUser b ON a.BaseCreatorId = b.Id
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
                if (param.IpAddress?.Length > 0)
                {
                    builder.Append(" AND a.IpAddress like @IpAddress");
                    parameter.Add(DbParameterHelper.CreateDbParameter("@IpAddress", '%' + param.IpAddress + '%'));
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