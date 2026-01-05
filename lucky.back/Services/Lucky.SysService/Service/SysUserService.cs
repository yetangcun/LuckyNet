using Common.CoreLib.Extension.Common;
using LinqKit;
using Lucky.SysModel.Entity;
using Lucky.SysModel.Model.Input;
using Lucky.SysModel.Model.Output;
using Lucky.SysService.Rpsty.IRpsty;
using Lucky.SysService.Service.IService;

namespace Lucky.SysService.Service
{
    /// <summary>
    /// 用户服务
    /// </summary>
    public class SysUserService : ISysUserService
    {
        private readonly ISysRpsty _sysRpsty;

        /// <summary>
        /// 构造函数
        /// </summary>
        public SysUserService(ISysRpsty sysRpsty)
        {
            _sysRpsty = sysRpsty;
        }


        public async Task<(int, List<SysUserOutput>)> GetList(SysUserQueryInput req)
        {
            var where = PredicateBuilder.New<SysUser>(true); // 初始化为 true
            if (!string.IsNullOrWhiteSpace(req.Txt))
                where = where.And(x => x.Account.Contains(req.Txt) || x.RealName.NullCheck(req.Txt));

            if (req.Status.HasValue)
                where = where.And(x => x.Status == req.Status);

            var res = await _sysRpsty.GetPageListAsync(where.DefaultExpression, req.PageIndex, req.PageSize);
            var list = res.Item2.Select(x => new SysUserOutput
            {
                Uid = x.Id,
                Account = x.Account,
                Name = x.RealName,
            }).ToList();

            return (res.Item1, list);
        }
    }
}
