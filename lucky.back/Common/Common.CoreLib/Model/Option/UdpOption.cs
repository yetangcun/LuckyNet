namespace Common.CoreLib.Model.Option
{
    /// <summary>
    /// Udp配置
    /// </summary>
    public class UdpOption
    {
        /// <summary>
        /// 端口
        /// </summary>
        public int Port { get; set; } = 0;

        /// <summary>
        /// 服务器IP
        /// </summary>
        public string? Host { get; set; }

        /// <summary>
        /// 超时时间
        /// </summary>
        public int Timeout { get; set; } = 0;

        /// <summary>
        /// 是否自动启动
        /// </summary>
        public bool IsAutoStart { get; set; } = false;
    }
}
