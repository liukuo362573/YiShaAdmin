using Quartz;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using YiSha.Entity.SystemManage;
using YiSha.Service.SystemManage;
using YiSha.Util;
using YiSha.Util.Extension;
using YiSha.Util.Model;

namespace YiSha.Business.AutoJob
{
    public class JobExecute : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            return Task.Run(async () =>
            {
                TData obj = new TData();
                try
                {
                    #region 执行任务
                    switch (context.JobDetail.Key.Name)
                    {
                        case "数据库备份":
                            obj = await new DatabasesBackupJob().Start();
                            break;
                    }
                    #endregion
                }
                catch (Exception ex)
                {
                    obj.Message = ex.GetOriginalException().Message;
                    LogHelper.Write(ex);
                }

                try
                {
                    #region 更新下次运行时间
                    JobDataMap jobData = context.JobDetail.JobDataMap;
                    await new AutoJobService().SaveForm(new AutoJobEntity
                    {
                        Id = jobData["Id"].ParseToLong(),
                        NextStartTime = context.NextFireTimeUtc.Value.DateTime.AddHours(8)
                    });
                    #endregion

                    #region 记录执行状态
                    await new AutoJobLogService().SaveForm(new AutoJobLogEntity
                    {
                        JobGroupName = context.JobDetail.Key.Group,
                        JobName = context.JobDetail.Key.Name,
                        LogStatus = obj.Tag,
                        Remark = obj.Message
                    });
                    #endregion
                }
                catch (Exception ex)
                {
                    obj.Message = ex.GetOriginalException().Message;
                    LogHelper.Write(ex);
                }
            });
        }
    }
}
