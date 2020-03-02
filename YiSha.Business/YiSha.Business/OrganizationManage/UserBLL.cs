using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YiSha.Business.Cache;
using YiSha.Business.SystemManage;
using YiSha.Cache.Factory;
using YiSha.Entity;
using YiSha.Entity.OrganizationManage;
using YiSha.Entity.SystemManage;
using YiSha.Enum;
using YiSha.Enum.OrganizationManage;
using YiSha.Model;
using YiSha.Model.Param;
using YiSha.Model.Param.OrganizationManage;
using YiSha.Service.OrganizationManage;
using YiSha.Util;
using YiSha.Util.Extension;
using YiSha.Util.Model;
using YiSha.Web.Code;

namespace YiSha.Business.OrganizationManage
{
    public class UserBLL
    {
        private UserService userService = new UserService();
        private UserBelongService userBelongService = new UserBelongService();
        private DepartmentService departmentService = new DepartmentService();

        private DepartmentBLL departmentBLL = new DepartmentBLL();

        #region 获取数据
        public async Task<TData<List<UserEntity>>> GetList(UserListParam param)
        {
            TData<List<UserEntity>> obj = new TData<List<UserEntity>>();
            obj.Result = await userService.GetList(param);
            obj.TotalCount = obj.TotalCount;
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<List<UserEntity>>> GetPageList(UserListParam param, Pagination pagination)
        {
            TData<List<UserEntity>> obj = new TData<List<UserEntity>>();
            if (param != null)
            {
                if (param.DepartmentId != null)
                {
                    param.ChildrenDepartmentIdList = await departmentBLL.GetDepartmentIdList(param.DepartmentId.Value);
                }
            }
            obj.Result = await userService.GetPageList(param, pagination);
            List<UserBelongEntity> userBelongList = await userBelongService.GetList(new UserBelongEntity { UserIds = obj.Result.Select(p => p.Id.Value).ParseToStrings<long>() });
            List<DepartmentEntity> departmentList = await departmentService.GetList(new DepartmentListParam { Ids = userBelongList.Select(p => p.BelongId.Value).ParseToStrings<long>() });
            foreach (UserEntity user in obj.Result)
            {
                user.DepartmentName = departmentList.Where(p => p.Id == user.DepartmentId).Select(p => p.DepartmentName).FirstOrDefault();
            }
            obj.TotalCount = pagination.TotalCount;
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<UserEntity>> GetEntity(long id)
        {
            TData<UserEntity> obj = new TData<UserEntity>();
            obj.Result = await userService.GetEntity(id);

            await GetUserBelong(obj.Result);

            if (obj.Result.DepartmentId > 0)
            {
                DepartmentEntity departmentEntity = await departmentService.GetEntity(obj.Result.DepartmentId.Value);
                if (departmentEntity != null)
                {
                    obj.Result.DepartmentName = departmentEntity.DepartmentName;
                }
            }

            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<UserEntity>> CheckLogin(string userName, string password, int platform)
        {
            TData<UserEntity> obj = new TData<UserEntity>();
            if (userName.IsEmpty() || password.IsEmpty())
            {
                obj.Message = "用户名或密码不能为空";
                return obj;
            }
            UserEntity user = await userService.CheckLogin(userName);
            if (user != null)
            {
                if (user.UserStatus == (int)StatusEnum.Yes)
                {
                    if (user.Password == EncryptUserPassword(password, user.Salt))
                    {
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
                                    #region 多次登录用同一个token
                                    if (string.IsNullOrEmpty(user.WebToken))
                                    {
                                        user.WebToken = SecurityHelper.GetGuid();
                                    }
                                    #endregion
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

                        obj.Result = user;
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
            return obj;
        }
        #endregion

        #region 提交数据
        public async Task<TData<string>> SaveForm(UserEntity entity)
        {
            TData<string> obj = new TData<string>();
            if (userService.ExistUserName(entity))
            {
                obj.Message = "用户名已经存在！";
                return obj;
            }
            if (entity.Id.IsNullOrZero())
            {
                entity.Salt = GetPasswordSalt();
                entity.Password = EncryptUserPassword(entity.Password, entity.Salt);
            }
            if (!entity.Birthday.IsEmpty())
            {
                entity.Birthday = entity.Birthday.ParseToDateTime().ToString("yyyy-MM-dd");
            }
            await userService.SaveForm(entity);

            await RemoveCacheById(entity.Id.Value);

            obj.Result = entity.Id.ParseToString();
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData> DeleteForm(string ids)
        {
            TData obj = new TData();
            if (string.IsNullOrEmpty(ids))
            {
                obj.Message = "参数不能为空";
                return obj;
            }
            await userService.DeleteForm(ids);

            await RemoveCacheById(ids);

            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<long>> ResetPassword(UserEntity entity)
        {
            TData<long> obj = new TData<long>();
            if (entity.Id > 0)
            {
                UserEntity dbUserEntity = await userService.GetEntity(entity.Id.Value);
                if (dbUserEntity.Password == entity.Password)
                {
                    obj.Message = "密码未更改";
                    return obj;
                }
                entity.Salt = GetPasswordSalt();
                entity.Password = EncryptUserPassword(entity.Password, entity.Salt);
                await userService.ResetPassword(entity);

                await RemoveCacheById(entity.Id.Value);

                obj.Result = entity.Id.Value;
                obj.Tag = 1;
            }
            return obj;
        }

        public async Task<TData<long>> ChangePassword(ChangePasswordParam param)
        {
            TData<long> obj = new TData<long>();
            if (param.Id > 0)
            {
                if (string.IsNullOrEmpty(param.Password) || string.IsNullOrEmpty(param.NewPassword))
                {
                    obj.Message = "新密码不能为空";
                    return obj;
                }
                UserEntity dbUserEntity = await userService.GetEntity(param.Id.Value);
                if (dbUserEntity.Password != EncryptUserPassword(param.Password, dbUserEntity.Salt))
                {
                    obj.Message = "旧密码不正确";
                    return obj;
                }
                dbUserEntity.Salt = GetPasswordSalt();
                dbUserEntity.Password = EncryptUserPassword(param.NewPassword, dbUserEntity.Salt);
                await userService.ResetPassword(dbUserEntity);

                await RemoveCacheById(param.Id.Value);

                obj.Result = dbUserEntity.Id.Value;
                obj.Tag = 1;
            }
            return obj;
        }

        /// <summary>
        /// 用户自己修改自己的信息
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<TData<long>> ChangeUser(UserEntity entity)
        {
            TData<long> obj = new TData<long>();
            if (entity.Id > 0)
            {
                await userService.ChangeUser(entity);

                await RemoveCacheById(entity.Id.Value);

                obj.Result = entity.Id.Value;
                obj.Tag = 1;
            }
            return obj;
        }

        public async Task<TData> UpdateUser(UserEntity entity)
        {
            TData obj = new TData();
            await userService.UpdateUser(entity);

            obj.Tag = 1;
            return obj;
        }

        public async Task<TData> ImportUser(ImportParam param, List<UserEntity> list)
        {
            TData obj = new TData();
            if (list.Any())
            {
                foreach (UserEntity entity in list)
                {
                    UserEntity dbEntity = await userService.GetEntity(entity.UserName);
                    if (dbEntity != null)
                    {
                        entity.Id = dbEntity.Id;
                        if (param.IsOverride == 1)
                        {
                            await userService.SaveForm(entity);
                            await RemoveCacheById(entity.Id.Value);
                        }
                    }
                    else
                    {
                        await userService.SaveForm(entity);
                        await RemoveCacheById(entity.Id.Value);
                    }
                }
                obj.Tag = 1;
            }
            else
            {
                obj.Message = " 未找到导入的数据";
            }
            return obj;
        }

        #endregion

        #region 私有方法
        /// <summary>
        /// 密码MD5处理
        /// </summary>
        /// <param name="password"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        private string EncryptUserPassword(string password, string salt)
        {
            string md5Password = SecurityHelper.MD5Encrypt(password);
            string encryptPassword = SecurityHelper.MD5Encrypt(md5Password + salt);
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
        /// 移除缓存里面的token
        /// </summary>
        /// <param name="id"></param>
        private async Task RemoveCacheById(string ids)
        {
            foreach (long id in ids.Split(',').Select(p => long.Parse(p)))
            {
                await RemoveCacheById(id);
            }
        }

        private async Task RemoveCacheById(long id)
        {
            var dbEntity = await userService.GetEntity(id);
            if (dbEntity != null)
            {
                CacheFactory.Cache.RemoveCache(dbEntity.WebToken);
            }
        }

        /// <summary>
        /// 获取用户的职位和角色
        /// </summary>
        /// <param name="user"></param>
        private async Task GetUserBelong(UserEntity user)
        {
            List<UserBelongEntity> userBelongList = await userBelongService.GetList(new UserBelongEntity { UserId = user.Id });

            List<UserBelongEntity> roleBelongList = userBelongList.Where(p => p.BelongType == UserBelongTypeEnum.Role.ParseToInt()).ToList();
            if (roleBelongList.Count > 0)
            {
                user.RoleIds = string.Join(",", roleBelongList.Select(p => p.BelongId).ToList());
            }

            List<UserBelongEntity> positionBelongList = userBelongList.Where(p => p.BelongType == UserBelongTypeEnum.Position.ParseToInt()).ToList();
            if (positionBelongList.Count > 0)
            {
                user.PositionIds = string.Join(",", positionBelongList.Select(p => p.BelongId).ToList());
            }
        }
        #endregion
    }
}
