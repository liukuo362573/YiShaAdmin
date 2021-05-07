using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YiSha.Business.SystemManage;
using YiSha.Model.Result;
using YiSha.Util.Extension;
using YiSha.Util.Model;
using YiSha.Web.Code;
using YiSha.Web.Code.State;

namespace YiSha.Admin.Web.Filter
{
    public class AuthorizeFilterAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// 权限字符串，例如 organization:user:view
        /// </summary>
        private string Authorize { get; }

        public AuthorizeFilterAttribute() { }

        public AuthorizeFilterAttribute(string authorize) => Authorize = authorize;

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var user = await Operator.Instance.Current();
            if (user == null || user.UserId == 0)
            {
                NotSignIn(context);
                return;
            }

            // 是否系统管理员
            bool hasPermission = user.IsSystem == 1;
            if (hasPermission)
            {
                await next();
                return;
            }

            // 是否设置权限
            hasPermission = !string.IsNullOrEmpty(Authorize);
            if (hasPermission)
            {
                await next();
                return;
            }

            // 权限验证
            var list = await GetAuthorizeInfoList(user);
            if (list.TryAny())
            {
                // 新增和修改判断
                string action = context.RouteData.Values["Action"].ToString();
                if (action == "SaveFormJson")
                {
                    var id = context.HttpContext.Request.Form["Id"];
                    string contains = id.ParseToLong() > 0 ? "edit" : "add";
                    hasPermission = list.Any(p => p.Authorize.Contains(contains));
                }

                // TODO 缺失其他权限验证的处理
                else
                    hasPermission = true;
            }

            if (!hasPermission)
            {
                NoPermission(context);
                return;
            }

            _ = await next();
        }

        private async Task<IEnumerable<MenuAuthorizeInfo>> GetAuthorizeInfoList(OperatorInfo user)
        {
            var list = await new MenuAuthorizeBLL().GetAuthorizeList(user);
            if (!string.IsNullOrEmpty(Authorize)) return list.Data;
            return list.Data.Where(p => Authorize.Split(',').Contains(p.Authorize));
        }

        private void NotSignIn(ActionExecutingContext context)
        {
            // 防止用户选择记住我，页面一直在首页刷新
            if (CookieHelper.GetCookie("RememberMe").ParseToInt() == 1)
            {
                Operator.Instance.RemoveCurrent();
            }
            var urlHelper = GlobalContext.ServiceProvider?.GetService<IUrlHelperFactory>().GetUrlHelper(context);
            HandleFailure(context, "抱歉，没有登录或登录已超时", urlHelper.Action("Login", "Home"));
        }

        private void NoPermission(ActionExecutingContext context)
        {
            var urlHelper = GlobalContext.ServiceProvider?.GetService<IUrlHelperFactory>().GetUrlHelper(context);
            HandleFailure(context, "抱歉，没有权限", urlHelper.Action("NoPermission", "Home"));
        }

        private void HandleFailure(ActionExecutingContext context, string message, string redirect)
        {
            if (context.HttpContext.Request.IsAjaxRequest())
            {
                context.Result = new JsonResult(new TData { Message = message });
            }
            else
            {
                context.Result = new RedirectResult(redirect);
            }
        }
    }
}