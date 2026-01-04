using Lucky.BaseModel.Model;
using Lucky.SysModel.Model.Input;
using Lucky.SysModel.Model.Output;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace lucky.admin.Controllers.sys
{
    /// <summary>
    /// 用户管理
    /// </summary>
    public class SysUserController : SysBaseController
    {
        #region  用户管理
        /// <summary>
        /// 用户管理
        /// </summary>
        [HttpGet("list")]
        public async Task<ResModel<List<SysUserOutput>>> GetList([FromQuery] SysUserQueryInput req)
        {
            return ResModel<List<SysUserOutput>>.Success(new List<SysUserOutput>());
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
