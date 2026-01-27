using Common.CoreLib.Model.Option;
using GrpcTransCore.Services;
using Lucky.BaseService;
using Lucky.IotService.Extension.Handler;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Prtcl.Grpc;
using Prtcl.Grpc.extension;

namespace Lucky.IotService
{
    public static class IotModule
    {
        /// <summary>
        /// 服务注册
        /// </summary>
        /// <param name="services"></param>
        /// <param name="cfg"></param>
        public static void IotModuleLoad(this IServiceCollection services, IConfiguration cfg)
        {
            services.Configure<GrpcConfig>(cfg.GetSection("GrpcCfg"));
            services.AddSingleton<IGrpcCommonHandler, GrpcDefaultHandler>();
            services.BaseInitLoad(cfg); // 通用注册
        }

        /// <summary>
        /// 服务初始化
        /// </summary>
        public static void IotModuleInit(this IApplicationBuilder app, IConfiguration cfg)
        {
            // 创建异步包装器
            var _handle = app.ApplicationServices.GetService<IGrpcCommonHandler>();
            Func<TransReq?, Task<TransRes>> grpcHdl = async req =>
            {
                try
                {
                    // 对于gRPC服务，通常建议使用同步方式处理
                    // 或者在服务实现内部处理异步
                    return await _handle!.Handler(req);
                }
                catch (Exception ex)
                {
                    // 处理异常
                    Console.WriteLine($"处理gRPC请求时出错: {ex.Message}");
                    return new TransRes { Code = 500, Data = ex.Message };
                }
            };
            var grpcCfg = cfg.GetSection("GrpcCfg").Get<GrpcConfig>();
            GrpcServrHdl.GrpcServerStart("0.0.0.0", grpcCfg!.ListenPort, grpcHdl); // 启动gRPC服务
        }
    }
}
