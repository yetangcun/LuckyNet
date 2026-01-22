using Microsoft.Extensions.Options;
using Microsoft.Extensions.Caching.Memory;

namespace lucky.admin.Extensions.Middleware
{
    #region Ip限流中间件
    /// <summary>
    /// Ip限流中间件
    /// </summary>
    public class IpLimitMiddleware
    {
        private readonly int _maxRequests;  // 最大请求次数
        private readonly TimeSpan _timeOut;  // 过期时间
        private readonly RequestDelegate _next; // 下一个中间件
        private readonly IMemoryCache _memoryCache;  // 使用内存缓存来存储IP请求次数

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="next"></param>
        /// <param name="options"></param>
        /// <param name="memoryCache"></param>
        public IpLimitMiddleware(RequestDelegate next, IOptions<IpLimitOptions> options, IMemoryCache memoryCache)
        {
            _next = next;
            _memoryCache = memoryCache;
            _timeOut = options.Value.TimeOuts;
            _maxRequests = options.Value.MaxReqCounts;
        }

        /// <summary>
        /// 中间件逻辑
        /// </summary>
        /// <param name="context"></param>
        public async Task InvokeAsync(HttpContext context)
        {
            var cltIp = context.Connection.RemoteIpAddress?.ToString();
            if (string.IsNullOrEmpty(cltIp))
            {
                // 如果获取不到客户端IP，则放行
                await _next(context);
                return;
            }

            var reqCountKey = $"IpLimit:{cltIp}";
            if (!_memoryCache.TryGetValue(reqCountKey, out int reqCounts))
            {
                reqCounts = 0;
            }

            // 请求次数累加
            reqCounts++;

            // 检查是否超过限制
            if (reqCounts > _maxRequests)
            {
                // 若超过限制，则根据策略进行处理，比如返回429 Too Many Requests状态码
                context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                await context.Response.WriteAsync($"too many requests from the {cltIp}.");
                return;
            }

            // 设置缓存过期时间
            _memoryCache.Set(reqCountKey, reqCounts, _timeOut);

            // 假如没有超过限制，则继续处理请求
            await _next(context);
        }
    }

    // 还需要定义一个配置类IpRateLimitOptions来存储限流策略
    /// <summary>
    /// 限流配置
    /// </summary>
    public class IpLimitOptions
    {
        /// <summary>
        /// 最大请求次数
        /// </summary>
        public int MaxReqCounts { get; set; }

        /// <summary>
        /// 超时时间
        /// </summary>
        public TimeSpan TimeOuts { get; set; }
    }
    #endregion
}
