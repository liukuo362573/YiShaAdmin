using System.Text;
using YiSha.DataBase;
using YiSha.Enum.OrganizationManage;
using YiSha.Util;
using YiSha.Util.Extension;

namespace YiSha.Web.Code
{
    public class DataRepository : Repository
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
            var operatorInfo = await this.FindObject<OperatorInfo>(strSql.ToString());
            if (operatorInfo != null)
            {
                #region 角色
                strSql.Clear();
                strSql.Append(@"SELECT  a.BelongId as RoleId
                                FROM    SysUserBelong a
                                WHERE   a.UserId = " + operatorInfo.UserId + " AND ");
                strSql.Append("         a.BelongType = " + UserBelongTypeEnum.Role.ParseToInt());
                IEnumerable<RoleInfo> roleList = await this.FindList<RoleInfo>(strSql.ToString());
                operatorInfo.RoleIds = string.Join(",", roleList.Select(p => p.RoleId).ToArray());
                #endregion

                #region 部门名称
                strSql.Clear();
                strSql.Append(@"SELECT  a.DepartmentName
                                FROM    SysDepartment a
                                WHERE   a.Id = " + operatorInfo.DepartmentId);
                object departmentName = await this.FindObject(strSql.ToString());
                operatorInfo.DepartmentName = departmentName.ParseToString();
                #endregion
            }
            return operatorInfo;
        }

    }
}
