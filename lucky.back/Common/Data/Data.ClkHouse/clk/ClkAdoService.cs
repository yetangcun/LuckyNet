using Dapper;
using System.Data;
using ClickHouse.Driver.ADO;
using ClickHouse.Driver.Copy;
using ClickHouse.Driver.Utility;
using Common.CoreLib.Model.Option;
using Microsoft.Extensions.Options;

namespace Data.ClkHouse.clk
{
    public class ClkAdoService
    {
        private ClkhouseOption _clkOption;
        private readonly IHttpClientFactory _httpFactory;
        private readonly ClickHouseDataSource _dataSource;

        public ClkAdoService(IOptions<ClkhouseOption> clkOption, IHttpClientFactory httpFactory)
        {
            _clkOption = clkOption.Value;
            _httpFactory = httpFactory;

            //var connectStr = $"Host={_clkOption.Host};Port={_clkOption.Port};Username={_clkOption.User};Password={_clkOption.Password};Database={_clkOption.Database};";
            var connectStr = $"Host={_clkOption.Host};Port={_clkOption.Port};Username={_clkOption.User};Password={_clkOption.Password};";
            _dataSource = new ClickHouseDataSource(connectStr, _httpFactory);
        }

        /// <summary>
        /// 查询分页
        /// 涉及多个数据库的，在sql中采用dbName.tableName
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="countSql"></param>
        /// <param name="sql"></param>
        public async Task<(int, DataTable)> QueryPageAsync<T>(string countSql, string sql)
        {
            await using var connection = await _dataSource.OpenConnectionAsync();

            var count = await connection.ExecuteScalarAsync(countSql);

            int.TryParse(count.ToString(), out int total);

            var table = connection.ExecuteDataTable(sql);

            return (total, table);
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sql"></param>
        public async Task<DataTable> QueryAsync(string sql)
        {
            await using var connection = await _dataSource.OpenConnectionAsync();

            var table = connection.ExecuteDataTable(sql);

            return table;
        }

        /// <summary>
        /// 执行sql
        /// </summary>
        /// <param name="sql"></param>
        public async Task<int> ExecuteAsync(string sql)
        {
            await using var connection = await _dataSource.OpenConnectionAsync();

            return await connection.ExecuteStatementAsync(sql);
        }

        /// <summary>
        /// 执行sql
        /// 获取单个值
        /// </summary>
        /// <param name="sql"></param>
        public async Task<object> ExecuteScalarAsync(string sql)
        {
            await using var connection = await _dataSource.OpenConnectionAsync();

            return await connection.ExecuteScalarAsync(sql);
        }

        /// <summary>
        /// 批量插入
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tbName"></param>
        /// <param name="data"></param>
        public async Task<bool> BatchInsert<T>(string tbName, DataTable data)
        {
            if (data != null && data.Rows.Count >0)
            {

                await using ClickHouseConnection connection = _dataSource.CreateConnection();

                using var bulkInsert = new ClickHouseBulkCopy(connection)
                {
                    DestinationTableName = tbName,
                    MaxDegreeOfParallelism = 1,
                    BatchSize = data.Rows.Count + 1,
                    ColumnNames =  data.Columns.Cast<DataColumn>().Select(c => c.ColumnName).ToArray()
                };

                await bulkInsert.WriteToServerAsync(data, CancellationToken.None);

                return true;
            }

            return false;
        }
    }

    /// <summary>
    /// clickhouse + dapper 结合使用
    /// </summary>
    public class ClkDapperService
    {
        private ClkhouseOption _clkOption;
        private readonly IHttpClientFactory _httpFactory;
        private readonly ClickHouseDataSource _dataSource;

        public ClkDapperService(IOptions<ClkhouseOption> clkOption, IHttpClientFactory httpFactory)
        {
            _clkOption = clkOption.Value;
            _httpFactory = httpFactory; // var connectStr = $"Host={_clkOption.Host};Port={_clkOption.Port};Username={_clkOption.User};Password={_clkOption.Password};Database={_clkOption.Database};";
            var connectStr = $"Host={_clkOption.Host};Port={_clkOption.Port};Username={_clkOption.User};Password={_clkOption.Password};";
            _dataSource = new ClickHouseDataSource(connectStr, _httpFactory);
        }

        /// <summary>
        /// 查询分页
        /// 涉及多个数据库的，在sql中采用dbName.tableName
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="countSql"></param>
        /// <param name="sql">分页查询语句:SELECT * FROM users WHERE status = 1 ORDER BY id LIMIT 20 OFFSET 0</param>
        public async Task<(int, IEnumerable<T>)> QueryPageAsync<T>(string countSql, string sql, Dictionary<string, object>? prms1 = null, Dictionary<string, object>? prms2 = null)
        {
            using var connection = _dataSource.CreateConnection();

            var count = await connection.ExecuteScalarAsync(countSql, prms1);

            int.TryParse(count!.ToString(), out int total);

            var data = await connection.QueryAsync<T>(sql, prms2);

            return (total, data);
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sql"></param>
        public async Task<IEnumerable<T>> QueryAsync<T>(string sql, Dictionary<string, object>? prms = null)
        {
            using var connection = _dataSource.CreateConnection();

            var data = await connection.QueryAsync<T>(sql, prms);

            return data;
        }

        /// <summary>
        /// 执行sql
        /// </summary>
        /// <param name="sql"></param>
        public async Task<int> ExecuteAsync(string sql, Dictionary<string, object>? prms = null)
        {
            using var connection = await _dataSource.OpenConnectionAsync();

            return await connection.ExecuteAsync(sql, prms);
        }

        /// <summary>
        /// 执行sql
        /// 获取单个值
        /// </summary>
        /// <param name="sql"></param>
        public async Task<object?> ExecuteScalarAsync(string sql, Dictionary<string, object>? prms = null)
        {
            using var connection = await _dataSource.OpenConnectionAsync();

            return await connection.ExecuteScalarAsync(sql, prms);
        }

        /// <summary>
        /// 批量插入
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tbName"></param>
        /// <param name="data"></param>
        public async Task<bool> BatchInsert<T>(string tbName, DataTable data)
        {
            if (data != null && data.Rows.Count > 0)
            {

                using ClickHouseConnection connection = _dataSource.CreateConnection();

                using var bulkInsert = new ClickHouseBulkCopy(connection)
                {
                    DestinationTableName = tbName,
                    MaxDegreeOfParallelism = 1,
                    BatchSize = data.Rows.Count + 1,
                    ColumnNames = data.Columns.Cast<DataColumn>().Select(c => c.ColumnName).ToArray()
                };

                await bulkInsert.WriteToServerAsync(data, CancellationToken.None);

                return true;
            }

            return false;
        }
    }
}
