using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YiSha.Entity;
using YiSha.Model;
using YiSha.Model.Entity.OrganizationManage;
using YiSha.Model.Operator;
using YiSha.Util;

namespace YiSha.Admin.WebApi.Controllers
{
    /// <summary>
    /// 用户数据控制器
    /// </summary>
    [Authorize]
    [Route("[controller]/[action]")]
    public class UserController : Controller
    {
        /// <summary>
        /// 数据库上下文
        /// </summary>
        private MyDbContext DbCmd { get; }

        /// <summary>
        /// 用户数据控制器
        /// </summary>
        public UserController(MyDbContext myDbContext)
        {
            this.DbCmd = myDbContext;
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="userName">用户名称</param>
        /// <param name="password">用户密码</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login(string? userName = default, string? password = default)
        {
            var obj = new TData<OperatorInfo>();
            if (userName.IsNull() || password.IsNull())
            {
                obj.Message = "用户名或密码不能为空";
                return Json(obj);
            }

            

            var linqData = (from suser in DbCmd.SysUser
                            where suser.UserName == userName || suser.Mobile == userName || suser.Email == userName
                            select new UserEntity
                            {
                                Id = suser.Id,
                                BaseIsDelete = suser.BaseIsDelete,
                                BaseCreateTime = suser.BaseCreateTime,
                                BaseModifyTime = suser.BaseModifyTime,
                                BaseCreatorId = suser.BaseCreatorId,
                                BaseModifierId = suser.BaseModifierId,
                                BaseVersion = suser.BaseVersion,
                                UserName = suser.UserName,
                                Password = suser.Password,
                                Salt = suser.Salt,
                                RealName = suser.RealName,
                                DepartmentId = suser.DepartmentId,
                                Gender = suser.Gender,
                                Birthday = suser.Birthday,
                                Portrait = suser.Portrait,
                                Email = suser.Email,
                                Mobile = suser.Mobile,
                                QQ = suser.QQ,
                                WeChat = suser.WeChat,
                                LoginCount = suser.LoginCount,
                                UserStatus = suser.UserStatus,
                                IsSystem = suser.IsSystem,
                                IsOnline = suser.IsOnline,
                                FirstVisit = suser.FirstVisit,
                                PreviousVisit = suser.PreviousVisit,
                                LastVisit = suser.LastVisit,
                                Remark = suser.Remark,
                                WebToken = suser.WebToken,
                                ApiToken = suser.ApiToken,
                            });
            var userData = linqData.FirstOrDefault();
            if (userData != null)
            {
                //if (userData.UserStatus == (int)StatusEnum.Yes)
                //{
                //    if (userData.Password == EncryptUserPassword(password, userData.Salt))
                //    {
                //        userData.LoginCount++;
                //        userData.IsOnline = 1;

                //        #region 设置日期

                //        if (userData.FirstVisit == GlobalConstant.DefaultTime)
                //        {
                //            userData.FirstVisit = DateTime.Now;
                //        }
                //        if (userData.PreviousVisit == GlobalConstant.DefaultTime)
                //        {
                //            userData.PreviousVisit = DateTime.Now;
                //        }
                //        if (userData.LastVisit != GlobalConstant.DefaultTime)
                //        {
                //            userData.PreviousVisit = userData.LastVisit;
                //        }
                //        userData.LastVisit = DateTime.Now;

                //        #endregion

                //        userData.ApiToken = SecurityHelper.GetGuid(true);

                //        GetUserBelong(userData);
                //        //存储修改的内容
                //        DbCmd.SysUser.Update(userData);
                //        DbCmd.SaveChanges();

                //        Operator.Instance.AddCurrent(userData.ApiToken);
                //        obj.Data = Operator.Instance.Current(userData.ApiToken);
                //        obj.Message = "登录成功";
                //        obj.Tag = 1;
                //    }
                //    else
                //    {
                //        obj.Message = "密码不正确，请重新输入";
                //    }
                //}
                //else
                //{
                //    obj.Message = "账号被禁用，请联系管理员";
                //}
            }
            else
            {
                obj.Message = "账号不存在，请重新输入";
            }

            return Json(obj);
        }

        /// <summary>
        /// 用户退出登录
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public IActionResult LoginOff(string token = default)
        {
            var obj = new TData();
            Operator.Instance.RemoveCurrent(token);
            obj.Message = "登出成功";
            return Json(obj);
        }
    }
}
