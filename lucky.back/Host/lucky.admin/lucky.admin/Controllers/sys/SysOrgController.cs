using Lucky.BaseModel.Model;
using Lucky.SysModel.Model.Input;
using Lucky.SysModel.Model.Output;
//using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace lucky.admin.Controllers.sys
{
    /// <summary>
    /// 组织管理
    /// </summary>
    public class SysOrgController : SysBaseController
    {
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="req"></param>
        [HttpGet("pages")]
        public async Task<PageRes<List<SysOrgOutput>>> GetPages(SysOrgQueryInput req)
        {
            return PageRes<List<SysOrgOutput>>.Success(0, 0, null);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="id"></param>
        [HttpGet("{id}")]
        public async Task<ResModel<SysOrgOutput>> Get(long id)
        {
            return ResModel<SysOrgOutput>.Success(null);
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="input"></param>
        [HttpPost]
        public async Task<ResModel<bool>> Add([FromBody] SysOrgOptInput input)
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
        public async Task<ResModel<bool>> Edit([FromBody] SysOrgOptInput input)
        {
            return ResModel<bool>.Success(true);
        }
    }
}
