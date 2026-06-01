using Lucky.SysModel.Entity;
using Lucky.SysModel.Model.Input;
using Lucky.SysModel.Model.Output;
using Lucky.SysService.Rpsty.IRpsty;
using Lucky.SysService.Service.IService;
using System;
using System.Collections.Generic;
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
        /// 
        /// </summary>
        public async Task<List<SysMenuOutput>> GetMenuTree(SysMenuQueryInput input, bool isAdmin = false)
        {
            return null;
        }


        /// <summary>
        /// 
        /// </summary>
        public async Task<SysMenuOutput> Get(int id)
        {
            return null;
        }

        /// <summary>
        /// 
        /// </summary>

        public async Task<bool> Add(SysMenuOptInput input, long uid)
        {
            return true;
        }

        /// <summary>
        /// 
        /// </summary>

        public async Task<bool> Edit(SysMenuOptInput input, long uid)
        {
            return true;
        }

        /// <summary>
        /// 
        /// </summary>

        public async Task<bool> Del(int id, long uid)
        {
            return true;
        }
    }
}