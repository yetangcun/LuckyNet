using System.Text;
using RabbitMQ.Client;
using Lucky.BaseModel.Model;
using RabbitMQ.Client.Events;
using Common.CoreLib.Model.Option;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Concurrent;
using Common.CoreLib.Extension.Common;

namespace Prtcl.Rabbitmq
{
    public class RabbitmqService : IRabbitmqService
    {
        private readonly ChannelPool _pool;
        private readonly ILogger<RabbitmqService> _logger;

        public RabbitmqService(ChannelPool pool ,IOptions<RabbitmqOption> mqOption, ILogger<RabbitmqService> logger)
        {
            _pool = pool;
            _logger = logger;
        }

        #region 简单模式|工作队列
        /// <summary>
        /// 发布消息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="queueName"></param>
        public async Task<bool> PublishAsync<T>(T data, string queueName)
        {
            var channel = await _pool.AcquireChannelAsync();

            var json = data.ToJson();
            var content = Encoding.UTF8.GetBytes(json);

            // 没绑定交换机 使用的是rabbitmq默认的交换机
            // 队列名 是否持久化 是否对其他连接可见 是否自动删除 其他参数
            await channel.QueueDeclareAsync(queueName, true, false, false, null);
            await channel.BasicPublishAsync("", queueName, content);
            return true;
        }

        // private AsyncEventingBasicConsumer? _simpleConsumer = null; // private EventingBasicConsumer? _simpleConsumer = null;
        private List<AsyncEventingBasicConsumer> _consumers = new List<AsyncEventingBasicConsumer>();

        /// <summary>
        /// 批量消费者
        /// </summary>
        /// <param name="queueName"> 队列名称 </param>
        /// <param name="consumers"> 消费者数量 </param>
        public async Task ConsumerAsync(string queueName, int consumers = 1)
        {
            if (_consumers.Count >= consumers)
                return;

            for (int i = 0; i < consumers; i++)
            {
                var consumerChannel = await _pool.AcquireChannelAsync();

                if (consumerChannel == null || !consumerChannel.IsOpen)
                    continue;

                await consumerChannel.QueueDeclareAsync(queueName, true, false, false, null);

                // 第一个参数指定消息本身大小 0表示不限制
                // 第二个表示rabbitmq一次推送的消息不要超过此数
                // 第三个表示前面的设置应用于整个通道 false表示只应用于当前通道
                await consumerChannel.BasicQosAsync(0, 1, false);

                var _consumer = new AsyncEventingBasicConsumer(consumerChannel);
                _consumer.ShutdownAsync += _simpleConsumer_ShutdownAsync;
                _consumer.ReceivedAsync += async (sender, args) =>
                {
                    try
                    {
                        var json = Encoding.UTF8.GetString(args.Body.ToArray());
                        var content = json.ToObj<MqMsgModel>();
                        if (content == null)
                            return;

                        if (!MqExtension.MqHandlersDic.TryGetValue(content.MsgType, out var handler))
                            return;

                        if (handler == null)
                        {
                            await consumerChannel.BasicAckAsync(args.DeliveryTag, false);
                            return;
                        }

                        _logger.LogInformation($"消费ing: {json}");
                        var res = await handler.Handle(content);

                        if (res.Item1)
                            await consumerChannel.BasicAckAsync(args.DeliveryTag, false);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"{ex.Message},{ex.StackTrace},{ex.InnerException}");
                        await consumerChannel.BasicAckAsync(args.DeliveryTag, false);
                    }
                };

                await consumerChannel.BasicConsumeAsync(queueName, false, _consumer);

                _consumers.Add(_consumer);
            }
        }

        private Task _simpleConsumer_ShutdownAsync(object sender, ShutdownEventArgs @event)
        {
            _logger.LogError($"consumer shutdown: {@event.Cause},{@event.ReplyCode}-{@event.ReplyText};{@event.Exception?.Message}-{@event.Exception?.InnerException}");
            // throw new NotImplementedException();
            return Task.CompletedTask;
        }
        #endregion

        #region 订阅模式|发布订阅

        /// <summary>
        /// 生产发布
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="exchgName"></param>
        public async Task<bool> PublishSubscribe<T>(T data, string exchg)
        {

             var _channel = await _pool.AcquireChannelAsync();

            await _channel.ExchangeDeclareAsync(exchange: exchg, ExchangeType.Fanout);  // 创建fanout类型的交换机，此类型交换机即广播模式, 发布到交换机的每个消息都会被发送到所有绑定的队列中

            var msg = data.ToJson();

            var bts = Encoding.UTF8.GetBytes(msg);

            await _channel.BasicPublishAsync(exchg, "", bts);

            return true;
        }

        /// <summary>
        /// 消费订阅
        /// </summary>
        /// <param name="queueName"></param>
        /// <param name="exchangeName"></param>
        private AsyncEventingBasicConsumer? _consumer = null;
        public async Task ConsumerSubscribe(string exchg, string queueName = "subscribe_dft_queue")
        {
            if (_consumer != null && _consumer.IsRunning)
                return;

            var chl = await _pool.AcquireChannelAsync();

            await chl.ExchangeDeclareAsync(exchange: exchg, ExchangeType.Fanout); // 创建fanout类型的交换机，此类型交换机即广播模式, 订阅模式

            if (string.IsNullOrWhiteSpace(queueName))
                queueName = (await chl.QueueDeclareAsync()).QueueName; // 创建一个匿名队列
            else
                await chl.QueueDeclareAsync(queueName, true, false, false, null);

            await chl.QueueBindAsync(queueName, exchg, ""); // 绑定队列和交换机

            _consumer = new AsyncEventingBasicConsumer(chl);
            _consumer.ReceivedAsync += async (sender, args) =>
            {
                var json = Encoding.UTF8.GetString(args.Body.ToArray());
                var content = json.ToObj<MqMsgModel>();
                if (content == null) return;

                if (!MqExtension.MqHandlersDic.TryGetValue(content.MsgType, out var handler))
                    return;

                var res = await handler.Handle(content);

                if (res.Item1) await chl.BasicAckAsync(args.DeliveryTag, false);
            };

            await chl.BasicConsumeAsync(queueName, false, _consumer);
        }

        #endregion

        #region 路由

        public async Task<bool> PublishRouter(string exchg, string routeKey, object data)
        {
            var chl = await _pool.AcquireChannelAsync();

            await chl.ExchangeDeclareAsync(exchg, ExchangeType.Direct); // 创建direct类型的交换机,即路由模式或路由匹配，此类型交换机根据路由键将消息推送到与之对应的队列上

            var msg = data.ToJson();

            var bts = Encoding.UTF8.GetBytes(msg);

            await chl.BasicPublishAsync(exchg, routeKey, bts);

            return true;
        }

        private AsyncEventingBasicConsumer? _routeConsumer = null;
        public async Task ConsumerRouter(string exchg, string routeKey, string queueName = "route_dft_queue")
        {
            if (_routeConsumer != null && _routeConsumer.IsRunning)
                return;

            var chl = await _pool.AcquireChannelAsync(); // 检查通道是否可用，如果不可用，则重新获取一个可用的通道

            await chl.ExchangeDeclareAsync(exchg, ExchangeType.Direct);

            if (string.IsNullOrWhiteSpace(queueName))
                queueName = (await chl.QueueDeclareAsync()).QueueName;
            else
                await chl.QueueDeclareAsync(queueName, true, false, false, null);

            await chl.QueueBindAsync(queueName, exchg, routeKey, null);
            await chl.BasicQosAsync(0, 1, false);

            _routeConsumer = new AsyncEventingBasicConsumer(chl);
            _routeConsumer.ReceivedAsync += async (sender, args) =>
            {
                var json = Encoding.UTF8.GetString(args.Body.ToArray());
                var content = json.ToObj<MqMsgModel>();
                if (content == null)
                    return;

                if (!MqExtension.MqHandlersDic.TryGetValue(content.MsgType, out var handler))
                    return;

                var res = await handler.Handle(content);

                if (res.Item1)
                    await chl.BasicAckAsync(args.DeliveryTag, false);
            };

            await chl.BasicConsumeAsync(queueName, false, _routeConsumer);
        }

        #endregion

        #region 主题

        public async Task<bool> PublishTopic(string exchg, string topic, object data)
        {
            var chl = await _pool.AcquireChannelAsync();

            await chl.ExchangeDeclareAsync(exchg, ExchangeType.Topic);

            var msg = data.ToJson();

            var bts = Encoding.UTF8.GetBytes(msg);

            await chl.BasicPublishAsync(exchg, topic, bts);

            return true;
        }

        private AsyncEventingBasicConsumer? _tpcConsumer = null;
        public async Task ConsumerTopic(string exchg, string topic, string queueName = "topic_dft_queue")
        {
            var chl = await _pool.AcquireChannelAsync();

            await chl.ExchangeDeclareAsync(exchg, ExchangeType.Topic);

            if (string.IsNullOrEmpty(queueName)) // await _topicConsumerChannel.QueueDeclareAsync(queueName, true, false, false, null);
                queueName = (await chl.QueueDeclareAsync()).QueueName;
            else
                await chl.QueueDeclareAsync(queueName, true, false, false);

            await chl.QueueBindAsync(queueName, exchg, topic, null); // await _topicConsumerChannel.QueueBindAsync(queueName, exchangeName, topic_patter, null);

            _tpcConsumer = new AsyncEventingBasicConsumer(chl);
            _tpcConsumer.ReceivedAsync += async (sender, args) =>
            {
                var json = Encoding.UTF8.GetString(args.Body.ToArray());
                var content = json.ToObj<MqMsgModel>();

                if (content == null)
                    return;

                if (!MqExtension.MqHandlersDic.TryGetValue(content.MsgType, out var handler)) return;

                var res = await handler.Handle(content);

                if (res.Item1) await chl.BasicAckAsync(args.DeliveryTag, false);
            };

            await chl.BasicConsumeAsync(queueName, false, _tpcConsumer);
        }

        #endregion

    }


    public class ChannelPool
    {
        private bool _disposed = false;
        private readonly RabbitmqOption _mqOpt;
        private IConnection? _connection = null;
        private readonly SemaphoreSlim _semaphore;
        private readonly ILogger<ChannelPool> _logger;
        private readonly ConcurrentQueue<IChannel> _channelPool = new();

        /// <summary>
        /// 创建一个 Channel 池
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="maxChannels"></param>
        public ChannelPool(IOptions<RabbitmqOption> mqOption, ILogger<ChannelPool> logger)
        {
            _logger = logger;
            _mqOpt = mqOption.Value;
            _semaphore = new SemaphoreSlim(_mqOpt.Channels, _mqOpt.Channels); // _connection = BuildConnection();
        }


        private readonly int timeoutMilliseconds = 3000; // 获取 Channel 的超时时间，单位为毫秒
        /// <summary>
        /// 获取一个可用的 Channel
        /// </summary>
        /// <param name="timeoutMilliseconds">获取 Channel 的超时时间，单位为毫秒</param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<IChannel> AcquireChannelAsync(CancellationToken ct = default)
        {
            if (_disposed || _connection == null) throw new ObjectDisposedException(nameof(ChannelPool));

            var acquired = await _semaphore.WaitAsync(timeoutMilliseconds, ct);
            if (!acquired)
            {
                // 如果超时未获取到信号量，说明系统当前过载，直接抛出异常触发限流降级
                throw new TimeoutException($"获取 RabbitMQ Channel 超时 ({timeoutMilliseconds / 1000.0}秒)，系统当前繁忙");
            }

            if (_channelPool.TryDequeue(out var channel))
            {
                return channel;
            }

            var chl = await _connection!.CreateChannelAsync(cancellationToken: ct);

            return chl;
        }

        public void ReleaseChannel(IChannel channel)
        {
            if (_disposed)
            {
                channel.Dispose();
                _semaphore.Release();
                return;
            }

            if (channel.IsOpen)
            {
                _channelPool.Enqueue(channel);
            }
            else
            {
                channel.Dispose();
            }
            _semaphore.Release();
        }

        //// 实现 IAsyncDisposable 接口
        public async Task DisposeAsync()
        {
            if (_disposed || _connection == null) return;

            _disposed = true;

            // 1. 异步清空并销毁池子中所有闲置的 Channel
            while (_channelPool.TryDequeue(out var channel))
            {
                await channel.CloseAsync(); // 使用异步关闭，避免阻塞
                channel.Dispose();
            }

            // 2. 释放信号量资源
            _semaphore.Dispose();

            // 3. 异步关闭底层的共享 Connection
            if (_connection.IsOpen)
            {
                await _connection.CloseAsync(); // 优雅地与服务端断开
            }
            _connection.Dispose();
        }

        #region 私有方法

        private readonly SemaphoreSlim _connectLock = new(1, 1); // ✅ 连接重建锁

        public async Task<IConnection> BuildConnection(CancellationToken clt)
        {
            if (_connection is { IsOpen: true })
                return _connection;

            await _connectLock.WaitAsync(clt);

            try
            {
                if (_connection is { IsOpen: true })
                    return _connection;

                if (_connection != null)
                {
                    _connection.Dispose();
                    _connection = null;
                }

                var connFactory = new ConnectionFactory()
                {
                    Port = _mqOpt.Port,
                    HostName = _mqOpt.Ip,
                    UserName = _mqOpt.Usr,
                    Password = _mqOpt.Pwd,
                    AutomaticRecoveryEnabled = true, // 自动恢复连接
                    NetworkRecoveryInterval = TimeSpan.FromSeconds(6), // 重连恢复间隔
                };

                _connection = await connFactory.CreateConnectionAsync(clt);
                _connection.ConnectionBlockedAsync += _ConnectionBlockedAsync;
                _connection.CallbackExceptionAsync += _CallbackExceptionAsync;
                _connection.ConnectionShutdownAsync += _ConnectionShutdownAsync;
            }
            catch (Exception ex)
            {
                _logger.LogError($"BuildConnection: {ex.Message},{ex.StackTrace}");
            }
            finally
            {
                _connectLock.Release();
            }

            return _connection;  // _semaphore.Release(); 
        }

        private async Task _ConnectionShutdownAsync(object sender, ShutdownEventArgs @event)
        {
            string err = $"Shutdown:{@event.Cause}";
            if (@event.Exception != null)
                err = $"{err}{@event.Exception.Message},{@event.Exception.InnerException},{@event.Exception.StackTrace}";

            _logger.LogError(err);

            // await BuildConnection(CancellationToken.None);
        }

        private Task _CallbackExceptionAsync(object sender, CallbackExceptionEventArgs @event)
        {
            var msg = $"{@event.Exception.Message},{@event.Exception.InnerException},{@event.Exception.StackTrace}";

            _logger.LogError($"Error: {msg}");

            // BuildConnection();

            return Task.CompletedTask;
        }

        private async Task _ConnectionBlockedAsync(object sender, ConnectionBlockedEventArgs @event)
        {
            _logger.LogError($"Blocked: {@event.Reason}"); 
            // await BuildConnection(CancellationToken.None);
        }

        #endregion

    }

    //public class MqInitService : BackgroundService
    //{
    //    private readonly ChannelPool _pool;
    //    private readonly ILogger<MqInitService> _logger;

    //    public MqInitService(ChannelPool pool, ILogger<MqInitService> logger)
    //    {
    //        _pool = pool;
    //        _logger = logger;
    //    }

    //    protected override async Task ExecuteAsync(CancellationToken stopToken)
    //    {
    //        _logger.LogInformation("MQ 连接初始化开始");

    //        try
    //        {
    //            // ✅ 支持取消，SIGTERM 时自动退出
    //            await _pool.BuildConnection(stopToken);
    //            _logger.LogInformation("MQ 连接初始化完成");
    //        }
    //        catch (OperationCanceledException)
    //        {
    //            _logger.LogWarning("MQ 初始化被取消（应用正在关闭）");
    //        }
    //        catch (Exception ex)
    //        {
    //            _logger.LogError(ex, "MQ 连接初始化失败");
    //            // 可选：触发应用退出
    //            throw;
    //        }
    //    }

    //    public override async Task StopAsync(CancellationToken cancellationToken)
    //    {
    //        _logger.LogInformation("应用关闭，清理 MQ 资源...");
    //        await _pool.DisposeAsync();
    //        // ✅ 这里做清理，替代你之前的 DisposeAsync
    //        await base.StopAsync(cancellationToken);
    //    }
    //}
}
