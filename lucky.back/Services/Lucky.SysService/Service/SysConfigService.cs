using LinqKit;
using Lucky.BaseModel.Model;
using Lucky.SysModel.Entity;
using System.Linq.Expressions;
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
        private readonly ISysRpsty<SysConfig, int?> _cfgRpsty;

        public SysConfigService(ILogger<SysConfigService> logger, ISysRpsty<SysConfig, int?> cfgRpsty)
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
                Id = data.Id.Value,
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
            var where = PredicateBuilder.New<SysConfig>(x => !x.IsDel); // 初始化为 true
            if (!string.IsNullOrEmpty(req.CfgType))
            {
                where = where.And(x => x.CfgType == req.CfgType);
            }
            if (!string.IsNullOrEmpty(req.Txt))
            {
                where = where.And(x => x.Name.Contains(req.Txt));
            }


            var pgInfo = new PageInfo()
            {
                PageIndex = req.PageIndex,
                PageSize = req.PageSize,
                Sort = req.Sort,
                SortType = req.SortType
            };

            Expression<Func<SysConfig, SysConfigOutput>> expr = x => new SysConfigOutput()  // 1、这是最直接、最可控、最高效的方式
            {
                Id = x.Id.Value,
                Name = x.Name,
                Value = x.Value!,
                CfgType = x.CfgType,
                TypeName = x.TypeName,
                Sort = x.Sort,
                Status = x.Status,
                Code = x.Code,
                CreateTime = x.CreateTime,
                CreateUser = x.CreateUid.ToString(),
                IsSystem = x.IsSystem
            };

            var res = await _cfgRpsty.GetPagesAsync(where, expr, pgInfo);

            return res;
        }

        /// <summary>
        /// 新增或修改
        /// </summary>
        /// <param name="req"></param>
        public async Task<bool> Opt(SysConfigOptInput req)
        {
            if (req.Id <= 0)
            {
                var cfg = new SysConfig()
                {
                    Id = null,
                    Name = req.Name!,
                    Value = req.Value,
                    CfgType = req.CfgType,
                    TypeName = req.TypeName,
                    Sort = req.Sort,
                    Status = req.Status,
                    Code = req.Code,
                    CreateTime = DateTime.Now.ToUniversalTime(),
                };
                var res = await _cfgRpsty.AddAsync(cfg);
                return res != null;
            }
            else
            {
                var cfg = await _cfgRpsty.GetByIdAsync(req.Id);
                if (cfg == null) return false;
                cfg.Name = req.Name!;
                cfg.Value = req.Value;
                cfg.CfgType = req.CfgType;
                cfg.TypeName = req.TypeName;
                cfg.Sort = req.Sort;
                cfg.Status = req.Status;
                cfg.Code = req.Code;
                return (await _cfgRpsty.UpdateAsync(cfg)) > 0;
            }
        }
    }
}
