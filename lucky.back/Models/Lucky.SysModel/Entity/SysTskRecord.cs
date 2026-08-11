using Lucky.BaseModel.Model.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lucky.SysModel.Entity
{
    /// <summary>
    /// 系统任务执行记录
    /// </summary>
    [Table("sys_tsk_record")]
    public class SysTskRecord : BaseCreateEntity<long>
    {
        /// <summary>
        /// 任务ID
        /// </summary>
        [Column("tsk_id")]
        public int TskId { get; set; }

        /// <summary>
        /// 任务执行结果
        /// 0:失败 1:成功
        /// </summary>
        [Column("tsk_result")]
        public int TskResult { get; set; }

        /// <summary>
        /// 任务执行结果信息
        /// 执行失败时的错误信息
        /// </summary>
        [Column("tsk_msg")]
        public string TskMsg { get; set; } = string.Empty;

        /// <summary>
        /// 任务参数
        /// </summary>
        [Column("tsk_params")]
        public string TskParams { get; set; } = string.Empty;
    }
}
