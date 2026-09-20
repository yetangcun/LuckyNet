namespace Data.ClkHouse.clk
{
    public interface IClkService
    {
        Task<T?> GetAsync<T>(string sql, Dictionary<string, object>? param = null) where T : class;

        Task<(int, List<T>)> QueryPageAsync<T>(string countSql, string sql, Dictionary<string, object>? param1 = null, Dictionary<string, object>? param2 = null, int page = 1, int size = 10) where T : class;

        Task<List<T>> QueryAsync<T>(string sql, Dictionary<string, object>? param = null) where T : class;

        Task<int> ExecuteAsync(string sql, Dictionary<string, object>? param = null);

        Task<long> BatchInsertAsync(string table, List<string> cols, List<object[]> datas);

        Task<long> BatchInsertAsync<T>(string table, List<T> datas) where T : class;

        Task<object?> GetAsync(string sql, Dictionary<string, object>? param = null);

        void RegisterAsync<T>() where T : class;
    }
}
