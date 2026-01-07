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
        public required string Name { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; } = 0;

        /// <summary>
        /// 状态
        /// 0:禁用 1:正常
        /// </summary>
        public int Status { get; set; } = 1;

        /// <summary>
        /// 角色描述
        /// </summary>
        [StringLength(200)]
        public string? Remark { get; set; }
    }
}
