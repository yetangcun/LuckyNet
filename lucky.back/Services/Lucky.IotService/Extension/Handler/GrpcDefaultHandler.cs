using GrpcTransCore.Services;
using Prtcl.Grpc.extension;
using System;
using System.Text;
using System.Collections.Generic;

namespace Lucky.IotService.Extension.Handler
{
    public class GrpcDefaultHandler : IGrpcCommonHandler
    {
        /// <summary>
        /// Grpc默认处理实现
        /// </summary>
        /// <param name="reqParam"></param>
        public async Task<TransRes> Handler(TransReq? reqParam)
        {
            throw new NotImplementedException();
        }
    }
}
