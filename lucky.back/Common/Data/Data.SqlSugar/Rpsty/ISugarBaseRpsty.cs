using Common.CoreLib.Model.Option;
using System;
using System.Text;
using System.Collections.Generic;

namespace Data.SqlSugar.Rpsty
{
    public interface ISugarBaseRpsty<TEntity, TOption> where TEntity : class where TOption : DbDefaultOption
    {
    }
}
