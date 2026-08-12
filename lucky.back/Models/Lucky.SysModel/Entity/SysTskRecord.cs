using Lucky.BaseModel.Model.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lucky.SysModel.Entity
{
    /// <summary>
    /// 系统任务执行记录
    /// </summary>
    [Table("sys_tsk_record")]
    public class SysTskRecord : BaseCommonEntity<long>
    {
        /// <summary>
        /// 任务ID
        /// </summary>
        [Column("tsk_id")]
        public int TskId { get; set; }

        /// <summary>
        /// 任务执行结果
        /// 0未开始
        /// 1执行中
        /// 2成功
        /// 3失败
        /// </summary>
        [Column("status")]
        public int Status { get; set; }

        /// <summary>
        /// 任务执行结果信息
        /// 执行失败时的错误信息
        /// </summary>
        [Column("tsk_msg")]
        public string TskMsg { get; set; } = string.Empty;

        /// <summary>
        /// 任务参数
        /// </summary>
        [Column("tsk_param")]
        public string TskParam { get; set; } = string.Empty;

        /// <summary>
        /// 任务开始时间
        /// </summary>
        [Column("start_time")]
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 任务结束时间
        /// </summary>
        [Column("end_time")]
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Required]
        [Column("create_time", TypeName = "timestamp")]
        public DateTime CreateTime { get; set; }

    }
}
