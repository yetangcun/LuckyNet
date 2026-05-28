using Lucky.BaseModel.Model;
using Lucky.SysModel.Model.Input;
using Lucky.SysModel.Model.Output;
using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Http;

namespace lucky.admin.Controllers.sys
{
    /// <summary>
    /// 菜单管理
    /// </summary>
    public class SysMenuController : SysBaseController
    {
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="req"></param>
        [HttpGet("getPages")]
        public async Task<PageRes<List<SysMenuOutput>>> GetPages(SysMenuQueryInput req)
        {
            return PageRes<List<SysMenuOutput>>.Success(0, 0, null);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="id"></param>
        [HttpGet("{id}")]
        public async Task<ResModel<SysMenuOutput>> Get(long id)
        {
            return ResModel<SysMenuOutput>.Success(null);
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="input"></param>
        [HttpPost]
        public async Task<ResModel<bool>> Add([FromBody] SysMenuOptInput input)
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
        public async Task<ResModel<bool>> Edit([FromBody] SysMenuOptInput input)
        {
            return ResModel<bool>.Success(true);
        }
    }
}
