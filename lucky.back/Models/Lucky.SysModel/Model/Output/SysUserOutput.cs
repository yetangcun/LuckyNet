namespace Lucky.SysModel.Model.Output
{
    /// <summary>
    /// 系统用户输出
    /// </summary>
    public class SysUserOutput
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public long id { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string? account { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public string avatar { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string? realname { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string? password { get; set; }
    }

    /// <summary>
    /// 系统登录输出
    /// </summary>
    public class SysLoginOutput
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public string? uid { get; set; }

        ///// <summary>
        ///// 用户名|姓名
        ///// </summary>
        //public string? name { get; set; }

        ///// <summary>
        ///// 昵称
        ///// </summary>
        //public string? nickName { get; set; }

        ///// <summary>
        ///// 角色名称
        ///// </summary>
        //public string? roleName { get; set; }

        ///// <summary>
        ///// 头像
        ///// </summary>
        //public string? avatar { get; set; }

        /// <summary>
        /// 令牌
        /// </summary>
        public string? tkn { get; set; }

        ///// <summary>
        ///// 布局类型 1左侧  2顶部+左侧
        ///// 默认为1
        ///// </summary>
        //public int? layout { get; set; } = 1;
    }
}
