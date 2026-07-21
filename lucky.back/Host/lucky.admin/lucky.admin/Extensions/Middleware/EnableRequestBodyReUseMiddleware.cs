namespace lucky.admin.Extensions.Middleware
{
    /// <summary>
    /// 启用请求体缓存
    /// </summary>
    public class EnableRequestBodyReUseMiddleware
    {
        private readonly RequestDelegate _next;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="next"></param>
        public EnableRequestBodyReUseMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// 中间件处理逻辑
        /// </summary>
        /// <param name="context"></param>
        public async Task InvokeAsync(HttpContext context)
        {
            // 在模型绑定之前就开启缓冲，这样 Body 可以被多次读取
            if (context.Request.Body != null && context.Request.ContentLength < 1048576)
            {
                context.Request.EnableBuffering();
            }

            await _next(context);
        }
    }
}
