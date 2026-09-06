using Common.CoreLib.Model.Common;
using Lucky.BaseModel.Enum;
using System.Collections.Concurrent;

namespace Common.CoreLib.Extension.Common
{
    /// <summary>
    /// Mq扩展
    /// </summary>
    public class MqExtension
    {
        /// <summary>
        /// 消息处理字典
        /// </summary>
        public static ConcurrentDictionary<MsgType, IMqHandle> MqHandlersDic = new ConcurrentDictionary<MsgType, IMqHandle>();
    }

    /// <summary>
    /// 消息处理
    /// </summary>
    public interface IMqHandle
    {
        /// <summary>
        /// 处理接口
        /// </summary>
        Task<(bool, string)> Handle(MqMsgModel mqRecv);
    }
}
