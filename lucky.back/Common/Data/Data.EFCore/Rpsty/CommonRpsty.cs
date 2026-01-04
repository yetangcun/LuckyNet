using Common.CoreLib.Model.Option;
using Data.EFCore.Cxt;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Data.EFCore.Rpsty
{
    /// <summary>
    /// 通用仓储实现
    /// </summary>
    public class CommonRpsty<TCxt, TOpt> : ICommonRpsty where TCxt : CommonCxt where TOpt : EfcoreOption
    {
        private readonly TOpt _opt;
        private readonly TCxt _dbCxt;
        private readonly ILogger _logger;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="cxt"></param>
        /// <param name="opts"></param>
        /// <param name="logger"></param>
        public CommonRpsty(ICommonCxt cxt, IOptions<TOpt> opts, ILogger logger)
        {
            _opt = opts.Value;  
            _logger = logger;
            _dbCxt = cxt.GetDbCxt<TCxt, TOpt>(opts.Value);
        }

        /// <summary>
        /// 设置是否只读
        /// </summary>
        /// <param name="isReadOnly"></param>
        public void SetDbReadOnly(bool isReadOnly = false)
        {
            _opt.IsReadOnly = isReadOnly;
        }

        /// <summary>
        /// 同步事务
        /// </summary>
        /// <param name="act"></param>
        public bool ExeTransaction(Action act)
        {
            using (var tran = _dbCxt.Database.BeginTransaction())
            {
                try
                {
                    act();
                    tran.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "事务执行失败");
                }
                finally
                {
                    tran.Rollback();
                }
            }
            return false;
        }

        /// <summary>
        /// 异步事务
        /// </summary>
        /// <param name="act"></param>
        public async Task<bool> ExeTransactionAsync(Func<Task> act)
        {
            using (var tran = await _dbCxt.Database.BeginTransactionAsync())
            {
                try
                {
                    await act();
                    await tran.CommitAsync();
                    return true;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "异步事务执行失败");
                }
                finally
                {
                    await tran.RollbackAsync();
                }
            }
            return false;
        }
    }
}
