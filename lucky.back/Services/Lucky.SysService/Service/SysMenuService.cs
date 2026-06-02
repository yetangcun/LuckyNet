using Lucky.SysModel.Entity;
using Lucky.SysModel.Model.Input;
using Lucky.SysModel.Model.Output;
using Lucky.SysService.Rpsty.IRpsty;
using Lucky.SysService.Service.IService;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

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
            
            return null;
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
                parentId = x.ParentId.Value,
                icon = x.Icon,
                iconSize = x.IconSize,
                url = x.Path,
                sort = x.Sort.Value,
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
                Name = input.Name,
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
            return true;
        }

        /// <summary>
        /// 删除操作
        /// </summary>

        public async Task<bool> Del(int id, long uid)
        {
            return true;
        }
    }
}