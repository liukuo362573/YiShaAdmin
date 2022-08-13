using System.Linq.Expressions;
using YiSha.Common;
using YiSha.DataBase;
using YiSha.Entity.OrganizationManage;
using YiSha.Model.Param.OrganizationManage;
using YiSha.Util;

namespace YiSha.Service.OrganizationManage
{
    public class DepartmentService : Repository
    {
        #region 获取数据
        public async Task<List<DepartmentEntity>> GetList(DepartmentListParam param)
        {
            var expression = ListFilter(param);
            var list = await this.FindList(expression);
            return list.OrderBy(p => p.DepartmentSort).ToList();
        }

        public async Task<DepartmentEntity> GetEntity(long id)
        {
            return await this.FindEntity<DepartmentEntity>(id);
        }

        public async Task<int> GetMaxSort()
        {
            object result = await this.FindObject("SELECT MAX(DepartmentSort) FROM SysDepartment");
            int sort = result.ToInt();
            sort++;
            return sort;
        }

        /// <summary>
        /// 部门名称是否存在
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool ExistDepartmentName(DepartmentEntity entity)
        {
            var expression = ExtensionLinq.True<DepartmentEntity>();
            expression = expression.And(t => t.BaseIsDelete == 0);
            if (entity.Id.IsNullOrZero())
            {
                expression = expression.And(t => t.DepartmentName == entity.DepartmentName && t.ParentId == entity.ParentId);
            }
            else
            {
                expression = expression.And(t => t.DepartmentName == entity.DepartmentName && t.ParentId == entity.ParentId && t.Id != entity.Id);
            }
            return this.IQueryable(expression).Count() > 0 ? true : false;
        }

        /// <summary>
        /// 是否存在子部门
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool ExistChildrenDepartment(long id)
        {
            var expression = ExtensionLinq.True<DepartmentEntity>();
            expression = expression.And(t => t.ParentId == id);
            return this.IQueryable(expression).Count() > 0 ? true : false;
        }
        #endregion

        #region 提交数据
        public async Task SaveForm(DepartmentEntity entity)
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
                await db.Delete<DepartmentEntity>(idArr);
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
        private Expression<Func<DepartmentEntity, bool>> ListFilter(DepartmentListParam param)
        {
            var expression = ExtensionLinq.True<DepartmentEntity>();
            if (param != null)
            {
                if (!param.DepartmentName.IsNull())
                {
                    expression = expression.And(t => t.DepartmentName.Contains(param.DepartmentName));
                }
            }
            return expression;
        }
        #endregion
    }
}
