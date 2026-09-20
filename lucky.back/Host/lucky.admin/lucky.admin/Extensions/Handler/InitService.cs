using Data.ClkHouse.clk;
using Prtcl.Kfk;
using Prtcl.Rabbitmq;

namespace lucky.admin.Extensions.Handler
{
    /// <summary>
    /// 初始化服务
    /// </summary>
    public class InitService : BackgroundService
    {
        private readonly ChannelPool _pool;
        private readonly IClkService _clkService;
        private readonly IKfkService _kfkService;
        private readonly ILogger<InitService> _logger;

        /// <summary>
        /// 初始化服务
        /// </summary>
        /// <param name="pool"></param>
        /// <param name="clkService"></param>
        /// <param name="kfkService"></param>
        /// <param name="logger"></param>
        public InitService(ChannelPool pool, IClkService clkService, IKfkService kfkService, ILogger<InitService> logger)
        {
            _pool = pool;
            _logger = logger;
            _clkService = clkService;
            _kfkService = kfkService;
        }

        /// <summary>
        /// 执行后台服务
        /// </summary>
        /// <param name="cancelToken"></param>
        protected override async Task ExecuteAsync(CancellationToken cancelToken)
        {
            try
            {
                InitClk();  // 初始化ClickHouse、注册model
                await _pool.BuildConnection(CancellationToken.None); // 初始化数据库连接池
                _kfkService.Init();  // 初始化kafka
            }
            catch (Exception ex)
            {
                _logger.LogError($"InitService ExecuteAsync: {ex.Message},{ex.StackTrace},{ex.InnerException}");
            }
        }

        /// <summary>
        /// clickhouse 模型初始化注册
        /// </summary>
        private void InitClk()
        {
            _clkService.RegisterAsync<Controllers.sys.SysUserController.tst>();
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
