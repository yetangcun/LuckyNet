using System;
using System.Text;
using System.Collections.Generic;

namespace Lucky.SysModel.Model.Input
{
    /// <summary>
    /// 用户查询入参
    /// </summary>
    public class SysUserQueryInput
    {
    }

    /// <summary>
    /// 用户操作入参
    /// </summary>
    public class SysUserOptInput
    { 
    }

    /// <summary>
    /// 登录入参
    /// </summary>
    public class SysUserLoginInput
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public required string Account { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public required string Password { get; set; }
    }
}
