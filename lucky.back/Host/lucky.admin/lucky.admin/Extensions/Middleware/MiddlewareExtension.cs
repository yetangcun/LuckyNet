namespace lucky.admin.Extensions.Middleware
{
    /// <summary>
    /// 中间件扩展
    /// </summary>
    public static class MiddlewareExtension
    {
        /// <summary>
        /// ip限流中间件
        /// </summary>
        /// <param name="app"></param>
        public static IApplicationBuilder UseIpLimit(this IApplicationBuilder app)
        {
            return app.UseMiddleware<IpLimitMiddleware>();
        }
    }
}
