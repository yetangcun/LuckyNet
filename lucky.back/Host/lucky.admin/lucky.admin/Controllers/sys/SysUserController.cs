using Common.CoreLib.Extension.Common;
using Lucky.BaseModel.Model;
using Lucky.BaseService.Extension;
using Lucky.SysModel.Model.Input;
using Lucky.SysModel.Model.Output;
// using Microsoft.AspNetCore.Http;
using Lucky.SysService.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Tsk.Quartz.Jobs;
using Tsk.Quartz.Jobs.Example;

namespace lucky.admin.Controllers.sys
{
    /// <summary>
    /// 用户管理
    /// </summary>
    public class SysUserController : SysBaseController
    {
        private readonly ISysUserService _sysUserService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="sysUserService"></param>
        public SysUserController(ISysUserService sysUserService)
        {
            _sysUserService = sysUserService;
        }

        #region  用户管理
        /// <summary>
        /// 登录
        /// </summary>
        [HttpPost("loginHdl")]
        public async Task<ResModel<SysLoginOutput>> Post(
            [FromBody] SysUserLoginInput req,
            [FromServices] JobExtension jobExt,
            [FromServices] JwtAuthExtension jwt)
        {
            #region job 测试 "0/5 * * * * ?"  秒 分 时 【日(Day of month)】 月 【星期几(Day of week)】 【年(可选，可以忽略)】

            await jobExt.AddOnceJob<OnceTestJob>(null);  // 一次性 立即执行

            //await Task.Delay(100);
            //await jobExt.AddOnceDelayJob<OnceTestJob>(new Dictionary<string, object>() // 一次性 延迟执行
            //{
            //    {"name", "Quartz Once Delay 测试"}
            //}, TimeSpan.FromSeconds(6));

            //await Task.Delay(100);
            //await jobExt.AddIntervalJob<IntervalTestJob>(null, 4);  // 周期性循环执行

            //await Task.Delay(100);
            await jobExt.AddCornJob<CornTestJob>(null, "0/6 * * * * ?"); // corn 表达式 循环执行 每隔6秒执行一次

            #endregion

            #region jwt 测试

            var res = new SysLoginOutput();
            var token = jwt.GetToken(req.Account, "999999");
            res.Tkn = token;
            return ResModel<SysLoginOutput>.Success(res);

            #endregion
        }

        /// <summary>
        /// 用户管理
        /// </summary>
        [HttpGet("list")]
        public async Task<ResModel<List<SysUserOutput>>> GetList([FromQuery] SysUserQueryInput req)
        {
            var uid = HttpContext.GetUid();
            // var ip = HttpContext.GetClientIp();

            var res = await _sysUserService.GetList(req);
            return ResModel<List<SysUserOutput>>.Success(res.Item2);
        }
        #endregion

        #region 用户角色
        #endregion

        #region 用户组织
        #endregion

        #region 用户菜单
        #endregion
    }
}
