using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using YiSha.Admin.Web.Controllers;
using YiSha.Business.SystemManage;
using YiSha.Model.Result;
using YiSha.Util.Extension;
using YiSha.Util.Model;
using YiSha.Web.Code;

namespace YiSha.Admin.Web.Controllers
{
    public class AuthorizeFilterAttribute : ActionFilterAttribute
    {
        public AuthorizeFilterAttribute() { }

        public AuthorizeFilterAttribute(string authorize)
        {
            this.Authorize = authorize;
        }

        /// <summary>
        /// 权限字符串，例如 organization:user:view
        /// </summary>
        public string Authorize { get; set; }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            bool hasPermission = false;

            OperatorInfo user = await Operator.Instance.Current();
            if (user == null || user.UserId == 0)
            {
                #region 没有登录
                if (context.HttpContext.Request.IsAjaxRequest())
                {
                    TData obj = new TData();
                    obj.Message = "抱歉，没有登录或登录已超时";
                    context.Result = new JsonResult(obj);
                    return;
                }
                else
                {
                    context.Result = new RedirectResult("~/Home/Login");
                    return;
                }
                #endregion
            }
            else
            {
                // 系统用户拥有所有权限
                if (user.IsSystem == 1)
                {
                    hasPermission = true;
                }
                else
                {
                    // 权限判断
                    if (!string.IsNullOrEmpty(Authorize))
                    {
                        string[] authorizeList = Authorize.Split(',');
                        TData<List<MenuAuthorizeInfo>> objMenuAuthorize = await new MenuAuthorizeBLL().GetAuthorizeList(user);
                        List<MenuAuthorizeInfo> authorizeInfoList = objMenuAuthorize.Result.Where(p => authorizeList.Contains(p.Authorize)).ToList();
                        if (authorizeInfoList.Any())
                        {
                            hasPermission = true;

                            #region  新增和修改判断
                            if (context.RouteData.Values["Action"].ToString() == "SaveFormJson")
                            {
                                var id = context.HttpContext.Request.Form["Id"];
                                if (id.ParseToLong() > 0)
                                {
                                    if (!authorizeInfoList.Where(p => p.Authorize.Contains("edit")).Any())
                                    {
                                        hasPermission = false;
                                    }
                                }
                                else
                                {
                                    if (!authorizeInfoList.Where(p => p.Authorize.Contains("add")).Any())
                                    {
                                        hasPermission = false;
                                    }
                                }
                            }
                            #endregion
                        }
                        if (!hasPermission)
                        {
                            if (context.HttpContext.Request.IsAjaxRequest())
                            {
                                TData obj = new TData();
                                obj.Message = "抱歉，没有权限";
                                context.Result = new JsonResult(obj);
                            }
                            else
                            {
                                context.Result = new RedirectResult("~/Home/NoPermission");
                            }
                        }
                    }
                    else
                    {
                        hasPermission = true;
                    }
                }
                if (hasPermission)
                {
                    var resultContext = await next();
                }
            }
        }
    }
}
