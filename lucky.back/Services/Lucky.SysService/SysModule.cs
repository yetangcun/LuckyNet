using Common.CoreLib.Model.Option;
using Lucky.SysService.Cxt;
using Lucky.SysService.Rpsty;
using Lucky.SysService.Rpsty.IRpsty;
using Lucky.SysService.Service;
using Lucky.SysService.Service.IService;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Lucky.SysService
{
    /// <summary>
    /// 系统管理模块
    /// </summary>
    public static class SysModule
    {
        /// <summary>
        /// 初始化加载
        /// </summary>
        /// <param name="services"></param>
        /// <param name="cfg"></param>
        public static void SysModuleLoad(this IServiceCollection services, IConfiguration cfg)
        {
            services.Configure<SysDbOption>(cfg.GetSection("DbOption"));  // 添加数据库配置

            services.AddScoped<ISysCxt, SysCxt>();
            services.AddScoped<ISysRpsty, SysRpsty>();
            services.AddScoped<ISysUserService, SysUserService>();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="app"></param>
        /// <param name="cfg"></param>
        public static void SysModuleInit(this IApplicationBuilder app, IConfiguration cfg)
        {
        }
    }
}
