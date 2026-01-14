using StackExchange.Redis;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Common.CoreLib.Model.Option;
using Common.CoreLib.Extension.Common;

namespace Common.CoreLib.Cache
{
    /// <summary>
    /// 分布式缓存实现
    /// </summary>
    public class DistributeCache : IDistributeCache
    {
        private readonly RdisOption _option;
        private readonly ILogger<DistributeCache> _logger;
        private readonly ConfigurationOptions _redisConfig;

        private volatile IDatabase? _database;
        private readonly object _locker = new object();
        private volatile IConnectionMultiplexer? _redisConnection;

        /// <summary>
        /// 构造函数
        /// </summary>
        public DistributeCache(IOptions<RdisOption> options, ILogger<DistributeCache> logger)
        {
            _option = options.Value;
            _logger = logger;

            // redis配置项
            var host = _option.host ?? "127.0.0.1";
            _redisConfig = new ConfigurationOptions()
            {
                EndPoints =
                {
                    { host, _option.port }
                },
                Password = _option.pwd,
                DefaultDatabase = _option.dbNo,
                ClientName = _option.clientName,
                ConnectTimeout = _option.timeOut * 1000
            };

            InitRedis();
        }

        private void InitRedis()
        {
            if (_redisConnection != null && _redisConnection.IsConnected)
            {
                _database = _redisConnection.GetDatabase();
                return;
            }

            lock (_locker)
            {
                if (_redisConnection != null)
                    _redisConnection.Dispose();
                try
                {
                    _redisConnection = ConnectionMultiplexer.Connect(_redisConfig);
                    _database = _redisConnection.GetDatabase();
                }
                catch (Exception ex)
                {
                    _logger.LogError($"GetRedisConnection: {ex.Message},{ex.InnerException},{ex.StackTrace}");
                    _database = null;
                }
            }
        }

        #region 操作方法
        /// <summary>
        /// 判断缓存是否存在
        /// </summary>
        /// <param name="key"></param>
        public async Task<bool> ExistKey(string key)
        {
            if (_database == null)
                InitRedis();

            if (_database == null)
                return false;

            var res = await _database.KeyExistsAsync(key);
            return res;
        }

        /// <summary>
        /// 设置字符串值缓存
        /// </summary>
        public async Task<bool> SetStrCache(string key, string val)
        {
            if (_database == null)
                InitRedis();

            if (_database == null)
                return false;

            await _database.StringSetAsync(key, val);
            return true;
        }

        /// <summary>
        /// 设置字符串值缓存
        /// 带过期时间
        /// </summary>
        public async Task<bool> SetStrCache(string key, string val, TimeSpan ts)
        {
            if (_database == null)
                InitRedis();

            if (_database == null)
                return false;

            await _database.StringSetAsync(key, val, ts);
            return true;
        }

        /// <summary>
        /// 获取字符串值缓存
        /// </summary>
        public async Task<string?> GetStrCache(string key)
        {
            if (_database == null)
                InitRedis();

            if (_database == null)
                return string.Empty;

            var val = await _database.StringGetAsync(key);
            return val.HasValue ? val.ToString() : null;
        }

        /// <summary>
        /// 设置泛型值缓存
        /// </summary>
        public async Task<bool> SetTCache<T>(string key, T data)
        {
            if (_database == null)
                InitRedis();

            if (_database == null)
                return false;

            var val = data.ToJson();
            await _database.StringSetAsync(key, val);
            return false;
        }

        /// <summary>
        /// 设置泛型值缓存
        /// </summary>
        public async Task<bool> SetTCache<T>(string key, T data, TimeSpan ts)
        {
            if (_database == null)
                InitRedis();

            if (_database == null)
                return false;

            var val = data.ToJson();
            await _database.StringSetAsync(key, val, ts);
            return false;
        }

        /// <summary>
        /// 获取泛型值缓存
        /// </summary>
        public async Task<T?> GetTCache<T>(string key)
        {
            if (_database == null)
                InitRedis();

            if (_database == null)
                return default;

            var val = await _database.StringGetAsync(key);
            if (!val.IsNullOrEmpty)
            {
                var tObj = (val.ToString()).ToObj<T>();
                return tObj;
            }
            return default;
        }

        /// <summary>
        /// 删除缓存
        /// </summary>
        public async Task<bool> RemoveCache(string key)
        {
            if (_database == null)
                InitRedis();

            if (_database == null)
                return false;

            return await _database.KeyDeleteAsync(key);
        }

        /// <summary>
        /// 清空缓存
        /// </summary>
        public async void ClearAsync()
        {
            if (_database == null)
                InitRedis();

            if (_database == null)
                return;

            if (_redisConnection != null)
            {
                var endPoint = _redisConnection.GetEndPoints();
                var server = _redisConnection.GetServer(endPoint[0]);
                foreach (var key in server.Keys())
                {
                    await _database.KeyDeleteAsync(key);
                }
            }
        }

        /// <summary>
        /// 添加Hash缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="field"></param>
        /// <param name="value"></param>
        public async Task<bool> HashSet(string key, string field, string value)
        {
            if (_database == null)
                InitRedis();

            if (_database == null)
                return false;

            return await _database.HashSetAsync(key, field, value);
        }

        /// <summary>
        /// 批量添加Hash缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="kvs"></param>
        public async Task HashSets(string key, Dictionary<string, string> kvs)
        {
            if (_database == null)
                InitRedis();

            if (_database == null)
                return;

            var entrys = new HashEntry[kvs.Count];
            foreach (var kv in kvs)
            {
                entrys.Append(new HashEntry(kv.Key, kv.Value));
            }

            await _database.HashSetAsync(key, entrys);
        }

        /// <summary>
        /// 获取Hash缓存
        /// </summary>
        public async Task<string?> HashGet(string key, string field)
        {
            if (_database == null)
                InitRedis();
            if (_database == null)
                return null;

            return await _database.HashGetAsync(key, field);
        }

        /// <summary>
        /// 获取Hash Key下面的所有字段
        /// 谨慎使用 
        /// </summary>
        public async Task<HashEntry[]?> HashGets(string key, string field)
        {
            if (_database == null)
                InitRedis();
            if (_database == null)
                return null;

            return await _database.HashGetAllAsync(key);
        }

        /// <summary>
        /// 批量获取Hash缓存
        /// </summary>
        public async Task<Dictionary<string, string>?> PipelineBatchGets(string hashKey, params string[] fields)
        {
            if (_database == null)
                InitRedis();
            if (_database == null)
                return null;

            var batch = _database.CreateBatch(); // 创建批处理对象

            // 将多个操作加入批处理
            var tsks = new Task<RedisValue>[fields.Length];
            for (int i = 0; i < fields.Length; i++)
            {
                var field = fields[i];
                var tsk = batch.HashGetAsync(hashKey, field);
                tsks.Append(tsk);
            }

            batch.Execute(); // 一次性发送所有命令

            // 等待所有结果
            await Task.WhenAll(tsks);

            var dic = new Dictionary<string, string>();
            for (int i = 0; i < tsks.Length; i++)
            {
                var tsk = tsks[i];
                var val = await tsk;
                dic.Add(fields[i], val.ToString());
            }

            return dic;
        }

        /// <summary>
        /// 删除Hash缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="field"></param>
        public async Task<bool> HashDelete(string key, string field)
        {
            if (_database == null)
                InitRedis();
            if (_database == null) return false;
            return await _database.HashDeleteAsync(key, field);
        }
        #endregion

        /// <summary>
        /// 是否管理员
        /// </summary>
        //public async Task<bool> IsAdmin(string? uid)
        //{
        //    var userInfo = await GetTCache<UserCacheModel>($"{CommonConstModel.Prefix_UserInfoCaches}{uid}");
        //    if (!string.IsNullOrWhiteSpace(uid) && userInfo != null)
        //        return userInfo.RoleName == "超级管理员";

        //    return false;
        //}

        #region 分布式锁

        // lckVal: 锁值, 可以为任意值，但是必须与释放锁时一致，建议为machineId
        public async Task<bool> AcquireLck(string lckKey, string lckVal, int timeoutSeconds) // 加锁
        {
            if (_database == null)
                InitRedis();

            if (_database == null)
                return false;

            // 使用Lua脚本确保设置锁和过期时间是原子操作
            var luaScript = @"
                    if redis.call('setnx', KEYS[1], ARGV[1]) == 1 then
                        redis.call('expire', KEYS[1], ARGV[2])
                        return 1
                    else
                        return 0
                    end
               ";

            // 参数：锁的键，客户端ID（保证锁的唯一性），锁的过期时间
            RedisKey[] keys = { lckKey };
            RedisValue[] values = { lckVal, timeoutSeconds };

            // 执行Lua脚本
            var res = await _database.ScriptEvaluateAsync(
                luaScript,
                keys,
                values);

            return (int)res > 0;
        }

        /// <summary>
        /// 释放锁
        /// </summary>
        /// <param name="lckKey"></param>
        /// <param name="lckVal"></param>
        public async Task<bool> ReleaseLck(string lckKey, string lckVal) // 释放锁
        {
            if (_database == null)
                InitRedis();

            if (_database == null)
                return false;

            // 使用Lua脚本确保只有当锁的持有者是当前客户端时才删除锁
            var lua_script = @"
                    if redis.call('get', KEYS[1]) == ARGV[1] then
                        return redis.call('del', KEYS[1])
                    else
                        return 0
                    end";

            RedisKey[] keys = { lckKey };
            RedisValue[] values = { lckVal };

            // 执行Lua脚本并返回是否成功释放了锁
            var res = await _database.ScriptEvaluateAsync(
                lua_script,
                keys,
                values);

            return (int)res > 0;
        }

        #endregion

        #region 分布式锁1
        /// <summary>
        /// 加锁
        /// </summary>
        /// <param name="lockKey"></param>
        /// <param name="lockVal"></param>
        /// <param name="timeoutSeconds"></param>
        public async Task<bool> AcquireLck1(string lockKey, string lockVal, int timeoutSeconds) // 加锁
        { 
            if (_database == null)
                InitRedis();

            if (_database == null)
                return false;

            return await _database.StringSetAsync(lockKey, lockVal, TimeSpan.FromSeconds(timeoutSeconds), When.NotExists);
        }

        /// <summary>
        /// 释放锁
        /// </summary>
        /// <param name="lockKey"></param>
        public async Task<bool> ReleaseLck1(string lockKey) // 释放锁
        {
            if (_database == null)
                InitRedis();
            if (_database == null)
                return false;

            return await _database.KeyDeleteAsync(lockKey);
        }

        /// <summary>
        /// 尝试获取锁
        /// </summary>
        /// <param name="lockKey"></param>
        /// <param name="lockVal"></param>
        /// <param name="timeoutSeconds"></param>
        public async Task<bool> AcquireLck11(string lockKey, string lockVal, int timeoutSeconds) // 多次尝试获取锁
        {
            if (_database == null)
                InitRedis();
            if (_database == null)
                return false;

            var trys = 3; // 尝试次数

            while (trys > 0)
            {
                trys--;
                var resLck = await _database.LockTakeAsync(lockKey, lockVal, TimeSpan.FromSeconds(timeoutSeconds));
                if (resLck)
                    return true;
                await Task.Delay(TimeSpan.FromMilliseconds(new Random(Guid.NewGuid().GetHashCode()).Next(30, 70)));
            }

            return false;
        }
        /// <summary>
        /// 脚本方式释放锁
        /// </summary>
        /// <param name="lckKey"></param>
        /// <param name="lckVal"></param>
        public async Task<bool> ReleaseLck11(string lckKey, string lckVal) // 脚本方式释放锁
        {
            if (_database == null)
                InitRedis();
            if (_database == null)
                return false;

            var luaScript = @"
                if redis.call('GET',KEYS[1]) == ARGV[1] then
                    return redis.call('DEL',KEYS[1])
                else
                    return 0
                end";

            var res = await _database.ScriptEvaluateAsync(luaScript, new RedisKey[] { lckKey }, new RedisValue[] { lckVal });
            return (int)res > 0;
        }
        #endregion

        #region 分布式锁2
        /// <summary>
        /// 获取锁
        /// </summary>
        /// <param name="lockKey"></param>
        /// <param name="lockVal"></param>
        /// <param name="timeoutSeconds"></param>
        public async Task<bool> AcquireLck2(string lockKey, string lockVal, int timeoutSeconds) // 加锁
        {
            if (_database == null)
                InitRedis();
            if (_database == null)
                return false;

            return await _database.LockTakeAsync(lockKey, lockVal, TimeSpan.FromSeconds(timeoutSeconds));
        }

        /// <summary>
        /// 释放锁
        /// </summary>
        /// <param name="lockKey"></param>
        /// <param name="lockVal"></param>
        public async Task<bool> ReleaseLck2(string lockKey, string lockVal) // 释放锁
        {
            if (_database == null)
                InitRedis();
            if (_database == null)
                return false;

            return await _database.LockReleaseAsync(lockKey, lockVal);
        }
        #endregion
    }
}
