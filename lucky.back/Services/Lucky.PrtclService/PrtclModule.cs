using Common.CoreLib.Model.Option;
using Lucky.PrtclService.Rpsty;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Lucky.PrtclService.Service.IService;
using Microsoft.Extensions.DependencyInjection;

namespace Lucky.PrtclService
{
    /// <summary>
    /// 协议模块
    /// </summary>
    public static class PrtclModule
    {
        /// <summary>
        /// 加载服务
        /// </summary>
        public static void PrtclModuleLoad(this IServiceCollection services, IConfiguration cfg)
        {
            services.Configure<PrtclDbOption>(cfg.GetSection("DbOption"));  // 添加数据库配置
            services.AddScoped<IPrtclsService, PrtclsService>();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public static void PrtclModuleInit(this IApplicationBuilder app, IConfiguration cfg)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var prtclService = scope.ServiceProvider.GetService<IPrtclsService>();

                var dlls = new string[] { "Lucky.PrtclModel.dll" };
                prtclService!.CreateTables(dlls);
            }
        }
    }
}
