using Lucky.BaseModel.Model;
using Lucky.BaseService.Extension;
using Lucky.SysModel.Model.Input;
using Lucky.SysModel.Model.Output;
using Lucky.SysService.Service.IService;

// using Microsoft.AspNetCore.Http;
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
        public async Task<ResModel<List<SysOrgOutputTree>>> GetOrgTree([FromQuery] SysOrgQueryInput req)
        {
            var result = await _orgService.GetOrgTree(req);
            return ResModel<List<SysOrgOutputTree>>.Success(    result);
        }

        /// <summary>
        /// 查询组织树结构
        /// </summary>
        /// <param name="req"></param>
        [HttpGet("treeSels")]
        public async Task<ResModel<List<TreeSelectKV>>> GetOrgTreeSel([FromQuery] SysOrgQueryInput req)
        {
            var result = await _orgService.GetOrgTreeSel(req);
            return ResModel<List<TreeSelectKV>>.Success(result);
        }

        /// <summary>
        /// 根据id查询
        /// </summary>
        /// <param name="id"></param>
        [HttpGet("{id}")]
        public async Task<ResModel<SysOrgOutput>> Get(int id)
        {
            var result = await _orgService.Get(id);
            return ResModel<SysOrgOutput>.Success(result);
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="input"></param>
        [HttpPost]
        public async Task<ResModel<bool>> Opt([FromBody] SysOrgOptInput input)
        {
            var uid = HttpContext.GetUid();
            var result = await _orgService.Opt(input, uid);
            return result ? ResModel<bool>.Success(true) : ResModel<bool>.Failed(false, string.Empty);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete("{id}")]
        public async Task<ResModel<bool>> Del(int id)
        {
            var uid = HttpContext.GetUid();
            var res = await _orgService.Del(id, uid);
            return res ? ResModel<bool>.Success(true) : ResModel<bool>.Failed(false, string.Empty);
        }
    }
}
