namespace Common.CoreLib.Model.Option
{
    /// <summary>
    /// Redis配置
    /// </summary>
    public class RdisOption
    {
        /// <summary>
        /// 客户端名
        /// </summary>
        public string? clientName { get; set; }

        /// <summary>
        /// 服务地址
        /// </summary>
        public string? host { get; set; }

        /// <summary>
        /// 端口
        /// </summary>
        public int port { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string? username { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string? pwd { get; set; }

        /// <summary>
        /// 数据库编号
        /// 0~15
        /// </summary>
        public int dbNo { get; set; }

        /// <summary>
        /// 连接超时|秒
        /// </summary>
        public int timeOut { get; set; } = 6;
    }
}
