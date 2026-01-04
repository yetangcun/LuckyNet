using GrpcTransCore.Services;

namespace Prtcl.Grpc.extension
{
    /// <summary>
    /// grpc通用处理接口
    /// </summary>
    public interface IGrpcCommonHandler
    {
        /// <summary>
        /// 处理方法结构
        /// </summary>
        /// <param name="reqParam"></param>
        Task<TransRes> Handler(TransReq? reqParam);
    }
}
