using ClickHouse.Driver;
using ClickHouse.Driver.ADO;
using ClickHouse.Driver.Utility;
using Common.CoreLib.Model.Option;
using Microsoft.Extensions.Options;
using ClickHouse.Driver.ADO.Parameters;

namespace Data.ClkHouse.clk
{
    /// <summary>
    /// ClickHouse数据库服务
    /// 推荐采用ClickHouseClient
    /// 如果涉及多个数据库，请在sql中指定数据库名称，比如：db1.table1
    /// </summary>
    public class ClkService : IClkService
    {
        private ClkhouseOption _clkOption;
        private readonly ClickHouseClient clkClient;

        public ClkService(IOptions<ClkhouseOption> clkOption)
        {
            _clkOption = clkOption.Value;

            var secureSettings = new ClickHouseClientSettings
            {
                Host = _clkOption.Host,
                Port = _clkOption.Port,
                Protocol = string.Empty,
                Username = _clkOption.User,
                Password = _clkOption.Password,
                Database = _clkOption.Database,
            };

            clkClient = new ClickHouseClient(secureSettings);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        public async Task<(int, List<T>)> QueryPageAsync<T>(string countSql, string sql, Dictionary<string, object>? param1 = null, Dictionary<string, object>? param2 = null, int page = 1, int size = 10) where T : class
        {
            ClickHouseParameterCollection? prms2 = null;
            if (param2 != null)
            {
                foreach (var item in param2)
                {
                    prms2.AddParameter(item.Key, item.Value);
                }
            }
            var total = await clkClient.ExecuteScalarAsync(countSql, prms2);
            int.TryParse(total?.ToString(), out int count);

            ClickHouseParameterCollection? prms1 = null;
            if (param1 != null)
            {
                foreach (var item in param1)
                {
                    prms1.AddParameter(item.Key, item.Value);
                }
            }
            var res = await clkClient.QueryAsync<T>(sql, prms1).Skip((page - 1) * size).Take(size).ToListAsync();
            return (count, res);
        }

        /// <summary>
        /// 查询结果集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        public async Task<List<T>> QueryAsync<T>(string sql, Dictionary<string, object>? param = null) where T : class
        {
            ClickHouseParameterCollection? prms = null;
            if (param != null)
            {
                foreach (var item in param)
                {
                    prms.AddParameter(item.Key, item.Value);
                }
            }
            var res = await clkClient.QueryAsync<T>(sql, prms).ToListAsync();
            return res;
        }

        /// <summary>
        /// 执行sql
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        public async Task<int> ExecuteAsync(string sql, Dictionary<string, object>? param = null)
        {
            ClickHouseParameterCollection? prms = null;
            if (param != null)
            {
                foreach (var item in param)
                {
                    prms.AddParameter(item.Key, item.Value);
                }
            }
            var res = await clkClient.ExecuteNonQueryAsync(sql, prms);
            return res;
        }

        /// <summary>
        /// 获取单条数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        public async Task<T?> GetAsync<T>(string sql, Dictionary<string, object>? param = null) where T : class
        {
            ClickHouseParameterCollection? prms = null;
            if (param != null)
            {
                foreach (var item in param)
                {
                    prms.AddParameter(item.Key, item.Value);
                }
            }
            var res = await clkClient.QueryAsync<T>(sql, prms).FirstOrDefaultAsync();
            return res;
        }

        /// <summary>
        /// 批量插入
        /// </summary>
        /// <param name="table"></param>
        /// <param name="cols"></param>
        /// <param name="datas"></param>
        public async Task<long> BatchInsertAsync(string table, List<string> cols, List<object[]> datas)
        {
            var res = await clkClient.InsertBinaryAsync(table, cols, datas);
            
            return res;
        }

        /// <summary>
        /// 批量插入
        /// </summary>
        /// <typeparam name="T">泛型参数</typeparam>
        /// <param name="table">表名</param>
        /// <param name="datas">数据列表</param>
        public async Task<long> BatchInsertAsync<T>(string table, List<T> datas) where T : class
        {
            var res = await clkClient.InsertBinaryAsync(table, datas);
            return res;
        }
    }
}
