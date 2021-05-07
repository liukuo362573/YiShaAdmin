﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using YiSha.Data.Repository;
using YiSha.Entity.SystemManage;
using YiSha.Model.Param.SystemManage;
using YiSha.Util.Extension;
using YiSha.Util.Helper;
using YiSha.Util.Model;

namespace YiSha.Service.SystemManage
{
    public class AreaService : RepositoryFactory
    {
        #region 获取数据

        public async Task<List<AreaEntity>> GetList(AreaListParam param)
        {
            var expression = ListFilter(param);
            return await BaseRepository().FindList(expression);
        }

        public async Task<List<AreaEntity>> GetPageList(AreaListParam param, Pagination pagination)
        {
            var expression = ListFilter(param);
            return await BaseRepository().FindList(expression, pagination);
        }

        public async Task<AreaEntity> GetEntity(long id)
        {
            return await BaseRepository().FindEntity<AreaEntity>(id);
        }

        public async Task<AreaEntity> GetEntityByAreaCode(string areaCode)
        {
            return await BaseRepository().FindEntity<AreaEntity>(p => p.AreaCode == areaCode);
        }

        #endregion

        #region 提交数据

        public async Task SaveForm(AreaEntity entity)
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
            var idArr = TextHelper.SplitToArray<object>(ids, ',');
            await BaseRepository().Delete<AreaEntity>(idArr);
        }

        #endregion

        #region 私有方法

        private Expression<Func<AreaEntity, bool>> ListFilter(AreaListParam param)
        {
            var expression = LinqExtensions.True<AreaEntity>();
            if (param is { AreaName: { Length: > 0 } })
            {
                expression = expression.And(t => t.AreaName.Contains(param.AreaName));
            }
            return expression;
        }

        #endregion
    }
}