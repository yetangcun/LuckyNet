using Common.CoreLib.Extension.Common;
using Lucky.BaseModel.Model;
using Lucky.SysModel.Model.Input;
using Lucky.SysModel.Model.Output;
using Lucky.SysService.Service.IService;
// using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
            [FromServices] JwtAuthExtension jwt)
        {
            var res = new SysLoginOutput();
            var token = jwt.GetToken(req.Account, "999999");
            res.Tkn = token;
            return ResModel<SysLoginOutput>.Success(res);
        }

        /// <summary>
        /// 用户管理
        /// </summary>
        [HttpGet("list")]
        public async Task<ResModel<List<SysUserOutput>>> GetList([FromQuery] SysUserQueryInput req)
        {
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
