using Grpc.Core;
using GrpcTransCore.Services;
using System.Collections.Concurrent;

namespace Prtcl.Grpc
{
    public class GrpcClientHdl
    {/// <summary>
     /// 通道字段
     /// 创建通道开销很大, 所以尽可能共用通道
     /// </summary>
        static ConcurrentDictionary<string, Channel> _grpcChannelDic = new ConcurrentDictionary<string, Channel>();

        static object CHANNE_LOCK = new object();

        /// <summary>
        /// 获取通道对象
        /// </summary>
        public Channel GetGrpcChannel(int sPort, string channelKey, string sIp = "localhost")
        {
            if (string.IsNullOrWhiteSpace(channelKey))
                channelKey = "Net.Grpc.Dft.Channel";

            if (_grpcChannelDic.TryGetValue(channelKey, out var channel))
                return channel;

            lock (CHANNE_LOCK)
            {
                if (channel == null || (channel.State != ChannelState.Ready && channel.State != ChannelState.Connecting))
                {
                    channel = new Channel(sIp, sPort, ChannelCredentials.Insecure);
                    _grpcChannelDic[channelKey] = channel;
                }
            }

            return channel;
        }

        /// <summary>
        /// 通用调用
        /// 有入参有响应
        /// </summary>
        public async Task<TransRes> GrpcGeneralCall(int sPort, TransReq reqParam, string sIp = "localhost", string channelKey = "", int milliseconds = 6000)
        {
            var channel = GetGrpcChannel(sPort, channelKey, sIp);

            var grpcClient = new GrpcTransCoreService.GrpcTransCoreServiceClient(channel);

            return await grpcClient.GrpcGeneralCallAsync(reqParam, deadline: DateTime.UtcNow.AddMilliseconds(milliseconds));
        }

        /// <summary>
        /// 有入参无响应
        /// 此操作无啥用
        /// 最好不用
        /// </summary>
        public async Task GrpcGeneralCallWithoutResponse(int sPort, TransReq reqParam, string sIp = "localhost", string channelKey = "", int milliseconds = 4000)
        {
            var channel = GetGrpcChannel(sPort, channelKey, sIp);

            var grpcClient = new GrpcTransCoreService.GrpcTransCoreServiceClient(channel);

            await grpcClient.GrpcGeneralCallWithoutResponseAsync(reqParam, deadline: DateTime.UtcNow.AddMilliseconds(milliseconds));
        }

        /// <summary>
        /// 无入参有响应
        /// </summary>
        public async Task<TransRes> GrpcGeneralCallWithoutReqparam(int sPort, TransReq reqParam, string sIp = "localhost", string channelKey = "", int milliseconds = 4000)
        {
            var channel = GetGrpcChannel(sPort, channelKey, sIp);

            var grpcClient = new GrpcTransCoreService.GrpcTransCoreServiceClient(channel);

            return await grpcClient.GrpcGeneralCallWithoutReqparamAsync(null, deadline: DateTime.UtcNow.AddMilliseconds(milliseconds));
        }

        /// <summary>
        /// 客户端流式
        /// </summary>
        public async Task<TransRes> GrpcClientWayStream(int sPort, TransReq reqParam, string sIp = "localhost", string channelKey = "", int milliseconds = 4000)
        {
            var channel = GetGrpcChannel(sPort, channelKey, sIp);

            var grpcClient = new GrpcTransCoreService.GrpcTransCoreServiceClient(channel);

            var res = grpcClient.GrpcClientWayStream(deadline: DateTime.UtcNow.AddMilliseconds(milliseconds));

            await res.RequestStream.WriteAsync(reqParam);

            await res.RequestStream.CompleteAsync();

            return await res.ResponseAsync;
        }

        /// <summary>
        /// 服务端流式
        /// </summary>
        public TransRes GrpcServerWayStream(int sPort, TransReq reqParam, string sIp = "localhost", string channelKey = "", int milliseconds = 4000)
        {
            var channel = GetGrpcChannel(sPort, channelKey, sIp);

            var grpcClient = new GrpcTransCoreService.GrpcTransCoreServiceClient(channel);

            var res = grpcClient.GrpcServerWayStream(reqParam, deadline: DateTime.UtcNow.AddMilliseconds(milliseconds));

            //if (res.ResponseStream.MoveNext().Result)
            //    return res.ResponseStream.Current;

            return res.ResponseStream.Current;
        }

        /// <summary>
        /// 客户端服务端双向流
        /// </summary>
        public async Task<TransRes> GrpcTwoWayStream(int sPort, TransReq reqParam, string sIp = "localhost", string channelKey = "", int milliseconds = 4000)
        {
            var channel = GetGrpcChannel(sPort, channelKey, sIp);

            var grpcClient = new GrpcTransCoreService.GrpcTransCoreServiceClient(channel);

            var twoWayStream = grpcClient.GrpcTwoWayStream(deadline: DateTime.UtcNow.AddMilliseconds(milliseconds));

            await twoWayStream.RequestStream.WriteAsync(reqParam);

            var resStream = twoWayStream.ResponseStream;

            //if (resStream.MoveNext().Result)
            //    return resStream.Current;

            return resStream.Current;
        }
    }
}
