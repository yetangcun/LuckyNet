
namespace Common.CoreLib.Model.Enum
{
    /// <summary>
    /// 消息类型
    /// </summary>
    public enum MQType
    {
        /// <summary>
        /// 默认
        /// </summary>
        Default = 0,

        /// <summary>
        /// RabbitMq
        /// </summary>
        RabbitMq = 1,

        /// <summary>
        /// Kafka
        /// </summary>
        Kafka = 2,

        /// <summary>
        /// Mqtt
        /// </summary>
        Mqtt = 3,

        /// <summary>
        /// Redis
        /// </summary>
        Redis = 4,

        /// <summary>
        /// ActiveMq
        /// </summary>
        ActiveMq = 5,

        /// <summary>
        /// RocketMq
        /// </summary>
        RocketMq = 6
    }
}
