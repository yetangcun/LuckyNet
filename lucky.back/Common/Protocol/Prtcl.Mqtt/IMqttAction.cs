using Common.CoreLib.Model.Common;

namespace Prtcl.Mqtt
{
    /// <summary>
    /// Mqtt操作接口
    /// </summary>
    public interface IMqttAction
    {
        /// <summary>
        /// 发送消息
        /// </summary>
        Task<(bool, string)> PublishAsync(MqttMsgModel data);

        /// <summary>
        /// 发送消息
        /// </summary>
        Task<(bool, string)> PublishAsync(string tpc, MqttMsgModel data);

        /// <summary>
        /// 发送消息
        /// </summary>
        Task<(bool, string)> PublishAsync(MqttMsgModel data, string? deviceId = null);

        /// <summary>
        /// 发布消息
        /// </summary>
        Task<(bool, string)> Publish(MqttMsgModel data);

        /// <summary>
        /// 批量订阅主题
        /// </summary>
        Task<(bool, string)> SubscribeAsync(string[] topics);

        /// <summary>
        /// 订阅主题
        /// </summary>
        Task<string> SubscribeAsync(string topic);

        /// <summary>
        /// 批量取消订阅
        /// </summary>
        Task<(bool, string)> UnSubscribeAsync(string[] topics);

        /// <summary>
        /// 取消订阅
        /// </summary>
        Task<(bool, string)> UnSubscribeAsync(string topic);
    }
}
