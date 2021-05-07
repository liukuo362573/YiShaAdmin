using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YiSha.Business.Cache;
using YiSha.Entity.SystemManage;
using YiSha.Enum;
using YiSha.Enum.SystemManage;
using YiSha.Model.Result;
using YiSha.Util.Extension;
using YiSha.Util.Model;
using YiSha.Web.Code;

namespace YiSha.Business.SystemManage
{
    public class MenuAuthorizeBLL
    {
        private readonly MenuAuthorizeCache _menuAuthorizeCache = new();

        private readonly MenuCache _menuCache = new();

        #region 获取数据

        public async Task<TData<List<MenuAuthorizeInfo>>> GetAuthorizeList(OperatorInfo user)
        {
            var authorizeList = new List<MenuAuthorizeEntity>();
            var menuList = await _menuCache.GetList();
            var enableMenuIdList = menuList.Where(p => p.MenuStatus == (int)StatusEnum.Yes).Select(p => p.Id);
            var menuAuthorizeCacheList = await _menuAuthorizeCache.GetList(p => enableMenuIdList.Contains(p.MenuId));

            // 角色
            IEnumerable<MenuAuthorizeEntity> roleAuthorizeList = null;
            if (user.RoleIds?.Length > 0)
            {
                var roleIdList = user.RoleIds.Split(',').Select(long.Parse);
                roleAuthorizeList = menuAuthorizeCacheList.Where(p => roleIdList.Contains(p.AuthorizeId!.Value) && p.AuthorizeType == AuthorizeTypeEnum.Role.ParseToInt());
            }

            // 用户
            var userAuthorizeList = menuAuthorizeCacheList.Where(p => p.AuthorizeId == user.UserId && p.AuthorizeType == AuthorizeTypeEnum.User.ParseToInt());
            if (userAuthorizeList.Any())
            {
                authorizeList.AddRange(userAuthorizeList);
                roleAuthorizeList = roleAuthorizeList!.Where(p => !userAuthorizeList.Select(u => u.AuthorizeId).Contains(p.AuthorizeId));
            }

            // 排除重复的记录
            if (roleAuthorizeList?.Any() ?? false)
            {
                authorizeList.AddRange(roleAuthorizeList);
            }

            return new()
            {
                Tag = 1,
                Data = authorizeList.Select(authorize => new MenuAuthorizeInfo
                {
                    MenuId = authorize.MenuId,
                    AuthorizeId = authorize.AuthorizeId,
                    AuthorizeType = authorize.AuthorizeType,
                    Authorize = menuList.Where(t => t.Id == authorize.MenuId).Select(t => t.Authorize).FirstOrDefault()
                }).ToList()
            };
        }

        #endregion
    }
}