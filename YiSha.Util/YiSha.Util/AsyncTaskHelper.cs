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
      public static void StartTask(Func<Task> action)
      {
          StartTaskWithTry(action);
      }
      /// <summary>
      /// 开始异步任务
      /// </summary>
      /// <param name="action"></param>
      public static void StartTask(Action action)
      {
          StartTaskWithTry(action);
      } 
      private static void StartTaskWithTry(Action action)
      {
          try
          {
              action();
          }
          catch (Exception ex)
          {
              LogHelper.Error(ex);  
          }
      }
      
      private async static void StartTaskWithTry(Func<Task> action)
      {
          try
          {
              await action();
          }
          catch (Exception ex)
          {
              LogHelper.Error(ex);  
          }
      }
    }
}
