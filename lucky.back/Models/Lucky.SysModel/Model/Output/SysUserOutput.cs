using System;
using System.Text;
using System.Collections.Generic;

namespace Lucky.SysModel.Model.Output
{
    /// <summary>
    /// 系统用户输出
    /// </summary>
    public class SysUserOutput
    {
    }

    /// <summary>
    /// 系统登录输出
    /// </summary>
    public class SysLoginOutput
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public string? Uid { get; set; }

        /// <summary>
        /// 用户名|姓名
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public string? NickName { get; set; }

        /// <summary>
        /// 角色名称
        /// </summary>
        public string? RoleName { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public string? Avatar { get; set; }

        /// <summary>
        /// 令牌
        /// </summary>
        public string? Tkn { get; set; }
    }
}
