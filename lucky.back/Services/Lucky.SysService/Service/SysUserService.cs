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
        private readonly ISysRpsty<SysUser> _usrRpsty;
        private readonly ISysRpsty<SysRole> _roleRpsty;

        /// <summary>
        /// 构造函数
        /// </summary>
        public SysUserService(
            ISysRpsty<SysUser> usrRpsty,
            ISysRpsty<SysRole> roleRpsty)
        {
            _usrRpsty = usrRpsty;
            _roleRpsty = roleRpsty;
        }

        public async Task<(int, List<SysUserOutput>)> GetList(SysUserQueryInput req)
        {
            var where = PredicateBuilder.New<SysUser>(x => !x.IsDel); // 初始化为 true
            if (!string.IsNullOrWhiteSpace(req.Txt))
                where = where.And(x => x.Account.Contains(req.Txt) || (!string.IsNullOrWhiteSpace(x.RealName) && x.RealName.Contains(req.Txt)));

            if (req.Status.HasValue)
                where = where.And(x => x.Status == req.Status);

            /**************测试方法*****************/
            var maxId = await _usrRpsty.MaxAsync<long>(where, x => x.Id);   // 获取用户最大Id
            var maxRoleId = await _roleRpsty.MaxAsync<int>(null, x => x.Id);  // 查询角色最大Id

            var likeStr = $"%{req.Txt}%";
            var roleObj = await _roleRpsty.SqlQueryAsync<SysRoleOutput>($"select id,name,remark from sys_role where name like {likeStr};");
            var userObj = await _usrRpsty.SqlQueryAsync<SysUserOutput>($"select id,account,realname from sys_user where account like {likeStr} or realname like {likeStr};");

            var outModel = await _usrRpsty.GetByIdAsync<SysUser, long, SysUserOutput>(2, x => new SysUserOutput()
            {
                Id = x.Id,
                Account = x.Account,
                RealName = x.RealName
            });
            var lst = await _usrRpsty.GetListAsync(where);
            var lists = await _usrRpsty.GetListAsync(where, x => new SysUserOutput()
            {
                Id = x.Id,
                Account = x.Account,
                RealName = x.RealName
            });
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
            var res = await _usrRpsty.GetPagesAsync(where, expr, pgInfo);

            return (res.Item1, res.Item2);
        }
    }
}
