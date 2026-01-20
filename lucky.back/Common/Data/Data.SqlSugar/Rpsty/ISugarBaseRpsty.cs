using Common.CoreLib.Model.Option;

namespace Data.SqlSugar.Rpsty
{
    public interface ISugarBaseRpsty<TEntity, TOption> where TEntity : class where TOption : DbDefaultOption
    {
    }
}
