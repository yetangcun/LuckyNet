using LinqKit;
using Lucky.BaseModel;
using Lucky.BaseModel.Enum;
using Lucky.BaseModel.Model;
using Lucky.SysModel.Entity;
using Lucky.SysModel.Model.Input;
using Lucky.SysModel.Model.Output;
using Lucky.SysService.Rpsty.IRpsty;
using Lucky.SysService.Service.IService;
using System.Linq.Expressions;

namespace Lucky.SysService.Service
{
    public class SysRoleService : ISysRoleService
    {
        private readonly ISysRpsty<SysRole, int> _roleRpsty;
        private readonly ISysRpsty<SysRoleMenu, int> _roleMenuRpsty;
        private readonly ISysRpsty<SysMenu, int> _menuRpsty;

        public SysRoleService(
            ISysRpsty<SysRole, int> roleRpsty,
            ISysRpsty<SysRoleMenu, int> roleMenuRpsty,
            ISysRpsty<SysMenu, int> menuRpsty
            )
        {
            _roleRpsty = roleRpsty;
            _roleMenuRpsty = roleMenuRpsty;
            _menuRpsty = menuRpsty;
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
                id = x.Id,
                roleType = x.RoleType,
                name = x.Name,
                word = x.Word,
                sort = x.Sort,
                status = x.Status,
                remark = x.Remark
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
                id = x.Id,
                roleType = x.RoleType,
                name = x.Name,
                sort = x.Sort,
                word = x.Word,
                status = x.Status,
                remark = x.Remark
            };
            var data = await _roleRpsty.GetByIdAsync(id, expr);
            return data;
        }

        /// <summary>
        /// 新增
        /// </summary>
        public async Task<bool> Add(SysRoleOptInput input, long uid)
        {
            var maxId = await _roleRpsty.MaxAsync(x => true, x => x.Id);
            var model = new SysRole()
            {
                Id = maxId + 1,
                Name = input.Name,
                Word = input.Word,
                Sort = input.Sort??0,
                Status = input.Status??1,
                RoleType = input.RoleType == null ? RoleType.General : input.RoleType.Value,
                Remark = input.Remark,
                CreateTime = DateTime.Now,
                CreateUid = uid
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
            old.Word = input.Word;
            old.Remark = input.Remark;
            old.Sort = input.Sort??0;
            old.Status = input.Status??1;
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

        /// <summary>
        /// 获取角色下拉列表
        /// </summary>
        /// <param name="uid"></param>
        public async Task<List<SelectKV>?> GetSelList(long uid)
        {
            var where = PredicateBuilder.New<SysRole>(x => !x.IsDel);
            var res = await _roleRpsty.GetListAsync(where, x => new SelectKV()
            {
                label = x.Name,
                value = x.Id.ToString()
            });
            return res;
        }

        /// <summary>
        /// 获取角色菜单
        /// </summary>
        public async Task<List<int>?> GetRoleMenus(int roleId)
        {
            var role = await _roleRpsty.GetByIdAsync(roleId);
            if (role != null && role.RoleType == RoleType.Super)
            {
                var menuIds = await _menuRpsty.GetListAsync(x => !x.IsDel, x => new
                {
                    id = x.Id
                });

                return menuIds.Select(x => x.id).ToList();
            }
            var where = PredicateBuilder.New<SysRoleMenu>(x => x.RoleId == roleId);
            var res = await _roleMenuRpsty.GetListAsync(where, x => x.MenuId);
            return res;
        }

        /// <summary>
        /// 设置角色菜单
        /// </summary>
        public async Task<bool> SetRoleMenus(SetRoleMenusInput input, long uid)
        {
            var role = await _roleRpsty.GetByIdAsync(input.roleId);
            if (role != null && role.RoleType == RoleType.Super)
                return true;

            if (input.roleId <= 0 || input.menuIds == null) return false;
            var where = PredicateBuilder.New<SysRoleMenu>(x => x.RoleId == input.roleId);
            var olds = await _roleMenuRpsty.GetListAsync(where);
            await _roleMenuRpsty.DeleteRangeAsync(olds);

            var menus = await _menuRpsty.GetListAsync(x => input.menuIds.Contains(x.Id) && x.MenuType == MenuType.Menu);
            var menuIds = menus.Select(x => x.Id).ToList();

            #region 补充信号量、防并发
            GlobalConstant.Glb_semaphore.WaitOne();
            try
            {
                var maxId = await _roleMenuRpsty.MaxAsync<int>(null, x => x.Id) + 1;
                var adds = menuIds.Select(x => new SysRoleMenu()
                {
                    Id = maxId++,
                    RoleId = input.roleId,
                    MenuId = x
                }).ToList();
                var res = await _roleMenuRpsty.AddRangeAsync(adds);
                GlobalConstant.Glb_semaphore.Release();
                return res > 0;
            }
            catch (Exception)
            {
                GlobalConstant.Glb_semaphore.Release();
                return false;
            }

            #endregion
        }
    }
}
