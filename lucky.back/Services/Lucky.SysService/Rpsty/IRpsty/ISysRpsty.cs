using Data.EFCore.Rpsty;
using Lucky.BaseModel.Model.Entity;

namespace Lucky.SysService.Rpsty.IRpsty
{
    public interface ISysRpsty<TEntity,TKey> : ICommonRpsty<TEntity, TKey> where TEntity : BaseCommonEntity<TKey>
    {
    }
}
