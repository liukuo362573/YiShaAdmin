using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YiSha.Data.Repository;
using YiSha.Enum.OrganizationManage;
using YiSha.Util;
using YiSha.Util.Extension;

namespace YiSha.Web.Code
{
    public class DataRepository : RepositoryFactory
    {
        public async Task<OperatorInfo> GetUserByToken(string token)
        {
            if (!SecurityHelper.IsSafeSqlParam(token))
            {
                return null;
            }
            token = token.ParseToString().Trim();

            var strSql = new StringBuilder();
            strSql.Append(@"SELECT  a.Id as UserId,
                                    a.UserStatus,
                                    a.IsOnline,
                                    a.UserName,
                                    a.RealName,
                                    a.Portrait,
                                    a.DepartmentId,
                                    a.WebToken,
                                    a.ApiToken,
                                    a.IsSystem
                            FROM    SysUser a
                            WHERE   WebToken = '" + token + "' or ApiToken = '" + token + "'  ");
            var operatorInfo = await BaseRepository().FindObject<OperatorInfo>(strSql.ToString());
            if (operatorInfo != null)
            {
                #region 角色
                strSql.Clear();
                strSql.Append(@"SELECT  a.BelongId as RoleId
                                FROM    SysUserBelong a
                                WHERE   a.UserId = " + operatorInfo.UserId + " AND ");
                strSql.Append("         a.BelongType = " + UserBelongTypeEnum.Role.ParseToInt());
                IEnumerable<RoleInfo> roleList = await BaseRepository().FindList<RoleInfo>(strSql.ToString());
                operatorInfo.RoleIds = string.Join(",", roleList.Select(p => p.RoleId).ToArray());
                #endregion

                #region 部门名称
                strSql.Clear();
                strSql.Append(@"SELECT  a.DepartmentName
                                FROM    SysDepartment a
                                WHERE   a.Id = " + operatorInfo.DepartmentId);
                object departmentName = await BaseRepository().FindObject(strSql.ToString());
                operatorInfo.DepartmentName = departmentName.ParseToString();
                #endregion
            }
            return operatorInfo;
        }

    }
}
