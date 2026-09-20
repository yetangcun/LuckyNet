using Common.CoreLib.Model.Option;
using Lucky.BaseModel.Model;
using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Lucky.BaseModel.Enum;
using Microsoft.Extensions.Options;
using Common.CoreLib.Extension.Common;

namespace Prtcl.Kfk
{
    public class KfkService : IKfkService
    {
        private readonly KfkOption _kfkOpt;
        private readonly ILogger<KfkService> _logger;

        private IProducer<string, string>? _prd;
        private IConsumer<Ignore, string>? _cns;


        private readonly ProducerConfig _prdCfg;
        private readonly ConsumerConfig _cnsCfg;

        /// <summary>
        /// Constructor
        /// 构造函数
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="kfkOpt"></param>
        public KfkService(ILogger<KfkService> logger, IOptions<KfkOption> kfkOpt)
        {
            _logger = logger;
            _kfkOpt = kfkOpt.Value;

            var server = $"{_kfkOpt.Host}:{_kfkOpt.Port}";

            switch (_kfkOpt.Rtype)
            {
                case 0: // 生产者或消费者
                    _prdCfg = new ProducerConfig()
                    {
                        BootstrapServers = server
                    };

                    _cnsCfg = new ConsumerConfig()
                    {
                        BootstrapServers = server,
                        EnableAutoCommit = false, // 关闭自动确认
                        AllowAutoCreateTopics = false, // 禁止自动创建主题
                        SessionTimeoutMs = 6000,
                        StatisticsIntervalMs = 4000,
                        AutoOffsetReset = AutoOffsetReset.Earliest,
                        EnablePartitionEof = true,
                        MaxPollIntervalMs = 300000, // 最大轮询间隔 默认5分钟 如果出现重复消费可以设置更大一些
                        GroupId = _kfkOpt.CnsGrp
                    };
                    break;
                case 1: // 生产者
                    _prdCfg = new ProducerConfig()
                    {
                        BootstrapServers = server
                    };
                    break;
                case 2: // 消费者
                    _cnsCfg = new ConsumerConfig()
                    {
                        BootstrapServers = server,
                        EnableAutoCommit = false, // 关闭自动确认
                        AllowAutoCreateTopics = false, // 禁止自动创建主题
                        SessionTimeoutMs = 6000,
                        StatisticsIntervalMs = 4000,
                        AutoOffsetReset = AutoOffsetReset.Earliest,
                        EnablePartitionEof = true,
                        MaxPollIntervalMs = 300000, // 最大轮询间隔 默认5分钟 如果出现重复消费可以设置更大一些
                        GroupId = _kfkOpt.CnsGrp
                    };
                    break;
            }
        }

        public void Init()
        {
            switch (_kfkOpt.Rtype)
            {
                case 0:
                    _prd = new ProducerBuilder<string, string>(_prdCfg)
                        .SetErrorHandler((_, er) =>
                        {
                            Console.WriteLine($"生产异常回调: {er}");
                        })
                        .Build();

                    _cns = new ConsumerBuilder<Ignore, string>(_cnsCfg)
                        .SetErrorHandler((_, msg) => // 出现异常回调
                        {
                            Console.WriteLine($"消费异常回调: {msg}");
                        })
                        .SetStatisticsHandler((_, msg) =>  // 统计回调
                        {
                            // Console.WriteLine($"统计回调: {msg}");
                        })
                        .SetPartitionsAssignedHandler((_, msg) =>
                        {
                            Console.WriteLine($"分区挂载分配回调: {msg}");
                        })
                        .SetPartitionsLostHandler((_, msg) =>
                        {
                            Console.WriteLine($"分区丢失回调: {msg}");
                        })
                        .SetPartitionsRevokedHandler((_, msg) =>
                        {
                            Console.WriteLine($"分区取消失效回调: {msg}");
                        })
                        .SetOffsetsCommittedHandler((_, msg) =>
                        {
                            Console.WriteLine($"偏移量确认回调: {msg}");
                        })
                        .SetLogHandler((_, msg) =>
                        {
                            Console.WriteLine($"日志记录回调: {msg}");
                        })
                        .Build();
                    break;
                case 1:
                    _prd = new ProducerBuilder<string, string>(_prdCfg)
                        .SetErrorHandler((_, er) =>
                        {
                            Console.WriteLine($"生产异常回调: {er}");
                        })
                        .Build();
                    break;
                case 2:
                    _cns = new ConsumerBuilder<Ignore, string>(_cnsCfg)
                        .SetErrorHandler((_, msg) => // 出现异常回调
                        {
                            Console.WriteLine($"消费异常回调: {msg}");
                        })
                        .SetStatisticsHandler((_, msg) =>  // 统计回调
                        {
                            // Console.WriteLine($"统计回调: {msg}");
                        })
                        .SetPartitionsAssignedHandler((_, msg) =>
                        {
                            Console.WriteLine($"分区挂载分配回调: {msg}");
                        })
                        .SetPartitionsLostHandler((_, msg) =>
                        {
                            Console.WriteLine($"分区丢失回调: {msg}");
                        })
                        .SetPartitionsRevokedHandler((_, msg) =>
                        {
                            Console.WriteLine($"分区取消失效回调: {msg}");
                        })
                        .SetOffsetsCommittedHandler((_, msg) =>
                        {
                            Console.WriteLine($"偏移量确认回调: {msg}");
                        })
                        .SetLogHandler((_, msg) =>
                        {
                            Console.WriteLine($"日志记录回调: {msg}");
                        })
                        .Build();
                    break;
            }
        }

        #region 生产者
        public async Task PubAsync(KfkMsgModel msg, string? host = null, int? port = 0, string? tpic = null)
        {
            if (string.IsNullOrWhiteSpace(tpic))
                tpic = _kfkOpt.Tpc;

            if (string.IsNullOrWhiteSpace(host))
            {
                host = _kfkOpt.Host;
                port = _kfkOpt.Port;
            }

            if (msg.Pid == null)
                msg.Pid = _kfkOpt.DftPid;

            var cfg = new ProducerConfig()
            {
                BootstrapServers = $"{host}:{port}"
            };

            using var prd = new ProducerBuilder<string, string>(cfg).Build();
            var data = new Message<string, string>()
            {
                Key = msg.Sid!,
                Value = msg.ToJson()
            };

            try
            {
                await prd.ProduceAsync(tpic, data); // 生产者生产消息finally
            }
            finally
            {
                // 确保消息真正发出，最多等 3 秒
                prd.Flush(TimeSpan.FromSeconds(3));
            }
        }

        public async Task PubAsync<T>(T? data, MsgType msgType, string tpc, int? pid = 0)
        {
            if (string.IsNullOrWhiteSpace(tpc) || _prdCfg == null)
                return;

            if (pid == null || pid < 0)
                pid = _kfkOpt.DftPid;

            var sid = IdGreator.GetNxtId().ToString();
            var msg = new KfkMsgModel()
            {
                MsgType = msgType,
                Tpc = tpc,
                Pid = pid,
                Msg = data.ToJson()
            };

            using var prd = new ProducerBuilder<string, string>(_prdCfg).Build();

            var message = new Message<string, string>()
            {
                Key = sid,
                Value = msg.ToJson()
            };

            try
            {
                await prd.ProduceAsync(tpc, message); // 生产者生产消息
            }
            finally
            {
                // 确保消息真正发出，最多等 3 秒
                prd.Flush(TimeSpan.FromSeconds(3));
            }
        }


        public async Task PublishAsync(KfkMsgModel msg)
        {
            if (string.IsNullOrWhiteSpace(msg.Tpc) || _prd == null)
                return;

            if (msg.Pid == null)
                msg.Pid = _kfkOpt.DftPid;

            var sid = msg.Sid; msg.Sid = null;
            var data = new Message<string, string>()
            {
                Key = sid!,
                Value = msg.ToJson()
            };

            await _prd.ProduceAsync(msg.Tpc, data); // 生产者生产消息
        }


        public async Task PublishAsync<T>(T? data, MsgType msgType, string tpc, int? pid = 0)
        {
            if (string.IsNullOrWhiteSpace(tpc) || _prd == null)
                return;

            if (pid == null || pid < 0)
                pid = _kfkOpt.DftPid;

            var sid = IdGreator.GetNxtId().ToString();
            var msg = new KfkMsgModel()
            {
                MsgType = msgType,
                Tpc = tpc,
                Pid = pid,
                Msg = data.ToJson()
            };

            var message = new Message<string, string>()
            {
                Key = sid,
                Value = msg.ToJson()
            };

            

            await _prd.ProduceAsync(tpc, message); // 生产者生产消息
        }
        #endregion

        #region 消费者

        private bool _isDisposed = false;

        public async Task ConsumeAsync<T>(IEnumerable<string> tpcs, IMqConsumerHdl hdl, string? host = null, int? port = 0, CancellationToken cancelToken = default)
        {
            if (tpcs == null || tpcs.Count() == 0)
                return;

            if (string.IsNullOrWhiteSpace(host) || port == null)
            {
                port = _kfkOpt.Port;
                host = _kfkOpt.Host;
            }

            var cns = new ConsumerBuilder<Ignore, string>(_cnsCfg)
                        .SetErrorHandler((_, msg) => // 出现异常回调
                        {
                            Console.WriteLine($"消费异常回调: {msg}");
                        })
                        .SetStatisticsHandler((_, msg) =>  // 统计回调
                        {
                            // Console.WriteLine($"统计回调: {msg}");
                        })
                        .SetPartitionsAssignedHandler((_, msg) =>
                        {
                            Console.WriteLine($"分区挂载分配回调: {msg}");
                        })
                        .SetPartitionsLostHandler((_, msg) =>
                        {
                            Console.WriteLine($"分区丢失回调: {msg}");
                        })
                        .SetPartitionsRevokedHandler((_, msg) =>
                        {
                            Console.WriteLine($"分区取消失效回调: {msg}");
                        })
                        .SetOffsetsCommittedHandler((_, msg) =>
                        {
                            Console.WriteLine($"偏移量确认回调: {msg}");
                        })
                        .SetLogHandler((_, msg) =>
                        {
                            Console.WriteLine($"日志记录回调: {msg}");
                        })
                        .Build();

            cns.Subscribe(tpcs);

            var tsk = new Task(async () =>
            {
                while (!_isDisposed)
                {
                    try
                    {
                        var cnsRes = cns.Consume(cancelToken); // 读取消息
                        if (cnsRes.IsPartitionEOF)
                        {
                            // 没有消费可消费
                            await Task.Delay(10);
                            continue;
                        }

                        var offsets = cnsRes.Offset.Value; // 每条消息的偏移量
                        KfkMsgModel? msgObj = cnsRes.Message.Value.ToObj<KfkMsgModel>(); // 消息反序列化
                        msgObj!.Sid = cnsRes.Message.Key.ToString(); // 设置消息唯一会话Id
                        // Console.WriteLine($"consume--{cnsRes.Topic}--{cnsRes.Partition.Value}--{cnsRes.Message.Value}");
                        if (msgObj != null)
                        {
                            var res = await hdl.hdl(msgObj);  // 取得消息开始业务消费
                            if (res.Status) cns.Commit(cnsRes);  // 消息消费完成确认
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"消费1异常: {ex.Message}");
                    }
                }

            }, TaskCreationOptions.LongRunning);

            tsk.Start();

            await Task.Delay(32);
        }


        public async Task ConsumerAsync(IEnumerable<string> tpcs, IMqConsumerHdl hdl, CancellationToken cancelToken = default)
        {
            if (tpcs == null || tpcs.Count() == 0 || _cns == null)
                return;

            _cns.Subscribe(tpcs);

            var tsk = new Task(async () =>
            {
                while (!_isDisposed)
                {
                    try
                    {
                        var cnsRes = _cns.Consume(cancelToken); // 读取消息
                        if (cnsRes.IsPartitionEOF)
                        {
                            // 没有消费可消费
                            await Task.Delay(10);
                            continue;
                        }

                        var offsets = cnsRes.Offset.Value; // 每条消息的偏移量
                        var msgObj = cnsRes.Message.Value.ToObj<KfkMsgModel>(); // 消息反序列化
                        if (msgObj != null)
                        {
                            msgObj.Sid = cnsRes.Message.Key.ToString(); // 设置消息唯一会话Id Console.WriteLine($"{cnsRes.Topic}--{cnsRes.Partition.Value}--{cnsRes.Message.Value}");
                            var res = await hdl.hdl(msgObj);  // 取得消息开始业务消费
                            if (res.Status) _cns.Commit(cnsRes);  // 消息消费完成确认
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"消费1异常: {ex.Message}");
                    }
                }

            }, TaskCreationOptions.LongRunning);

            tsk.Start(); await Task.Delay(32);
        }


        public async Task ConsumerAsync(Dictionary<string, List<int>> tpcs, IMqConsumerHdl hdl, CancellationToken cancelToken = default)
        {
            if (tpcs == null || tpcs.Count() == 0 || _cns == null || _cns.Handle == null)
                return;

            var tsk = new Task(async () =>
            {
                var tps = new List<TopicPartitionOffset>();
                foreach (var tp in tpcs)
                {
                    foreach (var vl in tp.Value)
                    {
                        tps.Add(new TopicPartitionOffset(new TopicPartition(tp.Key, vl), offset: Offset.Stored));
                    }
                }
                _cns.Assign(tps);

                while (!_isDisposed)
                {
                    try
                    {
                        var cnsRes = _cns.Consume(cancelToken); // 读取消息
                        if (cnsRes.IsPartitionEOF)
                        {
                            // 没有消费可消费
                            await Task.Delay(10);
                            continue;
                        }

                        var offsets = cnsRes.Offset.Value; // 每条消息的偏移量
                        var msgObj = cnsRes.Message.Value.ToObj<KfkMsgModel>(); // 消息反序列化
                        if (msgObj != null)
                        {
                            msgObj.Sid = cnsRes.Message.Key.ToString(); // 设置消息唯一会话Id Console.WriteLine($"{cnsRes.Topic}--{cnsRes.Partition.Value}--{cnsRes.Message.Value}");
                            var res = await hdl.hdl(msgObj);  // 取得消息开始业务消费
                            if (res.Status) _cns.Commit(cnsRes);  // 消息消费完成确认
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"消费1异常: {ex.Message}");
                    }
                }

            }, TaskCreationOptions.LongRunning);

            tsk.Start(); await Task.Delay(32);
        }

        #endregion

        /// <summary>
        /// 资源释放
        /// </summary>
        public void Dispose()
        {
            _isDisposed = true;

            if (_cns != null)
            {
                _cns.Close();
                _cns.Dispose();
            }

            if (_prd != null)
            {
                _prd.Dispose();
            }
        }

    }
}
