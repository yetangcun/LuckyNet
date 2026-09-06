using Prtcl.Rabbitmq;

namespace lucky.admin.Extensions.Handler
{
    /// <summary>
    /// 初始化服务
    /// </summary>
    public class InitService : BackgroundService
    {
        private readonly ChannelPool _pool;
        private readonly ILogger<InitService> _logger;

        /// <summary>
        /// 初始化服务
        /// </summary>
        /// <param name="pool"></param>
        /// <param name="logger"></param>
        public InitService(ChannelPool pool, ILogger<InitService> logger)
        {
            _pool = pool;
            _logger = logger;
        }

        /// <summary>
        /// 执行后台服务
        /// </summary>
        /// <param name="cancelToken"></param>
        protected override async Task ExecuteAsync(CancellationToken cancelToken)
        {
            try
            {
                await _pool.BuildConnection(CancellationToken.None);
            }
            catch (Exception ex)
            {
                _logger.LogError($"InitService ExecuteAsync: {ex.Message},{ex.StackTrace},{ex.InnerException}");
            }
        }

        /// <summary>
        /// 停止后台服务
        /// </summary>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        public override async Task StopAsync(CancellationToken cancelToken)
        {
            try
            {
                await _pool.DisposeAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"InitService StopAsync: {ex.Message},{ex.StackTrace},{ex.InnerException}");
            }
            await base.StopAsync(cancelToken);
        }
    }
}
