using System.Text;
using System.Data;
using ClickHouse.Ado;
using Common.CoreLib.Model.Option;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace Data.ClkHouse
{
    /// <summary>
    /// 复用连接池高性能版本
    /// </summary>
    public class ClkhouseDbExtension
    {
        private ClkhouseOption _option;
        private readonly ILogger<ClkhouseDbExtension> _logger;
        private ConcurrentDictionary<string, ClickHouseConnection> _conn_pls = new();

        public ClkhouseDbExtension(IOptions<ClkhouseOption> options, ILogger<ClkhouseDbExtension> logger)
        {
            _logger = logger;
            _option = options.Value;

            if (string.IsNullOrWhiteSpace(_option.Database))
                throw new Exception("未配置数据库连接");

            var connStr = $"Compress=True;CheckCompressedHash=False;Compressor=lz4;Host={_option.Host};Port={_option.Port};Database={_option.Database};User={_option.User};Password={_option.Password};SocketTimeout=20000";
            var settings = new ClickHouseConnectionSettings(connStr);
            var dft_conn = new ClickHouseConnection(settings);
            dft_conn.Open();
            _conn_pls.TryAdd(_option.Database, dft_conn);
        }

        private async Task BuildConn(string dbName)
        {
            var connStr = $"Compress=True;CheckCompressedHash=False;Compressor=lz4;Host={_option.Host};Port={_option.Port};Database={dbName};User={_option.User};Password={_option.Password};SocketTimeout=20000";
            var settings = new ClickHouseConnectionSettings(connStr);
            var dft_conn = new ClickHouseConnection(settings);
            await dft_conn.OpenAsync();

            if (!_conn_pls.ContainsKey(dbName))
            {
                _conn_pls.TryAdd(dbName, dft_conn);
            }
            else _conn_pls[connStr] = dft_conn;
        }

        public async Task<ClickHouseConnection> GetChConnection(string? dbName = null)
        {
            if (string.IsNullOrWhiteSpace(dbName))
            {
                var _clk_conn = _conn_pls[_option.Database];
                if (_clk_conn.State != ConnectionState.Open)
                {
                    _clk_conn.Dispose();
                    await BuildConn(_option.Database);
                }

                return _conn_pls[_option.Database];
            }
            else if (_conn_pls.ContainsKey(dbName))
            {
                var _clk_conn = _conn_pls[dbName];
                if (_clk_conn.State != ConnectionState.Open)
                {
                    _clk_conn.Dispose();
                    await BuildConn(dbName);
                }
                return _conn_pls[dbName];
            }
            else
            {
                var connStr = $"Compress=True;CheckCompressedHash=False;Compressor=lz4;Host={_option.Host};Port={_option.Port};Database={dbName};User={_option.User};Password={_option.Password};SocketTimeout=20000";
                var settings = new ClickHouseConnectionSettings(connStr);
                var conn = new ClickHouseConnection(settings);
                await conn.OpenAsync(); _conn_pls.TryAdd(dbName, conn);
                return conn;
            }
        }

        /// <summary>
        /// 单个操作|增、删、改
        /// </summary>
        public async Task<bool> Opt(string sql, string? dbName = null)
        {
            var res = false;

            try
            {
                var conn = await GetChConnection(dbName);
                var cmd = conn.CreateCommand(sql);
                res = cmd.ExecuteNonQuery() > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}\r\n{ex.InnerException}\r\n{ex.StackTrace}");
                res = false;
            }

            return res;
        }

        /* 批量写入参考
           class MyPersistableObject:IEnumerable{
	            public string MyStringField;
	            public DateTime MyDateField;
	            public int MyIntField;
	            // Count and order of returns must match column order in SQL INSERT
	            public IEnumerator GetEnumerator(){
		            yield return MyDateField;
		            yield return MyDateField;
		            yield return MyStringField;
		            yield return (ushort)MyIntField;
	            }
            }

            //... somewhere elsewhere ...
            var list=new List<MyPersistableObject>();

            // fill the list to insert
            list.Add(new MyPersistableObject());
            var command=connection.CreateCommand();
            command.CommandText="INSERT INTO test (date,time,str,int) VALUES @bulk";
            command.Parameters.Add(new ClickHouseParameter{
	            ParameterName="bulk",
	            Value=list
            });
            command.ExecuteNonQuery();
         */

        /// <summary>
        /// 批量写入
        /// sql参考: insert into my_table(id,name,age,intime) values
        /// </summary>
        public async Task<int> BulkInsert<T>(string sql, T[] lists, string? dbName = null)
        {
            var nums = 0;
            try
            {
                var conn = await GetChConnection(dbName);

                using var command = conn.CreateCommand();

                var srb = new StringBuilder(sql);
                var tp = typeof(T);
                var lens = lists.Length;
                var pros = tp.GetProperties();

                #region 自定义类型
                if (pros.Length <1)
                {
                    var fields = tp.GetFields();
                    for (var i = 0; i < lens; i++)
                    {
                        var paramStr = (i < lens - 1) ? $"({fields.Select(x => $"@{x.Name}_{i}").Aggregate((x, y) => $"{x},{y}")})," : $"({fields.Select(x => $"@{x.Name}_{i}").Aggregate((x, y) => $"{x},{y}")})";
                        var itm = lists[i];
                        fields.ToList().ForEach(x =>
                        {
                            command.Parameters.Add(new ClickHouseParameter { ParameterName = $"{x.Name}_{i}", Value = tp.GetField(x.Name)?.GetValue(itm) });
                        });
                        srb.Append(paramStr);
                    }
                    command.CommandText = srb.ToString();
                    nums = await command.ExecuteNonQueryAsync();
                    return nums;
                }
                #endregion

                #region 匿名类型
                for (var i = 0; i < lens; i++)
                {
                    var paramStr = (i < lens - 1) ? $"({pros.Select(x => $"@{x.Name}_{i}").Aggregate((x, y) => $"{x},{y}")})," : $"({pros.Select(x => $"@{x.Name}_{i}").Aggregate((x, y) => $"{x},{y}")})";
                    var itm = lists[i];
                    pros.ToList().ForEach(x =>
                    {
                        command.Parameters.Add(new ClickHouseParameter { ParameterName = $"{x.Name}_{i}", Value = tp.GetProperty(x.Name)?.GetValue(itm) });
                    });
                    srb.Append(paramStr);
                }
                command.CommandText = srb.ToString();
                nums = await command.ExecuteNonQueryAsync();
                #endregion
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message},{ex.InnerException},{ex.StackTrace}\r\n");
                nums = 0;
            }
            return nums;
        }

        /// <summary>
        /// 查询单个值
        /// </summary>
        public async Task<T?> Getscalar<T>(string sql, string? dbName = null) where T : class
        {
            try
            {
                var conn = await GetChConnection(dbName);
                var cmd = conn.CreateCommand(sql);
                var res = cmd.ExecuteScalar();
                return (T)res;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}\r\n{ex.InnerException}\r\n{ex.StackTrace}");
                return null;
            }
        }

        /// <summary>
        /// 条件查询单条|列表
        /// </summary>
        public async Task<DataTable?> GetList(string sql, DataTable dt, string? dbName = null)
        {
            try
            {
                var conn = await GetChConnection(dbName);
                using (var cmd = conn.CreateCommand(sql))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        var fields = reader.FieldCount;
                        reader.ReadAll(r =>
                        {
                            var row = dt.NewRow();
                            for (var i = 0; i < fields; ++i)
                                row[i] = r.GetValue(i);

                            dt.Rows.Add(row);
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}\r\n{ex.InnerException}\r\n{ex.StackTrace}");
            }
            return dt;
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        public async Task<(int, DataTable)> GetBypage(string countSql, string pageSql, DataTable dt, string? dbName = null)
        {
            ulong counts = 0;

            try
            {
                var conn = await GetChConnection(dbName);

                using (var cmd = conn.CreateCommand(countSql))
                {
                    counts = (ulong)(cmd.ExecuteScalar());
                }

                using (var cmd = conn.CreateCommand(pageSql))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        var fields = reader.FieldCount;
                        reader.ReadAll(r =>
                        {
                            var row = dt.NewRow();
                            for (var i = 0; i < fields; ++i)
                            {
                                row[i] = r.GetValue(i);
                            }
                            dt.Rows.Add(row);
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}\r\n{ex.InnerException}\r\n{ex.StackTrace}");
            }

            return ((int)counts, dt);
        }

        #region 补充完善, 支持非配置文件中的其他服务器地址的数据库

        public async Task<ClickHouseConnection> GetChConnection2(ClkhouseOption obj)
        {
            var ky = $"{obj.Host}:{obj.Database}";
            if (_conn_pls.ContainsKey(ky))
            {
                var _clk_conn = _conn_pls[ky];
                if (_clk_conn.State != ConnectionState.Open)
                {
                    _clk_conn.Dispose();
                    var connStr = $"Compress=True;CheckCompressedHash=False;Compressor=lz4;Host={obj.Host};Port={obj.Port};Database={obj.Database};User={obj.User};Password={obj.Password};SocketTimeout=20000";
                    var settings = new ClickHouseConnectionSettings(connStr);
                    var conn = new ClickHouseConnection(settings);
                    await conn.OpenAsync();
                    _conn_pls[ky] = conn;
                    return conn;
                }

                return _clk_conn;
            }
            else
            {
                var connStr = $"Compress=True;CheckCompressedHash=False;Compressor=lz4;Host={obj.Host};Port={obj.Port};Database={obj.Database};User={obj.User};Password={obj.Password};SocketTimeout=20000";
                var settings = new ClickHouseConnectionSettings(connStr);
                var conn = new ClickHouseConnection(settings);

                await conn.OpenAsync(); _conn_pls.TryAdd(ky, conn);

                return conn;
            }
        }

        /// <summary>
        /// 单个操作|增、删、改
        /// </summary>
        public async Task<bool> Opt2(string sql, ClkhouseOption obj)
        {
            var res = false;

            try
            {
                var conn = await GetChConnection2(obj);
                var cmd = conn.CreateCommand(sql);
                res = cmd.ExecuteNonQuery() > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}\r\n{ex.InnerException}\r\n{ex.StackTrace}");
                res = false;
            }

            return res;
        }

        /* 批量写入参考
           class MyPersistableObject:IEnumerable{
	            public string MyStringField;
	            public DateTime MyDateField;
	            public int MyIntField;
	            // Count and order of returns must match column order in SQL INSERT
	            public IEnumerator GetEnumerator(){
		            yield return MyDateField;
		            yield return MyDateField;
		            yield return MyStringField;
		            yield return (ushort)MyIntField;
	            }
            }

            //... somewhere elsewhere ...
            var list=new List<MyPersistableObject>();

            // fill the list to insert
            list.Add(new MyPersistableObject());
            var command=connection.CreateCommand();
            command.CommandText="INSERT INTO test (date,time,str,int) VALUES @bulk";
            command.Parameters.Add(new ClickHouseParameter{
	            ParameterName="bulk",
	            Value=list
            });
            command.ExecuteNonQuery();
         */

        /// <summary>
        /// 批量写入
        /// </summary>
        public async Task<int> BulkInsert2<T>(string sql, string paramName, List<T> lists, ClkhouseOption obj)
        {
            var nums = 0;
            try
            {
                var conn = await GetChConnection2(obj);
                var command = conn.CreateCommand(sql);
                command.Parameters.Add(new ClickHouseParameter
                {
                    ParameterName = paramName,
                    Value = lists
                });
                nums = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}\r\n{ex.InnerException}\r\n{ex.StackTrace}");
                nums = 0;
            }
            return nums;
        }

        /// <summary>
        /// 查询单个值
        /// </summary>
        public async Task<T?> Getscalar2<T>(string sql, ClkhouseOption obj) where T : class
        {
            try
            {
                var conn = await GetChConnection2(obj);
                var cmd = conn.CreateCommand(sql);
                var res = cmd.ExecuteScalar();
                return (T)res;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}\r\n{ex.InnerException}\r\n{ex.StackTrace}");
                return null;
            }
        }

        /// <summary>
        /// 条件查询单条|列表
        /// </summary>
        public async Task<DataTable?> GetList2(string sql, DataTable dt, ClkhouseOption obj)
        {
            try
            {
                var conn = await GetChConnection2(obj);
                using (var cmd = conn.CreateCommand(sql))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        var fields = reader.FieldCount;
                        reader.ReadAll(r =>
                        {
                            var row = dt.NewRow();
                            for (var i = 0; i < fields; ++i)
                                row[i] = r.GetValue(i);

                            dt.Rows.Add(row);
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}\r\n{ex.InnerException}\r\n{ex.StackTrace}");
            }
            return dt;
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        public async Task<(int, DataTable)> GetBypage2(string countSql, string pageSql, DataTable dt, ClkhouseOption obj)
        {
            ulong counts = 0;

            try
            {
                var conn = await GetChConnection2(obj);

                using (var cmd = conn.CreateCommand(countSql))
                {
                    counts = (ulong)(cmd.ExecuteScalar());
                }

                using (var cmd = conn.CreateCommand(pageSql))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        var fields = reader.FieldCount;
                        reader.ReadAll(r =>
                        {
                            var row = dt.NewRow();
                            for (var i = 0; i < fields; ++i)
                            {
                                row[i] = r.GetValue(i);
                            }
                            dt.Rows.Add(row);
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}\r\n{ex.InnerException}\r\n{ex.StackTrace}");
            }

            return ((int)counts, dt);
        }

        #endregion
    }
}
