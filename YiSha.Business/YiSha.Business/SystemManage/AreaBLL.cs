using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YiSha.Business.Cache;
using YiSha.Entity.SystemManage;
using YiSha.Model.Param.SystemManage;
using YiSha.Model.Result;
using YiSha.Service.SystemManage;
using YiSha.Util.Extension;
using YiSha.Util.Model;

namespace YiSha.Business.SystemManage
{
    public class AreaBLL
    {
        private readonly AreaService _areaService = new();

        private readonly AreaCache _areaCache = new();

        #region 获取数据

        public async Task<TData<List<AreaEntity>>> GetList(AreaListParam param)
        {
            var areaList = await _areaCache.GetList();
            if (param?.AreaName?.Length > 0)
            {
                areaList = areaList.Where(t => t.AreaName.Contains(param.AreaName)).ToList();
            }
            return new() { Data = areaList, Tag = 1 };
        }

        public async Task<TData<List<AreaEntity>>> GetPageList(AreaListParam param, Pagination pagination)
        {
            return new()
            {
                Data = await _areaService.GetPageList(param, pagination),
                Total = pagination.TotalCount,
                Tag = 1
            };
        }

        public async Task<TData<List<ZtreeInfo>>> GetZtreeAreaList(AreaListParam param)
        {
            var list = await _areaCache.GetList();
            return new()
            {
                Tag = 1,
                Data = list.Select(area => new ZtreeInfo
                {
                    id = area.AreaCode.ParseToLong(),
                    pId = area.ParentAreaCode.ParseToLong(),
                    name = area.AreaName
                }).ToList()
            };
        }

        public async Task<TData<AreaEntity>> GetEntity(long id)
        {
            var data = await _areaService.GetEntity(id);
            return new() { Tag = data == null ? 0 : 1, Data = data };
        }

        public async Task<TData<AreaEntity>> GetEntityByAreaCode(string areaCode)
        {
            var data = await _areaService.GetEntityByAreaCode(areaCode);
            return new() { Tag = data == null ? 0 : 1, Data = data };
        }

        #endregion

        #region 提交数据

        public async Task<TData<string>> SaveForm(AreaEntity entity)
        {
            await _areaService.SaveForm(entity);
            return new() { Data = entity.Id.ParseToString(), Tag = 1 };
        }

        public async Task<TData> DeleteForm(string ids)
        {
            await _areaService.DeleteForm(ids);
            return new() { Tag = 1 };
        }

        #endregion

        #region 公有方法

        public void SetAreaParam<T>(T t) where T : BaseAreaParam
        {
            if (t is BaseAreaParam { AreaId: { Length: > 0 } } param)
            {
                string[] areaIdArr = param.AreaId.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                if (areaIdArr.Length >= 1)
                {
                    param.ProvinceId = areaIdArr[0].ParseToInt();
                }
                if (areaIdArr.Length >= 2)
                {
                    param.CityId = areaIdArr[1].ParseToInt();
                }
                if (areaIdArr.Length >= 3)
                {
                    param.CountyId = areaIdArr[2].ParseToInt();
                }
            }
        }

        public void SetAreaEntity<T>(T t) where T : BaseAreaEntity
        {
            if (t is BaseAreaEntity { AreaId: { Length: > 0 } } entity)
            {
                string[] areaIdArr = entity.AreaId.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                entity.ProvinceId = areaIdArr.Length >= 1 ? areaIdArr[0].ParseToInt() : 0;
                entity.CityId = areaIdArr.Length >= 2 ? areaIdArr[1].ParseToInt() : 0;
                entity.CountyId = areaIdArr.Length >= 3 ? areaIdArr[2].ParseToInt() : 0;
            }
        }

        public void SetAreaId<T>(T t) where T : BaseAreaEntity
        {
            if (t is BaseAreaEntity entity)
            {
                entity.AreaId = string.Empty;
                if (!entity.ProvinceId.IsNullOrZero())
                {
                    entity.AreaId += entity.ProvinceId.ParseToString() + ",";
                    if (!entity.CityId.IsNullOrZero())
                    {
                        entity.AreaId += entity.CityId.ParseToString() + ",";
                        if (!entity.CountyId.IsNullOrZero())
                        {
                            entity.AreaId += entity.CountyId.ParseToString() + ",";
                        }
                    }
                }
                entity.AreaId = entity.AreaId.Trim(',');
            }
        }

        #endregion
    }
}