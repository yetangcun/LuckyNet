using Common.CoreLib.Extension.Common;
using Common.CoreLib.Model.Option;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Prtcl.Grpc;

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
            services.Configure<MqttOption>(cfg.GetSection("MqttOption")); // 添加Mqtt配置
            services.Configure<UdpOption>(cfg.GetSection("UdpOption")); // 添加Udp配置
            services.Configure<RdisOption>(cfg.GetSection("RdisOption")); // 添加Redis配置
            services.AddSingleton<JwtAuthExtension>(); // 添加Jwt认证
            services.AddSingleton<GrpcClientHdl>();    // 添加Grpc客户端
        }
    }
}
