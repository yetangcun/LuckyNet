using Lucky.BaseModel.Enum;

namespace Common.CoreLib.Model.Common
{
    /// <summary>
    /// Mqtt消息模型
    /// </summary>
    public class MqttMsgModel
    {
        /// <summary>
        /// 消息类型
        /// </summary>
        public MsgType MsgType { get; set; }

        /// <summary>
        /// 消息ID
        /// </summary>
        public string? MsgId { get; set; }

        /// <summary>
        /// 消息内容: Json
        /// </summary>
        public string? Data { get; set; } = string.Empty;
    }
}
