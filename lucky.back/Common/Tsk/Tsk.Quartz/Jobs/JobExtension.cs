using Quartz;
using Microsoft.Extensions.Logging;

namespace Tsk.Quartz.Jobs
{
    /// <summary>
    /// 调度任务
    /// </summary>
    public class JobExtension
    {
        private readonly ILogger<JobExtension> _logger;
        private readonly ISchedulerFactory _schedulerFactory;

        public JobExtension(
            ILogger<JobExtension> logger,
            ISchedulerFactory schedulerFactory)
        {
            _logger = logger;
            _schedulerFactory = schedulerFactory;
        }

        private IScheduler? _scheduler;   // 调度器
        private bool _isrunning = false;  // 是否正在运行
        private readonly SemaphoreSlim _lock = new(1, 1);  // 锁

        /// <summary>
        /// 获取调度器（单例）
        /// </summary>
        private async Task<IScheduler> GetSchedulerAsync()
        {
            if (_isrunning && _scheduler != null && !_scheduler.IsShutdown)
                return _scheduler;

            await _lock.WaitAsync();

            if (_isrunning && _scheduler != null && !_scheduler.IsShutdown)
                return _scheduler;

            try
            {
                if (_scheduler != null)
                    await _scheduler.Clear();

                _scheduler = await _schedulerFactory.GetScheduler();
                if (!_scheduler.IsStarted)
                {
                    await _scheduler.Start();
                    _logger.LogInformation("Quartz调度器已启动");
                }

                _isrunning = true; return _scheduler;
            }
            finally
            {
                _lock.Release();
            }
        }

        /// <summary>
        /// 一次性任务
        /// 立即执行
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jobPrms"></param>
        public async Task<bool> AddOnceJob<T>(Dictionary<string, object>? jobPrms) where T : IJob
        {
            var scheduler = await GetSchedulerAsync();

            var jobName = typeof(T).Name;
            var jobKey = JobKey.Create(jobName);
            if (await scheduler.CheckExists(jobKey))
                await scheduler.DeleteJob(jobKey);

            var job = JobBuilder.Create<T>().WithIdentity(jobKey);
            if (jobPrms != null)
            {
                var jobDataMap = new JobDataMap(); jobDataMap.PutAll(jobPrms);
                job.UsingJobData(jobDataMap);
            }

            bool replace = true;
            await scheduler.AddJob(job.Build(), replace, true);
            await scheduler.TriggerJob(jobKey);  // 立即执行
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
            var scheduler = await GetSchedulerAsync();

            var jobName = typeof(T).Name;
            var jobKey = JobKey.Create(jobName);
            if (await scheduler.CheckExists(jobKey))
                await scheduler.DeleteJob(jobKey);

            var schedl = SimpleScheduleBuilder.Create().WithRepeatCount(0);
            var trigger = TriggerBuilder.Create()
                .WithIdentity(jobName)
                .WithSchedule(schedl)
                .StartAt(runTime)
                .Build();
            var job = JobBuilder.Create<T>().WithIdentity(jobKey);
            if (jobPrms != null)
            {
                var jobDataMap = new JobDataMap(); jobDataMap.PutAll(jobPrms);
                job.UsingJobData(jobDataMap);
            }
            await scheduler.ScheduleJob(job.Build(), trigger); return (true, string.Empty);
        }

        /// <summary>
        /// 延迟任务
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jobPrms"></param>
        /// <param name="ts">延迟时间:秒</param>
        public async Task<(bool, string?)> AddOnceDelayJob<T>(Dictionary<string, object>? jobPrms, TimeSpan ts, int repeats = 0, int repeatInterval = 1) where T : IJob
        {
            var scheduler = await GetSchedulerAsync();

            var jobName = typeof(T).Name;
            var jobKey = JobKey.Create(jobName);
            if (await scheduler.CheckExists(jobKey))
                await scheduler.DeleteJob(jobKey);

            var job = JobBuilder.Create<T>().WithIdentity(jobKey);
            if (jobPrms != null)
            {
                var jobDataMap = new JobDataMap(); jobDataMap.PutAll(jobPrms);
                job.UsingJobData(jobDataMap);
            }

            var times = DateTime.Now.Add(ts);
            var schedl = SimpleScheduleBuilder.Create().WithInterval(TimeSpan.FromSeconds(repeatInterval)).WithRepeatCount(repeats);
            var trigger = TriggerBuilder.Create()
                .WithIdentity(jobName)
                .WithSchedule(schedl)
                .StartAt(times)
                .Build();

            // 延迟执行
            await scheduler.ScheduleJob(job.Build(), trigger); return (true, string.Empty);
        }

        /// <summary>
        /// 周期性间隔任务
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jobPrms"></param>
        /// <param name="intervals">单位: 秒</param>
        public async Task<(bool,string?)> AddIntervalJob<T>(Dictionary<string, object>? jobPrms, int intervals = 1) where T : IJob
        {
            var scheduler = await GetSchedulerAsync();

            var jobName = typeof(T).Name;
            var jobKey = JobKey.Create(jobName);
            if (await scheduler.CheckExists(jobKey))
                return (false, "任务已存在");

            var job = JobBuilder.Create<T>().WithIdentity(jobKey);
            if (jobPrms != null)
            {
                var jobDataMap = new JobDataMap(); jobDataMap.PutAll(jobPrms);
                job.UsingJobData(jobDataMap);
            }
            var schedl = SimpleScheduleBuilder.Create()
                .WithInterval(TimeSpan.FromSeconds(intervals))
                .RepeatForever();
            var trigger = TriggerBuilder.Create()
                .WithIdentity(jobName)
                .WithSchedule(schedl)
                .StartNow()
                .Build();

            await scheduler.ScheduleJob(job.Build(), trigger); return (true, string.Empty);
        }

        /// <summary>
        /// 周期性Cron配置任务
        /// </summary>
        public async Task<(bool, string?)> AddCornJob<T>(Dictionary<string, object>? jobPrms, string corn) where T : IJob
        {
            var scheduler = await GetSchedulerAsync();

            var jobName = typeof(T).Name;
            var jobKey = JobKey.Create(jobName);
            if (await scheduler.CheckExists(jobKey))
            {
                return (false, "任务已存在"); // await scheduler.DeleteJob(jobKey);
            }

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
            await scheduler.ScheduleJob(job.Build(), trigger); return (true, string.Empty);
        }

        /// <summary>
        /// 暂停任务
        /// </summary>
        public async Task<(bool, string?)> PauseJob(string jobName)
        {
            var scheduler = await GetSchedulerAsync();

            var jobKey = JobKey.Create(jobName);
            if (!await scheduler.CheckExists(jobKey)) return (false, $"调度任务不存在: {jobName}");

            await scheduler.PauseJob(jobKey);
            return (true, string.Empty);
        }

        /// <summary>
        /// 恢复任务
        /// </summary>
        public async Task<(bool, string?)> ResumeJob(string jobName)
        {
            var scheduler = await GetSchedulerAsync();

            var jobKey = JobKey.Create(jobName);
            if (!await scheduler.CheckExists(jobKey)) return (false, $"调度任务不存在: {jobName}");

            await scheduler.ResumeJob(jobKey);
            return (true, string.Empty);
        }

        /// <summary>
        /// 删除任务
        /// </summary>
        /// <param name="jobName"></param>
        public async Task<(bool, string?)> DeleteJob(string jobName)
        {
            var scheduler = await GetSchedulerAsync();

            var jobKey = JobKey.Create(jobName);
            if (!await scheduler.CheckExists(jobKey)) return (false, $"调度任务不存在: {jobName}");

            await scheduler.DeleteJob(jobKey);
            return (true, string.Empty);
        }
    }
}
