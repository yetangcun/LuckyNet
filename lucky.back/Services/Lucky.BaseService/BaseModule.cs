using Common.CoreLib.Extension.Common;
using Common.CoreLib.Model.Option;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Lucky.BaseService
{
    /// <summary>
    /// 基础模块
    /// </summary>
    public static class BaseModule
    {
        /// <summary>
        /// 模块初始化
        /// </summary>
        /// <param name="services"></param>
        /// <param name="cfg"></param>
        public static void BaseInitLoad(this IServiceCollection services, IConfiguration cfg)
        {
            services.Configure<JwtOptions>(cfg.GetSection("JwtOptions")); // 添加Jwt配置
            services.AddSingleton<JwtAuthExtension>(); // 添加Jwt认证
        }
    }
}
