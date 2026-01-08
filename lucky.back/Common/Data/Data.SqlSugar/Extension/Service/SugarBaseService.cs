using System;
using System.Text;
using Data.SqlSugar.Rpsty;
using System.Collections.Generic;
using Common.CoreLib.Model.Option;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;

namespace Data.SqlSugar.Extension.Service
{
    public class SugarBaseService<TEntity, TOption> : SugarBaseRpsty<TEntity, TOption> where TEntity : class, new() where TOption : DbDefaultOption
    {
        public SugarBaseService(TOption option, ILogger logger) : base(option, logger) { }
    }
}
