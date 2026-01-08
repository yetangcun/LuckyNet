using Common.CoreLib.Model.Option;
using Lucky.BaseModel.Enum;
using SqlSugar;
using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Data.SqlSugar.Rpsty
{
    public class SugarBaseRpsty<TEntity, TOption> : SimpleClient<TEntity> where TEntity : class, new() where TOption : DbDefaultOption
    {
        private readonly TOption _option;
        private readonly ILogger _logger;

        public SugarBaseRpsty(TOption option, ILogger logger)
        {
            _option = option;
            _logger = logger;
        }

        /// <summary>
        /// 统一获取SqlSugarDbClient
        /// </summary>
        protected ISqlSugarClient GetSqlSugarDbClient(DatabaseType? dbType = null, string? connString = null, bool isRead = false)
        {
            var realDbType = dbType ?? _option.DbType;
            var realConnString = connString ?? (isRead ? _option.SlaveConnString : _option.MasterConnString);
            var connCfg = new ConnectionConfig()
            {
                ConnectionString = realConnString,
                DbType = (DbType)realDbType,
                IsAutoCloseConnection = true
            };
            var client = new SqlSugarClient(connCfg, act =>
            {
                act.Aop.OnLogExecuted = (sql, pars) =>
                {
                    var sqlStr = UtilMethods.GetNativeSql(sql, pars);
                    Console.WriteLine(sqlStr);
                    _logger.LogDebug(sqlStr);
                };
            });
            return client;
        }
    }
}
