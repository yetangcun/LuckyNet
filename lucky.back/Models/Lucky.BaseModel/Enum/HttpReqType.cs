namespace Lucky.BaseModel.Enum
{
    /// <summary>
    /// http请求方式枚举
    /// </summary>
    public enum HttpOptType
    {
        /// <summary>
        /// 查询
        /// </summary>
        Query = 1,

        /// <summary>
        /// 添加
        /// </summary>
        Add = 2,

        /// <summary>
        /// 修改
        /// </summary>
        Modify = 3,

        /// <summary>
        /// 删除
        /// </summary>
        Delete = 4,

        /// <summary>
        /// 导入
        /// </summary>
        Patch = 5,

        /// <summary>
        /// 导出
        /// </summary>
        Options = 6,

        /// <summary>
        /// 登录
        /// </summary>
        Login = 7,

        /// <summary>
        /// 登出
        /// </summary>
        Logout = 8,
    }
}
