using Lucky.BaseModel.Model;
using Microsoft.AspNetCore.Mvc;
using Common.CoreLib.Extension.Common;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace lucky.admin.Extensions.Filters
{
    /// <summary>
    /// 全局过滤器
    /// </summary>
    public class GlbFilter : IAsyncActionFilter
    {

        private readonly JwtAuthExtension _jwt;

        /// <summary>
        /// 构造函数
        /// </summary>
        public GlbFilter(JwtAuthExtension jwt)
        {
            _jwt = jwt;
        }


        private readonly string[] _whites = { "LgHdlAsync" };  // 不需要过滤的路径

        /// <summary>
        /// 过滤器
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next"></param>
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (_whites.Any(path => context.HttpContext.Request.Path.Value?.IndexOf(path) == -1))
            {
                if (!context.HttpContext.Request.Headers.Authorization.Any())
                {
                    context.HttpContext.Response.StatusCode = 401;
                    context.Result = new JsonResult(ResModel<string>.Failed("UnAuthorize", "未授权", 401));
                    return;
                }

                var token = context.HttpContext.Request.Headers.Authorization[0];
                if (!string.IsNullOrEmpty(token))
                {
                    token = token.Substring(7);
                    var results = await _jwt.CheckToken(token);
                    if (!results.Item4)
                    {
                        context.Result = new JsonResult(ResModel<string>.Failed("UnAuthorize", "未授权", 401));
                        return;
                    }

                    context.HttpContext.Items.Add("usrkey", results.Item3); // 当前用户id
                    if (results.Item1 > DateTime.Now && results.Item1.Subtract(DateTime.Now).TotalMinutes < 9) // 距离当前token失效小于10分钟,则刷新token
                    {
                        var tokens = _jwt.GetToken(results.Item2, results.Item3); // context.HttpContext.Response.Headers.Add("fresh_token", tokens.Item1);
                        context.HttpContext.Response.Headers.Append("fresh_token", tokens.Item1);
                    }
                }
                else
                {
                    context.HttpContext.Response.StatusCode = 401;
                    context.Result = new JsonResult(ResModel<string>.Failed("UnAuthorize", "未授权", 401));
                    return;
                }
            }
        }
    }

    #region Ip限流中间件
    /// <summary>
    /// Ip限流中间件
    /// </summary>
    public class IpLimitMiddleware
    {
        private readonly int _maxRequests;  // 最大请求次数
        private readonly TimeSpan _timeOut;  // 过期时间
        private readonly RequestDelegate _next;
        private readonly IMemoryCache _memoryCache;  // 使用内存缓存来存储IP请求次数

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="next"></param>
        /// <param name="memoryCache"></param>
        /// <param name="options"></param>
        public IpLimitMiddleware(RequestDelegate next, IMemoryCache memoryCache, IOptions<IpLimitOptions> options)
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
