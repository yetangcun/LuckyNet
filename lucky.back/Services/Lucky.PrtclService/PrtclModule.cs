using Common.CoreLib.Model.Option;
using Lucky.PrtclService.Rpsty;
using Lucky.PrtclService.Service.IService;
using Microsoft.Extensions.Configuration;
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
    }
}
