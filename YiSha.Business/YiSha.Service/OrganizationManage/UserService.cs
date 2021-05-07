using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using YiSha.Data.Repository;
using YiSha.Entity.OrganizationManage;
using YiSha.Enum.OrganizationManage;
using YiSha.Model.Param.OrganizationManage;
using YiSha.Util.Extension;
using YiSha.Util.Helper;
using YiSha.Util.Model;

namespace YiSha.Service.OrganizationManage
{
    public class UserService : RepositoryFactory
    {
        #region 获取数据

        public async Task<List<UserEntity>> GetList(UserListParam param)
        {
            var expression = ListFilter(param);
            return await BaseRepository().FindList(expression);
        }

        public async Task<List<UserEntity>> GetPageList(UserListParam param, Pagination pagination)
        {
            var expression = ListFilter(param);
            return await BaseRepository().FindList(expression, pagination);
        }

        public async Task<UserEntity> GetEntity(long id)
        {
            return await BaseRepository().FindEntity<UserEntity>(id);
        }

        public async Task<UserEntity> GetEntity(string userName)
        {
            return await BaseRepository().FindEntity<UserEntity>(p => p.UserName == userName);
        }

        public async Task<UserEntity> CheckLogin(string userName)
        {
            var expression = LinqExtensions.True<UserEntity>();
            expression = expression.And(t => t.UserName == userName);
            expression = expression.Or(t => t.Mobile == userName);
            expression = expression.Or(t => t.Email == userName);
            return await BaseRepository().FindEntity(expression);
        }

        public bool ExistUserName(UserEntity entity)
        {
            var expression = LinqExtensions.True<UserEntity>();
            expression = expression.And(t => t.BaseIsDelete == 0);
            expression = expression.And(t => t.UserName == entity.UserName);
            if (!entity.Id.IsNullOrZero())
            {
                expression = expression.And(t => t.Id != entity.Id);
            }
            return BaseRepository().AsQueryable(expression).Any();
        }

        #endregion

        #region 提交数据

        public async Task UpdateUser(UserEntity entity)
        {
            await BaseRepository().Update(entity);
        }

        public async Task SaveForm(UserEntity entity)
        {
            var db = await BaseRepository().BeginTrans();
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
                if (entity.PositionIds?.Length > 0)
                {
                    foreach (long positionId in TextHelper.SplitToArray<long>(entity.PositionIds, ','))
                    {
                        var positionBelongEntity = new UserBelongEntity
                        {
                            UserId = entity.Id,
                            BelongId = positionId,
                            BelongType = UserBelongTypeEnum.Position.ParseToInt()
                        };
                        await positionBelongEntity.Create();
                        await db.Insert(positionBelongEntity);
                    }
                }

                // 角色
                if (entity.RoleIds?.Length > 0)
                {
                    foreach (long roleId in TextHelper.SplitToArray<long>(entity.RoleIds, ','))
                    {
                        var departmentBelongEntity = new UserBelongEntity
                        {
                            UserId = entity.Id,
                            BelongId = roleId,
                            BelongType = UserBelongTypeEnum.Role.ParseToInt()
                        };
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
            var db = await BaseRepository().BeginTrans();
            try
            {
                var idArr = TextHelper.SplitToArray<object>(ids, ',');
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
            await BaseRepository().Update(entity);
        }

        public async Task ChangeUser(UserEntity entity)
        {
            await entity.Modify();
            await BaseRepository().Update(entity);
        }

        #endregion

        #region 私有方法

        private Expression<Func<UserEntity, bool>> ListFilter(UserListParam param)
        {
            var expression = LinqExtensions.True<UserEntity>();
            if (param != null)
            {
                if (param.UserName?.Length > 0)
                {
                    expression = expression.And(t => t.UserName.Contains(param.UserName));
                }
                if (param.UserIds?.Length > 0)
                {
                    long[] userIdList = TextHelper.SplitToArray<long>(param.UserIds, ',');
                    expression = expression.And(t => userIdList.Contains(t.Id.Value));
                }
                if (param.Mobile?.Length > 0)
                {
                    expression = expression.And(t => t.Mobile.Contains(param.Mobile));
                }
                if (param.UserStatus > -1)
                {
                    expression = expression.And(t => t.UserStatus == param.UserStatus);
                }
                if (param.StartTime.HasValue)
                {
                    expression = expression.And(t => t.BaseModifyTime >= param.StartTime);
                }
                if (param.EndTime.HasValue)
                {
                    param.EndTime = param.EndTime.Value.Date.Add(new TimeSpan(23, 59, 59));
                    expression = expression.And(t => t.BaseModifyTime <= param.EndTime);
                }
                if (param.ChildrenDepartmentIdList is { Count: > 0 })
                {
                    expression = expression.And(t => param.ChildrenDepartmentIdList.Contains(t.DepartmentId.Value));
                }
            }
            return expression;
        }

        #endregion
    }
}