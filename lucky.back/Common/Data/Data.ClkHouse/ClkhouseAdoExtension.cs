using System.Data;
using ClickHouse.Ado;
using Microsoft.Extensions.Options;
using Common.CoreLib.Model.Option;

namespace netserver.clkhouse
{
    /// <summary>
    /// clickhouse.ado
    /// 不适宜大高并发
    /// </summary>
    public class ClkhouseAdoExtension
    {
        private ClkhouseOption _option;

        public ClkhouseAdoExtension(IOptions<ClkhouseOption> options)
        {
            _option = options.Value;
        }

        public ClickHouseConnection GetChConnection(string? dbName = null)
        {
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

            return conn;
        }

        /// <summary>
        /// 单个操作|增、删、改
        /// </summary>
        public bool Opt(string sql, string? dbName = null)
        {
            using (var conn = GetChConnection(dbName))
            {
                var cmd = conn.CreateCommand(sql);
                var res = cmd.ExecuteNonQuery();
                return true;
            }
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
        public int BulkInsert<T>(string sql, string paramName, List<T> lists, string? dbName = null)
        {
            using(var conn = GetChConnection(dbName))
            {
                var command = conn.CreateCommand(sql);
                command.Parameters.Add(new ClickHouseParameter
                {
                    ParameterName = paramName,
                    Value = lists
                });
                var results = command.ExecuteNonQuery();
                return results;
            }
        }

        /// <summary>
        /// 查询单个值
        /// </summary>
        public T Getscalar<T>(string sql, string? dbName = null) where T : class
        {
            using (var conn = GetChConnection(dbName))
            {
                var cmd = conn.CreateCommand(sql);
                var res = cmd.ExecuteScalar();
                return (T)res;
            }
        }

        /// <summary>
        /// 条件查询单条|列表
        /// </summary>
        public DataTable GetList(string sql, DataTable dt, string? dbName = null)
        {
            using (var conn = GetChConnection(dbName))
            {
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
                        return dt;
                    }
                }
            }
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        public (int, DataTable) GetBypage(string countSql, string pageSql, DataTable dt, string? dbName = null)
        {
            ulong counts = 0;
            using (var conn = GetChConnection(dbName))
            {
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

                return ((int)counts, dt);
            }
        }
    }
}
