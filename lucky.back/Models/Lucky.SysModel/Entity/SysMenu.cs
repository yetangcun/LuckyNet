using Lucky.BaseModel.Enum;
using Lucky.BaseModel.Model.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lucky.SysModel.Entity
{
    /// <summary>
    /// 菜单
    /// </summary>
    [Table("sys_menu")]
    public class SysMenu : BaseFullEntity<int>
    {
        /// <summary>
        /// 名称
        /// </summary>
        [StringLength(40)]
        [Required]
        [Column("name")]
        public required string Name { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        [StringLength(10)]
        [Column("code")]
        public string? Code { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        [StringLength(20)]
        [Column("icon")]
        public string? Icon { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        [StringLength(10)]
        [Column("icon_size")]
        public string? IconSize { get; set; }

        /// <summary>
        /// 路由地址
        /// </summary>
        [StringLength(60)]
        [Column("path")]
        public string? Path { get; set; }

        /// <summary>
        /// 父级Id
        /// </summary>
        [Column("parent_id")]
        public int? ParentId { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        [Column("sort")]
        public int? Sort { get; set; }

        /// <summary>
        /// 菜单类型
        /// </summary>
        [Column("menu_type")]
        public MenuType MenuType { get; set; }

        /// <summary>
        /// 是否隐藏
        /// </summary>
        [Column("is_hidden")]
        public bool IsHidden { get; set; } = false;

        /// <summary>
        /// 状态
        /// </summary>
        [Column("status")]
        public int Status { get; set; }
    }
}
