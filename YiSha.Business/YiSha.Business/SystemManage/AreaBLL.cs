﻿using System;
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
            TData<List<AreaEntity>> obj = new TData<List<AreaEntity>>();
            List<AreaEntity> areaList = await _areaCache.GetList();
            if (param != null)
            {
                if (!param.AreaName.IsEmpty())
                {
                    areaList = areaList.Where(t => t.AreaName.Contains(param.AreaName)).ToList();
                }
            }
            obj.Data = areaList;
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<List<AreaEntity>>> GetPageList(AreaListParam param, Pagination pagination)
        {
            TData<List<AreaEntity>> obj = new TData<List<AreaEntity>>();
            obj.Data = await _areaService.GetPageList(param, pagination);
            obj.Total = pagination.TotalCount;
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<List<ZtreeInfo>>> GetZtreeAreaList(AreaListParam param)
        {
            var obj = new TData<List<ZtreeInfo>>();
            obj.Data = new List<ZtreeInfo>();
            List<AreaEntity> list = await _areaCache.GetList();
            foreach (AreaEntity area in list)
            {
                obj.Data.Add(new ZtreeInfo
                {
                    id = area.AreaCode.ParseToLong(),
                    pId = area.ParentAreaCode.ParseToLong(),
                    name = area.AreaName
                });
            }
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<AreaEntity>> GetEntity(long id)
        {
            TData<AreaEntity> obj = new TData<AreaEntity>();
            obj.Data = await _areaService.GetEntity(id);
            if (obj.Data != null)
            {
                obj.Tag = 1;
            }
            return obj;
        }

        public async Task<TData<AreaEntity>> GetEntityByAreaCode(string areaCode)
        {
            TData<AreaEntity> obj = new TData<AreaEntity>();
            obj.Data = await _areaService.GetEntityByAreaCode(areaCode);
            if (obj.Data != null)
            {
                obj.Tag = 1;
            }
            return obj;
        }

        #endregion

        #region 提交数据

        public async Task<TData<string>> SaveForm(AreaEntity entity)
        {
            TData<string> obj = new TData<string>();
            await _areaService.SaveForm(entity);
            obj.Data = entity.Id.ParseToString();
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData> DeleteForm(string ids)
        {
            TData obj = new TData();
            await _areaService.DeleteForm(ids);
            obj.Tag = 1;
            return obj;
        }

        #endregion

        #region 公有方法

        public void SetAreaParam<T>(T t) where T : BaseAreaParam
        {
            if (t != null)
            {
                BaseAreaParam param = t as BaseAreaParam;
                if (param != null)
                {
                    if (!string.IsNullOrEmpty(param.AreaId))
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
            }
        }

        public void SetAreaEntity<T>(T t) where T : BaseAreaEntity
        {
            if (t != null)
            {
                BaseAreaEntity entity = t as BaseAreaEntity;
                if (entity != null)
                {
                    if (!string.IsNullOrEmpty(entity.AreaId))
                    {
                        string[] areaIdArr = entity.AreaId.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                        if (areaIdArr.Length >= 1)
                        {
                            entity.ProvinceId = areaIdArr[0].ParseToInt();
                        }
                        else
                        {
                            entity.ProvinceId = 0;
                        }
                        if (areaIdArr.Length >= 2)
                        {
                            entity.CityId = areaIdArr[1].ParseToInt();
                        }
                        else
                        {
                            entity.CityId = 0;
                        }
                        if (areaIdArr.Length >= 3)
                        {
                            entity.CountyId = areaIdArr[2].ParseToInt();
                        }
                        else
                        {
                            entity.CountyId = 0;
                        }
                    }
                }
            }
        }

        public void SetAreaId<T>(T t) where T : BaseAreaEntity
        {
            if (t != null)
            {
                BaseAreaEntity entity = t as BaseAreaEntity;
                if (entity != null)
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
        }

        #endregion
    }
}