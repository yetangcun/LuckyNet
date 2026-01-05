using Lucky.BaseModel.Model;

namespace Lucky.SysModel.Model.Input
{
    /// <summary>
    /// 用户查询入参
    /// </summary>
    public class SysUserQueryInput : PageInfo
    {
        /// <summary>
        /// 搜索文本
        /// </summary>
        public string? Txt { get; set; }

        /// <summary>
        /// 组织ID
        /// </summary>
        public int? OrgId { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int? Status { get; set; }
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
