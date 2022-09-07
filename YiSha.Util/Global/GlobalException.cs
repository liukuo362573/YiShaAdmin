using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Web;
using YiSha.Util.Model;

namespace YiSha.Util
{
    /// <summary>
    /// 全局异常捕获
    /// </summary>
    public class GlobalException : IExceptionFilter, IAsyncExceptionFilter
    {
        /// <summary>
        /// 全局异常捕获
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public Task OnExceptionAsync(ExceptionContext context)
        {
            if (GlobalConstant.IsProduction)
            {
                OnException(context);
            }
            return Task.CompletedTask;
        }

        /// <summary>
        /// 全局异常捕获
        /// </summary>
        /// <param name="context"></param>
        public void OnException(ExceptionContext context)
        {
            LogHelper.Error(context.Exception);

            var errorMessage = context.Exception.GetOriginalException().Message;
            if (context.HttpContext.Request.IsAjaxRequest())
            {
                //抱歉，系统错误，请联系管理员！
                context.Result = new JsonResult("System Error!");
                context.ExceptionHandled = true;
            }
            else
            {
                context.Result = new RedirectResult($"~/Home/Error?message={HttpUtility.UrlEncode(errorMessage)}");
                context.ExceptionHandled = true;
            }
        }
    }
}
