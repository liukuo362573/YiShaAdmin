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
    public class MenuBLL
    {
        private readonly MenuService _menuService = new();

        private readonly MenuCache _menuCache = new();

        #region 获取数据

        public async Task<TData<List<MenuEntity>>> GetList(MenuListParam param)
        {
            var list = await _menuCache.GetList();
            list = ListFilter(param, list);
            return new() { Data = list, Tag = 1 };
        }

        public async Task<TData<List<ZtreeInfo>>> GetZtreeList(MenuListParam param)
        {
            var list = await _menuCache.GetList();
            list = ListFilter(param, list);
            return new()
            {
                Data = list.Select(menu => new ZtreeInfo
                {
                    id = menu.Id,
                    pId = menu.ParentId,
                    name = menu.MenuName
                }).ToList(),
                Tag = 1
            };
        }

        public async Task<TData<MenuEntity>> GetEntity(long id)
        {
            var entity = await _menuService.GetEntity(id);
            if (entity != null)
            {
                if (entity.ParentId > 0)
                {
                    var parentMenu = await _menuService.GetEntity(entity.ParentId.Value);
                    entity.ParentName = parentMenu?.MenuName;
                }
                else
                {
                    entity.ParentName = "主目录";
                }
            }
            return new() { Data = entity, Tag = 1 };
        }

        public async Task<TData<int>> GetMaxSort(long parentId)
        {
            return new() { Data = await _menuService.GetMaxSort(parentId), Tag = 1 };
        }

        #endregion

        #region 提交数据

        public async Task<TData<string>> SaveForm(MenuEntity entity)
        {
            if (!entity.Id.IsNullOrZero() && entity.Id == entity.ParentId)
            {
                return new() { Tag = 0, Message = "不能选择自己作为上级菜单！" };
            }
            if (_menuService.ExistMenuName(entity))
            {
                return new() { Tag = 0, Message = "菜单名称已经存在！" };
            }
            await _menuService.SaveForm(entity);
            _menuCache.Remove();
            return new() { Data = entity.Id.ParseToString(), Tag = 1 };
        }

        public async Task<TData> DeleteForm(string ids)
        {
            await _menuService.DeleteForm(ids);
            _menuCache.Remove();
            return new() { Tag = 1 };
        }

        #endregion

        #region 私有方法

        private List<MenuEntity> ListFilter(MenuListParam param, List<MenuEntity> list)
        {
            if (param != null)
            {
                if (param.MenuName?.Length > 0)
                {
                    list = list.Where(p => p.MenuName.Contains(param.MenuName)).ToList();
                }
                if (param.MenuStatus > -1)
                {
                    list = list.Where(p => p.MenuStatus == param.MenuStatus).ToList();
                }
            }
            return list;
        }

        #endregion
    }
}