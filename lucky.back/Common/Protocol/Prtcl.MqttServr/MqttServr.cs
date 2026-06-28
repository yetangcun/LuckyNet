using System.Text;
using MQTTnet.Server;
using System.Buffers;
using MQTTnet.Protocol;
using Common.CoreLib.Model.Option;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Concurrent;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;

namespace Prtcl.MqttServr
{
    /// <summary>
    /// Mqtt服务
    /// </summary>
    public class MqttServr : IMqttServr
    {
        private MqttServer? _mqttServer;
        private readonly MqttServrOption _option;
        private readonly ILogger<MqttServr> _logger;

        private Task? _mqttStatusCheckTask = null;

        /// <summary>
        /// 构造函数
        /// </summary>
        public MqttServr(IOptions<MqttServrOption> option, ILogger<MqttServr> logger)
        {
            _option = option.Value;
            _logger = logger;

            _cltDic = new ConcurrentDictionary<string, ClientInfo>();
            _cltSubscribes = new ConcurrentDictionary<string, List<string>>();

            _logger.LogInformation($"MqttServer【::{_option.Port}】首次初始化...");
            _Init().GetAwaiter().GetResult();

            if (_mqttStatusCheckTask != null)
            {
                _mqttStatusCheckTask.Dispose();
                _mqttStatusCheckTask = null;
            }

            if (_mqttStatusCheckTask == null)
            {
                _mqttStatusCheckTask = new Task(async () =>
                {
                    await Task.Delay(1000 * 30);
                    while (true)
                    {
                        try
                        {
                            if (_mqttServer == null || !_mqttServer.IsStarted)
                            {
                                if (!IsSelfStop)
                                {
                                    _logger.LogInformation($"MqttServer【::{_option.Port}】正在重新启动...");
                                    await _Init();
                                    await StartAsync();
                                }
                            }
                            _logger.LogInformation($"MqttServer【::{_option.Port}】运行状态:{_mqttServer?.IsStarted}...");
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError($"MqttServer【::{_option.Port}】重启失败:{ex.Message},{ex.InnerException},{ex.StackTrace}");
                        }
                        await Task.Delay(1000 * 60 * 10); // 每隔10分钟检查一次服务状态
                    }
                }, TaskCreationOptions.LongRunning);
            }
            if (_mqttStatusCheckTask != null && _mqttStatusCheckTask.Status != TaskStatus.Running) _mqttStatusCheckTask.Start(); // 启动任务
        }

        private async Task _Init()
        {
            try
            {
                _logger.LogInformation($"MqttServer【::{_option.Port}】正在初始化...");
                if (_mqttServer != null)
                {
                    if (_mqttServer.IsStarted) await _mqttServer.StopAsync();
                    _mqttServer.Dispose(); _mqttServer = null;
                    _cltDic.Clear(); _cltSubscribes.Clear(); // GC.Collect();
                    await Task.Delay(1000);
                }

                MqttServerOptionsBuilder optionsBuilder = new MqttServerOptionsBuilder();
                optionsBuilder.WithDefaultEndpointPort(_option.Port); // 设置 服务端 端口号
                optionsBuilder.WithDefaultEndpoint();
                if (!string.IsNullOrEmpty(_option.MqttCertificatePath))
                {
                    // 读取证书文件
                    // X509Certificate2 certificate = new X509Certificate2(_option.MqttCertificatePath, _option.MqttCertificatePassword, X509KeyStorageFlags.Exportable); // 已过时
                    X509Certificate2 certificate = X509CertificateLoader.LoadCertificateFromFile(_option.MqttCertificatePath);

                    // 设置加密端口号
                    optionsBuilder.WithEncryptedEndpointPort(_option.SSLPort);

                    // 启用加密端口
                    optionsBuilder.WithEncryptedEndpoint();

                    // 设置加密端口所使用的证书
                    optionsBuilder.WithEncryptionCertificate(certificate.Export(X509ContentType.Pfx));

                    // 设置加密端口所使用的SSL协议
                    optionsBuilder.WithEncryptionSslProtocol(SslProtocols.Tls12);
                }

                optionsBuilder.WithPersistentSessions(true);// 持续会话
                optionsBuilder.WithConnectionBacklog(102400); // 最大连接数
                optionsBuilder.WithTcpKeepAliveTime(_option.MqttKeepAliveTime * 1000); // 设置 TCP keep-alive 时间为30秒
                optionsBuilder.WithDefaultCommunicationTimeout(TimeSpan.FromSeconds(10)); // 设置默认通信超时时间为 10 秒
                optionsBuilder.WithTcpKeepAliveInterval(_option.MqttKeepAliveInterval * 1000); // 设置 TCP keep-alive 间隔时间为 10 秒。

                MqttServerOptions options = optionsBuilder.Build();
                options.DefaultEndpointOptions.NoDelay = true;
                options.DefaultEndpointOptions.KeepAlive = true;
                options.DefaultEndpointOptions.TcpKeepAliveRetryCount = 3;
                _mqttServer = new MqttServerFactory().CreateMqttServer(options);

                // 内部事件处理，然后触发外部事件
                _mqttServer.StartedAsync += OnStartedAsync; // 服务器启动事件
                _mqttServer.StoppedAsync += OnStoppedAsync; // 服务器停止事件
                _mqttServer.ApplicationMessageNotConsumedAsync += OnApplicationMessageNotConsumed; // 当MQTT服务器接收到消息但没有任何订阅者消费此消息时,会触发此事件
                _mqttServer.ClientConnectedAsync += OnClientConnected; // 客户端接入
                _mqttServer.ClientDisconnectedAsync += OnClientDisconnected; // 客户端断开连接
                _mqttServer.ValidatingConnectionAsync += OnValidatingConnection; // 验证连接 用户名和密码
                _mqttServer.ClientSubscribedTopicAsync += OnClientSubscribedTopic; // 客户端订阅主题事件
                _mqttServer.ClientUnsubscribedTopicAsync += OnClientUnsubscribedTopic; // 客户端取消订阅主题事件

                //_mqttServer.InterceptingPublishAsync += args =>  // 拦截发布消息,进行额外处理
                //{
                //    args.ProcessPublish = true;
                //    return Task.CompletedTask;
                //};
                // _mqttServer.InterceptingClientEnqueueAsync += OnInterceptingClientEnqueue; //消息接收事件(在客户端消息队列中添加消息时触发。可以使用此事件来拦截并处理发送到客户端的消息)
                // _mqttServer.InterceptingInboundPacketAsync += OnInterceptingInboundPacket; //消息接收事件(在 MQTT 服务器接收到入站数据包时触发。可以使用此事件来拦截并处理接收到的消息)
                // _mqttServer.InterceptingOutboundPacketAsync += OnInterceptingOutboundPacket; //消息接收事件(在 MQTT 服务器发送出站数据包时触发。可以使用此事件来拦截并处理要发送的消息)
                // _mqttServer.StartAsync().GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                _logger.LogError($"MQTT服务启动失败:{ex.Message}");
            }
        }

        /// <summary>
        /// 启动mqtt服务
        /// </summary>
        /// <returns></returns>
        public async Task StartAsync()
        {
            try
            {
                if (_mqttServer != null && _mqttServer.IsStarted)
                {
                    _logger.LogInformation($"MqttServer【::{_option.Port}】正在运行");
                    return;
                }

                if (IsSelfStop)
                    IsSelfStop = false;

                _logger.LogInformation($"MqttServer【::{_option.Port}】正在启动...");
                await _mqttServer!.StartAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"MqttServer【::{_option.Port}】启动失败:{ex.Message}");
            }
        }

        private bool IsSelfStop = false;
        /// <summary>
        /// 停止mqtt服务
        /// </summary>
        public async Task StopAsync()
        {
            await _mqttServer.StopAsync();
            IsSelfStop = true;
        }

        /// <summary>
        /// 获取已连接的mqtt客户端数量
        /// </summary>
        public int GetConnectedClientsCountAsync()
        {
            return _cltDic.Count;
        }

        /// <summary>
        /// 获取已连接的mqtt客户端信息
        /// </summary>
        public ClientInfo? GetClientsInfoAsync(string clientId)
        {
            if (_cltDic.ContainsKey(clientId)) return _cltDic[clientId];
            return null;
        }

        /// <summary>
        /// 获取已连接的mqtt客户端订阅的Topic列表
        /// </summary>
        public List<string> GetClientSubscribesAsync(string clientId)
        {
            if (_cltSubscribes.ContainsKey(clientId)) return _cltSubscribes[clientId];
            return new List<string>();
        }

        /// <summary>
        /// 清空指定客户端的待处理消息
        /// </summary>
        public async Task CleanClientMessages(string clientId)
        {
            var session = await _mqttServer!.GetSessionAsync(clientId);
            if (session?.PendingApplicationMessagesCount > 0)
            {
                await session.ClearApplicationMessagesQueueAsync();
            }
        }

        /// <summary>
        /// 清空所有待处理的消息
        /// </summary>
        public async Task CleanAllPendingMessages()
        {
            var sessions = await _mqttServer!.GetSessionsAsync();

            if (sessions == null || sessions.Count <= 1) return;

            foreach (var session in sessions)
            {
                if (session.PendingApplicationMessagesCount > 0)
                {
                    await session.ClearApplicationMessagesQueueAsync();
                }
            }
        }

        #region 消息拦截器，事件处理

        private readonly ConcurrentDictionary<string, ClientInfo> _cltDic; // 客户端信息
        private readonly ConcurrentDictionary<string, List<string>> _cltSubscribes;  // 客户端订阅的主题列表

        private Task OnStartedAsync(EventArgs arg)
        {
            _logger.LogInformation($"MqttServer【::{_option.Port}】启动成功!");
            return Task.CompletedTask;
        }

        private async Task OnStoppedAsync(EventArgs args)
        {
            try
            {
                _logger.LogInformation($"MqttServer【::{_option.Port}】已停止!");
                await Task.Delay(6000); // 延迟6秒重启，防止端口占用

                if (!IsSelfStop)        // 如果不是自己手动停止的,则重新启动
                {
                    await _Init();          // 重新初始化
                    await StartAsync();     // 调用你的StartAsync方法重启
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"MqttServer【::{_option.Port}】 重启失败:{ex.Message}--{ex.StackTrace}--{ex.InnerException}!");
            }
        }

        private Task OnApplicationMessageNotConsumed(ApplicationMessageNotConsumedEventArgs args)
        {
            _logger.LogWarning($"未处理消息:{args.ApplicationMessage.Topic},{UTF8Encoding.UTF8.GetString(args.ApplicationMessage.Payload.ToArray())}");
            return Task.CompletedTask;
        }

        private Task OnValidatingConnection(ValidatingConnectionEventArgs cxt)
        {
            if (cxt.UserName == "admin" && cxt.Password == "123456")
            {
                cxt.ReasonCode = MqttConnectReasonCode.Success;
            }
            else
            {
                cxt.ReasonCode = MqttConnectReasonCode.BadUserNameOrPassword;
                _logger.LogError($"客户端{cxt.ClientId}请求连接失败:{cxt.ReasonCode},{cxt.ReasonString}");
            }

            return Task.CompletedTask;
        }

        private Task OnClientConnected(ClientConnectedEventArgs args)
        {
            var clitInfo = new ClientInfo
            {
                ClientId = args.ClientId,
                Address = args.RemoteEndPoint.ToString(),
                ConnectTime = DateTime.Now
            };

            if (_cltDic.ContainsKey(args.ClientId))
            {
                _cltDic[args.ClientId] = clitInfo;
                _logger.LogInformation($"客户端{args.ClientId}接入, 在线客户端总数:{_cltDic.Count()}");
                return Task.CompletedTask;
            }

            _cltDic.TryAdd(args.ClientId, clitInfo);
            _logger.LogInformation($"客户端{args.ClientId}接入, 在线客户端总数:{_cltDic.Count()}");
            return Task.CompletedTask;
        }

        private Task OnClientDisconnected(ClientDisconnectedEventArgs args)
        {
            _logger.LogInformation($"客户端{args.ClientId}已断开");
            _cltDic.TryRemove(args.ClientId, out _);         // 删除客户端信息
            _cltSubscribes.TryRemove(args.ClientId, out _);  // 删除客户端订阅的Topic
            return Task.CompletedTask;
        }

        private Task OnClientSubscribedTopic(ClientSubscribedTopicEventArgs args)
        {
            _logger.LogInformation($"客户端{args.ClientId}订阅了主题{args.TopicFilter.Topic}\r\n");

            if (!_cltSubscribes.ContainsKey(args.ClientId))
            {
                _cltSubscribes.TryAdd(args.ClientId, new List<string>());
                _cltSubscribes[args.ClientId].Add(args.TopicFilter.Topic);
                return Task.CompletedTask;
            }

            if (!_cltSubscribes[args.ClientId].Contains(args.TopicFilter.Topic))
                _cltSubscribes[args.ClientId].Add(args.TopicFilter.Topic);

            return Task.CompletedTask;
        }

        private Task OnClientUnsubscribedTopic(ClientUnsubscribedTopicEventArgs args)
        {
            _logger.LogInformation($"客户端{args.ClientId}取消订阅主题{args.TopicFilter}\r\n");

            if (_cltSubscribes.ContainsKey(args.ClientId))
            {
                if (_cltSubscribes[args.ClientId].Contains(args.TopicFilter))
                    _cltSubscribes[args.ClientId].Remove(args.TopicFilter);
            }

            return Task.CompletedTask;
        }
        #endregion
    }


    /// <summary>
    /// 客户端信息
    /// </summary>
    public class ClientInfo
    {
        /// <summary>
        /// 客户端Id
        /// </summary>
        public required string ClientId { get; set; }

        /// <summary>
        /// 连接地址
        /// </summary>
        public string? Address { get; set; }

        /// <summary>
        /// 连接时间
        /// </summary>
        public DateTime ConnectTime { get; set; }
    }
}
