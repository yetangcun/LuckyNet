using Lucky.BaseModel.Enum;
using System.Collections;

namespace Common.CoreLib.Model.Common
{
    /// <summary>
    /// 消息基类
    /// </summary>
    public class MqMsgModel
    {
        #region 消息发布模型

        /// <summary>
        /// 消息类型
        /// </summary>
        public MsgType MsgType { get; set; } = MsgType.Unknown;

        /// <summary>
        /// 唯一会话Id
        /// </summary>
        public string? Sid { get; set; }

        /// <summary>
        /// 消息
        /// Json 字符串
        /// </summary>
        public string? JsonMsg { get; set; }

        /// <summary>
        /// 其他参数补充
        /// </summary>
        public Hashtable? Hashtable { get; set; }
    }

    /// <summary>
    /// kafka消息发布模型
    /// </summary>
    public class KfkMsgModel : MqMsgModel
    {
        /// <summary>
        /// 主题
        /// </summary>
        public string Tpc { get; set; } = string.Empty;

        /// <summary>
        /// 分区Id
        /// </summary>
        public int? Pid { get; set; }
    }

    /// <summary>
    /// RabbitMq消息发布模型
    /// </summary>
    public class RabbitMsgModel : MqMsgModel
    {
    }

    #endregion

    #region 消息消费处理结果模型

    /// <summary>
    /// MQ消息消费(处理)的结果模型
    /// </summary>
    public class MqMsgRes
    {
        /// <summary>
        /// 消息唯一标识
        /// </summary>
        public string? Sid { get; set; }

        /// <summary>
        /// 处理状态
        /// 成功：true
        /// 失败：false
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        /// 处理结果信息
        /// 成功：""
        /// 失败：错误信息
        /// 也可以是json串
        /// </summary>
        public string? Msg { get; set; }
    }

    #endregion

    /// <summary>
    /// 消费者逻辑处理接口
    /// </summary>
    public interface IMqHdl
    {
        /// <summary>
        /// 消费者处理接口
        /// </summary>
        Task<MqMsgRes> hdl(KfkMsgModel data);
    }
}
