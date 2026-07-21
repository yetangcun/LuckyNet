using System;
using System.Text;
using System.Collections.Generic;

namespace Common.CoreLib.Extension.Channel
{
    /// <summary>
    /// 频道扩展接口定义
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IChannelExtension<T> where T : class
    {
    }

    /// <summary>
    /// 频道消费者接口定义
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IChannelConsumer<T> where T : class
    {
        /// <summary>
        /// 消费消息
        /// </summary>
        /// <param name="batch">消息</param>
        /// <param name="cancellationToken">消息</param>
        Task Consume(IReadOnlyList<T> batch, CancellationToken? cancellationToken);
    }
}
