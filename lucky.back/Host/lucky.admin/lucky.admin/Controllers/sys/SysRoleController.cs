using Lucky.BaseModel.Model;
using Lucky.SysModel.Model.Input;
using Lucky.SysModel.Model.Output;
//using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace lucky.admin.Controllers.sys
{
    /// <summary>
    /// 角色管理
    /// </summary>
    public class SysRoleController : SysBaseController
    {
        #region 角色管理
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="req"></param>
        [HttpGet("getPages")]
        public async Task<PageRes<List<SysRoleOutput>>> GetPages(SysRoleQueryInput req)
        {
            return PageRes<List<SysRoleOutput>>.Success(0, 0, null);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="id"></param>
        [HttpGet("{id}")]
        public async Task<ResModel<SysRoleOutput>> Get(long id)
        {
            return ResModel<SysRoleOutput>.Success(null);
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="input"></param>
        [HttpPost]
        public async Task<ResModel<bool>> Add([FromBody] SysRoleOptInput input)
        {
            return ResModel<bool>.Success(true);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete("{id}")]
        public async Task<ResModel<bool>> Del(long id)
        {
            return ResModel<bool>.Success(true);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="input"></param>
        [HttpPut]
        public async Task<ResModel<bool>> Edit([FromBody] SysRoleOptInput input)
        {
            return ResModel<bool>.Success(true);
        }
        #endregion

        #region 角色菜单
        #endregion
    }
}
