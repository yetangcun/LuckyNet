using StackExchange.Redis;

namespace Common.CoreLib.Cache
{
    /// <summary>
    /// 分布式缓存接口
    /// </summary>
    public interface IDistributeCache
    {
        /// <summary>
        /// 检测缓存是否存在
        /// </summary>
        Task<bool> ExistKey(string key);

        /// <summary>
        /// 设置字符串缓存
        /// </summary>
        Task<bool> SetStrCache(string key, string val);

        /// <summary>
        /// 设置字符串缓存
        /// </summary>
        Task<bool> SetStrCache(string key, string val, TimeSpan ts);

        /// <summary>
        /// 获取字符串缓存
        /// </summary>
        Task<string?> GetStrCache(string key);

        /// <summary>
        /// 设置泛型值缓存
        /// </summary>
        Task<bool> SetTCache<T>(string key, T data);

        /// <summary>
        /// 设置泛型值缓存
        /// </summary>
        Task<bool> SetTCache<T>(string key, T data, TimeSpan ts);

        /// <summary>
        /// 获取泛型值缓存
        /// </summary>
        Task<T?> GetTCache<T>(string key);

        /// <summary>
        /// 删除缓存
        /// </summary>
        Task<bool> RemoveCache(string key);

        /// <summary>
        /// 清空缓存
        /// </summary>
        void ClearAsync();

        /// <summary>
        /// 添加Hash缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="field"></param>
        /// <param name="value"></param>
        Task<bool> HashSet(string key, string field, string value);

        /// <summary>
        /// 批量添加Hash缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="kvs"></param>
        Task HashSets(string key, Dictionary<string, string> kvs);

        /// <summary>
        /// 获取Hash缓存
        /// </summary>
        Task<string?> HashGet(string key, string field);

        /// <summary>
        /// 获取Hash Key下面的所有字段
        /// 谨慎使用 
        /// </summary>
        Task<HashEntry[]?> HashGets(string key, string field);

        /// <summary>
        /// 批量获取Hash缓存
        /// </summary>
        Task<Dictionary<string, string>?> PipelineBatchGets(string hashKey, params string[] fields);

        /// 获取分布式锁
        Task<bool> AcquireLck(string lckKey, string lckVal, int timeoutSeconds);
        /// <summary>
        /// 释放分布式锁
        /// </summary>
        Task<bool> ReleaseLck(string lckKey, string lckVal);

        /// <summary>
        /// 获取分布式锁
        /// </summary>
        Task<bool> AcquireLck1(string lockKey, string lockVal, int timeoutSeconds); // 加锁
        /// <summary>
        /// 释放分布式锁
        /// </summary>
        /// <param name="lockKey"></param>
        Task<bool> ReleaseLck1(string lockKey);

        /// <summary>
        /// 获取分布式锁
        /// </summary>
        /// <param name="lockKey"></param>
        /// <param name="lockVal"></param>
        /// <param name="timeoutSeconds"></param>
        Task<bool> AcquireLck2(string lockKey, string lockVal, int timeoutSeconds);
        /// <summary>
        /// 释放分布式锁
        /// </summary>
        Task<bool> ReleaseLck2(string lockKey, string lockVal);

        /// <summary>
        /// 获取分布式锁
        /// </summary>
        Task<bool> AcquireLck11(string lockKey, string lockVal, int timeoutSeconds);

        /// <summary>
        /// 释放分布式锁
        /// </summary>
        Task<bool> ReleaseLck11(string lockKey, string lockVal);

    }
}
