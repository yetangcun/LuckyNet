using SqlSugar;
using Lucky.PrtclModel.Entity;
using System.Linq.Expressions;
using Common.CoreLib.Model.Option;
using Lucky.PrtclModel.Model.Input;
using Lucky.PrtclModel.Model.Output;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Data.SqlSugar.Extension.Service;
using Lucky.PrtclService.Service.IService;

namespace Lucky.PrtclService.Rpsty
{
    public class PrtclsService : SugarBaseService<Prtcl, PrtclDbOption>, IPrtclsService
    {
        private readonly ILogger<PrtclsService> _logger;

        public PrtclsService(IOptions<PrtclDbOption> option, ILogger<PrtclsService> logger) : base(option.Value, logger)
        {
            _logger = logger;
        }

        public async Task<(int, List<PrtclOutput>?)> GetPageListAsync(PrtclQueryInput req)
        {
            try
            {
                //if(req.Id > 0)
                //{
                //    var prtcl = new Prtcl()
                //    {
                //        id = req.Id,
                //        name = req.Name
                //    };
                //    var id = await InsertAsync(prtcl); // 插入数据
                //}

                //CreateTable<PrtclGrpc>();  // 创建表
                //if (req.Id > 0)
                //{
                //    var prtcl = new PrtclGrpc()
                //    {
                //        id = req.Id,
                //        name = req.Name
                //    };
                //    var id = await Context.Insertable(prtcl).ExecuteCommandAsync();
                //}
                //var prtclGrpcs = await Context.Queryable<PrtclGrpc>().ToListAsync();

                IsSugarReadOnly = true; // 读写分离, 标识为读

                var where = Expressionable.Create<Prtcl>();
                where.AndIF(!string.IsNullOrWhiteSpace(req.Name), x => x.name == req.Name); // 筛选条件

                Expression<Func<Prtcl, PrtclOutput>> expr = x => new PrtclOutput()
                {
                    Name = x.name
                };

                var pgInfo = new BaseModel.Model.PageInfo()
                {
                    PageIndex = req.PageIndex,
                    PageSize = req.PageSize,
                    Sort = req.Sort,
                    SortType = req.SortType
                };

                var lst = await GetPages(where.ToExpression(), expr, pgInfo); // 调用自带的分页查询

                return await GetPageListAsync(where.ToExpression(), expr, pgInfo); // 自己扩展的分页查询
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "");
                return (0, null);
            }
        }
    }
}
