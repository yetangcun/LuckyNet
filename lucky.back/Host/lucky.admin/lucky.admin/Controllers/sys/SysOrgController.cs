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
    /// 组织管理
    /// </summary>
    public class SysOrgController : SysBaseController
    {
        private readonly ISysOrgService _orgService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="orgService"></param>
        public SysOrgController(
        ISysOrgService orgService)
        {
            _orgService = orgService;
        }

        /// <summary>
        /// 查询组织树结构
        /// </summary>
        /// <param name="req"></param>
        [HttpGet("tree")]
        public async Task<ResModel<List<SysOrgOutput>>> GetOrgTree([FromQuery] SysOrgQueryInput req)
        {
            return ResModel<List<SysOrgOutput>>.Success(null);
        }

        /// <summary>
        /// 根据id查询
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
            var uid = HttpContext.GetUid();
            return ResModel<bool>.Success(true);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete("{id}")]
        public async Task<ResModel<bool>> Del(long id)
        {
            var uid = HttpContext.GetUid();
            return ResModel<bool>.Success(true);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="input"></param>
        [HttpPut]
        public async Task<ResModel<bool>> Edit([FromBody] SysOrgOptInput input)
        {
            var uid = HttpContext.GetUid();
            return ResModel<bool>.Success(true);
        }
    }
}
