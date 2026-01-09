using Lucky.SysService.Cxt;
using Common.CoreLib.Model.Option;
using Microsoft.Extensions.Options;

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
                var dbOption = app.ApplicationServices.GetService<IOptions<SysDbOption>>();
                using (var scope = app.ApplicationServices.CreateScope())
                {
                    var sysCxt = scope.ServiceProvider.GetService<ISysCxt>();
                    sysCxt.SetDbOption(dbOption.Value);
                    var res = sysCxt.InitTable();
                    if (res)
                        sysCxt.InitData();
                }
            }
        }
    }
}
