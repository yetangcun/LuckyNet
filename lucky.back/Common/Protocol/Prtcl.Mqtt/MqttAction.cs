using MQTTnet;
using System.Text;
using System.Text.Json;
using MQTTnet.Protocol;
using Prtcl.Mqtt.Handler;
using Lucky.BaseModel.Enum;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Common.CoreLib.Model.Option;
using Common.CoreLib.Model.Common;
using Common.CoreLib.Extension.Common;
using Common.CoreLib.Util;

namespace Prtcl.Mqtt
{
    public class MqttAction : IMqttAction
    {
        private readonly MqttOption _option;
        private readonly ILogger<MqttAction> _logger;

        private readonly IMqttClient? _mqttClt;
        // private MqttClientOptions? _options;
        // private readonly ProtobufUtil _protoUtil;

        /// <summary>
        /// 构造函数
        /// </summary>
        public MqttAction(IOptions<MqttOption> options, ProtobufUtil proUtil, ILogger<MqttAction> logger)
        {
            _option = options.Value; // _protoUtil = proUtil;
            _logger = logger;

            try
            {
                _mqttClt = new MqttClientFactory().CreateMqttClient(); // var jwtToken = GetJwtToken(_option.ClientId, _option.Account, secretKey);

                // 配置连接选项
                var msgObj = new MqttMsgModel() { MsgType = MsgType.Up_Device_Status, Data = _option.ClientId };
                var msgJson = msgObj.ToJson();
                var msgBytes = Encoding.UTF8.GetBytes(msgJson);
                var opts = new MqttClientOptionsBuilder()
                    .WithClientId(_option.ClientId)
                    .WithTcpServer(_option.Host, _option.Port)
                    .WithCredentials(_option.User, _option.Passwd) //.WithCredentials(_option.Account, jwtToken)
                    .WithCleanStart() // 设置会话保持, 默认为true
                                      //.WithTlsOptions(options =>
                                      //{
                                      //    options.UseTls();
                                      //})
                    .WithWillTopic(MqttModule.DeviceOfflineTpc)  // 遗嘱主题
                    .WithWillPayload(msgBytes)  // 断开连接时发送的遗嘱消息
                    .WithKeepAlivePeriod(TimeSpan.FromSeconds(10))
                    .Build();

                // 绑定事件
                _mqttClt.ConnectedAsync += MqttConnectedAsync;
                _mqttClt.DisconnectedAsync += OnMqttDisconnected;
                _mqttClt.ApplicationMessageReceivedAsync += OnMqttReceivedAsync;

                _mqttClt.ConnectAsync(opts).GetAwaiter().GetResult(); // 连接mqtt服务端
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Mqtt连接失败");
            }
        }

        /// <summary>
        /// 接收消息处理
        /// </summary>
        private async Task OnMqttReceivedAsync(MqttApplicationMessageReceivedEventArgs args)
        {
            var json = Encoding.UTF8.GetString(args.ApplicationMessage.Payload);

            #region 兼容旧版本协议格式
            var tpc = args.ApplicationMessage.Topic;
            if (!MqttModule.Tpcs.Any(x => tpc.StartsWith(x)) && MqttModule.OldRecvHandler != null)
            {
                await MqttModule.OldRecvHandler(tpc, json);
                return;
            }
            #endregion

            var msg = JsonSerializer.Deserialize<MqttMsgModel>(json); if (msg == null) return;
            MqttModule.MsgHandleDic.TryGetValue(msg.MsgType, out IMqttRecvHandler? hdl);  // 获取消息处理器
            if (hdl != null)
            {
                var res = await hdl.Handle(msg); // 消息处理
                if (res.Item1)
                    await args.AcknowledgeAsync(CancellationToken.None); // 确认消息
                else
                {
                    // 加入死信队列
                }
            }
        }

        private bool _isConnecting = false;

        /// <summary>
        /// 断开连接处理
        /// </summary>
        private async Task OnMqttDisconnected(MqttClientDisconnectedEventArgs args)
        {
            if (_isConnecting)
                return;

            var retrys = 0; // 最多重试无数次

            _isConnecting = true;

            while (_mqttClt != null && !_mqttClt.IsConnected)
            {
                await Task.Delay(10000); // 10秒重连一次

                try
                {
                    await _mqttClt.ReconnectAsync();
                    if (_mqttClt.IsConnected)
                    {
                        _logger.LogInformation($"Mqtt客户端:{_option.ClientId}重接成功");
                        break;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Mqtt重接失败{retrys},{ex.Message}");
                }

                retrys++;
            }

            _isConnecting = false;
        }

        /// <summary>
        /// 连接成功处理
        /// </summary>
        private async Task MqttConnectedAsync(MqttClientConnectedEventArgs args)
        {
            _logger.LogInformation($"Connect MqttServer 【{_option.Host}:{_option.Port}】 success...");

            // 连接成功后，订阅主题
            await SubscribeAsync(MqttModule.Tpcs);
        }

        /// <summary>
        /// 发布Protobuf格式的消息
        /// </summary>
        public async Task<(bool, string)> PublishAsync(MqttMsgModel data)
        {
            if (_mqttClt == null || !_mqttClt.IsConnected)
                return (false, "mqtt未连接");

            var tp = MqttModule.MsgTopicMap[data.MsgType];
            var datas = Encoding.UTF8.GetBytes(data.ToJson());
            // var datas = _protoUtil.Tobytes(data); // 获取Protocol Buffer字节数组

            var builder = new MqttApplicationMessageBuilder()
                .WithTopic(tp)
                .WithPayload(datas)
                .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.AtMostOnce)
                .Build();

            try
            {
                var result = await _mqttClt.PublishAsync(builder, CancellationToken.None);
                return (true, string.Empty);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"消息发布最终失败: {ex.Message}");
                return (false, ex.Message);
            }
        }

        /// <summary>
        /// 发布Protobuf格式的消息
        /// </summary>
        public async Task<(bool, string)> PublishAsync(string tpc, MqttMsgModel data)
        {
            if (_mqttClt == null || !_mqttClt.IsConnected)
                return (false, "mqtt未连接");

            var datas = Encoding.UTF8.GetBytes(data.ToJson());
            // var datas = _protoUtil.Tobytes(data); // 获取Protocol Buffer字节数组

            var builder = new MqttApplicationMessageBuilder()
                .WithTopic(tpc)
                .WithPayload(datas)
                .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.AtMostOnce)
                .Build();

            try
            {
                var result = await _mqttClt.PublishAsync(builder, CancellationToken.None);
                return (true, string.Empty);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"消息发布最终失败: {ex.Message}");
                return (false, ex.Message);
            }
        }

        /// <summary>
        /// 发布Protobuf格式的消息
        /// </summary>
        public async Task<(bool, string)> PublishAsync(MqttMsgModel data, string? deviceId = null)
        {
            if (_mqttClt == null || !_mqttClt.IsConnected)
                return (false, "mqtt未连接");

            var tp = MqttModule.MsgTopicMap[data.MsgType];
            if(!string.IsNullOrWhiteSpace(deviceId))
                tp = string.Format(tp, deviceId);

            var datas = Encoding.UTF8.GetBytes(data.ToJson());
            // var datas = _protoUtil.Tobytes(data); // 获取Protocol Buffer字节数组

            var builder = new MqttApplicationMessageBuilder()
                .WithTopic(tp)
                .WithPayload(datas)
                .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.AtMostOnce)
                .Build();

            try
            {
                var result = await _mqttClt.PublishAsync(builder, CancellationToken.None);
                return (true, string.Empty);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"消息发布最终失败: {ex.Message}");
                return (false, ex.Message);
            }
        }

        /// <summary>
        /// 发布消息
        /// </summary>
        public async Task<(bool, string)> Publish(MqttMsgModel data)
        {
            if (_mqttClt == null || !_mqttClt.IsConnected)
                return (false, "mqtt未连接");

            var json = JsonSerializer.Serialize(data);
            var tp = MqttModule.MsgTopicMap[data.MsgType];
            {
                var builder = new MqttApplicationMessageBuilder()
                    .WithTopic(tp)
                    .WithPayload(json)
                    .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.AtMostOnce)
                    // .WithRetainFlag()  // 设置为保留消息
                    .Build();

                var res = await _mqttClt.PublishAsync(builder);

                return (res.IsSuccess, res.ReasonString);
            }
        }

        // 订阅主题
        public async Task<(bool, string)> SubscribeAsync(string[] topics)
        {
            if (_mqttClt == null || !_mqttClt.IsConnected)
                return (false, "mqtt服务未连接");

            foreach (var topic in topics)
            {
                try
                {
                    var topicFilter = new MqttTopicFilterBuilder().WithTopic(topic)
                        .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.AtLeastOnce)
                        .Build();

                    await _mqttClt.SubscribeAsync(topicFilter);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"订阅{topic}失败");
                }
            }

            return (true, "");
        }

        // 订阅主题
        public async Task<string> SubscribeAsync(string topic)
        {
            if (_mqttClt == null || !_mqttClt.IsConnected)
                return "mqtt未连接";

            try
            {
                var topicFilter = new MqttTopicFilterBuilder()
                    .WithTopic(topic)
                    .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.AtMostOnce)
                    .Build();

                var res = await _mqttClt.SubscribeAsync(topicFilter);
                return res.ReasonString;
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                return err;
            }
        }

        // 批量取消订阅
        public async Task<(bool, string)> UnSubscribeAsync(string[] topics)
        {
            if (_mqttClt == null || !_mqttClt.IsConnected) return (false, "mqtt未连接");

            foreach (var topic in topics)
            {
                try
                {
                    await _mqttClt.UnsubscribeAsync(topic);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"取消订阅{topic}失败");
                }
            }

            return (true, string.Empty);
        }

        // 取消订阅
        public async Task<(bool, string)> UnSubscribeAsync(string topic)
        {
            if (_mqttClt == null || !_mqttClt.IsConnected)
                return (false, "mqtt未连接");

            bool res = false;
            var msg = string.Empty;

            try
            {
                await _mqttClt.UnsubscribeAsync(topic);
            }
            catch (Exception ex)
            {
                res = false;
                msg = ex.Message;
                _logger.LogError(ex, $"取消订阅{topic}失败");
            }

            return (res, msg);
        }

    }
}
