using Dapper;
using System.Data;
using Data.Dapper.DbUtil;
using Lucky.BaseModel.Enum;
using System.Collections.Concurrent;
using Microsoft.Extensions.DependencyInjection;

namespace Data.Dapper
{
    /// <summary>
    /// Dapper工具类
    /// </summary>
    public class DapperUtil
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ConcurrentDictionary<DatabaseType, IDbUtil?> _dbUtil = new();

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="serviceProvider"></param>
        public DapperUtil(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// 获取数据库实例
        /// </summary>
        /// <param name="databaseType"></param>
        private IDbUtil? GetDbUtil(DatabaseType databaseType)
        {
            var dbutil = databaseType switch
            {
                DatabaseType.Mysql => _dbUtil.GetOrAdd(databaseType, _ => _serviceProvider.GetService<MysqlUtil>()),
                DatabaseType.Sqlserver => _dbUtil.GetOrAdd(databaseType, _ => _serviceProvider.GetService<MssqlUtil>()),
                DatabaseType.Postgresql => _dbUtil.GetOrAdd(databaseType, _ => _serviceProvider.GetService<PostgresqlUtil>()),
                _ => throw new ArgumentOutOfRangeException(nameof(databaseType), databaseType, "没有该类型数据库的实现"),
            };

            // 空值检查
            return dbutil ?? throw new InvalidOperationException($"无法解析数据库工具: {databaseType}");
        }

        /// <summary>
        /// 获取最大id
        /// </summary>
        public int GetMaxId(string tableName, string connectionString, DatabaseType databaseType)
        {
            var dbUtil = GetDbUtil(databaseType);
            return dbUtil.GetMaxId(tableName, connectionString);
        }

        /// <summary>
        /// 单个查询
        /// </summary>
        public async Task<T?> GetAsync<T>(string sql, string connectionString, DatabaseType databaseType, object? prms = null) where T : class
        {
            var dbUtil = GetDbUtil(databaseType);
            return await dbUtil.GetAsync<T>(sql, connectionString, prms);
        }

        /// <summary>
        /// 条件查询|列表查询
        /// </summary>
        public async Task<List<T>?> GetListAsync<T>(string sql, string connectionString, DatabaseType databaseType, object? prms = null) where T : class
        {
            var dbUtil = GetDbUtil(databaseType);
            if (dbUtil == null)
                return default;

            return await dbUtil.GetListAsync<T>(sql, connectionString, prms);
        }

        /// <summary>
        /// 分页查询
        /// 参数化
        /// </summary>
        public async Task<(int, List<T>)> GetByPageAsync<T>(string sql, string connectionString, DatabaseType databaseType, object? prms = null) where T : class
        {
            var dbUtil = GetDbUtil(databaseType);
            if (dbUtil == null)
                return default;

            return await dbUtil.GetByPageAsync<T>(sql, connectionString, prms);
        }

        /// <summary>
        /// sql增|删|改
        /// </summary>
        public async Task<bool> Opt(string sql, string connectionString, DatabaseType databaseType, object? prms = null)
        {
            var dbUtil = GetDbUtil(databaseType);
            if (dbUtil == null)
                return false;

            return await dbUtil.Opt(sql, connectionString, prms);
        }

        /// <summary>
        /// sql批量新增
        /// </summary>
        public async Task<bool> SqlTransOptMany(string sql, string connectionString, DatabaseType databaseType)
        {
            var dbUtil = GetDbUtil(databaseType);
            if (dbUtil == null)
                return false;

            return await dbUtil.SqlTransOptMany(sql, connectionString);
        }

        /// <summary>
        /// 新增
        /// </summary>
        public async Task<bool> Insert<T>(T entity, string connectionString, DatabaseType databaseType) where T : class
        {
            var dbUtil = GetDbUtil(databaseType);
            if (dbUtil == null)
                return false;

            return await dbUtil.Insert<T>(entity, connectionString);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        public async Task<bool> InsertMany<T>(List<T> entities, string connectionString, DatabaseType databaseType) where T : class
        {
            var dbUtil = GetDbUtil(databaseType);
            if (dbUtil == null)
                return false;

            return await dbUtil.InsertMany(entities, connectionString);
        }

        /// <summary>
        /// 更新
        /// </summary>
        public async Task<bool> Update<T>(T entity, string connectionString, DatabaseType databaseType) where T : class
        {
            var dbUtil = GetDbUtil(databaseType);
            if (dbUtil == null)
                return false;

            return await dbUtil.Update<T>(entity, connectionString);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        public async Task<bool> UpdateMany<T>(List<T> entities, string connectionString, DatabaseType databaseType) where T : class
        {
            var dbUtil = GetDbUtil(databaseType);
            if (dbUtil == null)
                return false;

            return await dbUtil.UpdateMany(entities, connectionString);
        }

        /// <summary>
        /// 删除
        /// </summary>
        public async Task<bool> Del<T>(T entity, string connectionString, DatabaseType databaseType) where T : class
        {
            var dbUtil = GetDbUtil(databaseType);
            if (dbUtil == null)
                return false;

            return await dbUtil.Del(entity, connectionString);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        public async Task<bool> DelMany<T>(List<T> entities, string connectionString, DatabaseType databaseType) where T : class
        {
            var dbUtil = GetDbUtil(databaseType);
            if (dbUtil == null)
                return false;

            return await dbUtil.UpdateMany(entities, connectionString);
        }

        /// <summary>
        /// 批量写入
        /// </summary>
        public async Task<bool> BulkInsert(DataTable table, string connectionString, DatabaseType databaseType)
        {
            var dbUtil = GetDbUtil(databaseType);
            if (dbUtil == null)
                return false;

            var res = await dbUtil.BulkInsert(table, connectionString);
            return res;
        }

        /// <summary>
        /// 多语句查询结果
        /// 多条sql组成的sqls
        /// </summary>
        public async Task<List<string>?> GetMulResults(string sqls, string connectionString, DatabaseType databaseType)
        {
            var dbUtil = GetDbUtil(databaseType);
            if (dbUtil == null)
                return null;

            return await dbUtil.GetMulResults(sqls, connectionString);
        }

        public async Task<TResult> GetMulResultAsync<TResult>(string sqls, string connectionString, DatabaseType databaseType, Func<SqlMapper.GridReader, Task<TResult>> mapper)
        {
            var dbUtil = GetDbUtil(databaseType);
            if (dbUtil == null)
                return default;

            return await dbUtil.GetMulResultAsync<TResult>(sqls, connectionString, mapper);
        }

        public async Task<object?> GetScalar(string sql, string connectionString, DatabaseType databaseType)
        {
            var dbUtil = GetDbUtil(databaseType);
            if (dbUtil == null)
                return null;

            return await dbUtil.ExeScardar(sql, connectionString);
        }

        public async Task<object?> GetScalar(string sql, string connectionString, DatabaseType databaseType, object? prms = null)
        {
            var dbUtil = GetDbUtil(databaseType);
            if (dbUtil == null)
                return null;

            return await dbUtil.ExeScardar(sql, connectionString, prms);
        }
    }
}
