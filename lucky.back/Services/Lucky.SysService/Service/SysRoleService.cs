using LinqKit;
using Lucky.BaseModel.Enum;
using Lucky.BaseModel.Model;
using Lucky.SysModel.Entity;
using System.Linq.Expressions;
using Lucky.SysModel.Model.Input;
using Lucky.SysModel.Model.Output;
using Lucky.SysService.Rpsty.IRpsty;
using Lucky.SysService.Service.IService;

namespace Lucky.SysService.Service
{
    public class SysRoleService : ISysRoleService
    {
        private readonly ISysRpsty<SysRole, int> _roleRpsty;
        private readonly ISysRpsty<SysRoleMenu, int> _roleMenuRpsty;

        public SysRoleService(
            ISysRpsty<SysRole, int> roleRpsty,
            ISysRpsty<SysRoleMenu, int> roleMenuRpsty
            )
        {
            _roleRpsty = roleRpsty;
            _roleMenuRpsty = roleMenuRpsty;
        }

        /// <summary>
        /// 
        /// </summary>
        public async Task<(int, List<SysRoleOutput>)> GetPages(SysRoleQueryInput input, bool isAdmin = false)
        {
            var where = PredicateBuilder.New<SysRole>(x => !x.IsDel); // 初始化为 true
            if (!string.IsNullOrEmpty(input.txt))
                where.And(x => x.Name.Contains(input.txt));

            if (input.roleType != null)
                where.And(x => x.RoleType == input.roleType);

            var pgInfo = new PageInfo()
            {
                PageIndex = input.PageIndex,
                PageSize = input.PageSize,
                Sort = input.Sort,
                SortType = input.SortType
            };

            Expression<Func<SysRole, SysRoleOutput>> expr = x => new SysRoleOutput()  // 1、这是最直接、最可控、最高效的方式
            {
                Id = x.Id,
                RoleType = x.RoleType,
                Name = x.Name,
                Remark = x.Remark
            };
            var res = await _roleRpsty.GetPagesAsync(where, expr, pgInfo);
            return res;
        }


        /// <summary>
        /// 根据id查询角色信息
        /// </summary>
        public async Task<SysRoleOutput?> Get(int id)
        {
            Expression<Func<SysRole, SysRoleOutput>> expr = x => new SysRoleOutput()  // 1、这是最直接、最可控、最高效的方式
            {
                Id = x.Id,
                RoleType = x.RoleType,
                Name = x.Name,
                Remark = x.Remark
            };
            var data = await _roleRpsty.GetByIdAsync(id, expr);
            return data;
        }

        /// <summary>
        /// 新增
        /// </summary>
        public async Task<bool> Add(SysRoleOptInput input, long uid)
        {
            var maxId = await _roleRpsty.MaxAsync<int>(null, x => x.Id);
            var model = new SysRole()
            {
                Id = maxId + 1,
                Name = input.Name,
                RoleType = input.RoleType == null ? RoleType.General : input.RoleType.Value,
                Remark = input.Remark
            };

            var res = await _roleRpsty.AddAsync(model);

            return res != null;
        }

        /// <summary>
        /// 更新操作
        /// </summary>
        public async Task<bool> Edit(SysRoleOptInput input, long uid)
        {
            var old = await _roleRpsty.GetByIdAsync(input.Id);

            if(old == null) return false;

            old.RoleType = input.RoleType!.Value;
            old.Name = input.Name;
            old.Remark = input.Remark;
            old.UpdateTime = DateTime.Now;
            old.UpdateUid = uid;

            var res = await _roleRpsty.UpdateAsync(old);

            return res>0;
        }

        /// <summary>
        /// 删除
        /// </summary>
        public async Task<bool> Del(int id, long uid)
        {
            var old = await _roleRpsty.GetByIdAsync(id);

            if (old != null)
            {
                old.DelUid = uid;
                old.IsDel = true;
                old.DelTime = DateTime.Now;

                var res = await _roleRpsty.UpdateAsync(old);

                return res > 0;
            }

            return false;
        }
    }
}
