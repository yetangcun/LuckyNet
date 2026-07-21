using System;
using System.Text;
using System.Collections.Generic;
using System.Threading.Channels;

namespace Common.CoreLib.Extension.Channel
{
    /// <summary>
    /// 频道扩展实现
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ChannelExtension<T> : IChannelExtension<T>, IDisposable where T : class
    {
        private readonly Channel<T> _channel;

        public ChannelExtension(Channel<T> channel)
        {
            _channel = channel;
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
