using Common.CoreLib.Model.Option;
using Data.EFCore.Cxt;
using System.Reflection;
using Lucky.BaseModel.Model;
using System.Linq.Expressions;
using Lucky.BaseModel.Model.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

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
        public CommonRpsty(ICommonCxt cxt, TOpt opt, ILogger logger)
        {
            _opt = opt;  
            _logger = logger;
            _dbCxt = cxt.GetDbCxt<TCxt, TOpt>(opt);
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
        /// 添加
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        public async Task<T> AddAsync<T>(T entity) where T : class
        {
            var obj = await _dbCxt.AddAsync(entity);
            await _dbCxt.SaveChangesAsync();
            return obj.Entity;
        }

        /// <summary>
        /// 批量添加
        /// </summary>
        public async Task<int> AddRangeAsync<T>(List<T> entities) where T : class
        {
            await _dbCxt.AddRangeAsync(entities);
            var nums = await _dbCxt.SaveChangesAsync();
            return nums;
        }

        /// <summary>
        /// 删除
        /// </summary>
        public async Task<int> DeleteAsync<T>(T entity) where T : class
        {
            _dbCxt.Remove(entity);
            var nums = await _dbCxt.SaveChangesAsync();
            return nums;
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        public async Task<int> DeleteRangeAsync<T>(List<T> entities) where T : class
        {
            _dbCxt.RemoveRange(entities);
            var nums = await _dbCxt.SaveChangesAsync();
            return nums;
        }

        /// <summary>
        /// 修改
        /// </summary>
        public async Task<int> UpdateAsync<T>(T entity) where T : class
        {
            _dbCxt.Update(entity);
            var nums = await _dbCxt.SaveChangesAsync();
            return nums;
        }

        /// <summary>
        /// 批量修改
        /// </summary>
        public async Task<int> UpdateRangeAsync<T>(List<T> entities) where T : class
        {
            _dbCxt.UpdateRange(entities);
            var nums = await _dbCxt.SaveChangesAsync();
            return nums;
        }

        /// <summary>
        /// 条件查询实体列表
        /// </summary>
        public async Task<List<T>> GetListAsync<T>(Expression<Func<T, bool>>? where) where T : class
        {
            _opt.IsReadOnly = true;
            var query = _dbCxt.Set<T>().AsNoTracking();
            if (where != null)
                query = query.Where(where);
            var datas = await query.ToListAsync();
            return datas;
        }

        /// <summary>
        /// 条件查询结果列表
        /// </summary>
        public async Task<List<TResult>> GetListAsync<T, TResult>(Expression<Func<T, bool>>? where, Expression<Func<T, TResult>> selectors) where T : class
        {
            _opt.IsReadOnly = true;
            var query = _dbCxt.Set<T>().AsNoTracking();
            if (where != null)
                query = query.Where(where);
            var datas = await query.Select(selectors).ToListAsync();
            return datas;
        }

        /// <summary>
        /// 根据Id查询实体对象
        /// </summary>
        public async Task<T?> GetByIdAsync<T, TKey>(TKey id) where T : BaseCommonEntity<TKey>
        {
            _opt.IsReadOnly = true;
            var query = _dbCxt.Set<T>().AsNoTracking();
            var data = await query.FirstOrDefaultAsync(x => x.Id!.Equals(id));
            return data;
        }

        /// <summary>
        /// 根据Id查询结果对象
        /// </summary>
        public async Task<TResult?> GetByIdAsync<T, TKey, TResult>(TKey id, Expression<Func<T, TResult>> selectors) where T : BaseCommonEntity<TKey>
        {
            _opt.IsReadOnly = true;
            var query = _dbCxt.Set<T>().AsNoTracking();
            var data = await query.Where(x => x.Id!.Equals(id)).Select(selectors).FirstOrDefaultAsync();
            return data;
        }

        /*  下面示例中的u表示的是泛型参数，表示实体类型(如下为:User)
            // 值类型示例（int, DateTime, bool等）
            int? userId = await repo.GetScalarAsync<User, int>(
            u => u.Email == "test@example.com", 
            u => u.Id);

            // 引用类型示例（string, 自定义class等）
            string? email = await repo.GetScalarAsync<User, string>(
            u => u.Id == 1, 
            u => u.Email);
        */
        /// <summary>
        /// 获取单个字段的值
        /// </summary>
        public async Task<TField?> GetScalarAsync<T, TField>(
            Expression<Func<T, bool>>? where, 
            Expression<Func<T, TField>> field) where T : class
        {
            _opt.IsReadOnly = true;
            var query = _dbCxt.Set<T>().AsNoTracking();
            if (where != null)
                query = query.Where(where);

            //// 判断 TField 是否为值类型 额外保险校验
            //if (typeof(TField).IsValueType)
            //{
            //    // 值类型：需要转换为可空类型
            //    var paramObj = field.Parameters[0];
            //    var convertedBody = Expression.Convert(field.Body, typeof(TField?));
            //    var nullableSelector = Expression.Lambda<Func<T, TField?>>(convertedBody, paramObj);
            //    return await query.Select(nullableSelector).DefaultIfEmpty().FirstOrDefaultAsync();
            //}
            //else // 引用类型：直接使用
            //{
            //    return await query.Select(field).DefaultIfEmpty().FirstOrDefaultAsync();
            //}

            // 引用类型：使用 DefaultIfEmpty()
            return await query.Select(field).DefaultIfEmpty().FirstOrDefaultAsync();
        }

        /// <summary>
        /// 获取数量
        /// </summary>
        public async Task<int> CountAsync<T>(Expression<Func<T, bool>>? where) where T : class
        {
            _opt.IsReadOnly = true;
            var query = _dbCxt.Set<T>().AsNoTracking();
            if (where != null)
                query = query.Where(where);
            var count = await query.CountAsync();
            return count;
        }

        /// <summary>
        /// 获取最大值
        /// </summary>
        public async Task<TField?> MaxAsync<T, TField>(Expression<Func<T, bool>>? where, Expression<Func<T, TField>> field) where T : class
        {
            _opt.IsReadOnly = true;
            var query = _dbCxt.Set<T>().AsNoTracking();
            if (where != null)
                query = query.Where(where);

            // 判断是否为值类型 额外保险校验
            //if (typeof(TField).IsValueType)
            //{
            //    // 对于值类型，需要将表达式转换为可空类型
            //    var parameter = field.Parameters[0];
            //    var nullableType = typeof(TField?);
            //    var convertExpr = Expression.Convert(field.Body, nullableType);
            //    var nullableField = Expression.Lambda<Func<T, TField?>>(convertExpr, parameter);
            //    // 使用可空类型的 MaxAsync 重载
            //    return await query.Select(nullableField).DefaultIfEmpty().MaxAsync();
            //}
            //else
            //{
            //    // 对于引用类型，可以直接使用
            //    return await query.Select(field).DefaultIfEmpty().MaxAsync();
            //}

            var max = await query.Select(field).DefaultIfEmpty().MaxAsync();
            return max;
        }

        /// <summary>
        /// 执行Sql
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        public async Task<int> ExecuteSqlAsync(string sql, params object[] parameters)
        {
            _opt.IsReadOnly = false;
            var nums = await _dbCxt.Database.ExecuteSqlRawAsync(sql, parameters);
            return nums;
        }

        /// <summary>
        /// 执行sql事务
        /// </summary>
        public async Task<int> ExecuteSqlTransactionAsync(string sql, params object[] parameters)
        {
            _opt.IsReadOnly = false;
            using (var transaction = await _dbCxt.Database.BeginTransactionAsync())
            {
                var nums = await _dbCxt.Database.ExecuteSqlRawAsync(sql, parameters);
                await transaction.CommitAsync();
                return nums;
            }
        }

        /// <summary>
        /// 执行查询sql
        /// </summary>
        public async Task<List<TResult>> SqlQueryParams<TResult>(string sql, params object[] parameters)
        {
            _opt.IsReadOnly = true;
            var query = _dbCxt.Database.SqlQueryRaw<TResult>(sql, parameters);
            var datas = await query.ToListAsync();
            return datas;
        }


        /*
        // 注意：执行sql查询时，请使用 {name} 语法，而不是使用 '{name}' 语法
        public async Task<List<User>> GetUsersByNameAsync(string name)
        {
            return await _dbCxt.Database.SqlQuery<User>(
                $"SELECT * FROM Users WHERE Name = {name}")
                .ToListAsync();
        }
        // 注意：执行sql查询时，请使用 %{keyword}% 语法，而不是使用 '%{keyword}%' 语法
        string keyword = "张";
        var users = await _dbCxt.Database.SqlQuery<User>(
            $"SELECT * FROM Users WHERE Name LIKE %{keyword}%")
            .ToListAsync();
        // 总之：就是不需要使用单引号包裹参数
         */
        /// <summary>
        /// 执行查询sql
        /// 此方法不会存在sql注入问题
        /// </summary>
        public async Task<List<TResult>> SqlQueryAsync<TResult>(FormattableString sql)
        {
            _opt.IsReadOnly = true;
            var query = _dbCxt.Database.SqlQuery<TResult>(sql);
            var datas = await query.ToListAsync();
            return datas;
        }

        #region 分页查询

        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="where"></param>
        /// <param name="page"></param>
        public async Task<(int, List<T>)> GetPageListAsync<T>(Expression<Func<T, bool>>? where, PageInfo page) where T : class
        {
            _opt.IsReadOnly = true;
            var query = _dbCxt.Set<T>().AsNoTracking().AsQueryable();
            if (where != null)
                query = query.Where(where);

            #region 排序

            if (!string.IsNullOrEmpty(page.Sort))
            {
                IOrderedQueryable<T>? ordered = null;
                var arrs = page.Sort.Split(',', StringSplitOptions.RemoveEmptyEntries); // 获取排序字段, 去掉空格
                var lens = arrs.Length;
                if (!string.IsNullOrWhiteSpace(page.SortType) && page.SortType.Equals("asc", StringComparison.OrdinalIgnoreCase))
                {
                    for (var i = 0; i < lens; i++)
                    {
                        var itm = arrs[i].Trim();
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

            var skips = page.PageSize * (page.PageIndex - 1);
            var nums = await query.CountAsync(); // 获取总记录数
            var datas = await query              // 获取分页数据
                .Skip(skips)
                .Take(page.PageSize).ToListAsync();

            return (nums, datas);
        }

        /// <summary>
        /// 获取分页数据
        /// 查询指定的字段
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="where"></param>
        /// <param name="selector"></param>
        /// <param name="page"></param>
        public async Task<(int, List<TResult>)> GetPagesAsync<T, TResult>(Expression<Func<T, bool>>? where, 
            Expression<Func<T, TResult>> seletors, 
            PageInfo page) where T : class
        {
            _opt.IsReadOnly = true;
            var query = _dbCxt.Set<T>().AsNoTracking();
            if (where != null)
                query = query.Where(where);

            #region 排序

            if (!string.IsNullOrEmpty(page.Sort))
            {
                IOrderedQueryable<T>? ordered = null;
                var arrs = page.Sort.Split(',', StringSplitOptions.RemoveEmptyEntries); // 获取排序字段, 去掉空格
                var lens = arrs.Length;
                if (!string.IsNullOrWhiteSpace(page.SortType) && page.SortType.Equals("asc", StringComparison.OrdinalIgnoreCase))
                {
                    for (var i = 0; i < lens; i++)
                    {
                        var itm = arrs[i].Trim();
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

            var skips = page.PageSize * (page.PageIndex - 1);
            var nums = await query.CountAsync(); // 获取总记录数
            var datas = await query              // 获取分页数据
                .Select(seletors)
                .Skip(skips)
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
