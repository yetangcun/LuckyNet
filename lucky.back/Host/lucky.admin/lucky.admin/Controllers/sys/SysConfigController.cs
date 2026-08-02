using Lucky.BaseModel.Model;
using Lucky.BaseService.Extension;
using Lucky.SysModel.Model.Input;
using Lucky.SysModel.Model.Output;
using Lucky.SysService.Service.IService;
//using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace lucky.admin.Controllers.sys
{
    /// <summary>
    /// 系统配置
    /// </summary>
    public class SysConfigController : SysBaseController
    {
        private readonly ILogger<SysConfigController> _logger;
        private readonly ISysConfigService _sysConfigService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="sysConfigService"></param>
        public SysConfigController(ILogger<SysConfigController> logger, ISysConfigService sysConfigService)
        {
            _logger = logger;
            _sysConfigService = sysConfigService;
        }

        /// <summary>
        /// 获取系统配置分页数据
        /// </summary>
        /// <param name="req"></param>
        [HttpGet("pages")]
        public async Task<PageRes<List<SysConfigOutput>>> GetPages([FromQuery]SysConfigQueryInput req)
        {
            var res = await _sysConfigService.GetPages(req);
            return PageRes<List<SysConfigOutput>>.Success(res.Item1, res.Item2);
        }

        /// <summary>
        /// 新增或修改系统配置
        /// </summary>
        /// <param name="input"></param>
        [HttpPost]
        public async Task<ResModel<bool>> Opt([FromBody] SysConfigOptInput input)
        {
            var uid = HttpContext.GetUid();
            var res = await _sysConfigService.Opt(input, uid);
            return ResModel<bool>.Success(res);
        }

        /// <summary>
        /// 删除系统配置
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete("{id}")]
        public async Task<ResModel<bool>> Del(int id)
        {
            var uid = HttpContext.GetUid();
            var res = await _sysConfigService.Del(id, uid);
            return ResModel<bool>.Success(res);
        }

        /// <summary>
        /// 获取系统配置详情
        /// </summary>
        /// <param name="id"></param>
        [HttpGet("{id}")]
        public async Task<ResModel<SysConfigOutput>> Get(int id)
        {
            var res = await _sysConfigService.GetById(id);
            return ResModel<SysConfigOutput>.Success(res);
        }

        /// <summary>
        /// 获取系统配置列表
        /// </summary>
        [HttpGet("list")]
        public async Task<ResModel<List<SelectKV>>> GetList()
        {
            var res = await _sysConfigService.GetList();
            return ResModel<List<SelectKV>>.Success(res);
        }

        /// <summary>
        /// 获取系统配置列表根据类型
        /// </summary>
        /// <param name="cfgType"></param>
        /// <returns></returns>
        [HttpGet("getByType/{cfgType}")]
        public async Task<ResModel<List<SysConfigOutput>>> GetConfig(string cfgType)
        {
            var res = await _sysConfigService.GetConfigs(cfgType);
            return ResModel<List<SysConfigOutput>>.Success(res);
        }
    }
}
