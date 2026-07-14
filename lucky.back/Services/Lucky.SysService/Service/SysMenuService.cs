using Lucky.SysModel.Entity;
using Lucky.SysModel.Model.Input;
using Lucky.SysModel.Model.Output;
using Lucky.SysService.Rpsty.IRpsty;
using Lucky.SysService.Service.IService;
using System.Linq.Expressions;

namespace Lucky.SysService.Service
{
    public class SysMenuService : ISysMenuService
    {
        private readonly ISysRpsty<SysMenu, int> _menuRpsty;
        private readonly ISysRpsty<SysRoleMenu, int> _roleMenuRpsty;

        public SysMenuService(
            ISysRpsty<SysMenu, int> menuRpsty,
            ISysRpsty<SysRoleMenu, int> roleMenuRpsty
            )
        {
            _menuRpsty = menuRpsty;
            _roleMenuRpsty = roleMenuRpsty;
        }

        /// <summary>
        /// 查询菜单树
        /// </summary>
        public async Task<List<SysMenuOutput>> GetMenuTree(SysMenuQueryInput input, bool isAdmin = false)
        {
            var data = await _menuRpsty.GetListAsync(x => !x.IsDel);
            var lst = new List<SysMenuOutput>();

            var topMenus = data.Where(x => x.ParentId == 0 || x.ParentId == -1 || x.ParentId == null);
            var lens = topMenus.Count();
            for (var l = 0; l < lens; l++)
            {
                var obj = topMenus.ElementAt(l);
                var outObj = new SysMenuOutput()
                {
                    id = obj.Id,
                    parentId = obj.ParentId,
                    name = obj.Name,
                    code = obj.Code,
                    url = obj.Path,
                    icon = obj.Icon,
                    sort = obj.Sort,
                    status = obj.Status,
                    iconSize = obj.IconSize,
                    menuType = obj.MenuType
                };

                var childs = data.Where(x => x.ParentId == obj.Id);

                if (childs.Any())
                    BldMenuTree(outObj, data);

                lst.Add(outObj);
            }

            return lst;
        }

        private void BldMenuTree(SysMenuOutput res, List<SysMenu> menus)
        {
            var data = menus.Where(x => x.ParentId == res.id);

            if (data.Any())
            {
                res.children = new List<SysMenuOutput>();

                var lens = data.Count();

                for(var l = 0; l < lens; l++)
                {
                    var obj = data.ElementAt(l);
                    var tmpObj = new SysMenuOutput()
                    {
                        id = obj.Id,
                        parentId = obj.ParentId,
                        name = obj.Name,
                        code = obj.Code,
                        url = obj.Path,
                        icon = obj.Icon,
                        sort = obj.Sort,
                        status = obj.Status,
                        iconSize = obj.IconSize,
                        menuType = obj.MenuType
                    };

                    var chlds = menus.Where(x=>x.ParentId == obj.Id);

                    if (chlds.Any())
                        BldMenuTree(tmpObj, menus);

                    res.children.Add(tmpObj);
                }
            }
        }

        /// <summary>
        /// 根据id查询
        /// </summary>
        public async Task<SysMenuOutput?> Get(int id)
        {
            Expression<Func<SysMenu, SysMenuOutput>> expr = x => new SysMenuOutput()  // 1、这是最直接、最可控、最高效的方式
            {
                id = x.Id,
                name = x.Name,
                code = x.Code,
                parentId = x.ParentId ?? 0,
                icon = x.Icon,
                iconSize = x.IconSize,
                url = x.Path,
                status = x.Status,
                sort = x.Sort ?? 0,
                menuType = x.MenuType
            };

            var data = await _menuRpsty.GetByIdAsync(id, expr);
            return data;
        }

        /// <summary>
        /// 新增操作
        /// </summary>

        public async Task<bool> Add(SysMenuOptInput input, long uid)
        {
            var maxId = await _menuRpsty.MaxAsync<int>(null, x => x.Id);
            var model = new SysMenu()
            {
                Id = maxId + 1,
                Name = input.Name ?? string.Empty,
                Code = input.Code,
                Icon = input.Icon,
                IconSize = input.IconSize.ToString(),
                ParentId = input.ParentId,
                Path = input.Path,
                Sort = input.Sort,
                MenuType = input.MenuType,
                Status = input.Status,
                CreateTime = DateTime.Now,
                CreateUid = uid
            };
            var res = await _menuRpsty.AddAsync(model);
            return res != null;
        }

        /// <summary>
        /// 编辑操作
        /// </summary>
        public async Task<bool> Edit(SysMenuOptInput input, long uid)
        {
            var model = await _menuRpsty.GetByIdAsync(input.Id);
            if (model != null)
            {
                model.Name = input.Name ?? string.Empty;
                model.Status = input.Status;
                model.Sort = input.Sort;
                model.MenuType = input.MenuType;
                model.Icon = input.Icon;
                model.IconSize = input.IconSize.ToString();
                model.Path = input.Path;
                model.ParentId = input.ParentId;
                model.UpdateUid = uid;
                model.UpdateTime = DateTime.Now;

                var res = await _menuRpsty.UpdateAsync(model);

                return res > 0;
            }
            return false;
        }

        /// <summary>
        /// 删除操作
        /// </summary>
        public async Task<bool> Del(int id, long uid)
        {
            var model = await _menuRpsty.GetByIdAsync(id);
            if (model != null)
            {
                model.DelUid = uid;
                model.DelTime = DateTime.Now;
                model.IsDel = true;
                await _menuRpsty.UpdateAsync(model);
                return true;
            }
            return false;
        }
    }
}