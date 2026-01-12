using Prtcl.Grpc.extension;
using GrpcTransCore.Services;

namespace lucky.admin.Extensions.Handler
{
    /// <summary>
    /// grpc 默认处理
    /// </summary>
    public class GrpcDefaultHandler : IGrpcCommonHandler
    {
        /// <summary>
        /// Grpc默认处理
        /// </summary>
        /// <param name="reqParam"></param>
        public async Task<TransRes> Handler(TransReq? reqParam)
        {
            throw new NotImplementedException();
        }
    }
}
