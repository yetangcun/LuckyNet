using Lucky.BaseModel;
using Lucky.BaseModel.Model;
using Microsoft.AspNetCore.Mvc;
using Common.CoreLib.Extension.Common;
using Microsoft.AspNetCore.Mvc.Filters;

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


        private readonly string[] _whites = { "loginHdl" };  // 不需要过滤的路径

        /// <summary>
        /// 过滤器
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next"></param>
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (_whites.Any(path => context.HttpContext.Request.Path.Value.IndexOf(path) != -1)) // 白名单
            {
                await next();
            }
            else // 需要过滤
            {
                if (!context.HttpContext.Request.Headers.Authorization.Any())
                {
                    context.HttpContext.Response.StatusCode = 401;
                    context.Result = new JsonResult(ResModel<string>.Failed("UnAuth", "未授权", 401));
                    return;
                }

                var token = context.HttpContext.Request.Headers.Authorization[0];
                //if (string.IsNullOrEmpty(token))
                //{
                //    context.HttpContext.Response.StatusCode = 401;
                //    context.Result = new JsonResult(ResModel<string>.Failed("UnAuthorize", "未授权", 401));
                //    return;
                //}
                token = token.Substring(7);
                var results = await _jwt.CheckToken(token);
                if (!results.Item4)
                {
                    context.Result = new JsonResult(ResModel<string>.Failed("UnAuth", "未授权", 401));
                    return;
                }

                context.HttpContext.Items.Add(GlobalConstant.U_ID, results.Item3); // 当前用户id
                if (results.Item1 > DateTime.Now && results.Item1.Subtract(DateTime.Now).TotalMinutes < 9) // 距离当前token失效小于10分钟,则刷新token
                {
                    var tken = _jwt.GetToken(results.Item2, results.Item3); // context.HttpContext.Response.Headers.Add("fresh_token", tokens.Item1);
                    context.HttpContext.Response.Headers.Append("fresh_token", tken);
                }

                await next();
            }
        }
    }
}
