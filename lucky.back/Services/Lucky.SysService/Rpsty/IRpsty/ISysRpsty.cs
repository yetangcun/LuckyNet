using Data.EFCore.Rpsty;

namespace Lucky.SysService.Rpsty.IRpsty
{
    public interface ISysRpsty<TEntity> : ICommonRpsty<TEntity> where TEntity : class
    {
    }
}
