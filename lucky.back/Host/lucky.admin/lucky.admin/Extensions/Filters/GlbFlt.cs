using Common.CoreLib.Extension.Common;
using Lucky.BaseModel;
using Lucky.BaseModel.Model;
using Lucky.SysModel.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text;

namespace lucky.admin.Extensions.Filters
{
    /// <summary>
    /// 记录过滤器
    /// </summary>
    public class GlbFlt : IActionFilter
    {
        private readonly ILogger<GlbFlt> _logger;
        private readonly JwtAuthExtension _jwt;


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="jwt"></param>
        public GlbFlt(ILogger<GlbFlt> logger, JwtAuthExtension jwt)
        {
            _logger = logger;
            _jwt = jwt;
        }

        /// <summary>
        /// 过滤器
        /// </summary>
        /// <param name="context"></param>
        public async void OnActionExecuted(ActionExecutedContext context)
        {
            var reqPath = context.HttpContext.Request.Path;
            var reqMethod = context.HttpContext.Request.Method;
            var reqIp = context.HttpContext.Connection.RemoteIpAddress.ToString();

            // 读取请求信息（此时 Body 仍可读取，因为已在 Executing 中开启缓冲）
            var reqBody = await ReadRequestData(context.HttpContext.Request);
            var uid = context.HttpContext.Items[GlobalConstant.U_ID];

            // 计算耗时
            var startTime = context.HttpContext.Items["ReqStartTime"] as DateTime?;
            var duration = startTime.HasValue
                ? DateTime.UtcNow - startTime.Value
                : TimeSpan.Zero;

            // ---- 4. 判断执行结果 ----
            var isSuccess = context.Exception == null || context.ExceptionHandled;
            var statusCode = context.HttpContext.Response.StatusCode;
            var errorMessage = context.Exception?.Message ?? (isSuccess ? null : "Unknown error");

            // 构造日志模型
            long.TryParse(uid?.ToString(), out long _uid);
            var logModel = new SysLog()
            {
                Id = IdGreator.GetNxtId(),
                CreateTime = DateTime.Now,
                CreateUid = _uid,
                ReqIp = reqIp,
                ReqParams = reqBody,
                ReqUrl = reqPath,
                ReqType = reqMethod,
                Status = isSuccess ? 1 : 0,
                ErrMsg = errorMessage,
            };

            if (isSuccess)
            {
                _logger.LogInformation(
                    "{Method} {Path} succeeded: {Body}, {times}",
                    reqMethod,
                    reqPath,
                    reqBody,
                    duration
                );
            }
            else
            {
                _logger.LogError(
                    new EventId(statusCode),
                    context.Exception,
                    "{Method} {Path} failed: {Error}, {times}",
                    reqMethod,
                    reqPath,
                    errorMessage,
                    duration
                );
            }
        }

        private readonly string[] _whites = { "loginHdl" };  // 不需要过滤的路径

        /// <summary>
        /// 过滤器
        /// </summary>
        /// <param name="context"></param>
        public async void OnActionExecuting(ActionExecutingContext context)
        {
            // 1. 启用 Body 缓冲，让后续能重复读取
            context.HttpContext.Request.EnableBuffering();

            // 2. 记录开始时间（用于计算耗时）
            context.HttpContext.Items["ReqStartTime"] = DateTime.UtcNow;

            if (!_whites.Any(path => context.HttpContext.Request.Path.Value.IndexOf(path) > -1))
            {
                if (!context.HttpContext.Request.Headers.Authorization.Any())
                {
                    context.HttpContext.Response.StatusCode = 401;
                    context.Result = new JsonResult(ResModel<string>.Failed("UnAuth", "未授权", 401));
                    return;
                }

                var token = context.HttpContext.Request.Headers.Authorization[0];
                if (!string.IsNullOrEmpty(token))
                {
                    token = token.Substring(7);
                    var results = await _jwt.CheckToken(token);
                    if (!results.Item4)
                    {
                        context.Result = new JsonResult(ResModel<string>.Failed("UnAuth", "未授权", 401));
                        return;
                    }

                    context.HttpContext.Items.Add(GlobalConstant.U_ID, results.Item3); // 当前用户id
                    if (results.Item1 > DateTime.Now && results.Item1.Subtract(DateTime.Now).TotalMinutes < 9) // 距离当前token失效小于9分钟,则刷新token
                    {
                        var tken = _jwt.GetToken(results.Item2, results.Item3); // context.HttpContext.Response.Headers.Add("fresh_token", tokens.Item1);
                        context.HttpContext.Response.Headers.Append("fresh_token", tken);
                    }
                }
                else
                {
                    context.HttpContext.Response.StatusCode = 401;
                    context.Result = new JsonResult(ResModel<string>.Failed("UnAuth", "未授权", 401));
                    return;
                }
            }
        }

        /// <summary>
        /// 读取请求数据（Body 或 Query），统一返回字符串
        /// </summary>
        private async Task<string?> ReadRequestData(HttpRequest request)
        {
            // 1. 如果是 POST/PUT/PATCH，读取 Body
            if (HttpMethods.IsPost(request.Method) ||
                HttpMethods.IsPut(request.Method) ||
                HttpMethods.IsPatch(request.Method))
            {
                return await ReadRequestBody(request);
            }

            // 2. 如果是 GET/DELETE/HEAD/OPTIONS 等，从 QueryString 取值
            return ReadQueryString(request);
        }

        /// <summary>
        /// 读取请求体（带截断和异常处理）
        /// </summary>
        private async Task<string?> ReadRequestBody(HttpRequest request)
        {
            try
            {
                request.Body.Position = 0;

                using var reader = new StreamReader(
                    request.Body,
                    encoding: Encoding.UTF8,
                    detectEncodingFromByteOrderMarks: false,
                    bufferSize: 1024,
                    leaveOpen: true
                );
                var body = await reader.ReadToEndAsync();

                request.Body.Position = 0;

                return Truncate(body, 4096);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to read request body");
                return "[ReadFailed]";
            }
        }

        /// <summary>
        /// 读取 QueryString（支持复杂对象格式）
        /// </summary>
        private string ReadQueryString(HttpRequest request)
        {
            try
            {
                // 方式一：直接输出原始 Query 字符串
                //var rawQuery = request.QueryString.ToString();
                //if (!string.IsNullOrEmpty(rawQuery))
                //{
                //    return Truncate(rawQuery, 4096);
                //}

                // 方式二（推荐）：将 Query 转为 JSON 对象，更结构化
                var queryDict = request.Query
                    .ToDictionary(
                        kv => kv.Key,
                        kv => kv.Value.ToString()
                    );

                return System.Text.Json.JsonSerializer.Serialize(queryDict);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to read query string");
                return "[ReadFailed]";
            }
        }

        /// <summary>
        /// 截断字符串到指定长度
        /// </summary>
        private string? Truncate(string text, int maxLength)
        {
            if (string.IsNullOrEmpty(text)) return null;
            if (text.Length <= maxLength) return text;
            return text.Substring(0, maxLength) + "... (truncated)";
        }
    }
}
