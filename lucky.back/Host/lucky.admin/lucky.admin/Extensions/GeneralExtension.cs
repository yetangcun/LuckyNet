using Lucky.PrtclService;
using Lucky.SysService;

namespace lucky.admin.Extensions
{
    /// <summary>
    /// 通用扩展
    /// </summary>
    public static class GeneralExtension
    {
        /// <summary>
        /// 通用加载
        /// </summary>
        public static void GeneralLoad(IServiceCollection services, IConfiguration configuration)
        {

        }

        /// <summary>
        /// 通用初始化
        /// </summary>
        public static void GeneralInit(this IApplicationBuilder app, IConfiguration cfg)
        {
            var isInit = cfg.GetValue<bool>("CommonCfg:IsInitDb");
            if (isInit)
            {
                app.SysModuleInit(cfg);   // 初始化系统管理模块
                app.PrtclModuleInit(cfg); // 初始化协议模块
            }
        }
    }
}
