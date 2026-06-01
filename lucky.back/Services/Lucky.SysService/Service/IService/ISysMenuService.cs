using System;
using System.Text;
using System.Collections.Generic;
using Lucky.SysModel.Model.Input;
using Lucky.SysModel.Model.Output;

namespace Lucky.SysService.Service.IService
{
    public interface ISysMenuService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="isAdmin"></param>
        Task<List<SysMenuOutput>> GetMenuTree(SysMenuQueryInput input, bool isAdmin = false);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        Task<SysMenuOutput> Get(int id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="uid"></param>
        Task<bool> Add(SysMenuOptInput input, long uid);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="uid"></param>
        Task<bool> Edit(SysMenuOptInput input, long uid);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        Task<bool> Del(int id, long uid);
    }
}
