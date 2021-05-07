using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using YiSha.Data.Repository;
using YiSha.Entity.SystemManage;
using YiSha.Model.Param.SystemManage;
using YiSha.Util.Extension;
using YiSha.Util.Helper;

namespace YiSha.Service.SystemManage
{
    public class MenuService : RepositoryFactory
    {
        #region 获取数据

        public async Task<List<MenuEntity>> GetList(MenuListParam param)
        {
            var expression = ListFilter(param);
            var list = await BaseRepository().FindList(expression);
            return list.OrderBy(p => p.MenuSort).ToList();
        }

        public async Task<MenuEntity> GetEntity(long id)
        {
            return await BaseRepository().FindEntity<MenuEntity>(id);
        }

        public async Task<int> GetMaxSort(long parentId)
        {
            string where = string.Empty;
            if (parentId > 0)
            {
                where += " AND ParentId = " + parentId;
            }
            var result = await BaseRepository().FindEntity<int>("SELECT MAX(MenuSort) FROM SysMenu where BaseIsDelete = 0 " + where);
            return result + 1;
        }

        public bool ExistMenuName(MenuEntity entity)
        {
            var expression = LinqExtensions.True<MenuEntity>();
            expression = expression.And(t => t.BaseIsDelete == 0);
            expression = expression.And(t => t.MenuName == entity.MenuName);
            expression = expression.And(t => t.MenuType == entity.MenuType);
            if (!entity.Id.IsNullOrZero())
            {
                expression = expression.And(t => t.Id != entity.Id);
            }
            return BaseRepository().AsQueryable(expression).Any();
        }

        #endregion

        #region 提交数据

        public async Task SaveForm(MenuEntity entity)
        {
            if (entity.Id.IsNullOrZero())
            {
                await entity.Create();
                await BaseRepository().Insert(entity);
            }
            else
            {
                await entity.Modify();
                await BaseRepository().Update(entity);
            }
        }

        public async Task DeleteForm(string ids)
        {
            var db = await BaseRepository().BeginTrans();
            try
            {
                var idArr = TextHelper.SplitToArray<object>(ids, ',');
                await db.Delete<MenuEntity>(p => idArr.Contains(p.Id.Value) || idArr.Contains(p.ParentId.Value));
                await db.Delete<MenuAuthorizeEntity>(p => idArr.Contains(p.MenuId.Value));
                await db.CommitTrans();
            }
            catch
            {
                await db.RollbackTrans();
                throw;
            }
        }

        #endregion

        #region 私有方法

        private Expression<Func<MenuEntity, bool>> ListFilter(MenuListParam param)
        {
            var expression = LinqExtensions.True<MenuEntity>();
            if (param != null)
            {
                if (param.MenuName?.Length > 0)
                {
                    expression = expression.And(t => t.MenuName.Contains(param.MenuName));
                }
                if (param.MenuStatus > -1)
                {
                    expression = expression.And(t => t.MenuStatus == param.MenuStatus);
                }
            }
            return expression;
        }

        #endregion
    }
}