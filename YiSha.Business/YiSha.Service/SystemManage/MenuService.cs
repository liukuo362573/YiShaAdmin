using System.Linq.Expressions;
using YiSha.Common;
using YiSha.DataBase;
using YiSha.Entity.SystemManage;
using YiSha.Model.Param.SystemManage;
using YiSha.Util;

namespace YiSha.Service.SystemManage
{
    public class MenuService : Repository
    {
        #region 获取数据
        public async Task<List<MenuEntity>> GetList(MenuListParam param)
        {
            var expression = ListFilter(param);
            var list = await this.FindList<MenuEntity>(expression);
            return list.OrderBy(p => p.MenuSort).ToList();
        }

        public async Task<MenuEntity> GetEntity(long id)
        {
            return await this.FindEntity<MenuEntity>(id);
        }

        public async Task<int> GetMaxSort(long parentId)
        {
            string where = string.Empty;
            if (parentId > 0)
            {
                where += " AND ParentId = " + parentId;
            }
            object result = await this.FindObject("SELECT MAX(MenuSort) FROM SysMenu where BaseIsDelete = 0 " + where);
            int sort = result.ToInt();
            sort++;
            return sort;
        }

        public bool ExistMenuName(MenuEntity entity)
        {
            var expression = ExtensionLinq.True<MenuEntity>();
            expression = expression.And(t => t.BaseIsDelete == 0);
            if (entity.Id.IsNullOrZero())
            {
                expression = expression.And(t => t.MenuName == entity.MenuName && t.MenuType == entity.MenuType);
            }
            else
            {
                expression = expression.And(t => t.MenuName == entity.MenuName && t.MenuType == entity.MenuType && t.Id != entity.Id);
            }
            return this.IQueryable(expression).Count() > 0 ? true : false;
        }
        #endregion

        #region 提交数据
        public async Task SaveForm(MenuEntity entity)
        {
            if (entity.Id.IsNullOrZero())
            {
                await entity.Create();
                await this.Insert(entity);
            }
            else
            {
                await entity.Modify();
                await this.Update(entity);
            }
        }

        public async Task DeleteForm(string ids)
        {
            var db = await this.BeginTrans();
            try
            {
                long[] idArr = TextHelper.SplitToArray<long>(ids, ',');
                await db.Delete<MenuEntity>(p => idArr.Contains(p.Id) || idArr.Contains(p.ParentId));
                await db.Delete<MenuAuthorizeEntity>(p => idArr.Contains(p.MenuId));
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
            var expression = ExtensionLinq.True<MenuEntity>();
            if (param != null)
            {
                if (!string.IsNullOrEmpty(param.MenuName))
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
