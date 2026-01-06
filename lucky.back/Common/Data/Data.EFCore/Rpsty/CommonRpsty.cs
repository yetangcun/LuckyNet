using Common.CoreLib.Model.Option;
using Data.EFCore.Cxt;
using Lucky.BaseModel.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Data.EFCore.Rpsty
{
    /// <summary>
    /// 通用仓储实现
    /// </summary>
    public class CommonRpsty<TCxt, TOpt> : ICommonRpsty where TCxt : CommonCxt where TOpt : DbDefaultOption
    {
        private readonly TOpt _opt;
        protected readonly TCxt _dbCxt;
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

        #region 分页查询

        public async Task<(int, List<T>)> GetPageListAsync<T>(Expression<Func<T, bool>>? where, PageInfo page) where T : class
        {
            var query = _dbCxt.Set<T>().AsQueryable();
            if (where != null)
                query = query.Where(where);

            #region 排序

            if (!string.IsNullOrEmpty(page.Sort))
            {
                IOrderedQueryable<T>? ordered = null;
                var arrs = page.Sort.Split(',', StringSplitOptions.RemoveEmptyEntries);
                var lens = arrs.Length;
                if (!string.IsNullOrWhiteSpace(page.SortType) && page.SortType.Equals("asc", StringComparison.OrdinalIgnoreCase))
                {
                    for (var i = 0; i < lens; i++)
                    {
                        var itm = arrs[i];
                        var prop = typeof(T).GetProperty(itm, BindingFlags.Public | BindingFlags.IgnoreCase);  // ✅ 新增：验证字段是否存在
                        if (prop == null)
                            continue;

                        if (i == 0)
                            ordered = query.OrderBy(x => EF.Property<object>(x, itm));
                        else
                            ordered = ordered!.ThenBy(x => EF.Property<object>(x, itm));
                    }
                }
                else
                {
                    for (var i = 0; i < lens; i++)
                    {
                        var itm = arrs[i];
                        var prop = typeof(T).GetProperty(itm, BindingFlags.Public | BindingFlags.IgnoreCase);  // ✅ 新增：验证字段是否存在
                        if (prop == null)
                            continue;

                        if (i == 0)
                            ordered = query.OrderByDescending(x => EF.Property<object>(x, itm));
                        else
                            ordered = ordered!.ThenByDescending(x => EF.Property<object>(x, itm));
                    }
                }

                if (ordered != null)
                    query = ordered;
            }
            else if (typeof(T).GetProperty("CreateTime") != null)
                query = query.OrderByDescending(x => EF.Property<object>(x, "CreateTime"));
            else if (typeof(T).GetProperty("Id") != null)
                query = query.OrderBy(x => EF.Property<object>(x, "Id"));

            #endregion

            var nums = await query.CountAsync(); // 获取总记录数
            var datas = await query                // 获取分页数据
                .AsNoTracking()
                .Skip(page.Skips)
                .Take(page.PageSize)
                .ToListAsync();

            return (nums, datas);
        }

        #endregion

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
