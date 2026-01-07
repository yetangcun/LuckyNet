using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lucky.BaseModel.Model.Entity
{
    /// <summary>
    /// 通用实体
    /// </summary>
    public class BaseCommonEntity<T>
    {
        /// <summary>
        /// 主键Id 
        /// Guid
        /// </summary>
        [Key]
        [Required]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public required T Id { get; set; }

        /// <summary>
        /// 是否删除
        /// 0 未删
        /// 1 已删
        /// </summary>
        [Required]
        [Column("is_del")]
        public bool IsDel { get; set; } = false;
    }

    /// <summary>
    /// 基本创建
    /// </summary>
    public class BaseCreateEntity<T> : BaseCommonEntity<T>
    {
        /// <summary>
        /// 创建时间
        /// </summary>
        [Required]
        [Column("create_time")]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 创建人id
        /// </summary>
        [Column("create_uid")]
        public long CreateUid { get; set; }

    }

    /// <summary>
    /// 基本删除
    /// </summary>
    public class BaseDelEntity<T> : BaseCommonEntity<T>
    {
        /// <summary>
        /// 删除时间
        /// </summary>
        //[Comment("删除时间")]
        [Column("del_time")]
        public DateTime? DelTime { get; set; }

        /// <summary>
        /// 删除人Id
        /// </summary>
        //[MaxLength(20)]
        // [Comment("删除人Id")]
        [Column("del_uid")]
        public long? DelUid { get; set; }

    }

    /// <summary>
    /// 基本完整
    /// </summary>
    public class BaseFullEntity<T> : BaseCommonEntity<T>
    {
        /// <summary>
        /// 创建时间
        /// </summary>
        [Column("create_time")]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 创建人id
        /// </summary>
        [Column("create_uid")]
        public long CreateUid { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        [Column("update_time")]
        public DateTime? UpdateTime { get; set; }

        /// <summary>
        /// 修改人Id
        /// </summary>
        [Column("update_uid")]
        public long? UpdateUid { get; set; }

        /// <summary>
        /// 删除时间
        /// </summary>
        [Column("del_time")]
        public DateTime? DelTime { get; set; }

        /// <summary>
        /// 删除人Id
        /// </summary>
        [Column("del_uid")]
        public long? DelUid { get; set; }

    }
}
