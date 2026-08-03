using Common.CoreLib.Extension.Common;
using Lucky.SysModel;

//using lucky.admin.Extensions.Filters;
using Lucky.SysModel.Entity;
using Lucky.SysModel.Model.Output;
using Lucky.SysService.Rpsty.IRpsty;
using System.Threading.Channels;

namespace lucky.admin.Extensions.Handler
{
    /// <summary>
    /// 频道默认处理
    /// </summary>
    public class InitHandler
    {
        private readonly Channel<SysLog> _channel;
        private readonly ILogger<InitHandler> _logger;
        //private readonly ISysRpsty<SysLog, long> _logRpsty;
        private readonly IServiceProvider _prd;


        /// <summary>
        /// 构造函数
        /// </summary>
        public InitHandler(ILogger<InitHandler> logger, ChannelExtension channel, IServiceProvider prd)
        {
            //_logRpsty = logRpsty;
            _prd = prd;
            _logger = logger;
            _channel = channel.GetOrCreate<SysLog>("SysLogChannel");
        }

        private Task? _tsk = null;

        /// <summary>
        /// 启动
        /// </summary>
        /// <returns></returns>
        public async Task Start()
        { 
            if (_tsk == null)
            {
                var lst = new List<SysLog>();
                var runCounts = 0;
                _tsk = new Task(async x =>
                {
                    while (true)
                    {
                        try
                        {
                            _channel.Reader.TryRead(out var log);

                            if (log != null) 
                                lst.Add(log);
                            else 
                                await Task.Delay(1000);

                            if (lst.Count >= 100 || (runCounts > 9 && lst.Count > 0))
                            {
                                using var scope = _prd.CreateScope();
                                var _logRpsty = scope.ServiceProvider.GetRequiredService<ISysRpsty<SysLog, long>>();
                                await _logRpsty.AddRangeAsync(lst);
                                lst.Clear(); runCounts = 0;
                            }
                            else 
                                runCounts++;

                            if (runCounts > 100)
                            {
                                runCounts = 0;
                                await Task.Delay(6000);
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "ChannelDftHandler");
                        }
                    }
                },
                TaskCreationOptions.LongRunning);

                _tsk.Start();
            }
        }

        /// <summary>
        /// 初始化缓存
        /// </summary>
        /// <param name="cacheType"></param>
        /// <returns></returns>
        public async Task InitCache(string cacheType = "")
        {
            using var scope = _prd.CreateScope();

            if (string.IsNullOrWhiteSpace(cacheType))
            {
                await LoadRoleCache(scope);
            }
            else
            {
                switch (cacheType)
                {
                    case "RoleCache":
                        {
                            await LoadRoleCache(scope);
                        }
                        break;
                    default: break;
                }
            }
        }

        #region 初始化缓存
        /// <summary>
        /// 加载角色缓存  
        /// </summary>
        /// <param name="scope"></param>
        private async Task<bool> LoadRoleCache(IServiceScope? scope = null)
        {
            if (scope == null)
            {
                scope = _prd.CreateScope();
            }
            var rpsty = scope.ServiceProvider.GetRequiredService<ISysRpsty<SysRole, int>>();
            var roles = await rpsty.GetListAsync(x => !x.IsDel && x.Status == 1);
            if (roles != null && roles.Any())
            {
                SysModelModule.RoleCache.Clear();
                SysModelModule.RoleCache.AddRange(roles.Select(x => new SysRoleOutput()
                {
                    id = x.Id,
                    name = x.Name,
                    roleType = x.RoleType
                }));
            }
            scope.Dispose(); return true;
        }
        #endregion

        /// <summary>
        /// 停止
        /// </summary>
        public void Stop()
        {
            _tsk?.Dispose();
        }
    }
}
