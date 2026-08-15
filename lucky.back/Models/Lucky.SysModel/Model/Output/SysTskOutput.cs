using System.Text.Json.Serialization;

namespace Lucky.SysModel.Model.Output
{
    /// <summary>
    /// 系统任务输出
    /// </summary>
    public class SysTskOutput
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public required string name { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public required string code { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int status { get; set; }

        /// <summary>
        /// 执行策略
        /// </summary>
        public string? cron { get; set; } = "* * * * *";

        /// <summary>
        /// 参数模型
        /// </summary>
        public string? paramModel { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string? remark { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime createTime { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? updateTime { get; set; }

        /// <summary>
        /// 创建用户
        /// </summary>
        [JsonIgnore]
        public long createUserId { get; set; }

        /// <summary>
        /// 创建用户
        /// </summary>
        public string? createUser { get; set; }

        /// <summary>
        /// 更新用户
        /// </summary>
        [JsonIgnore]
        public long? updateUserId { get; set; }

        /// <summary>
        /// 更新用户
        /// </summary>
        public string? updateUser { get; set; }

    }

    /// <summary>
    ///  任务记录输出
    /// </summary>
    public class SysTskRecordOutput
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long id { get; set; }

        /// <summary>
        /// 任务Id
        /// </summary>
        public long tskId { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime startTime { get; set; }

        /// <summary>
        ///  结束时间
        /// </summary>
        public DateTime endTime { get; set; }

        /// <summary>
        /// 状态
        /// 0未开始
        /// 1执行中
        /// 2成功
        /// 3失败
        /// </summary>
        public int status { get; set; }

        /// <summary>
        /// 任务消息
        /// </summary>
        public string? tskMsg { get; set; }

        /// <summary>
        /// 任务参数
        /// </summary>
        public string? tskParam { get; set; }
    }
}
