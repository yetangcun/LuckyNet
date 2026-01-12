using Lucky.BaseModel;
using Microsoft.AspNetCore.Http;
using System.Text.RegularExpressions;

namespace Lucky.BaseService.Extension
{
    public static class HttpContextExtension
    {
        /// <summary>
        /// 获取客户端IP
        /// </summary>
        /// <param name="context"></param>
        public static string GetClientIp(this HttpContext context)
        {
            if (context == null) return "";
            var ip = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();

            if (string.IsNullOrEmpty(ip))
                ip = context.Connection.RemoteIpAddress?.ToString();

            if (string.IsNullOrEmpty(ip))
                throw new Exception("获取IP失败");

            if (ip.Contains("::1"))
                ip = "127.0.0.1";

            // result = result.Replace("::ffff:", "");
            ip = ip.Split(':')?.FirstOrDefault() ?? "127.0.0.1";
            ip = IsIp(ip) ? ip : "127.0.0.1";
            return ip;
        }

        /// <summary>
        /// 判断是否IP
        /// </summary>
        /// <param name="ip"></param>
        public static bool IsIp(string ip)
        {
            return Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
        }

        /// <summary>
        /// 获取当前用户id
        /// </summary>
        /// <param name="context"></param>
        public static long GetUid(this HttpContext context)
        {
            var uid = context.Items[GlobalConstant.U_ID]!.ToString();
            return !string.IsNullOrEmpty(uid) ? long.Parse(uid) : 0;
        }

    }
}
