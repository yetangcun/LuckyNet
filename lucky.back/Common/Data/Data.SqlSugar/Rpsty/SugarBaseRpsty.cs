using SqlSugar;
using System.Reflection;
using Lucky.BaseModel.Model;
using System.Linq.Expressions;
using Common.CoreLib.Model.Option;
using Microsoft.Extensions.Logging;

namespace Data.SqlSugar.Rpsty
{
    public class SugarBaseRpsty<TEntity, TOption> : SimpleClient<TEntity> where TEntity : class, new() where TOption : DbDefaultOption
    {
        private readonly TOption _option;
        private readonly ILogger _logger;

        /// <summary>
        /// 数据库读标识
        /// </summary>
        public bool IsSugarReadOnly { get; set; } = false;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="option"></param>
        /// <param name="logger"></param>
        public SugarBaseRpsty(TOption option, ILogger logger)
        {
            _option = option;
            _logger = logger;
        }

        private ISqlSugarClient? _cxt;
        public override ISqlSugarClient Context  // 重写覆盖父类属性
        {
            get
            {
                if (_cxt != null) return _cxt;
                var realConnString = IsSugarReadOnly ? _option.SlaveConnString : _option.MasterConnString;
                var connCfg = new ConnectionConfig()
                {
                    ConnectionString = realConnString,
                    DbType = (DbType)_option.DbType,
                    IsAutoCloseConnection = true
                };
                _cxt = new SqlSugarClient(connCfg, act =>
                {
                    act.Aop.OnLogExecuted = (sql, pars) =>
                    {
                        var sqlStr = UtilMethods.GetNativeSql(sql, pars);
                        Console.WriteLine(sqlStr);
                        _logger.LogDebug(sqlStr);
                    };
                });
                return _cxt;
            }
        }

        #region 查询

        /// <summary>
        /// 判断是否存在
        /// </summary>
        public async Task<bool> Any(Expression<Func<TEntity, bool>> where)
        {
            return await Context.Queryable<TEntity>().AnyAsync(where);
        }

        /// <summary>
        /// 预查询
        /// </summary>
        public ISugarQueryable<TEntity> Queryable()
        {
            return Context.Queryable<TEntity>();
        }

        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <typeparam name="TOut">输出结构</typeparam>
        /// <param name="where">条件表达式</param>
        /// <param name="selectors">结果表达式</param>
        public async Task<(int,List<TOut>)> GetPageListAsync<TOut>(Expression<Func<TEntity, bool>>? where, Expression<Func<TEntity, TOut>> selectors, PageInfo page)
        {
            var count = 0;
            var list = new List<TOut>();
            var query = Context.Queryable<TEntity>();

            #region 排序
            var clsType = typeof(TEntity);
            if (!string.IsNullOrEmpty(page.Sort))
            {
                var arrs = page.Sort.Split(',', StringSplitOptions.RemoveEmptyEntries); // 获取排序字段, 去掉空格
                var lens = arrs.Length;
                var orders = new List<OrderByModel>();
                if (!string.IsNullOrWhiteSpace(page.SortType) && page.SortType.Equals("asc", StringComparison.OrdinalIgnoreCase))
                {
                    for (var i = 0; i < lens; i++)
                    {
                        var itm = arrs[i].Trim();
                        var prop = clsType.GetProperty(itm, BindingFlags.Public | BindingFlags.IgnoreCase);  // ✅ 新增：验证字段是否存在
                        if (prop == null)
                            continue;
                        orders.Add(new OrderByModel()
                        {
                            OrderByType = OrderByType.Asc,
                            FieldName = itm
                        });
                    }
                }
                else
                {
                    for (var i = 0; i < lens; i++)
                    {
                        var itm = arrs[i];
                        var prop = clsType.GetProperty(itm, BindingFlags.Public | BindingFlags.IgnoreCase);  // ✅ 新增：验证字段是否存在
                        if (prop == null)
                            continue;

                        orders.Add(new OrderByModel()
                        {
                            OrderByType = OrderByType.Asc,
                            FieldName = itm
                        });
                    }
                }

                if (orders.Any())
                    query.OrderBy(orders);
            }
            else if (clsType.GetProperty("CreateTime") != null)
                query = query.OrderByDescending(x => "CreateTime");
            else if (clsType.GetProperty("Id") != null)
                query = query.OrderBy(x => "Id");

            #endregion


            if (where == null)
            {
                count = await query.CountAsync(where);
                list = await Context.Queryable<TEntity>().Select(selectors).ToPageListAsync(page.PageIndex, page.PageSize);
                return (count, list);
            }

            count = await query.Where(where).CountAsync();
            if (count == 0)
                return (0, new List<TOut>());

            list = await query.Where(where).Select(selectors).ToPageListAsync(page.PageIndex, page.PageSize);

            return (count, list);
        }

        /// <summary>
        /// 获取自定义结构列表
        /// </summary>
        /// <typeparam name="TOut"></typeparam>
        /// <param name="where"></param>
        /// <param name="selectors"></param>
        public async Task<List<TOut>> GetListAsync<TOut>(Expression<Func<TEntity, bool>>? where, Expression<Func<TEntity, TOut>> selectors)
        {
            if (where == null)
            {
                return await Context.Queryable<TEntity>().Select(selectors).ToListAsync();
            }
            return await Context.Queryable<TEntity>().Where(where).Select(selectors).ToListAsync();
        }

        /// <summary>
        /// 根据主值查询单条数据
        /// </summary>
        /// <param name="pkVal">主键值</param>
        public async Task<TOut> GetById<TOut>(object pkVal, Expression<Func<TEntity, TOut>> selectors)
        {
            return await Context.Queryable<TEntity>().Select(selectors).InSingleAsync(pkVal);
        }

        /// <summary>
        /// 根据主值查询单个字段值
        /// </summary>
        public async Task<TOut> GetScalar<TOut>(Expression<Func<TEntity, bool>>? where, Expression<Func<TEntity, TOut>> selectors)
        {
            if (where == null)
            {
                return await Context.Queryable<TEntity>().Select(selectors).FirstAsync();
            }
            return await Context.Queryable<TEntity>().Where(where).Select(selectors).FirstAsync();
        }

        /// <summary>
        /// 获取最大值
        /// </summary>
        public async Task<TOut> MaxAsync<TOut>(Expression<Func<TEntity, bool>>? where, Expression<Func<TEntity, TOut>> selectors)
        {
            if (where == null)
            {
                return await Context.Queryable<TEntity>().MaxAsync(selectors);
            }
            return await Context.Queryable<TEntity>().Where(where).MaxAsync(selectors);
        }

        #endregion

        #region 增删改
        /// <summary>
        /// 添加
        /// </summary>
        public async Task<int> AddAsync(TEntity entity)
        {
            return await Context.Insertable(entity).ExecuteCommandAsync();
        }

        /// <summary>
        ///  批量插入
        /// </summary>
        public async Task<int> AddAsync(List<TEntity> entitys)
        {
            return await Context.Insertable(entitys).ExecuteCommandAsync();
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        public async Task<int> UpdateAsync(List<TEntity> entitys)
        {
            return await Context.Updateable(entitys).ExecuteCommandAsync();
        }

        /// <summary>
        /// 条件更新
        /// </summary>
        public async Task<int> UpdateAsync(Expression<Func<TEntity, bool>> where)
        {
            return await Context.Updateable<TEntity>().Where(where).ExecuteCommandAsync();
        }

        /// <summary>
        /// 删除
        /// </summary>
        public async Task<int> DeleteAsync(object pkVal)
        {
            return await Context.Deleteable<TEntity>(pkVal).ExecuteCommandAsync();
        }

        #endregion

        #region 初始化库、创建表

        public bool InitDb()
        {
            return Context.DbMaintenance.CreateDatabase();
        }

        public void CreateTable()
        {
            var rs = InitDb();
            Context.CodeFirst.InitTables<TEntity>();
        }

        public void CreateTable<T>()
        {
            var rs = InitDb();
            Context.CodeFirst.InitTables<T>();
        }

        /// <summary>
        /// 扫描程序集
        /// 批量创建表
        /// </summary>
        public void CreateTables(string[] dllNames)
        {
            var rs = InitDb();
            foreach (var dllName in dllNames)
            {
                var dllPath = $"{AppDomain.CurrentDomain.BaseDirectory}{dllName}";
                var assembly = Assembly.LoadFrom(dllPath);
                var entityTypes = assembly.GetTypes()
                    .Where(t => t.IsClass &&
                               !t.IsAbstract &&
                               t.Namespace?.EndsWith(".Entity") == true).ToList();
                // 创建表
                Context.CodeFirst.InitTables(entityTypes.ToArray());
            }
        }

        public bool CreateTableAsync(string tableName, List<DbColumnInfo> columns)
        {
            return Context.DbMaintenance.CreateTable(tableName, columns);
        }

        public bool DropTableAsync(string tableName)
        {
            return Context.DbMaintenance.DropTable(tableName);
        }

        #endregion

        #region 事务
        /// <summary>
        /// 有返回的事务
        /// </summary>
        public async Task<(bool, TOut?)> DoTransactionAsync<TOut>(Func<Task<TOut>> func)
        {
            var res = await Context.Ado.UseTranAsync(async () => await func());
            return (res.IsSuccess, res.Data);
        }

        /// <summary>
        /// 无返回的异步事务
        /// </summary>
        public async Task<(bool, string?)> DoTransactionAsync(Func<Task> func)
        {
            var res = await Context.Ado.UseTranAsync(async () => await func());
            return (res.IsSuccess, res.ErrorMessage);
        }

        /// <summary>
        /// 无返回的同步事务
        /// </summary>
        public async Task<(bool, string?)> DoTransaction(Action act)
        {
            var res = Context.Ado.UseTran(() => act());
            return (res.IsSuccess, res.ErrorMessage);
        }

        #endregion

        #region sql
        /// <summary>
        /// sql查询单条数据
        /// </summary>
        /// <typeparam name="TOut"></typeparam>
        /// <param name="sql"></param>
        /// <param name="args"></param>
        public async Task<TOut> SqlQuerySingleAsync<TOut>(string sql, params object[] args)
        {
            return await Context.Ado.SqlQuerySingleAsync<TOut>(sql, args);
        }

        /// <summary>
        /// sql查询列表
        /// </summary>
        public async Task<List<TOut>> SqlQueryListAsync<TOut>(string sql, params object[] args)
        {
            return await Context.Ado.SqlQueryAsync<TOut>(sql, args);
        }

        /// <summary>
        /// sql执行
        /// </summary>
        public async Task<int> SqlExecuteAsync(string sql, params object[] args)
        {
            return await Context.Ado.ExecuteCommandAsync(sql, args);
        }

        /// <summary>
        /// sql事务执行
        /// </summary>
        public async Task<(bool, string)> SqlExecuteTransAsync(string sql, params object[] args)
        {
            var res = await Context.Ado.UseTranAsync(() => Context.Ado.ExecuteCommandAsync(sql, args));
            var msg = res.IsSuccess ? res.Data.ToString() : res.ErrorMessage;
            return (res.IsSuccess, msg);
        }

        #endregion
    }
}
