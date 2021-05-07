using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YiSha.Business.Cache;
using YiSha.Entity.SystemManage;
using YiSha.Enum.SystemManage;
using YiSha.Model.Param.SystemManage;
using YiSha.Service.SystemManage;
using YiSha.Util.Extension;
using YiSha.Util.Model;

namespace YiSha.Business.SystemManage
{
    public class RoleBLL
    {
        private readonly RoleService _roleService = new();

        private readonly MenuAuthorizeService _menuAuthorizeService = new();

        private readonly MenuAuthorizeCache _menuAuthorizeCache = new();

        #region 获取数据

        public async Task<TData<List<RoleEntity>>> GetList(RoleListParam param)
        {
            var list = await _roleService.GetList(param);
            return new() { Data = list, Total = list.Count, Tag = 1 };
        }

        public async Task<TData<List<RoleEntity>>> GetPageList(RoleListParam param, Pagination pagination)
        {
            return new()
            {
                Data = await _roleService.GetPageList(param, pagination),
                Total = pagination.TotalCount,
                Tag = 1
            };
        }

        public async Task<TData<RoleEntity>> GetEntity(long id)
        {
            var menuAuthorizeList = await _menuAuthorizeService.GetList(new MenuAuthorizeEntity
            {
                AuthorizeId = id,
                AuthorizeType = AuthorizeTypeEnum.Role.ParseToInt()
            });
            // 获取角色对应的权限
            var roleEntity = await _roleService.GetEntity(id);
            roleEntity.MenuIds = string.Join(",", menuAuthorizeList.Select(p => p.MenuId));
            return new() { Data = roleEntity, Tag = 1 };
        }

        public async Task<TData<int>> GetMaxSort()
        {
            return new() { Data = await _roleService.GetMaxSort(), Tag = 1 };
        }

        #endregion

        #region 提交数据

        public async Task<TData<string>> SaveForm(RoleEntity entity)
        {
            if (_roleService.ExistRoleName(entity))
            {
                return new() { Tag = 0, Message = "角色名称已经存在！" };
            }
            await _roleService.SaveForm(entity);
            _menuAuthorizeCache.Remove();
            return new() { Data = entity.Id.ParseToString(), Tag = 1 };
        }

        public async Task<TData> DeleteForm(string ids)
        {
            await _roleService.DeleteForm(ids);
            _menuAuthorizeCache.Remove();
            return new() { Tag = 1 };
        }

        #endregion
    }
}