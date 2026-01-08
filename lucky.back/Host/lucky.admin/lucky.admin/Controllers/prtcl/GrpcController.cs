using Lucky.BaseModel.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Lucky.PrtclModel.Model.Input;
using Lucky.PrtclModel.Model.Output;
using Lucky.PrtclService.Service.IService;

namespace lucky.admin.Controllers.prtcl
{
    /// <summary>
    /// grpc
    /// </summary>
    public class GrpcController : PrtclBaseController
    {
        private readonly IPrtclsService _service;

        /// <summary>
        /// 构造函数
        /// </summary>
        public GrpcController(IPrtclsService service)
        {
            _service = service;
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        [HttpGet("list")]
        public async Task<PageRes<List<PrtclOutput>?>> GetList([FromQuery] PrtclQueryInput req)
        {
            var res = await _service.GetPageListAsync(req);
            var pages = res.Item1 / req.PageSize;
            if (res.Item1 % req.PageSize > 0)
            {
                pages++;
            }
            return PageRes<List<PrtclOutput>?>.Success(res.Item1, pages, res.Item2);
        }
    }
}
