using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YiSha.Cache.Factory;
using YiSha.Entity.OrganizationManage;
using YiSha.Enum;
using YiSha.Enum.OrganizationManage;
using YiSha.Model.Param;
using YiSha.Model.Param.OrganizationManage;
using YiSha.Service.OrganizationManage;
using YiSha.Util.Extension;
using YiSha.Util.Helper;
using YiSha.Util.Model;
using YiSha.Web.Code;

namespace YiSha.Business.OrganizationManage
{
    public class UserBLL
    {
        private readonly UserService _userService = new();

        private readonly UserBelongService _userBelongService = new();

        private readonly DepartmentService _departmentService = new();

        private readonly DepartmentBLL _departmentBll = new();

        #region 获取数据

        public async Task<TData<List<UserEntity>>> GetList(UserListParam param)
        {
            return new() { Data = await _userService.GetList(param), Tag = 1 };
        }

        public async Task<TData<List<UserEntity>>> GetPageList(UserListParam param, Pagination pagination)
        {
            param.DepartmentId ??= (await Operator.Instance.Current()).DepartmentId;
            param.ChildrenDepartmentIdList = await _departmentBll.GetChildrenDepartmentIdList(null, param.DepartmentId!.Value);

            var pageList = await _userService.GetPageList(param, pagination);
            var userBelongList = await _userBelongService.GetList(new UserBelongEntity { UserIds = pageList.Select(p => p.Id!.Value).ParseToStrings<long>() });
            var departmentList = await _departmentService.GetList(new DepartmentListParam { Ids = userBelongList.Select(p => p.BelongId!.Value).ParseToStrings<long>() });
            foreach (var user in pageList)
            {
                user.DepartmentName = departmentList.Where(p => p.Id == user.DepartmentId).Select(p => p.DepartmentName).FirstOrDefault();
            }
            return new() { Tag = 1, Data = pageList, Total = pagination.TotalCount };
        }

        public async Task<TData<UserEntity>> GetEntity(long id)
        {
            var data = await _userService.GetEntity(id);
            await GetUserBelong(data);
            if (data.DepartmentId > 0)
            {
                var departmentEntity = await _departmentService.GetEntity(data.DepartmentId.Value);
                data.DepartmentName = departmentEntity?.DepartmentName;
            }
            return new() { Data = data, Tag = 1 };
        }

        public async Task<TData<UserEntity>> CheckLogin(string userName, string password, int platform)
        {
            if (userName.IsEmpty() || password.IsEmpty())
            {
                return new() { Tag = 0, Message = "用户名或密码不能为空" };
            }

            var user = await _userService.CheckLogin(userName);
            if (user == null)
            {
                return new() { Tag = 0, Message = "账号不存在，请重新输入" };
            }
            if (user.UserStatus != (int)StatusEnum.Yes)
            {
                return new() { Tag = 0, Message = "账号被禁用，请联系管理员" };
            }
            if (user.Password != EncryptUserPassword(password, user.Salt))
            {
                return new() { Tag = 0, Message = "密码不正确，请重新输入" };
            }

            user.LoginCount++;
            user.IsOnline = 1;

            #region 设置日期

            if (user.FirstVisit == GlobalConstant.DefaultTime)
            {
                user.FirstVisit = DateTime.Now;
            }
            if (user.PreviousVisit == GlobalConstant.DefaultTime)
            {
                user.PreviousVisit = DateTime.Now;
            }
            if (user.LastVisit != GlobalConstant.DefaultTime)
            {
                user.PreviousVisit = user.LastVisit;
            }
            user.LastVisit = DateTime.Now;

            #endregion

            switch (platform)
            {
                case (int)PlatformEnum.Web:
                    if (GlobalContext.SystemConfig.LoginMultiple)
                    {
                        // 多次登录用同一个token
                        if (string.IsNullOrEmpty(user.WebToken))
                        {
                            user.WebToken = SecurityHelper.GetGuid();
                        }
                    }
                    else
                    {
                        user.WebToken = SecurityHelper.GetGuid();
                    }
                    break;

                case (int)PlatformEnum.WebApi:
                    user.ApiToken = SecurityHelper.GetGuid();
                    break;
            }
            await GetUserBelong(user);

            return new() { Data = user, Message = "登录成功", Tag = 1 };
        }

        #endregion

        #region 提交数据

        public async Task<TData<string>> SaveForm(UserEntity entity)
        {
            if (_userService.ExistUserName(entity))
            {
                return new() { Tag = 0, Message = "用户名已经存在！" };
            }
            if (entity.Id.IsNullOrZero())
            {
                entity.Salt = GetPasswordSalt();
                entity.Password = EncryptUserPassword(entity.Password, entity.Salt);
            }
            if (entity.Birthday?.Length > 0)
            {
                entity.Birthday = entity.Birthday.ParseToDateTime().ToString("yyyy-MM-dd");
            }

            await _userService.SaveForm(entity);
            await RemoveCacheById(entity.Id!.Value);
            return new() { Data = entity.Id.ParseToString(), Tag = 1 };
        }

        public async Task<TData> DeleteForm(string ids)
        {
            if (string.IsNullOrEmpty(ids))
            {
                return new() { Tag = 0, Message = "参数不能为空" };
            }

            await _userService.DeleteForm(ids);
            await RemoveCacheById(ids);
            return new() { Tag = 1 };
        }

        public async Task<TData<long>> ResetPassword(UserEntity entity)
        {
            if (!(entity.Id > 0))
            {
                return new();
            }

            var dbUserEntity = await _userService.GetEntity(entity.Id!.Value);
            if (dbUserEntity.Password == entity.Password)
            {
                return new() { Tag = 0, Message = "密码未更改" };
            }

            entity.Salt = GetPasswordSalt();
            entity.Password = EncryptUserPassword(entity.Password, entity.Salt);

            await _userService.ResetPassword(entity);
            await RemoveCacheById(entity.Id!.Value);
            return new() { Data = entity.Id!.Value, Tag = 1 };
        }

        public async Task<TData<long>> ChangePassword(ChangePasswordParam param)
        {
            if (!(param.Id > 0))
            {
                return new();
            }

            if (string.IsNullOrEmpty(param.Password) || string.IsNullOrEmpty(param.NewPassword))
            {
                return new() { Tag = 0, Message = "新密码不能为空" };
            }

            var dbUserEntity = await _userService.GetEntity(param.Id!.Value);
            if (dbUserEntity.Password != EncryptUserPassword(param.Password, dbUserEntity.Salt))
            {
                return new() { Tag = 0, Message = "旧密码不正确" };
            }

            dbUserEntity.Salt = GetPasswordSalt();
            dbUserEntity.Password = EncryptUserPassword(param.NewPassword, dbUserEntity.Salt);

            await _userService.ResetPassword(dbUserEntity);
            await RemoveCacheById(param.Id!.Value);
            return new() { Data = dbUserEntity.Id!.Value, Tag = 1 };
        }

        /// <summary>
        /// 用户自己修改自己的信息
        /// </summary>
        public async Task<TData<long>> ChangeUser(UserEntity entity)
        {
            if (entity.Id > 0)
            {
                await _userService.ChangeUser(entity);
                await RemoveCacheById(entity.Id!.Value);
                return new() { Data = entity.Id!.Value, Tag = 1 };
            }
            return new();
        }

        public async Task<TData> UpdateUser(UserEntity entity)
        {
            await _userService.UpdateUser(entity);
            return new() { Tag = 1 };
        }

        public async Task<TData> ImportUser(ImportParam param, List<UserEntity> list)
        {
            if (!list.Any())
            {
                return new() { Tag = 0, Message = "未找到导入的数据" };
            }

            foreach (var entity in list)
            {
                var dbEntity = await _userService.GetEntity(entity.UserName);
                if (dbEntity != null)
                {
                    entity.Id = dbEntity.Id;
                    if (param.IsOverride == 1)
                    {
                        await _userService.SaveForm(entity);
                        await RemoveCacheById(entity.Id!.Value);
                    }
                }
                else
                {
                    await _userService.SaveForm(entity);
                    await RemoveCacheById(entity.Id!.Value);
                }
            }
            return new() { Tag = 1 };
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 密码MD5处理
        /// </summary>
        private string EncryptUserPassword(string password, string salt)
        {
            string md5Password = SecurityHelper.Md5Encrypt(password);
            string encryptPassword = SecurityHelper.Md5Encrypt(md5Password + salt);
            return encryptPassword;
        }

        /// <summary>
        /// 密码盐
        /// </summary>
        private string GetPasswordSalt()
        {
            return new Random().Next(1, 100000).ToString();
        }

        /// <summary>
        /// 移除缓存里面的token
        /// </summary>
        private async Task RemoveCacheById(string ids)
        {
            foreach (long id in ids.Split(',').Select(long.Parse))
            {
                await RemoveCacheById(id);
            }
        }

        private async Task RemoveCacheById(long id)
        {
            var entity = await _userService.GetEntity(id);
            if (entity != null)
            {
                CacheFactory.Cache.RemoveCache(entity.WebToken);
            }
        }

        /// <summary>
        /// 获取用户的职位和角色
        /// </summary>
        private async Task GetUserBelong(UserEntity user)
        {
            var userBelongList = await _userBelongService.GetList(new UserBelongEntity { UserId = user.Id });
            var roleBelongList = userBelongList.Where(p => p.BelongType == UserBelongTypeEnum.Role.ParseToInt());
            if (roleBelongList.Any())
            {
                user.RoleIds = string.Join(",", roleBelongList.Select(p => p.BelongId));
            }

            var positionBelongList = userBelongList.Where(p => p.BelongType == UserBelongTypeEnum.Position.ParseToInt());
            if (positionBelongList.Any())
            {
                user.PositionIds = string.Join(",", positionBelongList.Select(p => p.BelongId));
            }
        }

        #endregion
    }
}