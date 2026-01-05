using System.ComponentModel.DataAnnotations.Schema;

namespace Lucky.SysModel.Entity
{
    /// <summary>
    /// 用户角色
    /// </summary>
    [Table("sys_user_role")]
    public class SysUserRole
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 角色Id
        /// </summary>
        public int RoleId { get; set; }
    }
}
