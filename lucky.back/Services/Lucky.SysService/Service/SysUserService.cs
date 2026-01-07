using LinqKit;
using Lucky.BaseModel.Model;
using Lucky.SysModel.Entity;
using System.Linq.Expressions;
using Lucky.SysModel.Model.Input;
using Lucky.SysModel.Model.Output;
using Lucky.SysService.Rpsty.IRpsty;
using Common.CoreLib.Extension.Common;
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
            var where = PredicateBuilder.New<SysUser>(x => !x.IsDel); // 初始化为 true
            if (!string.IsNullOrWhiteSpace(req.Txt))
                where = where.And(x => x.Account.Contains(req.Txt) || (!string.IsNullOrWhiteSpace(x.RealName) && x.RealName.Contains(req.Txt)));

            if (req.Status.HasValue)
                where = where.And(x => x.Status == req.Status);

            /**************测试方法*****************/
            var maxId = await _sysRpsty.MaxAsync<SysUser, long>(where, x => x.Id);
            var likeStr = $"%{req.Txt}%";
            var outObj = await _sysRpsty.SqlQueryAsync<SysUserOutput>($"select \"Id\",\"Account\",\"RealName\" from sys_user where \"Account\" like {likeStr} or \"RealName\" like {likeStr};");
            /**************测试方法*****************/

            var pgInfo = new PageInfo()
            {
                PageIndex = req.PageIndex,
                PageSize = req.PageSize,
                Sort = req.Sort,
                SortType = req.SortType
            };

            //Expression<Func<SysUser, SysUserOutput>> expr = x => new SysUserOutput()  // 1、这是最直接、最可控、最高效的方式
            //{
            //    Uid = x.Id,
            //    Account = x.Account,
            //    Name = x.RealName
            //};
            var expr = SimpleMappingExtensions.AutoMap<SysUser, SysUserOutput>();  // 2、这是最简便的方式
            var res = await _sysRpsty.GetPagesAsync(where, expr, pgInfo);

            return (res.Item1, res.Item2);
        }
    }
}
