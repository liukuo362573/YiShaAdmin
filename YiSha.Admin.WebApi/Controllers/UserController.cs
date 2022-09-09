using Microsoft.AspNetCore.Mvc;
using YiSha.Admin.WebApi.Filter;
using YiSha.Cache;
using YiSha.Entity;
using YiSha.Enum;
using YiSha.Enum.OrganizationManage;
using YiSha.Model;
using YiSha.Model.Entity.OrganizationManage;
using YiSha.Model.Operator;
using YiSha.Util;

namespace YiSha.Admin.WebApi.Controllers
{
    /// <summary>
    /// 用户数据控制器
    /// </summary>
    [ApiController]
    [AuthorizeFilter]
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
                if (userData.UserStatus == (int)StatusEnum.Yes)
                {
                    if (userData.Password == EncryptUserPassword(password, userData.Salt))
                    {
                        userData.LoginCount++;
                        userData.IsOnline = 1;

                        #region 设置日期

                        if (userData.FirstVisit == GlobalConstant.DefaultTime)
                        {
                            userData.FirstVisit = DateTime.Now;
                        }
                        if (userData.PreviousVisit == GlobalConstant.DefaultTime)
                        {
                            userData.PreviousVisit = DateTime.Now;
                        }
                        if (userData.LastVisit != GlobalConstant.DefaultTime)
                        {
                            userData.PreviousVisit = userData.LastVisit;
                        }
                        userData.LastVisit = DateTime.Now;

                        #endregion

                        userData.ApiToken = SecurityHelper.GetGuid(true);

                        GetUserBelong(userData);
                        //存储修改的内容
                        DbCmd.SysUser.Update(userData);
                        DbCmd.SaveChanges();

                        Operator.Instance.AddCurrent(userData.ApiToken);
                        obj.Data = Operator.Instance.Current(userData.ApiToken);
                        obj.Message = "登录成功";
                        obj.Tag = 1;
                    }
                    else
                    {
                        obj.Message = "密码不正确，请重新输入";
                    }
                }
                else
                {
                    obj.Message = "账号被禁用，请联系管理员";
                }
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
        public IActionResult LoginOff(string token = default)
        {
            var obj = new TData();
            Operator.Instance.RemoveCurrent(token);
            obj.Message = "登出成功";
            return Json(obj);
        }

        #region 私有方法

        /// <summary>
        /// 密码MD5处理
        /// </summary>
        /// <param name="password"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        private string EncryptUserPassword(string password, string salt)
        {
            string md5Password = SecurityHelper.MD5ToHex(password);
            string encryptPassword = SecurityHelper.MD5ToHex(md5Password.ToLower() + salt).ToLower();
            return encryptPassword;
        }

        /// <summary>
        /// 密码盐
        /// </summary>
        /// <returns></returns>
        private string GetPasswordSalt()
        {
            return new Random().Next(1, 100000).ToString();
        }

        /// <summary>
        /// 移除缓存里面的 Token
        /// </summary>
        /// <param name="ids"></param>
        private void RemoveCacheById(string ids)
        {
            foreach (long id in ids.Split(',').Select(p => long.Parse(p)))
            {
                RemoveCacheById(id);
            }
        }

        private void RemoveCacheById(long id)
        {
            var dbEntity = DbCmd.SysUser.Where(W => W.Id == id).FirstOrDefault();
            if (dbEntity != null)
            {
                CacheFactory.Cache.RemoveCache(dbEntity.WebToken);
            }
        }

        /// <summary>
        /// 获取用户的职位和角色
        /// </summary>
        /// <param name="user"></param>
        private void GetUserBelong(UserEntity user)
        {
            var userBelongList = DbCmd.SysUserBelong.Where(W => W.UserId == user.Id);

            var roleBelongList = userBelongList.Where(p => p.BelongType == (int)UserBelongTypeEnum.Role).ToList();
            if (roleBelongList.Count > 0)
            {
                user.RoleIds = string.Join(",", roleBelongList.Select(p => p.BelongId).ToList());
            }

            var positionBelongList = userBelongList.Where(p => p.BelongType == (int)UserBelongTypeEnum.Position).ToList();
            if (positionBelongList.Count > 0)
            {
                user.PositionIds = string.Join(",", positionBelongList.Select(p => p.BelongId).ToList());
            }
        }

        #endregion
    }
}
