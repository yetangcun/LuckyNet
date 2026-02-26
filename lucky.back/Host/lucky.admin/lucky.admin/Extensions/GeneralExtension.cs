using Prtcl.Grpc;
using Lucky.SysService;
using Lucky.BaseService;
using Lucky.PrtclService;
using Prtcl.Grpc.extension;
using GrpcTransCore.Services;
using Alot2.Admin.Common.Cache;
using Common.CoreLib.Model.Option;
using lucky.admin.Extensions.Handler;

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
        public static void GeneralLoad(this IServiceCollection services, IConfiguration cfg)
        {
            services.Configure<GrpcConfig>(cfg.GetSection("GrpcCfg"));
            services.AddSingleton<IGrpcCommonHandler, GrpcDefaultHandler>();
            services.BaseInitLoad(cfg);
        }

        /// <summary>
        /// 通用初始化
        /// </summary>
        public static void GeneralInit(this IApplicationBuilder app, IConfiguration cfg)
        {
            app.SysModuleInit(cfg);   // 初始化系统管理模块
            app.PrtclModuleInit(cfg); // 初始化协议模块
            if (cfg.GetValue<int>("RedisServr:open") == 1) // 初始化Redis服务
            {
                RedisServer.Init(cfg);
            }

            // 创建异步包装器
            var _handle = app.ApplicationServices.GetService<IGrpcCommonHandler>();
            Func<TransReq?, Task<TransRes>> grpcHdl = async req =>
            {
                try
                {
                    // 对于gRPC服务，通常建议使用同步方式处理
                    // 或者在服务实现内部处理异步
                    return await _handle.Handler(req);
                }
                catch (Exception ex)
                {
                    // 处理异常
                    Console.WriteLine($"处理gRPC请求时出错: {ex.Message}");
                    return new TransRes { Code = 500, Data = ex.Message };
                }
            };
            var grpcCfg = cfg.GetSection("GrpcCfg").Get<GrpcConfig>();
            var logger = app.ApplicationServices.GetService<Serilog.ILogger>();
            GrpcServrHdl.GrpcServerStart("0.0.0.0", grpcCfg.ListenPort, grpcHdl, logger); // 启动gRPC服务
        }
    }
}
