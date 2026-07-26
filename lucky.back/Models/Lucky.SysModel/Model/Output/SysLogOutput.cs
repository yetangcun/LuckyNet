
namespace Lucky.SysModel.Model.Output
{
    /// <summary>
    /// 系统日志输出
    /// </summary>
    public class SysLogOutput
    {
        /// <summary>
        /// 日志ID
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 请求URL
        /// </summary>
        public string? reqUrl { get; set; }

        /// <summary>
        /// 请求方式
        /// </summary>
        public string? reqType { get; set; }

        /// <summary>
        /// 请求参数
        /// </summary>
        public string? reqParam { get; set; }

        /// <summary>
        /// 请求IP
        /// </summary>
        public string? reqIp { get; set; }

        /// <summary>
        /// 请求用户
        /// </summary>
        public string? reqUser { get; set; }

        /// <summary>
        /// 请求时间
        /// </summary>
        public DateTime reqTime { get; set; }

        /// <summary>
        /// 请求耗时
        /// 毫秒
        /// </summary>
        public decimal reqDuration { get; set; } = 0;

        /// <summary>
        /// 响应结果
        /// 1成功 0失败
        /// </summary>
        public int status { get; set; }

        /// <summary>
        /// 异常信息
        /// </summary>
        public string? msg { get; set; }
    }
}
