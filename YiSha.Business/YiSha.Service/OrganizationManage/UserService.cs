using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Data;
using System.Data.Common;
using YiSha.Data.Repository;
using YiSha.Entity.OrganizationManage;
using YiSha.Enum.OrganizationManage;
using YiSha.Model.Param.OrganizationManage;
using YiSha.Util;
using YiSha.Util.Model;
using YiSha.Util.Extension;
using YiSha.Enum;
using YiSha.Entity;
using YiSha.Data.EF;
using YiSha.Service.SystemManage;

namespace YiSha.Service.OrganizationManage
{
    public class UserService : RepositoryFactory
    {
        #region 获取数据
        public async Task<List<UserEntity>> GetList(UserListParam param)
        {
            var expression = ListFilter(param);
            var list = await this.BaseRepository().FindList(expression);
            return list.ToList();
        }

        public async Task<List<UserEntity>> GetPageList(UserListParam param, Pagination pagination)
        {
            var expression = ListFilter(param);
            var list = await this.BaseRepository().FindList(expression, pagination);
            return list.ToList();
        }

        public async Task<UserEntity> GetEntity(long id)
        {
            return await this.BaseRepository().FindEntity<UserEntity>(id);
        }

        public async Task<UserEntity> GetEntity(string userName)
        {
            return await this.BaseRepository().FindEntity<UserEntity>(p => p.UserName == userName);
        }

        public async Task<UserEntity> CheckLogin(string userName)
        {
            var expression = LinqExtensions.True<UserEntity>();
            expression = expression.And(t => t.UserName == userName);
            expression = expression.Or(t => t.Mobile == userName);
            expression = expression.Or(t => t.Email == userName);
            return await this.BaseRepository().FindEntity(expression);
        }

        public bool ExistUserName(UserEntity entity)
        {
            var expression = LinqExtensions.True<UserEntity>();
            expression = expression.And(t => t.BaseIsDelete == 0);
            if (entity.Id.IsNullOrZero())
            {
                expression = expression.And(t => t.UserName == entity.UserName);
            }
            else
            {
                expression = expression.And(t => t.UserName == entity.UserName && t.Id != entity.Id);
            }
            return this.BaseRepository().IQueryable(expression).Count() > 0 ? true : false;
        }
        #endregion

        #region 提交数据
        public async Task UpdateUser(UserEntity entity)
        {
            await this.BaseRepository().Update(entity);
        }

        public async Task SaveForm(UserEntity entity)
        {
            var db = await this.BaseRepository().BeginTrans();
            try
            {
                if (entity.Id.IsNullOrZero())
                {
                    await entity.Create();
                    await db.Insert(entity);
                }
                else
                {
                    await db.Delete<UserBelongEntity>(t => t.UserId == entity.Id);

                    // 密码不进行更新，有单独的方法更新密码
                    entity.Password = null;
                    await entity.Modify();
                    await db.Update(entity);
                }
                // 职位
                if (!string.IsNullOrEmpty(entity.PositionIds))
                {
                    foreach (long positionId in TextHelper.SplitToArray<long>(entity.PositionIds, ','))
                    {
                        UserBelongEntity positionBelongEntity = new UserBelongEntity();
                        positionBelongEntity.UserId = entity.Id;
                        positionBelongEntity.BelongId = positionId;
                        positionBelongEntity.BelongType = UserBelongTypeEnum.Position.ParseToInt();
                        await positionBelongEntity.Create();
                        await db.Insert(positionBelongEntity);
                    }
                }
                // 角色
                if (!string.IsNullOrEmpty(entity.RoleIds))
                {
                    foreach (long roleId in TextHelper.SplitToArray<long>(entity.RoleIds, ','))
                    {
                        UserBelongEntity departmentBelongEntity = new UserBelongEntity();
                        departmentBelongEntity.UserId = entity.Id;
                        departmentBelongEntity.BelongId = roleId;
                        departmentBelongEntity.BelongType = UserBelongTypeEnum.Role.ParseToInt();
                        await departmentBelongEntity.Create();
                        await db.Insert(departmentBelongEntity);
                    }
                }
                await db.CommitTrans();
            }
            catch
            {
                await db.RollbackTrans();
                throw;
            }
        }

        public async Task DeleteForm(string ids)
        {
            var db = await this.BaseRepository().BeginTrans();
            try
            {
                long[] idArr = TextHelper.SplitToArray<long>(ids, ',');
                await db.Delete<UserEntity>(idArr);
                await db.Delete<UserBelongEntity>(t => idArr.Contains(t.UserId.Value));
                await db.CommitTrans();
            }
            catch
            {
                await db.RollbackTrans();
                throw;
            }
        }

        public async Task ResetPassword(UserEntity entity)
        {
            await entity.Modify();
            await this.BaseRepository().Update(entity);
        }

        public async Task ChangeUser(UserEntity entity)
        {
            await entity.Modify();
            await this.BaseRepository().Update(entity);
        }
        #endregion

        #region 私有方法
        private Expression<Func<UserEntity, bool>> ListFilter(UserListParam param)
        {

            //var expression = LinqExtensions.True<UserEntity>();

            if (param.UserStatus == -1)
            {
                param.UserStatus = null;
            }

            //****根据查询字段自动过滤条件****
            var expression = LinqExtensions.GetExpressionItems<UserEntity,UserListParam>(param);

            if (param != null)
            {
                //if (!string.IsNullOrEmpty(param.UserName))
                //{
                //    expression = expression.And(t => t.UserName.Contains(param.UserName));
                //}


                if (!string.IsNullOrEmpty(param.UserIds))
                {
                    long[] userIdList = TextHelper.SplitToArray<long>(param.UserIds, ',');
                    expression = expression.And(t => userIdList.Contains(t.Id.Value));
                }

                //if (!string.IsNullOrEmpty(param.Mobile))
                //{
                //    expression = expression.And(t => t.Mobile.Contains(param.Mobile));
                //}
                //if (param.UserStatus > -1)
                //{
                //    expression = expression.And(t => t.UserStatus == param.UserStatus);
                //}

                if (!string.IsNullOrEmpty(param.StartTime.ParseToString()))
                {
                    expression = expression.And(t => t.BaseModifyTime >= param.StartTime);
                }
                if (!string.IsNullOrEmpty(param.EndTime.ParseToString()))
                {
                    param.EndTime = param.EndTime.Value.Date.Add(new TimeSpan(23, 59, 59));
                    expression = expression.And(t => t.BaseModifyTime <= param.EndTime);
                }
                if (param.ChildrenDepartmentIdList != null && param.ChildrenDepartmentIdList.Count > 0)
                {
                    expression = expression.And(t => param.ChildrenDepartmentIdList.Contains(t.DepartmentId.Value));
                }
            }
            return expression;
        }
        #endregion
    }
}
