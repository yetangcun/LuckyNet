using Common.CoreLib.Model.Option;
using Data.EFCore.Rpsty;
using Lucky.SysService.Cxt;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Lucky.SysService.Rpsty.IRpsty;

namespace Lucky.SysService.Rpsty
{
    public class SysUserRpsty : CommonRpsty<SysCxt, EfcoreOption>, ISysUserRpsty
    {
        private readonly SysCxt _cxt;
        readonly ILogger<SysUserRpsty> _logger;
        private readonly EfcoreOption _sysOpt; 

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="cxt"></param>
        /// <param name="opts"></param>
        /// <param name="logger"></param>
        public SysUserRpsty(ISysCxt cxt, IOptions<EfcoreOption> opts, ILogger<SysUserRpsty> logger) : base(cxt, opts, logger)
        {
            _logger = logger;
            _sysOpt = opts.Value;
            _cxt = cxt.GetDbCxt<SysCxt, EfcoreOption>(_sysOpt);
        }
    }
}
