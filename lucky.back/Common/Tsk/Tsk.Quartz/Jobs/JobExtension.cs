using Quartz;

namespace Tsk.Quartz.Jobs
{
    /// <summary>
    /// 一次性任务
    /// </summary>
    public class OnceJobExtension
    {
        private readonly ISchedulerFactory _schedulerFactory;
        public OnceJobExtension(ISchedulerFactory schedulerFactory)
        {
            _schedulerFactory = schedulerFactory;
        }

        public async Task<bool> AddJob<T>(Dictionary<string, object>? jobPrms) where T : IJob
        {
            var jobName = typeof(T).Name;
            var jobKey = JobKey.Create(jobName);

            var job = JobBuilder.Create<T>()
                .WithIdentity(jobKey);

            if (jobPrms != null)
            {
                var jobDataMap = new JobDataMap(); jobDataMap.PutAll(jobPrms);
                job.UsingJobData(jobDataMap);
            }

            var _scheduler = await _schedulerFactory.GetScheduler();
            // 🔥 关键：确保调度器已启动
            if (!_scheduler.IsStarted)
            {
                await _scheduler.Start();
            }

            await _scheduler.TriggerJob(jobKey); // 立即执行

            return true;
        }
    }

    /// <summary>
    /// 周期性间隔任务
    /// </summary>
    public class IntervalJobExtension
    {
        private readonly ISchedulerFactory _schedulerFactory;
        public IntervalJobExtension(ISchedulerFactory schedulerFactory)
        {
            _schedulerFactory = schedulerFactory;
        }

        /// <summary>
        /// 间隔任务
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jobPrms"></param>
        /// <param name="intervals">单位: 秒</param>
        public async Task<bool> AddJob<T>(Dictionary<string, object>? jobPrms, int intervals = 1) where T : IJob
        {
            var jobName = typeof(T).Name;
            var jobKey = JobKey.Create(jobName);

            var job = JobBuilder.Create<T>()
                .WithIdentity(jobKey);

            if (jobPrms != null)
            {
                var jobDataMap = new JobDataMap(); jobDataMap.PutAll(jobPrms);
                job.UsingJobData(jobDataMap);
            }

            var scheduler = SimpleScheduleBuilder.Create()
                .WithInterval(TimeSpan.FromSeconds(intervals)).RepeatForever();

            var trigger = TriggerBuilder.Create()
                .WithIdentity(jobName)
                .WithSchedule(scheduler)
                .StartNow()
                .Build();

            var _scheduler = await _schedulerFactory.GetScheduler();
            // 🔥 关键：确保调度器已启动
            if (!_scheduler.IsStarted)
            {
                await _scheduler.Start();
            }
            await _scheduler.ScheduleJob(job.Build(), trigger);
            return true;
        }
    }

    /// <summary>
    /// 周期性Cron配置任务
    /// </summary>
    public class CronJobExtension
    {
        private readonly ISchedulerFactory _schedulerFactory;
        public CronJobExtension(ISchedulerFactory schedulerFactory)
        {
            _schedulerFactory = schedulerFactory;
        }

        public async Task<bool> AddJob<T>(Dictionary<string, object>? jobPrms, string corn) where T : IJob
        {
            var jobName = typeof(T).Name;
            var jobKey = JobKey.Create(jobName);

            var job = JobBuilder.Create<T>()
                .WithIdentity(jobKey);

            if (jobPrms != null)
            {
                var jobDataMap = new JobDataMap(); jobDataMap.PutAll(jobPrms);
                job.UsingJobData(jobDataMap);
            }

            var trigger = TriggerBuilder.Create()
                .WithIdentity(jobName)
                .WithCronSchedule(corn)
                .StartNow()
                .Build();

            var _scheduler = await _schedulerFactory.GetScheduler();
            // 🔥 关键：确保调度器已启动
            if (!_scheduler.IsStarted)
            {
                await _scheduler.Start();
            }

            await _scheduler.ScheduleJob(job.Build(), trigger);
            return true;
        }
    }
}
