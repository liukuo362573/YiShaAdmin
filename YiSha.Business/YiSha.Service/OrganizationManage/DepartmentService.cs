﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using YiSha.Data.Repository;
using YiSha.Entity.OrganizationManage;
using YiSha.Model.Param.OrganizationManage;
using YiSha.Util.Extension;
using YiSha.Util.Helper;

namespace YiSha.Service.OrganizationManage
{
    public class DepartmentService : RepositoryFactory
    {
        #region 获取数据

        public async Task<List<DepartmentEntity>> GetList(DepartmentListParam param)
        {
            var expression = ListFilter(param);
            var list = await BaseRepository().FindList(expression);
            return list.OrderBy(p => p.DepartmentSort).ToList();
        }

        public async Task<DepartmentEntity> GetEntity(long id)
        {
            return await BaseRepository().FindEntity<DepartmentEntity>(id);
        }

        public async Task<int> GetMaxSort()
        {
            var result = await BaseRepository().FindEntity<int>("SELECT MAX(DepartmentSort) FROM SysDepartment");
            return result + 1;
        }

        /// <summary>
        /// 部门名称是否存在
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool ExistDepartmentName(DepartmentEntity entity)
        {
            var expression = LinqExtensions.True<DepartmentEntity>();
            expression = expression.And(t => t.BaseIsDelete == 0);
            if (entity.Id.IsNullOrZero())
            {
                expression = expression.And(t => t.DepartmentName == entity.DepartmentName);
            }
            else
            {
                expression = expression.And(t => t.DepartmentName == entity.DepartmentName && t.Id != entity.Id);
            }
            return BaseRepository().AsQueryable(expression).Count() > 0 ? true : false;
        }

        /// <summary>
        /// 是否存在子部门
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool ExistChildrenDepartment(long id)
        {
            var expression = LinqExtensions.True<DepartmentEntity>();
            expression = expression.And(t => t.ParentId == id);
            return BaseRepository().AsQueryable(expression).Count() > 0 ? true : false;
        }

        #endregion

        #region 提交数据

        public async Task SaveForm(DepartmentEntity entity)
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
            var expression = LinqExtensions.True<DepartmentEntity>();
            if (param != null)
            {
                if (!param.DepartmentName.IsEmpty())
                {
                    expression = expression.And(t => t.DepartmentName.Contains(param.DepartmentName));
                }
            }
            return expression;
        }

        #endregion
    }
}