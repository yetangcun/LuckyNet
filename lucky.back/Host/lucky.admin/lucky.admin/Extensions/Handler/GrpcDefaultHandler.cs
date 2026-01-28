using Prtcl.Grpc.extension;
using GrpcTransCore.Services;
using Google.Protobuf.WellKnownTypes;
using Common.CoreLib.Extension.Common;

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
            reqParam.Exts.TryUnpack(out StringValue vl);
            if (vl != null)
            {
                var dics = vl.Value.ToObj<Dictionary<string, string>>();
            }
            return new TransRes()
            {
                Data = vl.Value
            };
        }
    }
}
