using System.ComponentModel.DataAnnotations.Schema;

namespace Lucky.SysModel.Entity
{
    /// <summary>
    /// 角色菜单
    /// </summary>
    [Table("sys_role_menu")]
    public class SysRoleMenu
    {
        /// <summary>
        /// 角色Id
        /// </summary>
        [Column("role_id")]
        public int RoleId { get; set; }

        /// <summary>
        /// 菜单Id
        /// </summary>
        [Column("menu_id")]
        public int MenuId { get; set; }
    }
}
