using System;
using System.Text;
using Lucky.SysModel.Entity;
using System.Collections.Generic;
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
        public async Task<List<SysRoleOutput>> GetPages(SysRoleQueryInput input, bool isAdmin = false)
        {
            return null;
        }


        /// <summary>
        /// 
        /// </summary>
        public async Task<SysRoleOutput> Get(int id)
        {
            return null;
        }

        /// <summary>
        /// 
        /// </summary>

        public async Task<bool> Add(SysRoleOptInput input, long uid)
        {
            return true;
        }

        /// <summary>
        /// 
        /// </summary>

        public async Task<bool> Edit(SysRoleOptInput input, long uid)
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
