using Lucky.BaseModel.Model.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lucky.SysModel.Entity
{
    /// <summary>
    /// 用户组织
    /// </summary>
    [Table("sys_user_org")]
    public class SysUserOrg : BaseFullEntity<long>
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        [Column("user_id")]
        public long UserId { get; set; }

        /// <summary>
        /// 组织ID
        /// </summary>
        [Column("org_id")]
        public int OrgId { get; set; }
    }
}
