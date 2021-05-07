using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;
using System.Web;
using YiSha.Util.Extension;
using YiSha.Util.Helper;
using YiSha.Util.Model;

namespace YiSha.Admin.Web.Filter
{
    /// <summary>
    /// 全局异常过滤器
    /// </summary>
    public class GlobalExceptionFilter : IExceptionFilter, IAsyncExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            LogHelper.Error(context.Exception);
            string message = context.Exception.GetOriginalException().Message;
            IActionResult result = new RedirectResult("~/Home/Error?message=" + HttpUtility.UrlEncode(message));

            if (context.HttpContext.Request.IsAjaxRequest())
            {
                if (string.IsNullOrEmpty(message))
                {
                    message = "抱歉，系统错误，请联系管理员！";
                }
                result = new JsonResult(new TData { Message = message });
            }

            context.Result = result;
            context.ExceptionHandled = true;
        }

        public Task OnExceptionAsync(ExceptionContext context)
        {
            OnException(context);
            return Task.CompletedTask;
        }
    }
}