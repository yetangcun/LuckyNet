using Quartz;
using Tsk.Quartz.Jobs;
using Common.CoreLib.Model.Option;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Tsk.Quartz
{
    public static class QuartzModule
    {
        /// <summary>
        /// 模块初始化加载
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void QuartzModuleLoad(this IServiceCollection services, IConfiguration cfg)
        {
            var quartzSection = cfg.GetSection("Quartz");
            services.Configure<QuartzOption>(quartzSection);
            var quartzOption = quartzSection.Get<QuartzOption>();
            services.AddQuartz(opt =>
            {
                opt.UseDefaultThreadPool(4);
                opt.SchedulerId = quartzOption!.SchedulerId;
            });

            // 添加配置: 等所有任务执行完成后再关闭调度器
            services.AddQuartzHostedService(opt =>
            {
                opt.WaitForJobsToComplete = true;
            });

            services.AddSingleton<JobExtension>();
        }

        /// <summary>
        /// 模块初始化
        /// </summary>
        /// <param name="app"></param>
        /// <param name="cfg"></param>
        public static void QuartzModuleInit(this IApplicationBuilder app, IConfiguration cfg)
        {
        }
    }
}
