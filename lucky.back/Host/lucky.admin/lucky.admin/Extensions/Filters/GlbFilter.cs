using Common.CoreLib.Extension.Common;
using Lucky.BaseModel;
using Lucky.BaseModel.Model;
using Lucky.SysModel.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text;
using System.Threading.Channels;

namespace lucky.admin.Extensions.Filters
{
    /// <summary>
    /// 全局过滤器
    /// </summary>
    public class GlbFilter : IAsyncActionFilter
    {

        private readonly JwtAuthExtension _jwt;
        private readonly ILogger<GlbFilter> _logger;
        private readonly Channel<SysLog> _channel;

        /// <summary>
        /// 构造函数
        /// </summary>
        public GlbFilter(JwtAuthExtension jwt, ILogger<GlbFilter> logger, ChannelExtension channel)
        {
            _jwt = jwt;
            _logger = logger;
            _channel = channel.GetOrCreate<SysLog>("SysLogChannel");
        }


        private readonly string[] _whites = { "loginHdl" };  // 不需要过滤的路径

        /// <summary>
        /// 过滤器
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next"></param>
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // ============ Executing 阶段 ============
            context.HttpContext.Items["ReqStartTime"] = DateTime.UtcNow;

            if (!_whites.Any(path => context.HttpContext.Request.Path.Value.IndexOf(path) != -1)) // 需要过滤
            {
                if (!context.HttpContext.Request.Headers.Authorization.Any())
                {
                    context.HttpContext.Response.StatusCode = 401;
                    context.Result = new JsonResult(ResModel<string>.Failed("UnAuth", "未授权", 401));
                    await GlbLog(context, null);
                    return;
                }

                var token = context.HttpContext.Request.Headers.Authorization[0]; token = token.Substring(7);
                var results = await _jwt.CheckToken(token);
                if (!results.Item4)
                {
                    context.Result = new JsonResult(ResModel<string>.Failed("UnAuth", "未授权", 401));
                    await GlbLog(context, null);
                    return;
                }

                context.HttpContext.Items.Add(GlobalConstant.U_ID, results.Item3); // 当前用户id
                if (results.Item1 > DateTime.Now && results.Item1.Subtract(DateTime.Now).TotalMinutes < 9) // 距离当前token失效小于10分钟,则刷新token
                {
                    var tken = _jwt.GetToken(results.Item2, results.Item3); // context.HttpContext.Response.Headers.Add("fresh_token", tokens.Item1);
                    context.HttpContext.Response.Headers.Append("fresh_token", tken);
                }
            }

            var res = await next(); 
            await GlbLog(context, res);
        }

        private async Task GlbLog(ActionExecutingContext context, ActionExecutedContext? res)
        {
            // 记录日志
            var reqPath = context.HttpContext.Request.Path;
            var reqMethod = context.HttpContext.Request.Method;
            var reqIp = context.HttpContext.Connection.RemoteIpAddress.ToString();

            // 读取请求信息（此时 Body 仍可读取，因为已在 Executing 中开启缓冲）
            var reqBody = await ReadRequestData(context);
            var uid = context.HttpContext.Items[GlobalConstant.U_ID];

            // 计算耗时
            var startTime = context.HttpContext.Items["ReqStartTime"] as DateTime?;
            var duration = startTime.HasValue
                ? DateTime.UtcNow - startTime.Value
                : TimeSpan.Zero;

            // 判断执行结果 ----
            var isNull = res == null;
            var statusCode = isNull ? 401 : res.HttpContext.Response.StatusCode;
            var isSuccess = isNull ? false : res.Exception == null || res.ExceptionHandled;
            var errorMsg = isNull ? "UnAuth" : (isSuccess ? null : res.Exception?.Message);
            //var errorMsg = isNull ? "UnAuth" : res.Exception?.Message ?? (isSuccess ? null : "Unknown error");

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
                ErrMsg = errorMsg,
                ExecTime = Convert.ToDecimal(duration.TotalMilliseconds),
            };
            try
            {
                await _channel.Writer.WriteAsync(logModel);  // 写入日志
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "日志写入通道失败");
            }

            if (!isSuccess)
                _logger.LogError($"【{reqMethod}】-【{reqPath}】：{errorMsg},{duration}");

            //if (isSuccess)
            //{
            //    _logger.LogInformation(
            //        "{Method} {Path} succeeded: {Body}, {times}",
            //        reqMethod,
            //        reqPath,
            //        reqBody,
            //        duration
            //    );
            //}
            //else
            //{
            //    _logger.LogError(
            //        new EventId(statusCode),
            //        new Exception(errorMessage),
            //        "{Method} {Path} failed: {Error}, {times}",
            //        reqMethod,
            //        reqPath,
            //        errorMessage,
            //        duration
            //    );
            //}

            #region 模拟消费
            //var tsk = new Task(async () =>
            //{
            //    var lst = new List<SysLog>();
            //    var counts = 0;
            //    while (_channel.Reader.Count > 0)
            //    {
            //        var log = await _channel.Reader.ReadAsync();
            //        if (log != null) lst.Add(log);

            //        if (lst.Count >= 100 || _channel.Reader.Count < 1)
            //        {
            //            lst.Clear();
            //            counts = 0;
            //        }
            //        counts++;
            //    }
            //    var nums = _channel.Reader.Count;
            //});
            //tsk.Start();
            #endregion
        }


        /// <summary>
        /// 读取请求数据（Body 或 Query），统一返回字符串
        /// </summary>
        private async Task<string?> ReadRequestData(ActionExecutingContext context)
        {
            // 如果是 POST/PUT/PATCH，读取 Body
            string method = context.HttpContext.Request.Method;
            if ((HttpMethods.IsPost(method) || HttpMethods.IsPut(method)) && context.ActionArguments != null)  // || HttpMethods.IsPatch(method)
            {
                var body = context.ActionArguments;

                // 获取当前 Action 的参数描述信息
                var parameterDescriptors = context.ActionDescriptor.Parameters;

                // 创建一个新字典，只存放非 [FromServices] 的参数
                var filteredArguments = new Dictionary<string, object?>();

                foreach (var arg in context.ActionArguments)
                {
                    // 根据参数名匹配对应的描述信息
                    var paramDesc = parameterDescriptors.FirstOrDefault(p => p.Name == arg.Key);

                    // 判断绑定源是否为 Services（即 [FromServices] 注入的参数）
                    bool isFromServices = paramDesc?.BindingInfo?.BindingSource == BindingSource.Services;

                    // 如果不是 [FromServices]，则加入过滤后的字典
                    if (!isFromServices)
                    {
                        filteredArguments.Add(arg.Key, arg.Value);
                    }
                }

                // 4. 序列化过滤后的字典
                if (filteredArguments.Count == 0)
                {
                    return string.Empty;
                }

                return filteredArguments.ToJson();
            }

            // 2. 如果是 GET/DELETE/HEAD/OPTIONS 等，从 QueryString 取值
            return ReadQueryString(context.HttpContext.Request);
        }

        /// <summary>
        /// 读取 QueryString（支持复杂对象格式）
        /// </summary>
        private string ReadQueryString(HttpRequest request)
        {
            try
            {
                // 将 Query 转为 JSON，结构化
                var queryDict = request.Query
                    .ToDictionary(
                        kv => kv.Key,
                        kv => kv.Value.ToString()
                    );

                return queryDict.ToJson();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Read query failed:{ex.Message}--{ex.StackTrace}--{ex.InnerException}");
                return "";
            }
        }
    }
}
