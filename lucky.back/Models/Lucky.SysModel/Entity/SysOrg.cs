using Lucky.BaseModel.Enum;
using Lucky.BaseModel.Model.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lucky.SysModel.Entity
{
    /// <summary>
    /// 组织机构
    /// </summary>
    [Table("sys_org")]
    public class SysOrg : BaseFullEntity<int>
    {
        /// <summary>
        /// 名称
        /// </summary>
        [StringLength(60)]
        [Required]
        [Column("name")]
        public required string Name { get; set; }

        /// <summary>
        /// 组织类型
        /// </summary>
        [Column("org_type")]
        public OrganizationType OrgType { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        [StringLength(10)]
        [Column("code")]
        public string? Code { get; set; }

        /// <summary>
        /// 上级ID
        /// </summary>
        [Column("parent_id")]
        public int ParentId { get; set; }

        /// <summary>
        /// 领导ID
        /// </summary>
        [Column("leader_id")]
        public long LeaderId { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        [StringLength(20)]
        [Column("phone")]
        public string? Phone { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(200)]
        [Column("remark")]
        public string? Remark { get; set; }
    }
}
