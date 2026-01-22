using Dapper;
using Npgsql;
using System.Data;
using System.Text.Json;
using Dapper.Contrib.Extensions;

namespace Data.Dapper.DbUtil
{
    public class PostgresqlUtil : IDbUtil
    {
        private static readonly object LOCKOBJ = new object();

        /// <summary>
        /// 获取最大Id
        /// </summary>
        public int GetMaxId(string tableName, string connectionString)
        {
            lock (LOCKOBJ)
            {
                using (var conn = new NpgsqlConnection(connectionString))
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

        /// <summary>
        /// 单记录删除
        /// </summary>
        public async Task<bool> Del<T>(T entity, string connectionString) where T : class
        {
            using (var conn = new NpgsqlConnection(connectionString))
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
            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();

                var res = await conn.DeleteAsync(entities);

                conn.Close();

                return res;
            }
        }

        /// <summary>
        /// 单记录查询
        /// </summary>
        public async Task<T?> GetAsync<T>(string sql, string connectionString, object? prms = null) where T : class
        {
            using (var conn = new NpgsqlConnection(connectionString))
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
            using (var conn = new NpgsqlConnection(connectionString))
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
            using (var conn = new NpgsqlConnection(connectionString))
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
        public async Task<List<T>?> GetListAsync<T>(string sql, string connectionString, object? prms = null) where T : class
        {
            using (var conn = new NpgsqlConnection(connectionString))
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
            using (var conn = new NpgsqlConnection(connectionString))
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
            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();

                var result = await conn.InsertAsync(entities);

                conn.Close();

                return (result > 0);
            }
        }

        public async Task<bool> BulkInsert(DataTable table, string connectionString, string desTable = "")
        {
            using var connection = new NpgsqlConnection(connectionString);
            await connection.OpenAsync();

            // 二进制导入
            await using var writer = await connection.BeginBinaryImportAsync($"COPY {desTable} FROM STDIN (FORMAT BINARY)");

            var rows = table.Rows;
            var cols = table.Columns;
            foreach (DataRow row in rows)
            {
                await writer.StartRowAsync();

                foreach (DataColumn column in cols)
                {
                    var value = row[column];
                    if (value == DBNull.Value || value == null)
                        await writer.WriteNullAsync();
                    else
                        await WriteValueByType(writer, value, column.DataType);
                }
            }

            await writer.CompleteAsync(); return true;
        }

        /// <summary>
        /// 简单的批量插入（修正版）
        /// </summary>
        public async Task<int> SimpleBulkInsertAsync<T>(IEnumerable<T> data, string connectionString, string tableName) where T : class
        {
            if (!data.Any()) return 0;

            var properties = typeof(T).GetProperties();
            var columnNames = string.Join(", ", properties.Select(p => $"\"{p.Name}\""));
            var parameterNames = string.Join(", ", properties.Select(p => $"@{p.Name}"));

            var sql = $"INSERT INTO {tableName} ({columnNames}) VALUES ({parameterNames})";

            using var connection = new NpgsqlConnection(connectionString);
            await connection.OpenAsync(); // ✅ 添加 Open

            return await connection.ExecuteAsync(sql, data.ToList());
        }

        /// <summary>
        /// 使用 UNNEST 批量插入
        /// </summary>
        public async Task<int> BulkInsertWithUnnestAsync<T>(IEnumerable<T> data, string connectionString, string tableName) where T : class
        {
            var dataList = data.ToList();
            if (!dataList.Any()) return 0;

            var properties = typeof(T).GetProperties();
            var columnNames = string.Join(", ", properties.Select(p => $"\"{p.Name}\""));

            // 构建 UNNEST 参数
            var unnestParameters = new List<string>();
            var dynamicParameters = new DynamicParameters();

            for (int i = 0; i < properties.Length; i++)
            {
                var prop = properties[i];
                var paramName = $"p{i}";
                unnestParameters.Add($"UNNEST(@{paramName})");
                dynamicParameters.Add(paramName, dataList.Select(x => prop.GetValue(x)).ToArray());
            }

            var unnestSql = string.Join(", ", unnestParameters);
            var sql = $"INSERT INTO {tableName} ({columnNames}) SELECT {unnestSql}";

            using var connection = new NpgsqlConnection(connectionString);
            await connection.OpenAsync();

            return await connection.ExecuteAsync(sql, dynamicParameters);
        }

        /// <summary>
        /// 高性能批量插入（使用 COPY 命令）
        /// </summary>
        public async Task<bool> BulkCopyAsync<T>(IEnumerable<T> datas, string connectionString, string desTable = "") where T : class
        {
            using var connection = new NpgsqlConnection(connectionString);
            await connection.OpenAsync();

            try
            {
                await using var writer = await connection.BeginBinaryImportAsync($"COPY {desTable} FROM STDIN (FORMAT BINARY)");

                foreach (var data in datas)
                {
                    await writer.WriteRowAsync(CancellationToken.None, GetPropertyValues(data));
                }

                await writer.CompleteAsync(); return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"批量插入失败: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// sql操作
        /// </summary>
        public async Task<bool> Opt(string sql, string connectionString, object? prms = null)
        {
            using (var conn = new NpgsqlConnection(connectionString))
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
            using (var conn = new NpgsqlConnection(connectionString))
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
            using (var conn = new NpgsqlConnection(connectionString))
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
            using (var conn = new NpgsqlConnection(connectionString))
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
            using (var conn = new NpgsqlConnection(connectionString))
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
            await using var conn = new NpgsqlConnection(connectionString);
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

        private static object[] GetPropertyValues<T>(T item) where T : class
        {
            var properties = typeof(T).GetProperties();
            var values = new object[properties.Length];

            for (int i = 0; i < properties.Length; i++)
            {
                var value = properties[i].GetValue(item);
                values[i] = value ?? DBNull.Value;
            }

            return values;
        }
        private async Task WriteValueByType(NpgsqlBinaryImporter writer, object value, Type dataType)
        {
            if (dataType == typeof(int))
                await writer.WriteAsync((int)value);
            else if (dataType == typeof(string))
                await writer.WriteAsync((string)value);
            else if (dataType == typeof(bool))
                await writer.WriteAsync((bool)value);
            else if (dataType == typeof(DateTime))
                await writer.WriteAsync((DateTime)value);
            else if (dataType == typeof(decimal))
                await writer.WriteAsync((decimal)value);
            else if (dataType == typeof(double))
                await writer.WriteAsync((double)value);
            else if (dataType == typeof(float))
                await writer.WriteAsync((float)value);
            else if (dataType == typeof(long))
                await writer.WriteAsync((long)value);
            else if (dataType == typeof(byte[]))
                await writer.WriteAsync((byte[])value);
            else
                await writer.WriteAsync(value.ToString()); // 默认转为字符串
        }
    }
}
