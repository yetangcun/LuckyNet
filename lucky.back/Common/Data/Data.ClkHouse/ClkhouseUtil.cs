using System.Data;
using ClickHouse.Ado;
using Microsoft.Extensions.Options;
using Common.CoreLib.Model.Option;

namespace Data.ClkHouse
{
    public class ClkhouseUtil
    {
        private ClkhouseOption _option;
        private readonly ClickHouseConnection _clk_conn;

        public ClkhouseUtil(IOptions<ClkhouseOption> options)
        {
            _option = options.Value;
            var connStr = $"Compress=True;CheckCompressedHash=False;Compressor=lz4;Host={_option.Host};Port={_option.Port};Database={_option.Database};User={_option.User};Password={_option.Password};SocketTimeout=20000";
            var settings = new ClickHouseConnectionSettings(connStr);
            _clk_conn = new ClickHouseConnection(settings);
        }

        public async Task<(bool,ClickHouseConnection)> GetChConnection(string? dbName = null)
        {
            if (string.IsNullOrWhiteSpace(dbName) && _clk_conn != null)
            {
                if (!_clk_conn.State.HasFlag(ConnectionState.Open))
                    await _clk_conn.OpenAsync();

                return (false, _clk_conn);
            }

            var realDb = string.IsNullOrWhiteSpace(dbName) ? _option.Database : dbName;
            var connStr = $"Compress=True;CheckCompressedHash=False;Compressor=lz4;Host={_option.Host};Port={_option.Port};Database={realDb};User={_option.User};Password={_option.Password};SocketTimeout=20000";
            var settings = new ClickHouseConnectionSettings(connStr);
            var conn = new ClickHouseConnection(settings);

            try
            {
                conn.Open();
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
            }

            return (true, conn);
        }

        /// <summary>
        /// 单个操作|增、删、改
        /// </summary>
        public async Task<bool> Opt(string sql, string? dbName = null)
        {
            var itms = await GetChConnection(dbName);
            var cmd = itms.Item2.CreateCommand(sql);
            var res = cmd.ExecuteNonQuery();

            if (itms.Item1)
            {
                await itms.Item2.CloseAsync();
                await itms.Item2.DisposeAsync();
            }
            return true;
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
        public async Task<int> BulkInsert<T>(string sql, string paramName, List<T> lists, string? dbName = null)
        {
            var itms = await GetChConnection(dbName);
            var command = itms.Item2.CreateCommand(sql);
            command.Parameters.Add(new ClickHouseParameter
            {
                ParameterName = paramName,
                Value = lists
            });
            var results = command.ExecuteNonQuery();

            if (itms.Item1)
            {
                await itms.Item2.CloseAsync();
                await itms.Item2.DisposeAsync();
            }

            return results;
        }

        /// <summary>
        /// 查询单个值
        /// </summary>
        public async Task<T> Getscalar<T>(string sql, string? dbName = null) where T : class
        {
            var itms = await GetChConnection(dbName);
            var cmd = itms.Item2.CreateCommand(sql);
            var res = cmd.ExecuteScalar();

            if (itms.Item1)
            {
                await itms.Item2.CloseAsync();
                await itms.Item2.DisposeAsync();
            }

            return (T)res;
        }

        /// <summary>
        /// 条件查询单条|列表
        /// </summary>
        public async Task<DataTable> GetList(string sql, DataTable dt, string? dbName = null)
        {
            var itms = await GetChConnection(dbName);
            using (var cmd = itms.Item2.CreateCommand(sql))
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

                    if (itms.Item1)
                    {
                        await itms.Item2.CloseAsync();
                        await itms.Item2.DisposeAsync();
                    }

                    return dt;
                }
            }
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        public async Task<(int, DataTable)> GetBypage(string countSql, string pageSql, DataTable dt, string? dbName = null)
        {
            ulong counts = 0;
            var itms = await GetChConnection(dbName);
            using (var cmd = itms.Item2.CreateCommand(countSql))
            {
                counts = (ulong)(cmd.ExecuteScalar());
            }

            using (var cmd = itms.Item2.CreateCommand(pageSql))
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

            if (itms.Item1)
            {
                await itms.Item2.CloseAsync();
                await itms.Item2.DisposeAsync();
            }

            return ((int)counts, dt);
        }
    }
}
