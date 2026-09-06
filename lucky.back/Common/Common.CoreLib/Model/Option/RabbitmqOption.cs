namespace Common.CoreLib.Model.Option
{
    /// <summary>
    ///  RabbitMQ 配置选项
    /// </summary>
    public class RabbitmqOption
    {
        /// <summary>
        /// 服务地址
        /// </summary>
        public required string Ip { get; set; }

        /// <summary>
        /// 端口
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// 账号
        /// </summary>
        public required string Usr { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public required string Pwd { get; set; }

        /// <summary>
        /// 最大连接数
        /// </summary>
        public int Channels { get; set; } = 32;
    }
}
