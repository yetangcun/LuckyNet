using Lucky.BaseModel.Model.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lucky.SysModel.Entity
{
    /// <summary>
    /// 角色
    /// </summary>
    [Table("sys_role")]
    public class SysRole : BaseFullEntity<int>
    {
        /// <summary>
        /// 角色名称
        /// </summary>
        [StringLength(40)]
        [Column("name")]
        public required string Name { get; set; }

        /// <summary>
        /// 名称对应的英文描述
        /// </summary>
        [StringLength(20)]
        [Column("word")]
        public string? Word { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        [Column("sort")]
        public int Sort { get; set; } = 0;

        /// <summary>
        /// 状态
        /// 0:禁用 1:正常
        /// </summary>
        [Column("status")]
        public int Status { get; set; } = 1;

        /// <summary>
        /// 角色描述
        /// </summary>
        [StringLength(200)]
        [Column("remark")]
        public string? Remark { get; set; }
    }
}
