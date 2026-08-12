using Lucky.BaseModel.Model.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lucky.SysModel.Entity
{
    /// <summary>
    /// 系统任务
    /// </summary>
    [Table("sys_tsk")]
    public class SysTsk : BaseFullEntity<int>
    {
        /// <summary>
        ///  任务名称
        /// </summary>
        [Column("name")]
        public required string Name { get; set; }

        /// <summary>
        /// 任务代码
        /// </summary>
        [Column("code")]
        public required string Code { get; set; }

        /// <summary>
        ///  任务状态
        ///  0:停用 1:启用
        /// </summary>
        [Column("status")]
        public int Status { get; set; }

        /// <summary>
        /// 任务执行表达式
        /// </summary>
        [Column("cron")]
        public string? Cron { get; set; } = "* * * * *";

        /// <summary>
        /// 任务参数
        /// </summary>
        [Column("param_model")]
        public string? ParamModel { get; set; }

        /// <summary>
        /// 任务描述
        /// </summary>
        [Column("remark")]
        public string? Remark { get; set; }
    }
}
