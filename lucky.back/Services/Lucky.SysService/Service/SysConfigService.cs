using Lucky.SysModel.Entity;
using Lucky.SysModel.Model.Input;
using Lucky.SysModel.Model.Output;
using Lucky.SysService.Rpsty.IRpsty;
using Lucky.SysService.Service.IService;
using Microsoft.Extensions.Logging;

namespace Lucky.SysService.Service
{
    public class SysConfigService : ISysConfigService
    {
        private readonly ILogger<SysConfigService> _logger;
        private readonly ISysRpsty<SysConfig, int> _cfgRpsty;

        public SysConfigService(ILogger<SysConfigService> logger, ISysRpsty<SysConfig, int> cfgRpsty)
        {
            _logger = logger;
            _cfgRpsty = cfgRpsty;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        public async Task<bool> Del(int id)
        {
            var cfg = await _cfgRpsty.GetByIdAsync(id);
            if (cfg == null) return false;
            return (await _cfgRpsty.DeleteAsync(cfg)) > 0;
        }

        /// <summary>
        /// 获取单条数据
        /// </summary>
        /// <param name="id"></param>
        public async Task<SysConfigOutput?> GetById(int id)
        {
            var data = await _cfgRpsty.GetByIdAsync(id);
            if (data != null) return new SysConfigOutput()
            {
                Id = data.Id,
                Name = data.Name,
                Value = data.Value!,
                CfgType = data.CfgType,
                TypeName = data.TypeName,
                Sort = data.Sort,
                Status = data.Status,
                Code = data.Code,
            };
            return null;
        }

        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="req"></param>
        public async Task<(int, List<SysConfigOutput>)> GetPages(SysConfigQueryInput req)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 新增或修改
        /// </summary>
        /// <param name="req"></param>
        public async Task<bool> Opt(SysConfigOptInput req)
        {
            throw new NotImplementedException();
        }
    }
}
