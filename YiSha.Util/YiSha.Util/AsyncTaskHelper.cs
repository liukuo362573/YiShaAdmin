using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace YiSha.Util
{
    public class AsyncTaskHelper
    {
        /// <summary>
        /// 开始异步任务
        /// </summary>
        /// <param name="action"></param>
        public static void StartTask(Action action)
        {
            try
            {
                Action newAction = () =>
                { };
                newAction += action;
                Task task = new Task(newAction);
                task.Start();
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }
    }
}
