using Quartz;

namespace Tsk.Quartz.Jobs
{
    /// <summary>
    /// 一次性任务
    /// </summary>
    public class JobExtension
    {
        private readonly ISchedulerFactory _schedulerFactory;
        public JobExtension(ISchedulerFactory schedulerFactory)
        {
            _schedulerFactory = schedulerFactory;
        }

        /// <summary>
        /// 一次性任务
        /// 立即执行
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jobPrms"></param>
        public async Task<bool> AddOnceJob<T>(Dictionary<string, object>? jobPrms) where T : IJob
        {
            var jobName = typeof(T).Name;
            var jobKey = JobKey.Create(jobName);
            var _scheduler = await _schedulerFactory.GetScheduler();
            if (await _scheduler.CheckExists(jobKey))
            {
                await _scheduler.DeleteJob(jobKey); // return (false, $"调度任务中已存在: {jobName}");
            }

            var job = JobBuilder.Create<T>().WithIdentity(jobKey);
            if (jobPrms != null)
            {
                var jobDataMap = new JobDataMap(); jobDataMap.PutAll(jobPrms);
                job.UsingJobData(jobDataMap);
            }

            var replace = true;
            var durable = true;
            await _scheduler.AddJob(job.Build(), replace, durable);

            // 确保调度器已启动
            if (!_scheduler.IsStarted)
            {
                await _scheduler.Start();
            }

            await _scheduler.TriggerJob(jobKey); // 立即执行

            return true;
        }

        /// <summary>
        /// 一次性任务
        /// 在给定时间点执行
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jobPrms"></param>
        /// <param name="runTime"></param>
        /// <returns></returns>
        public async Task<(bool, string?)> AddOnceAtJob<T>(Dictionary<string, object>? jobPrms, DateTime runTime) where T : IJob
        {
            var jobName = typeof(T).Name;
            var jobKey = JobKey.Create(jobName);
            var _scheduler = await _schedulerFactory.GetScheduler();
            if (await _scheduler.CheckExists(jobKey))
                await _scheduler.DeleteJob(jobKey); // return (false, $"调度任务中已存在: {jobName}");

            var job = JobBuilder.Create<T>().WithIdentity(jobKey);
            if (jobPrms != null)
            {
                var jobDataMap = new JobDataMap(); jobDataMap.PutAll(jobPrms);
                job.UsingJobData(jobDataMap);
            }

            var scheduler = SimpleScheduleBuilder.Create().WithRepeatCount(0);
            var trigger = TriggerBuilder.Create()
                .WithIdentity(jobName)
                .WithSchedule(scheduler)
                .StartAt(runTime)
                .Build();

            // 确保调度器已启动
            if (!_scheduler.IsStarted) await _scheduler.Start();

            await _scheduler.ScheduleJob(job.Build(), trigger);

            return (true, string.Empty);
        }

        /// <summary>
        /// 延迟任务
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jobPrms"></param>
        /// <param name="ts">延迟时间:秒</param>
        public async Task<(bool, string?)> AddOnceDelayJob<T>(Dictionary<string, object>? jobPrms, TimeSpan ts, int repeats = 0, int repeatInterval = 1) where T : IJob
        {
            var jobName = typeof(T).Name;
            var jobKey = JobKey.Create(jobName);
            var _scheduler = await _schedulerFactory.GetScheduler();
            if (await _scheduler.CheckExists(jobKey)) await _scheduler.DeleteJob(jobKey); // return (false, $"调度任务中已存在: {jobName}");

            var job = JobBuilder.Create<T>().WithIdentity(jobKey);
            if (jobPrms != null)
            {
                var jobDataMap = new JobDataMap(); jobDataMap.PutAll(jobPrms);
                job.UsingJobData(jobDataMap);
            }

            var times = DateTime.Now.Add(ts);
            var scheduler = SimpleScheduleBuilder.Create().WithInterval(TimeSpan.FromSeconds(repeatInterval)).WithRepeatCount(repeats);
            var trigger = TriggerBuilder.Create()
                .WithIdentity(jobName)
                .WithSchedule(scheduler)
                .StartAt(times)
                .Build();

            if (!_scheduler.IsStarted) await _scheduler.Start(); // 判断调度器是否已启动

            // 延迟执行
            await _scheduler.ScheduleJob(job.Build(), trigger); return (true, string.Empty);
        }

        /// <summary>
        /// 周期性间隔任务
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jobPrms"></param>
        /// <param name="intervals">单位: 秒</param>
        public async Task<(bool,string?)> AddIntervalJob<T>(Dictionary<string, object>? jobPrms, int intervals = 1) where T : IJob
        {
            var jobName = typeof(T).Name;
            var jobKey = JobKey.Create(jobName);
            var _scheduler = await _schedulerFactory.GetScheduler();
            if (await _scheduler.CheckExists(jobKey)) await _scheduler.DeleteJob(jobKey); // return (false, $"调度任务中已存在: {jobName}");

            var job = JobBuilder.Create<T>().WithIdentity(jobKey);
            if (jobPrms != null)
            {
                var jobDataMap = new JobDataMap(); jobDataMap.PutAll(jobPrms);
                job.UsingJobData(jobDataMap);
            }

            var scheduler = SimpleScheduleBuilder.Create()
                .WithInterval(TimeSpan.FromSeconds(intervals))
                .RepeatForever();

            var trigger = TriggerBuilder.Create()
                .WithIdentity(jobName)
                .WithSchedule(scheduler)
                .StartNow()
                .Build();

            // 确保调度器已启动
            if (!_scheduler.IsStarted) await _scheduler.Start();

            await _scheduler.ScheduleJob(job.Build(), trigger); return (true, string.Empty);
        }

        /// <summary>
        /// 周期性Cron配置任务
        /// </summary>
        public async Task<(bool, string?)> AddCornJob<T>(Dictionary<string, object>? jobPrms, string corn) where T : IJob
        {
            var jobName = typeof(T).Name;
            var jobKey = JobKey.Create(jobName);
            var _scheduler = await _schedulerFactory.GetScheduler();
            if (await _scheduler.CheckExists(jobKey)) await _scheduler.DeleteJob(jobKey); // return (false, $"调度任务中已存在: {jobName}");

            var job = JobBuilder.Create<T>().WithIdentity(jobKey);
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

            if (!_scheduler.IsStarted) await _scheduler.Start(); // 确保调度器已启动

            await _scheduler.ScheduleJob(job.Build(), trigger); return (true, string.Empty);
        }
    }
}
