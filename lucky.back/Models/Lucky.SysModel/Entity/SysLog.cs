using Lucky.BaseModel.Enum;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lucky.SysModel.Entity
{
    /// <summary>
    /// 系统日志
    /// </summary>
    [Table("sys_log")]
    public class SysLog
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Key]
        [Required]
        [Comment("主键Id")]
        public long Id { get; set; }

        /// <summary>
        /// 接口地址
        /// </summary>
        [StringLength(100)]
        [Comment("接口地址")]
        public string? ReqUrl { get; set; }

        /// <summary>
        /// 请求参数
        /// </summary>
        [StringLength(400)]
        [Comment("请求参数")]
        public string? ReqParams { get; set; }

        /// <summary>
        /// 操作类型
        /// </summary>
        [Comment("操作类型")]
        public HttpOptType OptType { get; set; }

        /// <summary>
        /// 请求IP
        /// </summary>
        [StringLength(20)]
        [Comment("请求IP")]
        public string? ReqIp { get; set; }

        /// <summary>
        /// 执行状态
        /// 0:失败 
        /// 1:成功
        /// </summary>
        [Comment("执行状态")]
        public int Status { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        [StringLength(400)]
        [Comment("错误信息")]
        public string? ErrMsg { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Comment("创建时间")]
        public DateTime CreateTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 创建人
        /// </summary>
        [Comment("创建人")]
        public long CreateUid { get; set; }
    }
}
