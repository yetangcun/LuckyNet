using Dapper;
using System.Data;

namespace Data.Dapper
{
    public interface IDbUtil
    {
        /// <summary>
        /// 获取最大id
        /// </summary>
        int GetMaxId(string tableName, string connectionString);

        /// <summary>
        /// 查询单条记录
        /// </summary>
        Task<T?> GetAsync<T>(string sql, string connectionString) where T : class;

        /// <summary>
        /// 查询单条记录
        /// </summary>
        Task<T?> GetAsync<T>(string sql, string connectionString, object prms) where T : class;

        /// <summary>
        /// 查询记录列表
        /// </summary>
        Task<List<T>?> GetListAsync<T>(string sql, string connectionString) where T : class;

        /// <summary>
        /// 查询记录列表
        /// </summary>
        Task<List<T>?> GetListAsync<T>(string sql, string connectionString, object prms) where T : class;

        /// <summary>
        /// 分页查询
        /// </summary>
        Task<(int, List<T>)> GetByPageAsync<T>(string sql, string connectionString) where T : class;

        Task<(int, List<T>)> GetByPageAsync<T>(string sql, object prms, string connectionString) where T : class;

        /// <summary>
        /// 执行操作
        /// </summary>
        Task<bool> Opt(string sql, string connectionString);

        /// <summary>
        /// 执行操作
        /// </summary>
        Task<bool> Opt(string sql, string connectionString, object prms);

        /// <summary>
        /// 事务执行多个sql
        /// </summary>
        Task<bool> SqlTransOptMany(string sql, string connectionString);

        /// <summary>
        /// 插入
        /// </summary>
        Task<bool> Insert<T>(T entity, string connectionString) where T : class;

        /// <summary>
        /// 批量插入 
        /// </summary>
        Task<bool> InsertMany<T>(List<T> entities, string connectionString) where T : class;

        /// <summary>
        /// 修改
        /// </summary>
        Task<bool> Update<T>(T entity, string connectionString) where T : class;

        /// <summary>
        /// 批量修改
        /// </summary>
        Task<bool> UpdateMany<T>(List<T> entities, string connectionString) where T : class;

        /// <summary>
        /// 删除
        /// </summary>
        Task<bool> Del<T>(T entity, string connectionString) where T : class;

        /// <summary>
        /// 批量删除
        /// </summary>
        Task<bool> DelMany<T>(List<T> entities, string connectionString) where T : class;

        /// <summary>
        /// 批量写
        /// </summary>
        Task<bool> BulkInsert(DataTable table, string connectionString, string desTable = "");

        // Task<bool> BulkInsert<T>(IEnumerable<T> datas, string connectionString, string desTable = "") where T : class;

        /// <summary>
        /// 获取多个sql查询结果
        /// </summary>
        /// <param name="sqlCounts">sql的数量</param>
        Task<List<string>?> GetMulResults(string sqls, string connectionString);

        Task<TResult> GetMulResultAsync<TResult>(string sqls, string connectionString, Func<SqlMapper.GridReader, Task<TResult>> mapper);

        /// <summary>
        /// 执行查询返回一个结果
        /// </summary>
        Task<object?> ExeScardar(string sql, string connectionString);

        /// <summary>
        /// 执行查询返回一个结果
        /// </summary>
        Task<object?> ExeScardar(string sql, string connectionString, object prms);
    }
}
