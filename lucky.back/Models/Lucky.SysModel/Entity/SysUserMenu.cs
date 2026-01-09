using System.ComponentModel.DataAnnotations.Schema;

namespace Lucky.SysModel.Entity
{
    /// <summary>
    /// 用户菜单
    /// </summary>
    [Table("sys_user_menu")]
    public class SysUserMenu
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        [Column("user_id")]
        public long UserId { get; set; }

        /// <summary>
        /// 菜单ID
        /// </summary>
        [Column("menu_id")]
        public int MenuId { get; set; }
    }
}
