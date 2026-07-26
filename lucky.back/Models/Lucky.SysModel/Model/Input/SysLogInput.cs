
using Lucky.BaseModel.Model;

namespace Lucky.SysModel.Model.Input
{
    /// <summary>
    /// 系统日志查询参数
    /// </summary>
    public class SysLogQueryInput : PageInfo
    {
        /// <summary>
        /// 请求路径
        /// </summary>
        public string? reqPath { get; set; }

        /// <summary>
        /// 请求IP
        /// </summary>
        public string? reqIp { get; set; }

        /// <summary>
        /// 日志类型
        /// </summary>
        public string? reqType { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int? status { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? beginTime { get; set; }

        /// <summary>
        /// 截至时间
        /// </summary>
        public DateTime? endTime { get; set; }
    }
}
