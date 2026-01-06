using Common.CoreLib.Model.Option;
using Data.EFCore.Rpsty;
using Lucky.SysService.Cxt;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Lucky.SysService.Rpsty.IRpsty;

namespace Lucky.SysService.Rpsty
{
    public class SysRpsty : CommonRpsty<SysCxt, SysDbOption>, ISysRpsty
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="cxt"></param>
        /// <param name="opts"></param>
        /// <param name="logger"></param>
        public SysRpsty(ISysCxt cxt, IOptionsSnapshot<SysDbOption> opt, ILogger<SysRpsty> logger) : base(cxt, opt.Value, logger) { }
    }
}
