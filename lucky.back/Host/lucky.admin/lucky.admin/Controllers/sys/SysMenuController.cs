using Lucky.BaseModel.Model;
using Lucky.BaseService.Extension;
using Lucky.SysModel.Model.Input;
using Lucky.SysModel.Model.Output;
using Lucky.SysService.Service.IService;
using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Http;

namespace lucky.admin.Controllers.sys
{
    /// <summary>
    /// 菜单管理
    /// </summary>
    public class SysMenuController : SysBaseController
    {
        private readonly ISysMenuService _menuService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="menuService"></param>
        public SysMenuController(ISysMenuService menuService)
        {
            _menuService = menuService;
        }

        /// <summary>
        /// 查询菜单树列表
        /// </summary>
        /// <param name="req"></param>
        [HttpGet("getMenuTree")]
        public async Task<ResModel<List<SysMenuOutput>>> GetMenuTree([FromQuery] SysMenuQueryInput req)
        {
            var res = await _menuService.GetMenuTree(req);
            return ResModel<List<SysMenuOutput>>.Success(res);
        }

        /// <summary>
        /// 根据id查询
        /// </summary>
        /// <param name="id"></param>
        [HttpGet("{id}")]
        public async Task<ResModel<SysMenuOutput>> Get(int id)
        {
            var data = await _menuService.Get(id);
            return ResModel<SysMenuOutput>.Success(data);
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="input"></param>
        [HttpPost]
        public async Task<ResModel<bool>> Add([FromBody] SysMenuOptInput input)
        {
            var uid = HttpContext.GetUid();
            var res = await _menuService.Add(input, uid);
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
            var res = await _menuService.Del(id, uid);
            return res ? ResModel<bool>.Success(true) : ResModel<bool>.Failed(false);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="input"></param>
        [HttpPut]
        public async Task<ResModel<bool>> Edit([FromBody] SysMenuOptInput input)
        {
            var uid = HttpContext.GetUid();
            var res = await _menuService.Edit(input, uid);
            return res ? ResModel<bool>.Success(true) : ResModel<bool>.Failed(false);
        }
    }
}
