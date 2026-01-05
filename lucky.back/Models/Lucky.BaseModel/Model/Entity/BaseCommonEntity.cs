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
        // [Comment("主键Id")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public required T Id { get; set; }

        /// <summary>
        /// 删除标识
        /// 0 未删
        /// 1 已删
        /// </summary>
        [Required]
        //[Comment("删除标识")]
        public bool DelMarker { get; set; } = false;
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
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 创建人id
        /// </summary>
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
        public DateTime? DelTime { get; set; }

        /// <summary>
        /// 删除人Id
        /// </summary>
        //[MaxLength(20)]
        // [Comment("删除人Id")]
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
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 创建人id
        /// </summary>
        public long CreateUid { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? ModifyTime { get; set; }

        /// <summary>
        /// 修改人Id
        /// </summary>
        [MaxLength(20)]
        public long? ModifyUid { get; set; }

        /// <summary>
        /// 删除时间
        /// </summary>
        public DateTime? DelTime { get; set; }

        /// <summary>
        /// 删除人Id
        /// </summary>
        public long? DelUid { get; set; }

    }
}
