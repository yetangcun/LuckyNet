using Lucky.BaseModel.Model;
using Lucky.BaseService.Extension;
using Lucky.SysModel.Model.Input;
using Lucky.SysModel.Model.Output;
using Lucky.SysService.Service.IService;

//using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace lucky.admin.Controllers.sys
{
    /// <summary>
    /// 角色管理
    /// </summary>
    public class SysRoleController : SysBaseController
    {
        private readonly ISysRoleService _roleService;

        /// <summary>
        /// 
        /// </summary>
        public SysRoleController(
            ISysRoleService roleService
            )
        {
            _roleService = roleService;
        }

        #region 角色管理
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="req"></param>
        [HttpGet("pages")]
        public async Task<PageRes<List<SysRoleOutput>>> GetPages([FromQuery] SysRoleQueryInput req)
        {
            var uid = HttpContext.GetUid();

            var isAdmin = false;

            var res = await _roleService.GetPages(req, isAdmin);

            var pgs = res.Item1 % req.PageSize == 0 ? (res.Item1 / req.PageSize) : (res.Item1 / req.PageSize + 1);

            return PageRes<List<SysRoleOutput>>.Success(pgs, res.Item1, res.Item2);
        }

        /// <summary>
        /// 根据id查询
        /// </summary>
        /// <param name="id"></param>
        [HttpGet("{id}")]
        public async Task<ResModel<SysRoleOutput>> Get(int id)
        {
            var data = await _roleService.Get(id);
            return ResModel<SysRoleOutput>.Success(data);
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="input"></param>
        [HttpPost]
        public async Task<ResModel<bool>> Add([FromBody] SysRoleOptInput input)
        {
            var uid = HttpContext.GetUid();
            var res = await _roleService.Add(input, uid);
            return res ? ResModel<bool>.Success(true) : ResModel<bool>.Failed(false);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete("{id}")]
        public async Task<ResModel<bool>> Del(int id)
        {
            var uid = HttpContext.GetUid();
            var res = await _roleService.Del(id, uid);
            return res ? ResModel<bool>.Success(true) : ResModel<bool>.Failed(false);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="input"></param>
        [HttpPut]
        public async Task<ResModel<bool>> Edit([FromBody] SysRoleOptInput input)
        {
            var uid = HttpContext.GetUid();
            var res = await _roleService.Edit(input, uid);
            return res ? ResModel<bool>.Success(true) : ResModel<bool>.Failed(false);
        }
        #endregion

        #region 角色菜单
        #endregion
    }
}
