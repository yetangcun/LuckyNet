using Lucky.BaseModel.Model;
using Microsoft.AspNetCore.Mvc;
using Lucky.BaseService.Extension;
using Lucky.SysModel.Model.Input;
using Lucky.SysModel.Model.Output;
using Lucky.SysService.Service.IService;
//using Microsoft.AspNetCore.Http;

namespace lucky.admin.Controllers.sys
{
    /// <summary>
    /// 系统任务控制器
    /// </summary>
    public class SysTskController : SysBaseController
    {
        private readonly ISysTskService sysTskService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="sysTskService"></param>
        public SysTskController(
        ILogger<SysTskController> logger,
        ISysTskService sysTskService)
        {
            this.sysTskService = sysTskService;
        }

        /// <summary>
        /// 获取任务列表
        /// </summary>
        [HttpGet("pages")]
        public async Task<PageRes<List<SysTskOutput>>> GetPages([FromQuery] SysTskQueryInput req)
        {
            var uid = HttpContext.GetUid();
            var res = await sysTskService.GetPages(req);
            return PageRes<List<SysTskOutput>>.Success(res.Item1, res.Item2);
        }

        /// <summary>
        /// 添加/修改任务
        /// </summary>
        [HttpPost]
        public async Task<ResModel<bool>> Opt([FromBody] SysTskOptInput input)
        {
            var uid = HttpContext.GetUid();
            var res = await sysTskService.Opt(input, uid);
            return res.Item1 ? ResModel<bool>.Success(true) : ResModel<bool>.Failed(false, res.Item2);
        }

        /// <summary>
        /// 删除任务
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ResModel<bool>> Del(int id)
        {
            var uid = HttpContext.GetUid();
            var res = await sysTskService.Del(id, uid);
            return res.Item1 ? ResModel<bool>.Success(true) : ResModel<bool>.Failed(false, res.Item2);
        }

        /// <summary>
        /// 获取任务详情
        /// </summary>
        /// <param name="id"></param>
        [HttpGet("{id}")]
        public async Task<ResModel<SysTskOutput>> Get(int id)
        {
            var uid = HttpContext.GetUid();
            var res = await sysTskService.Get(id);
            return ResModel<SysTskOutput>.Success(res);
        }

        #region 任务记录
        #endregion
    }
}
