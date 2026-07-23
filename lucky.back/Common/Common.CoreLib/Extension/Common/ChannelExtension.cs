using Common.CoreLib.Model.Option;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Concurrent;
using System.Threading.Channels;

namespace Common.CoreLib.Extension.Common
{
    #region 采用接口面向对象方式

    /// <summary>
    /// 频道扩展实现
    /// </summary>
    public class ChannelExtension
    {
        /// <summary>
        /// 频道字典
        /// </summary>
        private readonly ConcurrentDictionary<string, IChannelSelf> _channels = new ConcurrentDictionary<string, IChannelSelf>();

        private readonly ILogger<ChannelExtension> _logger;

        /// <summary>
        /// 频道配置
        /// </summary>
        private readonly BoundedChannelOptions bopts;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="opts"></param>
        public ChannelExtension(IOptions<ChannelOption> opts, ILogger<ChannelExtension> logger)
        {
            var chlOption = opts.Value;
            bopts = new BoundedChannelOptions(chlOption.Capacity)
            {
                FullMode = BoundedChannelFullMode.Wait,
                AllowSynchronousContinuations = false
            };
            _logger = logger;
        }

        /// <summary>
        /// 获取或创建通道
        /// 核心方法：动态创建或获取通道（不需要改任何已有代码）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="customOpts"></param>
        public Channel<T> GetOrCreate<T>(string key, BoundedChannelOptions? customOpts = null)
        {
            var obj = (ChannelWrapper<T>)_channels.GetOrAdd(key, _ => new ChannelWrapper<T>(Channel.CreateBounded<T>(customOpts ?? bopts)));
            return obj.Chl;
        }

        /// <summary>
        /// 删除通道
        /// </summary>
        /// <param name="key"></param>
        public bool Remove(string key)
        {
            if (_channels.TryRemove(key, out var chl))
            {
                chl.Complete();
                return true;
            }

            return false;
        }

        /// <summary>
        /// 获取通道
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        public Channel<T>? Get<T>(string key)
        {
            if (_channels.TryGetValue(key, out var chl) && chl is ChannelWrapper<T> wapper)
                return wapper.Chl;

            return null;
        }

        /// <summary>
        /// 清空所有通道
        /// </summary>
        public void CompleteAll()
        {
            _logger.LogInformation("Channel CompleteAll Start...");
            foreach (var item in _channels)
            {
                try
                {
                    item.Value.Complete();
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Channel CompleteAll Faild: {ex.Message},{ex.StackTrace},{ex.InnerException}");
                }
            }
            _channels.Clear();
        }
    }

    /// <summary>
    /// 通道定义接口
    /// </summary>
    public interface IChannelSelf
    {
        /// <summary>
        /// 标识通道完成
        /// </summary>
        void Complete();
    }

    /// <summary>
    /// 通道包装类
    /// 在创建通道时，用一个轻量级包装类包装一下
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ChannelWrapper<T> : IChannelSelf
    {
        /// <summary>
        /// 通道
        /// </summary>
        public Channel<T> Chl { get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="channel"></param>
        public ChannelWrapper(Channel<T> channel) => Chl = channel;

        /// <summary>
        /// 标识通道完成
        /// </summary>
        public void Complete() => Chl.Writer.Complete();
    }

    #endregion

    #region 采用反射标识通道完成

    /// <summary>
    /// 频道扩展实现1
    /// </summary>
    public class ChannelExtension1
    {
        /// <summary>
        /// 频道字典
        /// </summary>
        private readonly ConcurrentDictionary<string, object> _channels = new ConcurrentDictionary<string, object>();

        private readonly ILogger<ChannelExtension> _logger;

        /// <summary>
        /// 频道配置
        /// </summary>
        private readonly BoundedChannelOptions bopts;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="opts"></param>
        /// <param name="logger"></param>
        public ChannelExtension1(IOptions<ChannelOption> opts, ILogger<ChannelExtension> logger)
        {
            var chlOption = opts.Value;
            bopts = new BoundedChannelOptions(chlOption.Capacity)
            {
                FullMode = BoundedChannelFullMode.Wait,
                AllowSynchronousContinuations = false
            };
            _logger = logger;
        }

        /// <summary>
        /// 获取或创建通道
        /// 核心方法：动态创建或获取通道（不需要改任何已有代码）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="customOpts"></param>
        public Channel<T> GetOrCreate<T>(string key, BoundedChannelOptions? customOpts = null)
        {
            var obj = (ChannelWrapper<T>)_channels.GetOrAdd(key, _ => Channel.CreateBounded<T>(customOpts ?? bopts));
            return obj.Chl;
        }

        /// <summary>
        /// 删除通道
        /// </summary>
        /// <param name="key"></param>
        public bool Remove(string key)
        {
            if (_channels.TryRemove(key, out var val))
            {
                Complete(val);
                return true;
            }

            return false;
        }

        /// <summary>
        /// 获取通道
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        public Channel<T>? Get<T>(string key)
        {
            if (_channels.TryGetValue(key, out var wrapper) && wrapper is ChannelWrapper<T> typed)
                return typed.Chl;

            return null;
        }

        /// <summary>
        /// 标识通道完成
        /// </summary>
        /// <param name="channel"></param>
        public void Complete(object channel)
        {
            var channelType = channel.GetType();

            // 确保它是泛型类型且基类是 Channel<>
            if (channelType.IsGenericType && channelType.BaseType?.Name == typeof(Channel<>).Name)
            {
                // 获取 Writer 属性 (Channel<T>.Writer)
                var writerProperty = channelType.GetProperty("Writer");
                if (writerProperty != null)
                {
                    // 获取 Writer 实例
                    var writer = writerProperty.GetValue(channel);

                    // 获取 Writer 的 Complete 方法并调用
                    var completeMethod = writer?.GetType().GetMethod("Complete");

                    // 【核心修复】：必须传入一个包含 null 的 object 数组，
                    // 对应 Complete(Exception? error = null) 中的 error 参数
                    completeMethod?.Invoke(writer, new object[] { null! });
                }
            }
        }

        /// <summary>
        /// 优雅关闭所有通道（通知消费者不再有数据写入）
        /// </summary>
        public void CompleteAll()
        {
            foreach (var chl in _channels)
            {
                try
                {
                    Complete(chl.Value);
                }
                catch (Exception ex)
                {
                    // 记录日志：防止某一个通道的关闭失败影响其他通道的清理
                    Console.WriteLine($"⚠️ 关闭通道 [{chl.Key}] 时发生异常: {ex.Message}");
                }
            }
        }
    }

    #endregion
}
