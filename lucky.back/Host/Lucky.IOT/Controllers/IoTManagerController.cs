using Lucky.BaseModel.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Prtcl.Grpc;

namespace Lucky.IOT.Controllers
{
    /// <summary>
    /// 物联网管理
    /// </summary>
    [Route("lot/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "IoT")]
    public class IoTManagerController : ControllerBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public IoTManagerController()
        {
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("list")]
        public async Task<ResModel<List<string>>> GetList([FromServices] GrpcClientHdl grpcClt)
        {
            await grpcClt.GrpcGeneralCall(9681, new GrpcTransCore.Services.TransReq() { Sid = 1, Opt = 1 });
            return ResModel<List<string>>.Success(new List<string>() { "1", "2", "3" });
        }
    }
}
