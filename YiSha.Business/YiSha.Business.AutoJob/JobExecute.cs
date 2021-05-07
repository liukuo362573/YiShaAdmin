using Quartz;
using Quartz.Impl.Triggers;
using System;
using System.Threading.Tasks;
using YiSha.Business.AutoJob.Job;
using YiSha.Entity.SystemManage;
using YiSha.Enum;
using YiSha.Service.SystemManage;
using YiSha.Util.Extension;
using YiSha.Util.Helper;
using YiSha.Util.Model;

namespace YiSha.Business.AutoJob
{
    public class JobExecute : IJob
    {
        private readonly AutoJobService _autoJobService = new();

        private readonly AutoJobLogService _autoJobLogService = new();

        public Task Execute(IJobExecutionContext context)
        {
            return Task.Run(async () =>
            {
                TData obj = new();
                AutoJobEntity dbJobEntity = null;
                try
                {
                    var jobData = context.JobDetail.JobDataMap;
                    var jobId = jobData["Id"].ParseToLong();
                    // 获取数据库中的任务
                    dbJobEntity = await _autoJobService.GetEntity(jobId);
                    if (dbJobEntity != null)
                    {
                        if (dbJobEntity.JobStatus == StatusEnum.Yes.ParseToInt())
                        {
                            CronTriggerImpl trigger = context.Trigger as CronTriggerImpl;
                            if (trigger != null)
                            {
                                if (trigger.CronExpressionString != dbJobEntity.CronExpression)
                                {
                                    // 更新任务周期
                                    trigger.CronExpressionString = dbJobEntity.CronExpression;
                                    await JobScheduler.GetScheduler().RescheduleJob(trigger.Key, trigger);
                                }

                                #region 执行任务

                                obj = context.JobDetail.Key.Name switch
                                {
                                    "数据库备份" => await new DatabasesBackupJob().Start(),
                                    _ => obj
                                };

                                #endregion
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    obj.Message = ex.GetOriginalException().Message;
                    LogHelper.Error(ex);
                }

                try
                {
                    if (dbJobEntity != null)
                    {
                        if (dbJobEntity.JobStatus == StatusEnum.Yes.ParseToInt())
                        {
                            #region 更新下次运行时间

                            await _autoJobService.SaveForm(new AutoJobEntity
                            {
                                Id = dbJobEntity.Id,
                                NextStartTime = context.NextFireTimeUtc.Value.DateTime.AddHours(8)
                            });

                            #endregion

                            #region 记录执行状态

                            await _autoJobLogService.SaveForm(new AutoJobLogEntity
                            {
                                JobGroupName = context.JobDetail.Key.Group,
                                JobName = context.JobDetail.Key.Name,
                                LogStatus = obj.Tag,
                                Remark = obj.Message
                            });

                            #endregion
                        }
                    }
                }
                catch (Exception ex)
                {
                    obj.Message = ex.GetOriginalException().Message;
                    LogHelper.Error(ex);
                }
            });
        }
    }
}