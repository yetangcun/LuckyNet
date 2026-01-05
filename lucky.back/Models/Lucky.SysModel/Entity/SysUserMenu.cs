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
        public long UserId { get; set; }

        /// <summary>
        /// 菜单ID
        /// </summary>
        public int MenuId { get; set; }
    }
}
