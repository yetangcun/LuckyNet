namespace Common.CoreLib.Model.Option
{
    /// <summary>
    /// mqtt配置
    /// </summary>
    public class MqttOption
    {
        /// <summary>
        /// 主机
        /// </summary>
        public string Host { get; set; } = "localhost";

        /// <summary>
        /// 端口
        /// </summary>
        public int Port { get; set; } = 1888;

        /// <summary>
        /// 客户端id
        /// </summary>
        public string? ClientId { get; set; }

        /// <summary>
        /// 用户账号
        /// </summary>
        public string? User { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string? Passwd { get; set; }
    }
}
