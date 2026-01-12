using System;
using System.Text;
using Prtcl.Mqtt.Handler;
using System.Collections.Generic;
using Common.CoreLib.Model.Common;

namespace Lucky.IotService.Extension.Handler
{
    public class MqttDefaultHandler : IMqttRecvHandler
    {
        /// <summary>
        /// 接收数据处理
        /// </summary>
        /// <param name="data"></param>
        public async Task<(bool, string)> Handle(MqttMsgModel data)
        {
            throw new NotImplementedException();
        }
    }
}
