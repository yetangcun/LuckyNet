using Lucky.BaseModel.Model.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lucky.SysModel.Entity
{
    /// <summary>
    /// 用户
    /// </summary>
    [Table("sys_user")]
    public class SysUser : BaseFullEntity<long>
    {
        /// <summary>
        /// 账号
        /// </summary>
        [StringLength(40)]
        [Column("account")]
        public required string Account { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [StringLength(40)]
        [Column("password")]
        public required string PassWord { get; set; }

        /// <summary>
        /// 真实姓名
        /// </summary>
        [StringLength(30)]
        [Column("realname")]
        public string? RealName { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        [StringLength(40)]
        [Column("email")]
        public string? Email { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        [StringLength(150)]
        [Column("avatar")]
        public string? Avatar { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        [StringLength(20)]
        [Column("phone")]
        public required string Phone { get; set; }

        /// <summary>
        /// 状态
        /// 1启用 0禁用 
        /// </summary>
        [Column("status")]
        public int Status { get; set; }
    }
}
