using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YiSha.Business.SystemManage;
using YiSha.Entity.SystemManage;
using YiSha.Enum;
using YiSha.Util.Extension;
using YiSha.Util.Helper;
using YiSha.Web.Code;

namespace YiSha.Admin.Web.Controllers
{
    /// <summary>
    /// 基础控制器，用来记录访问日志
    /// </summary>
    public class BaseController : Controller
    {
        protected OperatorInfo Current { get; private set; }

        private string Action { get; set; }

        private string Method { get; set; }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            await Initialize(context);

            var stopwatch = Stopwatch.StartNew();
            var resultContext = await next();
            stopwatch.Stop();

            string[] notLogAction = { "GetServerJson", "Error" };
            if (!notLogAction.Select(p => p.ToUpper()).Contains(Action.ToUpper()))
            {
                var operateEntity = BuildLogOperateEntity(context, resultContext, stopwatch);
                AsyncTaskHelper.StartTask(async () => await new LogOperateBLL().SaveForm(operateEntity));
            }
        }

        private async Task Initialize(ActionContext context)
        {
            Current = await Operator.Instance.Current();
            Action = context.RouteData.Values["Action"].ParseToString();
            Method = context.HttpContext.Request.Method.ToUpper();
        }

        private LogOperateEntity BuildLogOperateEntity(ActionExecutingContext context, ActionExecutedContext resultContext, Stopwatch stopwatch)
        {
            var operateEntity = LogRequestParam(context);
            operateEntity.ExecuteTime = stopwatch.ElapsedMilliseconds.ParseToInt();
            operateEntity.IpAddress = NetHelper.Ip;
            operateEntity.ExecuteUrl = BuildExecuteUrl(context);
            operateEntity.ExecuteResult = BuildExecuteResult(resultContext);
            operateEntity.LogStatus = GetLogStatus(resultContext);
            operateEntity.BaseCreatorId = Current?.UserId ?? 0;
            // operateEntity.IpLocation = IpLocationHelper.GetIpLocation(ipAddress);
            return operateEntity;
        }

        private LogOperateEntity LogRequestParam(ActionExecutingContext context)
        {
            var operateEntity = new LogOperateEntity();
            string queryString = context.HttpContext.Request.QueryString.Value.ParseToString();
            if (Method == "GET")
            {
                operateEntity.ExecuteParam = queryString;
            }
            else if (Method == "POST")
            {
                if (context.ActionArguments?.Count > 0)
                {
                    string json = JsonConvert.SerializeObject(context.ActionArguments);
                    operateEntity.ExecuteUrl += queryString;
                    operateEntity.ExecuteParam = TextHelper.GetSubString(json, 4000);
                }
                else
                {
                    operateEntity.ExecuteParam = queryString;
                }
            }
            return operateEntity;
        }

        private string BuildExecuteUrl(ActionContext context)
        {
            var area = context.RouteData.DataTokens["area"] + "/";
            var controller = context.RouteData.Values["controller"] + "/";
            string url = "/" + area + controller + Action;
            return url.Replace("//", "/");
        }

        private string BuildExecuteResult(ActionExecutedContext resultContext)
        {
            var builder = new StringBuilder();
            if (resultContext.Exception != null)
            {
                var exception = resultContext.Exception;
                builder.AppendLine(exception.Message);
                while (exception.InnerException != null)
                {
                    builder.AppendLine(exception.InnerException.Message);
                    exception = exception.InnerException;
                }
                builder.AppendLine(resultContext.Exception.StackTrace);
            }
            return TextHelper.GetSubString(builder.ToString(), 4000);
        }

        private int GetLogStatus(ActionExecutedContext resultContext)
        {
            return resultContext.Exception != null ? OperateStatusEnum.Fail.ParseToInt() : OperateStatusEnum.Success.ParseToInt();
        }
    }
}