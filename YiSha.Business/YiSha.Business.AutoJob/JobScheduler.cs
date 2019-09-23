using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YiSha.Business.AutoJob
{
    public class JobScheduler
    {
        private static object lockHelper = new object();

        private static IScheduler scheduler = null;
        public static IScheduler GetScheduler()
        {
            lock (lockHelper)
            {
                if (scheduler != null)
                {
                    return scheduler;
                }
                else
                {
                    ISchedulerFactory schedf = new StdSchedulerFactory();
                    IScheduler sched = schedf.GetScheduler().Result;
                    return sched;
                }
            }
        }
    }
}
