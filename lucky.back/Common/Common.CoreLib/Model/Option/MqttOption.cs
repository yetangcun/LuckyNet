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


    /// <summary>
    /// Mqtt服务配置
    /// </summary>
    public class MqttServrOption
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
        public string UserName { get; set; } = "AIOT2";

        /// <summary>
        /// mqtt密码
        /// </summary>
        public string Password { get; set; } = "AIOT2";

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
        public string? MqttCertificatePath { get; set; }

        /// <summary>
        /// mqtt证书文件密匙
        /// </summary>
        public string? MqttCertificatePassword { get; set; }

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
