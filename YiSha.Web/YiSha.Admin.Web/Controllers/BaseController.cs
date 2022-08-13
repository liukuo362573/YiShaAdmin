using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text;
using YiSha.Business.SystemManage;
using YiSha.Entity;
using YiSha.Entity.SystemManage;
using YiSha.Enum;
using YiSha.Util;
using YiSha.Util.Model;

namespace YiSha.Admin.Web.Controllers
{
    /// <summary>
    /// 基础控制器，用来记录访问日志
    /// </summary>
    public class BaseController : Controller
    {
        /// <summary>
        /// 动作执行前
        /// </summary>
        /// <param name="context">上下文</param>
        /// <param name="next">动作执行下去</param>
        /// <returns></returns>
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var sw = new Stopwatch();
            sw.Start();

            var action = context.RouteData.Values["Action"].ToStr();
            var user = await Operator.Instance.Current();

            if (GlobalConstant.IsDevelopment)
            {
                if (context.HttpContext.Request.Method.ToUpper() == "POST")
                {
                    var allowAction = new string[] { "LoginJson", "ExportUserJson", "CodePreviewJson" };
                    if (!allowAction.Select(p => p.ToUpper()).Contains(action.ToUpper()))
                    {
                        var obj = new TData();
                        obj.Message = "演示模式，不允许操作";
                        context.Result = new JsonResult(obj);
                        return;
                    }
                }
            }

            var resultContext = await next();

            sw.Stop();
            var ip = NetHelper.Ip;
            var operateEntity = new LogOperateEntity();
            var areaName = context.RouteData.DataTokens["area"] + "/";
            var controllerName = context.RouteData.Values["controller"] + "/";
            var currentUrl = "/" + areaName + controllerName + action;

            var notLogAction = new string[] { "GetServerJson", "Error" };
            if (!notLogAction.Select(p => p.ToUpper()).Contains(action.ToUpper()))
            {
                #region 获取请求参数

                switch (context.HttpContext.Request.Method.ToUpper())
                {
                    case "GET":
                        operateEntity.ExecuteParam = context.HttpContext.Request.QueryString.Value.ToStr();
                        break;

                    case "POST":
                        if (context.ActionArguments?.Count > 0)
                        {
                            operateEntity.ExecuteUrl += context.HttpContext.Request.QueryString.Value.ToStr();
                            operateEntity.ExecuteParam = TextHelper.GetSubString(JsonConvert.SerializeObject(context.ActionArguments), 4000);
                        }
                        else
                        {
                            operateEntity.ExecuteParam = context.HttpContext.Request.QueryString.Value.ToStr();
                        }
                        break;
                }

                #endregion

                #region 异常获取

                var sbException = new StringBuilder();
                if (resultContext.Exception != null)
                {
                    var exception = resultContext.Exception;
                    sbException.AppendLine(exception.Message);
                    while (exception.InnerException != null)
                    {
                        sbException.AppendLine(exception.InnerException.Message);
                        exception = exception.InnerException;
                    }
                    sbException.AppendLine(resultContext.Exception.StackTrace);
                    operateEntity.LogStatus = OperateStatusEnum.Fail.ToInt();
                }
                else
                {
                    operateEntity.LogStatus = OperateStatusEnum.Success.ToInt();
                }

                #endregion

                #region 日志实体
                
                if (user != null)
                {
                    operateEntity.BaseCreatorId = user.UserId;
                }

                operateEntity.ExecuteTime = sw.ElapsedMilliseconds.ToInt();
                operateEntity.IpAddress = ip;
                operateEntity.ExecuteUrl = currentUrl.Replace("//", "/");
                operateEntity.ExecuteResult = TextHelper.GetSubString(sbException.ToString(), 4000);

                #endregion

                //存储到数据库
                async void taskAction()
                {
                    //耗时的任务异步完成
                    await new LogOperateBLL().SaveForm(operateEntity);
                }
                AsyncTaskHelper.StartTask(taskAction);
            }
        }

        /// <summary>
        /// 动作执行后
        /// </summary>
        /// <param name="context">上下文</param>
        /// <param name="next">下个动作</param>
        /// <returns></returns>
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);
        }
    }
}