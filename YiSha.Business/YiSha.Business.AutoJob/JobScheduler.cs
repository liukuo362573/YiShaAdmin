using Quartz;
using Quartz.Impl;

namespace YiSha.Business.AutoJob
{
    public class JobScheduler
    {
        private static readonly object _lockHelper = new();

        private static readonly IScheduler _scheduler = null;

        public static IScheduler GetScheduler()
        {
            lock (_lockHelper)
            {
                if (_scheduler != null)
                {
                    return _scheduler;
                }
                ISchedulerFactory schedf = new StdSchedulerFactory();
                IScheduler sched = schedf.GetScheduler().Result;
                return sched;
            }
        }
    }
}