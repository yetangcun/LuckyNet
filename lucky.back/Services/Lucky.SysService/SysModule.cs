using Common.CoreLib.Model.Option;
using Lucky.SysModel;
using Lucky.SysService.Cxt;
using Lucky.SysService.Rpsty;
using Lucky.SysService.Rpsty.IRpsty;
using Lucky.SysService.Service;
using Microsoft.AspNetCore.Builder;
using Lucky.SysService.Service.IService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

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

            #region 扫描当前模块的所有Entity // services.AddScoped<ISysRpsty<object>, SysRpsty<object>>();

            var assembly = typeof(SysModelModule).Assembly;
            var entityTypes = assembly.GetTypes()
                .Where(t => t.IsClass &&
                           !t.IsAbstract &&
                           t.Namespace?.EndsWith(".Entity") == true).ToList();
            foreach (var entityType in entityTypes)
            {
                var rpstyInterface = typeof(ISysRpsty<>).MakeGenericType(entityType);
                var rpstyImp = typeof(SysRpsty<>).MakeGenericType(entityType);
                services.AddScoped(rpstyInterface, rpstyImp);
            }

            #endregion

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
