using Microsoft.Extensions.Hosting;

namespace Common.CoreLib.Extension.Common
{
    /// <summary>
    /// 退出清理扩展
    /// </summary>
    public class QuitOptExtension : IHostedService
    {
        private readonly IHostApplicationLifetime _appLifetime;
        private readonly ChannelExtension _channelExtension; // 注入你的通道管理器

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="appLifetime"></param>
        /// <param name="channelExtension"></param>
        public QuitOptExtension(IHostApplicationLifetime appLifetime, ChannelExtension channelExtension)
        {
            _appLifetime = appLifetime;
            _channelExtension = channelExtension;
        }

        private void OnApplicationStopping()
        {
            Console.WriteLine("🛑 应用正在关闭，开始执行清理任务...");

            // 在这里调用你的清理方法，例如通知所有通道完成写入
            _channelExtension.CompleteAll();

            // 可以在这里添加其他清理逻辑，如：
            // - 关闭数据库连接
            // - 释放非托管资源
            // - 记录应用关闭日志
        }

        /// <summary>
        /// 启动
        /// </summary>
        /// <param name="cancellationToken"></param>
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _appLifetime.ApplicationStopping.Register(OnApplicationStopping);
            return Task.CompletedTask;
        }

        /// <summary>
        /// 停止
        /// </summary>
        /// <param name="cancellationToken"></param>
        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
