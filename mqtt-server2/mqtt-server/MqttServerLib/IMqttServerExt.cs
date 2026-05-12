namespace MqttServerLib
{
    /// <summary>
    /// MQTT服务扩展接口
    /// </summary>
    public interface IMqttServerExt
    {
        /// <summary>
        /// 启动
        /// </summary>
        Task StartAsync();

        /// <summary>
        /// 停止
        /// </summary>
        /// <returns></returns>
        Task StopAsync();

        /// <summary>
        /// 获取当前连接的MQTT客户端数量
        /// </summary>
        int GetConnectedClientsCountAsync();

        /// <summary>
        /// 获取MQTT客户端信息
        /// </summary>
        ClientInfo? GetClientsInfoAsync(string clientId);

        /// <summary>
        /// 获取MQTT客户端的订阅主题
        /// </summary>
        List<string> GetClientSubscribesAsync(string clientId);

        /// <summary>
        /// 清理MQTT客户端待处理消息
        /// </summary>
        Task CleanClientMessages(string clientId);

        /// <summary>
        /// 清理所有待处理消息
        /// </summary>
        Task CleanAllPendingMessages();
    }
}
