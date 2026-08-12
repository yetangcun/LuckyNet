using LinqKit;
using Lucky.BaseModel.Model;
using Lucky.SysModel.Entity;
using Lucky.SysModel.Model.Input;
using Lucky.SysModel.Model.Output;
using Lucky.SysService.Rpsty.IRpsty;
using Lucky.SysService.Service.IService;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace Lucky.SysService.Service
{
    public class SysTskService : ISysTskService
    {
        private readonly ILogger<SysTskService> _logger;
        private ISysRpsty<SysTsk, int> _sysRpsty;
        private ISysRpsty<SysTskRecord, long> _tskRecordRpsty;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="sysRpsty"></param>
        public SysTskService(ILogger<SysTskService> logger, ISysRpsty<SysTsk, int> sysRpsty, ISysRpsty<SysTskRecord, long> tskRecordRpsty)
        {
            _logger = logger;
            _sysRpsty = sysRpsty;
            _tskRecordRpsty = tskRecordRpsty;
        }

        /// <summary>
        /// 删除
        /// </summary>
        public async Task<(bool, string)> Del(int id, long uid)
        {
            var entity = await _sysRpsty.GetByIdAsync(id);
            if (entity == null)
            {
                return (false, "数据不存在");
            }
            entity.IsDel = true;
            entity.DelTime = DateTime.Now;
            entity.DelUid = uid;
            var res = await _sysRpsty.UpdateAsync(entity);
            return res > 0 ? (true, string.Empty) : (false, "删除失败");
        }

        /// <summary>
        /// 获取
        /// </summary>
        public async Task<SysTskOutput?> Get(int id)
        {
            var entity = await _sysRpsty.GetByIdAsync(id);
            if (entity == null)
            {
                return null;
            }
            return new SysTskOutput()
            {
                id = entity.Id,
                name = entity.Name,
                code = entity.Code,
                remark = entity.Remark,
                status = entity.Status,
                cron = entity.Cron,
                paramModel = entity.ParamModel,
                createTime = entity.CreateTime,
                updateTime = entity.UpdateTime,
                createUser = "",
                updateUser = ""
            };
        }

        /// <summary>
        /// 获取分页
        /// </summary>
        public async Task<(int, List<SysTskOutput>)> GetPages(SysTskQueryInput req)
        {
            var where = PredicateBuilder.New<SysTsk>(x => !x.IsDel); // 初始化为 true
            if (!string.IsNullOrWhiteSpace(req.Txt))
            {
                where = where.And(x => x.Name.Contains(req.Txt) || x.Code.Contains(req.Txt));
            }

            var pgInfo = new PageInfo()
            {
                PageIndex = req.PageIndex,
                PageSize = req.PageSize,
                Sort = req.Sort,
                SortType = req.SortType
            };

            Expression<Func<SysTsk, SysTskOutput>> expr = x => new SysTskOutput()  // 1、这是最直接、最可控、最高效的方式
            {
                id = x.Id,
                name = x.Name,
                code = x.Code,
                remark = x.Remark,
                status = x.Status,
                cron = x.Cron,
                paramModel = x.ParamModel,
                createTime = x.CreateTime,
                updateTime = x.UpdateTime,
                createUser = "",
                updateUser = ""
            };

            var res = await _sysRpsty.GetPagesAsync(where, expr, pgInfo);

            return res;
        }

        /// <summary>
        /// 操作
        /// </summary>
        public async Task<(bool, string)> Opt(SysTskOptInput req, long uid)
        {
            if (req.Id == null || req.Id < 1)
            {
                var entity = new SysTsk()
                {
                    Name = req.Name,
                    Remark = req.Remark,
                    Status = req.Status??1,
                    Code = req.Code,
                    Cron = req.Cron,
                    ParamModel = req.ParamModel,
                    CreateTime = DateTime.Now,
                    CreateUid = uid
                };
                var res = await _sysRpsty.AddAsync(entity);
                return res != null ? (true, string.Empty) : (false, "添加失败");
            }
            else
            {
                var entity = await _sysRpsty.GetByIdAsync(req.Id.Value);
                if (entity == null)
                {
                    return (false, "数据不存在");
                }
                entity.Name = req.Name;
                entity.Remark = req.Remark;
                entity.Status = req.Status??1;
                entity.Code = req.Code;
                entity.Cron = req.Cron;
                entity.ParamModel = req.ParamModel;
                entity.UpdateTime = DateTime.Now;
                entity.UpdateUid = uid;
                var res = await _sysRpsty.UpdateAsync(entity);
                return res > 0 ? (true, string.Empty) : (false, "更新失败");
            }
        }

        /// <summary>
        /// 设置
        /// </summary>
        public async Task<(bool, string)> Set(int id, long uid)
        {
            var entity = await _sysRpsty.GetByIdAsync(id);
            if (entity == null)
            {
                return (false, "数据不存在");
            }

            entity.Status = entity.Status == 1 ? 0 : 1;
            entity.UpdateTime = DateTime.Now;
            entity.UpdateUid = uid;
            var res = await _sysRpsty.UpdateAsync(entity);
            return res > 0 ? (true, string.Empty) : (false, "设置失败");
        }

        /// <summary>
        /// 获取记录分页
        /// </summary>
        /// <returns></returns>
        public async Task<(int, List<SysTskRecordOutput>)> GetRecordPages(SysTskRecordInput req)
        {
            var where = PredicateBuilder.New<SysTskRecord>(x => true);
            if (req.TskId != null && req.TskId > 0)
                where = where.And(x => x.TskId == req.TskId);
            if (req.Status != null)
                where = where.And(x => x.Status == req.Status);
            if (req.StartTime != null)
                where = where.And(x => x.CreateTime >= req.StartTime);
            if (req.EndTime != null)
                where = where.And(x => x.CreateTime <= req.EndTime);
            var pgInfo = new PageInfo()
            {
                PageIndex = req.PageIndex,
                PageSize = req.PageSize,
                Sort = req.Sort,
                SortType = req.SortType
            };
            Expression<Func<SysTskRecord, SysTskRecordOutput>> expr = x => new SysTskRecordOutput()
            {
                Id = x.Id,
                TskId = x.TskId,
                Status = x.Status,
                StartTime = x.StartTime,
                EndTime = x.EndTime,
                TskParam = x.TskParam,
                TskMsg = x.TskMsg,
            };
            return await _tskRecordRpsty.GetPagesAsync(where, expr, pgInfo);
        }
    }
}
