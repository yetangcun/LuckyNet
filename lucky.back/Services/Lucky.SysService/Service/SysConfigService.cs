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
        public async Task<bool> Del(int id, long uid)
        {
            var cfg = await _cfgRpsty.GetByIdAsync(id);
            if (cfg == null) return false;
            cfg.IsDel = true;
            cfg.DelTime = DateTime.Now;
            cfg.DelUid = uid;
            return (await _cfgRpsty.UpdateAsync(cfg)) > 0;
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
                id = data.Id,
                name = data.Name,
                value = data.Value!,
                cfgType = data.CfgType,
                typeName = data.TypeName,
                sort = data.Sort,
                status = data.Status,
                code = data.Code,
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
                where = where.And(x => x.Name.Contains(req.Txt) || x.TypeName.Contains(req.Txt));
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
                id = x.Id,
                name = x.Name,
                sort = x.Sort,
                code = x.Code,
                value = x.Value!,
                status = x.Status,
                cfgType = x.CfgType,
                typeName = x.TypeName,
                createTime = x.CreateTime,
                createUser = x.CreateUid.ToString(),
                isSystem = x.IsSystem
            };

            var res = await _cfgRpsty.GetPagesAsync(where, expr, pgInfo);

            return res;
        }

        /// <summary>
        /// 新增或修改
        /// </summary>
        /// <param name="req"></param>
        public async Task<bool> Opt(SysConfigOptInput req, long uid)
        {
            if (req.Id <= 0)
            {
                var cfg = new SysConfig()
                {
                    Name = req.Name!,
                    Value = req.Value,
                    CfgType = req.CfgType,
                    TypeName = req.TypeName,
                    Sort = req.Sort,
                    Code = req.Code,
                    CreateUid = uid,
                    Status = req.Status,
                    IsSystem = req.IsSystem ?? true,
                    CreateTime = DateTime.Now,
                };
                var res = await _cfgRpsty.AddAsync(cfg);
                return res != null;
            }
            else
            {
                var cfg = await _cfgRpsty.GetByIdAsync(req.Id.Value);
                if (cfg == null) return false;
                cfg.Name = req.Name!;
                cfg.Value = req.Value;
                cfg.CfgType = req.CfgType;
                cfg.TypeName = req.TypeName;
                cfg.Sort = req.Sort;
                cfg.Code = req.Code;
                cfg.UpdateUid = uid;
                cfg.Status = req.Status;
                cfg.UpdateTime = DateTime.Now;
                cfg.IsSystem = req.IsSystem ?? true;
                return (await _cfgRpsty.UpdateAsync(cfg)) > 0;
            }
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <returns></returns>
        public async Task<List<SelectKV>> GetList()
        {
            var where = PredicateBuilder.New<SysConfig>(x => !x.IsDel && string.IsNullOrWhiteSpace(x.CfgType)); // 1、这是最直接、最可控、最高效的方式
            var res = await _cfgRpsty.GetListAsync(where);
            return res.Select(x => new SelectKV()
            {
                label = x.Name,
                value = x.Value
            }).ToList();
        }

        /// <summary>
        /// 获取配置
        /// </summary>
        /// <param name="cfgType"></param>
        public async Task<List<SysConfigOutput>> GetConfigs(string cfgType)
        {
            if (string.IsNullOrWhiteSpace(cfgType)) return new List<SysConfigOutput>();
            if (cfgType.IndexOf(",") > -1)
            {
                var types = (cfgType.Split(',')).Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
                var where = PredicateBuilder.New<SysConfig>(x => !x.IsDel && types.Contains(x.CfgType));
                var res = await _cfgRpsty.GetListAsync(where);
                return res.Select(x => new SysConfigOutput()
                {
                    id = x.Id,
                    name = x.Name,
                    value = x.Value!,
                    cfgType = x.CfgType,
                    typeName = x.TypeName,
                    sort = x.Sort,
                    status = x.Status,
                    code = x.Code,
                }).ToList();
            }
            else
            {
                var where = PredicateBuilder.New<SysConfig>(x => !x.IsDel && x.CfgType == cfgType);
                var res = await _cfgRpsty.GetListAsync(where);
                return res.Select(x => new SysConfigOutput()
                {
                    id = x.Id,
                    name = x.Name,
                    value = x.Value!,
                    cfgType = x.CfgType,
                    typeName = x.TypeName,
                    sort = x.Sort,
                    status = x.Status,
                    code = x.Code,
                }).ToList();
            }
        }
    }
}
