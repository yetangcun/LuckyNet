using GrpcTransCore.Services;
using Grpc.Core;
using Google.Protobuf.WellKnownTypes;

namespace Prtcl.Grpc
{
    public class GrpcServrHdl
    {
        /// <summary>
        /// 开启grpc服务侦听
        /// </summary>
        public static string GrpcServerStart(string listenIp, int listenPort, Func<TransReq?, Task<TransRes>> hdlFunc)
        {
            try
            {
                if (hdlFunc == null)
                    return "处理程序不能为空!";

                var grpcServr = new Server()
                {
                    Ports = { new ServerPort(listenIp, listenPort, ServerCredentials.Insecure) },
                    Services = { GrpcTransCoreService.BindService(new GrpcTransDefaultImpl(hdlFunc)) }
                };

                grpcServr.Start();
                Console.WriteLine($"Grpc server start at {listenIp}:{listenPort} success");
                return "success";
            }
            catch (Exception ex)
            {
                var msg = $"{ex.Message},{ex.InnerException},{ex.StackTrace}";
                Console.WriteLine(msg);
                return $"{ex.Message},{ex.InnerException},{ex.StackTrace}";
            }
        }
    }

    /// <summary>
    /// grpc服务实现
    /// </summary>
    public class GrpcTransDefaultImpl : GrpcTransCoreService.GrpcTransCoreServiceBase
    {
        /// <summary>
        /// 业务处理表达式
        /// </summary>
        private Func<TransReq?, Task<TransRes>> _handler;

        public GrpcTransDefaultImpl(Func<TransReq?, Task<TransRes>> handler)
        {
            _handler = handler;
        }

        /// <summary>
        /// 通用调用
        /// 有入参和响应
        /// </summary>
        public async override Task<TransRes> GrpcGeneralCall(TransReq req, ServerCallContext context)
        {
            var res = await _handler(req);

            return res;
        }

        /// <summary>
        /// 通用调用
        /// 有入参无响应
        /// </summary>
        public async override Task<Empty> GrpcGeneralCallWithoutResponse(TransReq req, ServerCallContext context)
        {
            await _handler(req);
            return new Empty();
        }

        /// <summary>
        /// 通用调用
        /// 无入参有响应
        /// </summary>
        public async override Task<TransRes> GrpcGeneralCallWithoutReqparam(Empty empty, ServerCallContext context)
        {
            var res = await _handler(null);
            return res;
        }

        /// <summary>
        /// 客户端流式
        /// </summary>
        public async override Task<TransRes> GrpcClientWayStream(IAsyncStreamReader<TransReq> reqParams, ServerCallContext context)
        {
            var res = await _handler(reqParams.Current);
            return res;
        }

        /// <summary>
        /// 服务端流式
        /// </summary>
        public async override Task GrpcServerWayStream(TransReq req, IServerStreamWriter<TransRes> resResult, ServerCallContext context)
        {
            // await base.GrpcServerWayStream(req, res, context);
            // await Task.CompletedTask;

            var res = await _handler(req);

            await resResult.WriteAsync(res, CancellationToken.None);
        }

        /// <summary>
        /// 双向流式
        /// </summary>
        public async override Task GrpcTwoWayStream(IAsyncStreamReader<TransReq> reqParams, IServerStreamWriter<TransRes> resResult, ServerCallContext context)
        {
            var res = await _handler(reqParams.Current);
            await resResult.WriteAsync(res, CancellationToken.None);
        }
    }
}
