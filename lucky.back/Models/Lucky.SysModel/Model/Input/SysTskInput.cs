using Lucky.BaseModel.Model;

namespace Lucky.SysModel.Model.Input
{
    /// <summary>
    /// 系统任务查询输入
    /// </summary>
    public class SysTskQueryInput : PageInfo
    {
        /// <summary>
        /// 关键字
        /// </summary>
        public string? Txt { get; set; } = string.Empty;
    }

    /// <summary>
    /// 系统任务操作输入
    /// </summary>
    public class SysTskOptInput
    {
        /// <summary>
        /// 任务ID
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// 任务名称
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// 任务代码
        /// </summary>
        public required string Code { get; set; }

        /// <summary>
        /// 任务执行表达式
        /// </summary>
        public string? Cron { get; set; }

        /// <summary>
        /// 参数模型
        /// </summary>
        public string? ParamModel { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int? Status { get; set; } = 1;

        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }
    }

    /// <summary>
    /// 系统任务记录查询输入
    /// </summary>
    public class SysTskRecordInput : PageInfo
    { 
        /// <summary>
        /// 任务ID
        /// </summary>
        public int? TskId { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int? Status { get; set; }
    }
}
