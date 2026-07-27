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
    public class SysLogService : ISysLogService
    {
        private readonly ILogger<SysLogService> _logger;
        private readonly ISysRpsty<SysLog, long> _logRpsty;

        public SysLogService(ISysRpsty<SysLog, long> logRpsty, ILogger<SysLogService> logger)
        {
            _logger = logger;
            _logRpsty = logRpsty;
        }

        /// <summary>
        /// 获取日志列表
        /// </summary>
        /// <param name="req"></param>
        public async Task<(int, List<SysLogOutput>)> GetPages(SysLogQueryInput req)
        {
            var where = PredicateBuilder.New<SysLog>(x => true); // 初始化为 true
            if (!string.IsNullOrWhiteSpace(req.reqPath))
                where.And(x => x.ReqUrl!.Contains(req.reqPath));

            if (!string.IsNullOrWhiteSpace(req.reqType))
                where.And(x => x.ReqType!.Contains(req.reqType));

            if (!string.IsNullOrWhiteSpace(req.reqIp))
                where.And(x => x.ReqIp!.Contains(req.reqIp));

            if (req.beginTime != null)
                where.And(x => x.CreateTime>=(req.beginTime));

            if (req.endTime != null)
                where.And(x => x.CreateTime<=(req.endTime));

            if (req.status != null && req.status != -1)
                where.And(x => x.Status == req.status);

            var pgInfo = new PageInfo()
            {
                PageIndex = req.PageIndex,
                PageSize = req.PageSize,
                Sort = req.Sort,
                SortType = req.SortType
            };

            Expression<Func<SysLog, SysLogOutput>> expr = x => new SysLogOutput()  // 1、这是最直接、最可控、最高效的方式
            {
                Id = x.Id,
                reqUrl = x.ReqUrl,
                reqType = x.ReqType,
                reqIp = x.ReqIp,
                reqParam = x.ReqParams,
                reqTime = x.CreateTime,
                status = x.Status,
                reqDuration = x.ExecTime,
                reqUser = x.CreateUid.ToString(),
                msg = x.ErrMsg
            };
            var res = await _logRpsty.GetPagesAsync(where, expr, pgInfo);
            return res;
        }
    }
}
