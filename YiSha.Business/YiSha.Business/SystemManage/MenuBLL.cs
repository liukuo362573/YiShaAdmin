﻿using System.Collections.Generic;
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
    public class MenuBLL
    {
        private readonly MenuService _menuService = new();

        private readonly MenuCache _menuCache = new();

        #region 获取数据

        public async Task<TData<List<MenuEntity>>> GetList(MenuListParam param)
        {
            var obj = new TData<List<MenuEntity>>();

            List<MenuEntity> list = await _menuCache.GetList();
            list = ListFilter(param, list);

            obj.Data = list;
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<List<ZtreeInfo>>> GetZtreeList(MenuListParam param)
        {
            var obj = new TData<List<ZtreeInfo>>();
            obj.Data = new List<ZtreeInfo>();

            List<MenuEntity> list = await _menuCache.GetList();
            list = ListFilter(param, list);

            foreach (MenuEntity menu in list)
            {
                obj.Data.Add(new ZtreeInfo
                {
                    id = menu.Id,
                    pId = menu.ParentId,
                    name = menu.MenuName
                });
            }

            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<MenuEntity>> GetEntity(long id)
        {
            TData<MenuEntity> obj = new TData<MenuEntity>();
            obj.Data = await _menuService.GetEntity(id);
            if (obj.Data != null)
            {
                long parentId = obj.Data.ParentId.Value;
                if (parentId > 0)
                {
                    MenuEntity parentMenu = await _menuService.GetEntity(parentId);
                    if (parentMenu != null)
                    {
                        obj.Data.ParentName = parentMenu.MenuName;
                    }
                }
                else
                {
                    obj.Data.ParentName = "主目录";
                }
            }
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData<int>> GetMaxSort(long parentId)
        {
            TData<int> obj = new TData<int>();
            obj.Data = await _menuService.GetMaxSort(parentId);
            obj.Tag = 1;
            return obj;
        }

        #endregion

        #region 提交数据

        public async Task<TData<string>> SaveForm(MenuEntity entity)
        {
            TData<string> obj = new TData<string>();
            if (!entity.Id.IsNullOrZero() && entity.Id == entity.ParentId)
            {
                obj.Message = "不能选择自己作为上级菜单！";
                return obj;
            }
            if (_menuService.ExistMenuName(entity))
            {
                obj.Message = "菜单名称已经存在！";
                return obj;
            }
            await _menuService.SaveForm(entity);

            _menuCache.Remove();

            obj.Data = entity.Id.ParseToString();
            obj.Tag = 1;
            return obj;
        }

        public async Task<TData> DeleteForm(string ids)
        {
            TData obj = new TData();
            await _menuService.DeleteForm(ids);

            _menuCache.Remove();
            obj.Tag = 1;
            return obj;
        }

        #endregion

        #region 私有方法

        private List<MenuEntity> ListFilter(MenuListParam param, List<MenuEntity> list)
        {
            if (param != null)
            {
                if (!string.IsNullOrEmpty(param.MenuName))
                {
                    list = list.Where(p => p.MenuName.Contains(param.MenuName)).ToList();
                }
                if (param.MenuStatus > 0)
                {
                    list = list.Where(p => p.MenuStatus == param.MenuStatus).ToList();
                }
            }
            return list;
        }

        #endregion
    }
}