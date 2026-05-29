using Common.CoreLib.Model.Option;
using Data.EFCore.Rpsty;
using Lucky.BaseModel.Model.Entity;
using Lucky.SysService.Cxt;
using Lucky.SysService.Rpsty.IRpsty;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Lucky.SysService.Rpsty
{
    public class SysRpsty<TEntity,TKey> : CommonRpsty<SysCxt, TEntity, SysDbOption, TKey>, ISysRpsty<TEntity, TKey> where TEntity : BaseCommonEntity<TKey>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="cxt"></param>
        /// <param name="opts"></param>
        /// <param name="logger"></param>
        public SysRpsty(ISysCxt cxt, IOptionsSnapshot<SysDbOption> opt, ILogger<SysRpsty<TEntity, TKey>> logger) : base(cxt, opt.Value, logger) { }
    }
}
