using Dapper;
using System.Data;
using System.Text.Json;
using Microsoft.Data.SqlClient;
using Dapper.Contrib.Extensions;

namespace Data.Dapper.DbUtil
{
    public class MssqlUtil : IDbUtil
    {
        /// <summary>
        /// 单条记录查询
        /// </summary>
        public async Task<T?> GetAsync<T>(string sql, string connectionString) where T : class
        {
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                var result = await conn.QueryAsync<T>(sql);
                conn.Close();
                return result.Any() ? result.First() : null;
            }
        }

        /// <summary>
        /// 单条记录查询
        /// </summary>
        public async Task<T?> GetAsync<T>(string sql, string connectionString, object prms) where T : class
        {
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                var result = await conn.QueryAsync<T>(sql, prms);
                conn.Close();
                return result.Any() ? result.First() : null;
            }
        }

        /// <summary>
        /// 列表查询
        /// </summary>
        public async Task<List<T>?> GetListAsync<T>(string sql, string connectionString) where T : class
        {
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                var result = await conn.QueryAsync<T>(sql);
                conn.Close();
                return result.ToList();
            }
        }

        /// <summary>
        /// 列表查询
        /// </summary>
        public async Task<List<T>?> GetListAsync<T>(string sql, string connectionString, object prms) where T : class
        {
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                var result = await conn.QueryAsync<T>(sql, prms);
                conn.Close();
                return result.ToList();
            }
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        public async Task<(int, List<T>)> GetByPageAsync<T>(string sql, string connectionString) where T : class
        {
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();

                using (var reader = conn.QueryMultiple(sql))
                {
                    var total = await reader.ReadFirstAsync<int>();

                    var data = await reader.ReadAsync<T>();

                    conn.Close();

                    return (total, data.ToList());
                }
            }
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        public async Task<(int, List<T>)> GetByPageAsync<T>(string sql, object prms, string connectionString) where T : class
        {
            using (var conn = new SqlConnection(connectionString))
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
        /// 多语句查询
        /// 多条sql组成的sqls
        /// </summary>
        public async Task<List<string>?> GetMulResults(string sqls, string connectionString)
        {
            var results = new List<string>();
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (var reader = await conn.QueryMultipleAsync(sqls))
                {
                    while(!reader.IsConsumed)
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
            await using var conn = new SqlConnection(connectionString);
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
        /// sql增|删|改
        /// </summary>
        public async Task<bool> Opt(string sql, string connectionString)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();

                var result = await conn.ExecuteAsync(sql);

                conn.Close();

                return (result > 0);
            }
        }

        /// <summary>
        /// sql增|删|改
        /// </summary>
        public async Task<bool> Opt(string sql, string connectionString, object prms)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();

                var result = await conn.ExecuteAsync(sql, prms);

                conn.Close();

                return (result > 0);
            }
        }

        /// <summary>
        /// sql批量新增
        /// </summary>
        public async Task<bool> SqlTransOptMany(string sql, string connectionString)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();

                var result = 0;
                var trans = conn.BeginTransaction();
                try
                {
                    var command = conn.CreateCommand();
                    command.CommandText = sql; command.Transaction = trans;
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
        /// 实体新增
        /// </summary>
        public async Task<bool> Insert<T>(T entity, string connectionString) where T : class
        {
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();

                var result = await conn.InsertAsync(entity);

                conn.Close();

                return (result > 0);
            }
        }


        /// <summary>
        /// 批量实体新增
        /// </summary>
        public async Task<bool> InsertMany<T>(List<T> entities, string connectionString) where T : class
        {
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();

                var result = await conn.InsertAsync(entities);

                conn.Close();

                return (result > 0);
            }
        }

        /// <summary>
        /// 更新
        /// </summary>
        public async Task<bool> Update<T>(T entity, string connectionString) where T : class
        {
            using (var conn = new SqlConnection(connectionString))
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
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();

                var res = await conn.UpdateAsync(entities);

                conn.Close();

                return res;
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        public async Task<bool> Del<T>(T entity, string connectionString) where T : class
        {
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();

                var res = await conn.DeleteAsync(entity);

                conn.Close();

                return res;
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        public async Task<bool> DelMany<T>(List<T> entities, string connectionString) where T : class
        {
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();

                var res = await conn.DeleteAsync(entities);

                conn.Close();

                return res;
            }
        }

        /// <summary>
        /// 批量写入操作
        /// </summary>
        public async Task<bool> BulkInsert(DataTable table, string connectionString, string desTable = "VosBlackList")
        {
            using (var conn = new SqlConnection(connectionString))
            {
                using (var bulkCopy = new SqlBulkCopy(conn))
                {
                    bulkCopy.DestinationTableName = desTable;
                    bulkCopy.BatchSize = table.Rows.Count;
                    conn.Open();

                    await bulkCopy.WriteToServerAsync(table);
                }
            }
            return true;
        }

        public async Task<object?> ExeScardar(string sql, string connectionString)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                var result = await conn.ExecuteScalarAsync(sql);
                conn.Close();
                return result;
            }
        }

        public async Task<object?> ExeScardar(string sql, string connectionString, object prms)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                var result = await conn.ExecuteScalarAsync(sql, prms);
                conn.Close();
                return result;
            }
        }

        private static readonly Object LOCKOBJ = new object();
        /// <summary>
        /// 获取最大id
        /// </summary>
        public int GetMaxId(string tableName, string connectionString)
        {
            lock (LOCKOBJ)
            {
                using (var conn = new SqlConnection(connectionString))
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
