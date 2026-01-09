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

                CreateTable<PrtclGrpc>();  // 创建表
                if (req.Id > 0)
                {
                    var prtcl = new PrtclGrpc()
                    {
                        id = req.Id,
                        name = req.Name
                    };
                    var id = await Context.Insertable(prtcl).ExecuteCommandAsync();
                }
                var prtclGrpcs = await Context.Queryable<PrtclGrpc>().ToListAsync();

                // IsSugarReadOnly = true; // 读写分离, 标识为读

                var where = Expressionable.Create<Prtcl>();
                where.AndIF(!string.IsNullOrWhiteSpace(req.Name), x => x.name == req.Name); // 筛选条件

                var pgModel = new PageModel()
                {
                    PageIndex = req.PageIndex,
                    PageSize = req.PageSize,
                    TotalCount = 0
                };
                var lst = GetPageList(where.ToExpression(), pgModel); // 调用自带的分页查询

                var pageInfo = new BaseModel.Model.PageInfo()
                {
                    PageIndex = req.PageIndex,
                    PageSize = req.PageSize,
                    Sort = req.Sort,
                    SortType = req.SortType
                };
                Expression<Func<Prtcl, PrtclOutput>> expr = x => new PrtclOutput()
                {
                    Name = x.name
                };
                return await GetPageListAsync(where.ToExpression(), expr, pageInfo); // 自己扩展的分页查询
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "");
                return (0, null);
            }
        }
    }
}
