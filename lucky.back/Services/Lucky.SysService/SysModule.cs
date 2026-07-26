using Common.CoreLib.Model.Option;
using Tsk.Quartz;
using Lucky.SysModel;
using Lucky.SysService.Cxt;
using Lucky.SysService.Rpsty;
using Microsoft.Extensions.Options;
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

            #region 扫描当前模块的所有Entity 
            // services.AddScoped<ISysRpsty<object>, SysRpsty<object>>(); // 此方法不可行
            var assembly = typeof(SysModelModule).Assembly;
            var entityTypes = assembly.GetTypes()
                .Where(t => t.IsClass &&
                           !t.IsAbstract &&
                           t.Namespace?.EndsWith(".Entity") == true).ToList();
            foreach (var entityType in entityTypes)
            {
                var typ = entityType.BaseType;
                if (typ == null || !typ.GenericTypeArguments.Any()) continue;

                var tpf = typ.GenericTypeArguments[0];
                var rpstyInterface = typeof(ISysRpsty<,>).MakeGenericType(entityType, tpf);
                var rpstyImp = typeof(SysRpsty<,>).MakeGenericType(entityType, tpf);
                services.AddScoped(rpstyInterface, rpstyImp);
            }
            #endregion

            services.AddScoped<ISysOrgService, SysOrgService>();
            services.AddScoped<ISysUserService, SysUserService>();
            services.AddScoped<ISysMenuService, SysMenuService>();
            services.AddScoped<ISysRoleService, SysRoleService>();
            services.AddScoped<ISysLogService, SysLogService>();

            services.QuartzModuleLoad(cfg); // 添加Quartz
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="app"></param>
        /// <param name="cfg"></param>
        public static void SysModuleInit(this IApplicationBuilder app, IConfiguration cfg)
        {
            if (cfg.GetValue<bool>("CommonCfg:IsInitDb"))
            {
                var dbOption = app.ApplicationServices.GetService<IOptions<SysDbOption>>();
                using (var scope = app.ApplicationServices.CreateScope())
                {
                    var sysCxt = scope.ServiceProvider.GetService<ISysCxt>();
                    sysCxt!.SetDbOption(dbOption!.Value);
                    var res = sysCxt.InitDbTable();
                    if (res)
                        sysCxt.InitData();
                }
            }
        }
    }
}
