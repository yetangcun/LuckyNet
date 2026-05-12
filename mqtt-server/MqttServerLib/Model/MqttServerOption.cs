namespace MqttServerLib.Model
{
    /// <summary>
    /// Mqtt服务配置
    /// </summary>
    public class MqttServerOption
    {
        /// <summary>
        /// mqtt端口
        /// </summary>
        public int Port { get; set; } = 1883;

        /// <summary>
        /// websocket端口
        /// </summary>
        public int WsPort { get; set; } = 8083;

        /// <summary>
        /// mqtt加密端口
        /// </summary>
        public int SSLPort { get; set; } = 8883;

        /// <summary>
        /// ws加密端口
        /// </summary>
        public int WssPort { get; set; } = 8084;

        /// <summary>
        /// mqtt用户名
        /// </summary>
        public string UserName { get; set; } = "admin";

        /// <summary>
        /// mqtt密码
        /// </summary>
        public string PassWord { get; set; } = "123456";

        /// <summary>
        /// ws链接mqtt地址
        /// </summary>
        public string WsAddress { get; set; } = "ws://localhost:8083";

        /// <summary>
        /// ws链接mqtt地址
        /// </summary>
        public string WsTopic { get; set; } = "ws/aiot2/topic";

        /// <summary>
        /// 升级等待时间
        /// </summary>
        public int UpgradeWaitingTime { get; set; } = 5000;

        /// <summary>
        /// mqtt证书文件路径
        /// </summary>
        public string? CertificatePath { get; set; }

        /// <summary>
        /// mqtt证书文件密匙
        /// </summary>
        public string? CertificatePassword { get; set; }

        /// <summary>
        /// mqtt保活时间 秒
        /// </summary>
        public int MqttKeepAliveTime { get; set; } = 30;

        /// <summary>
        /// mqtt心跳包间隔时长 秒
        /// </summary>
        public int MqttKeepAliveInterval { get; set; } = 10;
    }
}
