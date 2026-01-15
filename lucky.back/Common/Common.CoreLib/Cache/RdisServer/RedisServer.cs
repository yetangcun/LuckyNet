using CSRedis;
using Microsoft.Extensions.Configuration;

namespace Alot2.Admin.Common.Cache
{
    /// <summary>
    /// Redis服务器
    /// </summary>
    public class RedisServer
    {
        /// <summary>
        /// Redis缓存服务器
        /// </summary>
        public static CSRedisClient Cache;

        ///// <summary>
        ///// RedisSession服务器
        ///// </summary>
        // public static CSRedisClient Session;

        /// <summary>
        /// 初始化Redis服务器
        /// </summary>
        /// <param name="cfg"></param>
        public static void Init(IConfiguration cfg)
        {
            Cache = new CSRedisClient(cfg.GetValue<string>("RedisServr:Cache"));
            // Session = new CSRedisClient(cfg.GetValue<string>("RedisServr:Session"));
            // var deviceId = cfg.GetValue<string>("DeviceId");
            // Cache.HMSetAsync($"dev:{deviceId}", new object[]
            //           {
            //                    "memory", "spaceInfo.memory".ToString(),
            //                    "remaining", "spaceInfo.remainingMemory".ToString()
            //           });
        }
    }
}
