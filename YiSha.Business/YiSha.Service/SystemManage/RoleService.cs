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
using YiSha.Util.Model;
using YiSha.Util;

namespace YiSha.Service.SystemManage
{
    public class RoleService : RepositoryFactory
    {
        #region 获取数据
        public async Task<List<RoleEntity>> GetList(RoleListParam param)
        {
            var expression = ListFilter(param);
            var list = await this.BaseRepository().FindList(expression);
            return list.ToList();
        }

        public async Task<List<RoleEntity>> GetPageList(RoleListParam param, Pagination pagination)
        {
            var expression = ListFilter(param);
            var list = await this.BaseRepository().FindList(expression, pagination);
            return list.ToList();
        }

        public async Task<RoleEntity> GetEntity(long id)
        {
            return await this.BaseRepository().FindEntity<RoleEntity>(id);
        }

        public async Task<int> GetMaxSort()
        {
            object result = await this.BaseRepository().FindObject("SELECT MAX(role_sort) FROM sys_role");
            int sort = result.ParseToInt();
            sort++;
            return sort;
        }

        public bool ExistRoleName(RoleEntity entity)
        {
            var expression = LinqExtensions.True<RoleEntity>();
            expression = expression.And(t => t.BaseIsDelete == 0);
            if (entity.Id.IsNullOrZero())
            {
                expression = expression.And(t => t.RoleName == entity.RoleName);
            }
            else
            {
                expression = expression.And(t => t.RoleName == entity.RoleName && t.Id != entity.Id);
            }
            return this.BaseRepository().IQueryable(expression).Count() > 0 ? true : false;
        }
        #endregion

        #region 提交数据
        public async Task SaveForm(RoleEntity entity)
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
                    await db.Delete<MenuAuthorizeEntity>(t => t.AuthorizeId == entity.Id);
                    await entity.Modify();
                    await db.Update(entity);
                }
                // 角色对应的菜单、页面和按钮权限
                if (!string.IsNullOrEmpty(entity.MenuIds))
                {
                    foreach (long menuId in TextHelper.SplitToArray<long>(entity.MenuIds, ','))
                    {
                        MenuAuthorizeEntity menuAuthorizeEntity = new MenuAuthorizeEntity();
                        menuAuthorizeEntity.AuthorizeId = entity.Id;
                        menuAuthorizeEntity.MenuId = menuId;
                        menuAuthorizeEntity.AuthorizeType = AuthorizeTypeEnum.Role.ParseToInt();
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
            long[] idArr = TextHelper.SplitToArray<long>(ids, ',');
            await this.BaseRepository().Delete<RoleEntity>(idArr);
        }
        #endregion

        #region 私有方法
        private Expression<Func<RoleEntity, bool>> ListFilter(RoleListParam param)
        {
            var expression = LinqExtensions.True<RoleEntity>();
            if (param != null)
            {
                if (!string.IsNullOrEmpty(param.RoleName))
                {
                    expression = expression.And(t => t.RoleName.Contains(param.RoleName));
                }
                if (!string.IsNullOrEmpty(param.RoleIds))
                {
                    long[] roleIdArr = TextHelper.SplitToArray<long>(param.RoleIds, ',');
                    expression = expression.And(t => roleIdArr.Contains(t.Id.Value));
                }
                if (!string.IsNullOrEmpty(param.RoleName))
                {
                    expression = expression.And(t => t.RoleName.Contains(param.RoleName));
                }
                if (param.RoleStatus > -1)
                {
                    expression = expression.And(t => t.RoleStatus == param.RoleStatus);
                }
                if (!string.IsNullOrEmpty(param.StartTime.ParseToString()))
                {
                    expression = expression.And(t => t.BaseModifyTime >= param.StartTime);
                }
                if (!string.IsNullOrEmpty(param.EndTime.ParseToString()))
                {
                    param.EndTime = (param.EndTime.Value.ToString("yyyy-MM-dd") + " 23:59:59").ParseToDateTime();
                    expression = expression.And(t => t.BaseModifyTime <= param.EndTime);
                }
            }
            return expression;
        }
        #endregion
    }
}
