using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using YiSha.Data.Repository;
using YiSha.Entity.SystemManage;
using YiSha.Enum.SystemManage;
using YiSha.Model.Param.SystemManage;
using YiSha.Util.Extension;
using YiSha.Util.Helper;
using YiSha.Util.Model;

namespace YiSha.Service.SystemManage
{
    public class RoleService : RepositoryFactory
    {
        #region 获取数据

        public async Task<List<RoleEntity>> GetList(RoleListParam param)
        {
            var expression = ListFilter(param);
            return await BaseRepository().FindList(expression);
        }

        public async Task<List<RoleEntity>> GetPageList(RoleListParam param, Pagination pagination)
        {
            var expression = ListFilter(param);
            return await BaseRepository().FindList(expression, pagination);
        }

        public async Task<RoleEntity> GetEntity(long id)
        {
            return await BaseRepository().FindEntity<RoleEntity>(id);
        }

        public async Task<int> GetMaxSort()
        {
            var result = await BaseRepository().FindEntity<int>("SELECT MAX(RoleSort) FROM SysRole");
            return result + 1;
        }

        public bool ExistRoleName(RoleEntity entity)
        {
            var expression = LinqExtensions.True<RoleEntity>();
            expression = expression.And(t => t.BaseIsDelete == 0);
            expression = expression.And(t => t.RoleName == entity.RoleName);
            if (!entity.Id.IsNullOrZero())
            {
                expression = expression.And(t => t.Id != entity.Id);
            }
            return BaseRepository().AsQueryable(expression).Any();
        }

        #endregion

        #region 提交数据

        public async Task SaveForm(RoleEntity entity)
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
                    await db.Delete<MenuAuthorizeEntity>(t => t.AuthorizeId == entity.Id);
                    await entity.Modify();
                    await db.Update(entity);
                }
                // 角色对应的菜单、页面和按钮权限
                if (entity.MenuIds?.Length > 0)
                {
                    foreach (long menuId in TextHelper.SplitToArray<long>(entity.MenuIds, ','))
                    {
                        MenuAuthorizeEntity menuAuthorizeEntity = new MenuAuthorizeEntity
                        {
                            AuthorizeId = entity.Id,
                            MenuId = menuId,
                            AuthorizeType = AuthorizeTypeEnum.Role.ParseToInt()
                        };
                        await menuAuthorizeEntity.Create();
                        await db.Insert(menuAuthorizeEntity);
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
            var idArr = TextHelper.SplitToArray<long>(ids, ',');
            await BaseRepository().Delete<RoleEntity>(idArr);
        }

        #endregion

        #region 私有方法

        private Expression<Func<RoleEntity, bool>> ListFilter(RoleListParam param)
        {
            var expression = LinqExtensions.True<RoleEntity>();
            if (param != null)
            {
                if (param.RoleName?.Length > 0)
                {
                    expression = expression.And(t => t.RoleName.Contains(param.RoleName));
                }
                if (param.RoleIds?.Length > 0)
                {
                    long[] roleIdArr = TextHelper.SplitToArray<long>(param.RoleIds, ',');
                    expression = expression.And(t => roleIdArr.Contains(t.Id.Value));
                }
                if (param.RoleName?.Length > 0)
                {
                    expression = expression.And(t => t.RoleName.Contains(param.RoleName));
                }
                if (param.RoleStatus > -1)
                {
                    expression = expression.And(t => t.RoleStatus == param.RoleStatus);
                }
                if (param.StartTime.ParseToString()?.Length > 0)
                {
                    expression = expression.And(t => t.BaseModifyTime >= param.StartTime);
                }
                if (param.EndTime.ParseToString()?.Length > 0)
                {
                    param.EndTime = (param.EndTime?.ToString("yyyy-MM-dd") + " 23:59:59").ParseToDateTime();
                    expression = expression.And(t => t.BaseModifyTime <= param.EndTime);
                }
            }
            return expression;
        }

        #endregion
    }
}