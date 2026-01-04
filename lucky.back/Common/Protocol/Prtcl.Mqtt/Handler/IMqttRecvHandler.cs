using Common.CoreLib.Model.Common;

namespace Prtcl.Mqtt.Handler
{
    /// <summary>
    /// 接收消息处理
    /// </summary>
    public interface IMqttRecvHandler
    {
        Task<(bool, string)> Handle(MqttMsgModel data);
    }
}
