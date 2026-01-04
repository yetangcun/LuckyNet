using Prtcl.Mqtt.Handler;
using Lucky.BaseModel.Enum;
using System.Collections.Concurrent;
using Microsoft.Extensions.DependencyInjection;

namespace Prtcl.Mqtt
{
    public static class MqttModule
    {
        public static void MqttInitLoad(this IServiceProvider provider)
        {
            // var mqtt = provider.GetService<IMqttAction>();
            if (msgTopicMap == null) msgTopicMap = new ConcurrentDictionary<MsgType, string>();
            if (msgHandleDic == null) msgHandleDic = new ConcurrentDictionary<MsgType, IMqttRecvHandler>();

            // 设置消息类型和主题的映射
            msgTopicMap.AddOrUpdate(MsgType.Up_Device_Status, d => DeviceOfflineTpc, (_, _) => DeviceOfflineTpc);

            // 消息类型和处理程序的映射
            var hdl = provider.GetService<IMqttRecvHandler>();
            if (hdl != null)
            {
                msgHandleDic.AddOrUpdate(MsgType.Up_Device_Status, h => hdl, (_, _) => hdl);
            }
        }

        // 要订阅的主题列表
        public static string[] Tpcs =
        {
            DeviceOfflineTpc,  // 设备遗嘱主题
            "digitalhuman/up"  // 数字人设备上报主题
        };

        /// <summary>
        /// MQ消息处理
        /// 兼容旧版本
        /// </summary>
        public static Func<string, string, Task<(bool, string)>>? OldRecvHandler;

        /// <summary>
        /// MQ消息类型与主题的映射
        /// </summary>
        private static ConcurrentDictionary<MsgType, string>? msgTopicMap;
        public static ConcurrentDictionary<MsgType, string> MsgTopicMap
        {
            get
            {
                if (msgTopicMap == null) msgTopicMap = new ConcurrentDictionary<MsgType, string>();
                return msgTopicMap;
            }
            private set
            {
            }
        }

        public const string DeviceOfflineTpc = "device/offline";

        /// <summary>
        /// MQ消息类型与处理程序的映射
        /// </summary>
        private static ConcurrentDictionary<MsgType, IMqttRecvHandler>? msgHandleDic;
        public static ConcurrentDictionary<MsgType, IMqttRecvHandler> MsgHandleDic
        {
            get
            {
                if (msgHandleDic == null) msgHandleDic = new ConcurrentDictionary<MsgType, IMqttRecvHandler>();
                return msgHandleDic;
            }
            private set { }
        }

    }
}
