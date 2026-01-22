using Dapper;
using System.Data;
using MySqlConnector;
using System.Text.Json;
using Dapper.Contrib.Extensions;

namespace Data.Dapper.DbUtil
{
    public class MysqlUtil : IDbUtil
    {
        /// <summary>
        /// 删除
        /// </summary>
        public async Task<bool> Del<T>(T entity, string connectionString) where T : class
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                var res = await conn.DeleteAsync(entity);

                conn.Close();

                return res;
            }
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        public async Task<bool> DelMany<T>(List<T> entities, string connectionString) where T : class
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                var res = await conn.DeleteAsync(entities);

                conn.Close();

                return res;
            }
        }

        /// <summary>
        /// 查询单条记录
        /// </summary>
        public async Task<T?> GetAsync<T>(string sql, string connectionString, object? prms = null) where T : class
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                var result = await conn.QueryAsync<T>(sql, prms);
                conn.Close();
                return result.Any() ? result.First() : null;
            }
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        public async Task<(int, List<T>)> GetByPageAsync<T>(string sql, string connectionString, object? prms = null) where T : class
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                using (var reader = conn.QueryMultiple(sql, prms))
                {
                    var total = await reader.ReadFirstAsync<int>();

                    var data = await reader.ReadAsync<T>();

                    conn.Close();

                    return (total, data.ToList());
                }
            }
        }

        /// <summary>
        /// 执行查询返回一个结果
        /// </summary>
        public async Task<object?> ExeScardar(string sql, string connectionString, object? prms = null)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                var result = await conn.ExecuteScalarAsync(sql, prms);
                conn.Close();
                return result;
            }
        }

        /// <summary>
        /// 条件查询列表
        /// </summary>
        public async Task<List<T>?> GetListAsync<T>(string sql, string connString, object? prms = null) where T : class
        {
            using (var conn = new MySqlConnection(connString))
            {
                conn.Open();
                var result = await conn.QueryAsync<T>(sql, prms);
                conn.Close();
                return result.ToList();
            }
        }

        /// <summary>
        /// 写入
        /// </summary>
        public async Task<bool> Insert<T>(T entity, string connectionString) where T : class
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                var result = await conn.InsertAsync(entity);

                conn.Close();

                return (result > 0);
            }
        }

        /// <summary>
        /// 批量写入
        /// </summary>
        public async Task<bool> InsertMany<T>(List<T> entities, string connectionString) where T : class
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                var result = await conn.InsertAsync(entities);

                conn.Close();

                return (result > 0);
            }
        }

        /// <summary>
        /// 批量写入
        /// </summary>
        public async Task<bool> BulkInsert(DataTable table, string connectionString, string desTable = "")
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                var bulkCopy = new MySqlBulkCopy(conn);

                bulkCopy.DestinationTableName = desTable;

                await bulkCopy.WriteToServerAsync(table);
            }

            return true;
        }

        /// <summary>
        /// sql操作
        /// </summary>
        public async Task<bool> Opt(string sql, string connectionString, object? prms = null)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                var result = await conn.ExecuteAsync(sql, prms);

                conn.Close();

                return (result > 0);
            }
        }

        /// <summary>
        /// sql事务操作
        /// </summary>
        public async Task<bool> SqlTransOptMany(string sql, string connectionString)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                var result = 0;
                var trans = conn.BeginTransaction();
                try
                {
                    var command = conn.CreateCommand();
                    command.CommandText = sql;
                    command.Transaction = trans;
                    result = command.ExecuteNonQuery();
                    await trans.CommitAsync();
                }
                catch (Exception)
                {
                    await trans.RollbackAsync();
                }

                conn.Close();

                return (result > 0);
            }
        }

        /// <summary>
        /// 更新
        /// </summary>
        public async Task<bool> Update<T>(T entity, string connectionString) where T : class
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                var res = await conn.UpdateAsync(entity);

                conn.Close();

                return res;
            }
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        public async Task<bool> UpdateMany<T>(List<T> entities, string connectionString) where T : class
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                var res = await conn.UpdateAsync(entities);

                conn.Close();

                return res;
            }
        }

        /// <summary>
        /// 获取多个结果
        /// </summary>
        public async Task<List<string>?> GetMulResults(string sqls, string connectionString)
        {
            var results = new List<string>();
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using (var reader = await conn.QueryMultipleAsync(sqls))
                {
                    while (!reader.IsConsumed)
                    {
                        var objs = reader.Read<object>(); // var json = JsonConvert.SerializeObject(objs);
                        var json = JsonSerializer.Serialize(objs);
                        results.Add(json);
                    }
                }
            }
            return results;
        }

        public async Task<TResult> GetMulResultAsync<TResult>(string sqls, string connectionString, Func<SqlMapper.GridReader, Task<TResult>> mapper)
        {
            await using var conn = new MySqlConnection(connectionString);
            await conn.OpenAsync();

            await using var reader = await conn.QueryMultipleAsync(
                new CommandDefinition(
                    sqls,
                    commandTimeout: 10,
                    cancellationToken: CancellationToken.None
                )
            );

            return await mapper(reader);
        }

        /// <summary>
        /// 获取最大Id
        /// </summary>
        private static readonly object LOCKOBJ = new object();
        public int GetMaxId(string tableName, string connectionString)
        {
            lock (LOCKOBJ)
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    try
                    {
                        var maxid = conn.QueryFirst<int>($"select max(Id) from {tableName};");
                        conn.Close();
                        return maxid + 1;
                    }
                    catch
                    {
                        conn.Close();
                        return 1;
                    }
                }
            }
        }

    }
}
