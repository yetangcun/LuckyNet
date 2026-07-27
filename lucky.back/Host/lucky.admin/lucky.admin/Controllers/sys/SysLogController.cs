using Lucky.BaseModel.Model;
using Lucky.SysModel.Model.Input;
using Lucky.SysModel.Model.Output;
using Lucky.SysService.Service.IService;
//using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace lucky.admin.Controllers.sys
{
    /// <summary>
    /// 系统日志网关
    /// </summary>
    public class SysLogController : SysBaseController
    {
        private readonly ISysLogService sysLogService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="sysLogService"></param>
        public SysLogController(ISysLogService sysLogService)
        {
            this.sysLogService = sysLogService;
        }

        /// <summary>
        /// 获取日志分页
        /// </summary>
        /// <param name="req"></param>
        [HttpGet("pages")]
        public async Task<PageRes<List<SysLogOutput>>> GetPages([FromQuery] SysLogQueryInput req)
        {
            var res = await sysLogService.GetPages(req);
            var pgs = res.Item1 % req.PageSize == 0 ? (res.Item1 / req.PageSize) : (res.Item1 / req.PageSize) + 1;
            return PageRes<List<SysLogOutput>>.Success(res.Item1,  res.Item2);
        }
    }
}
