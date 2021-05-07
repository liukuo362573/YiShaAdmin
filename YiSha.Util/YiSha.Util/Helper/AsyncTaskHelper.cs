using System;
using System.Threading.Tasks;

namespace YiSha.Util.Helper
{
    public static class AsyncTaskHelper
    {
        /// <summary>
        /// 开始异步任务
        /// </summary>
        public static void StartTask(Action action)
        {
            try
            {
                Action newAction = () => { };
                newAction += action;
                var task = new Task(newAction);
                task.Start();
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }
    }
}