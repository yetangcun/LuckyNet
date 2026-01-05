using Lucky.BaseModel.Model.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lucky.SysModel.Entity
{
    /// <summary>
    /// 系统配置
    /// </summary>
    [Table("sys_config")]
    public class SysConfig : BaseFullEntity<int>
    {
        /// <summary>
        /// 配置类型
        /// </summary>
        [StringLength(40)]
        public string? CfgType { get; set; }

        /// <summary>
        /// 配置名称
        /// </summary>
        [StringLength(40)]
        public required string Name { get; set; }

        /// <summary>
        /// 配置值
        /// </summary>
        [StringLength(10)]
        public string? Value { get; set; }

        /// <summary>
        /// 配置编码
        /// </summary>
        [StringLength(10)]
        public string? Code { get; set; }

        /// <summary>
        /// 配置排序
        /// </summary>
        public int Sort { get; set; } = 0;

        /// <summary>
        /// 状态
        /// 0:禁用 1:正常
        /// </summary>
        public int Status { get; set; } = 1;

        /// <summary>
        /// 是否系统内置
        /// 内置配置无法删除、且所有用户共享
        /// 非系统内置的配置可删除、且归创建用户所属
        /// </summary>
        public bool IsSystem { get; set; } = false;

    }
}
