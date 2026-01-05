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
        public required string Name { get; set; }

        /// <summary>
        /// 组织类型
        /// </summary>
        public OrganizationType OrgType { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        [StringLength(10)]
        public string? Code { get; set; }

        /// <summary>
        /// 上级ID
        /// </summary>
        public int ParentId { get; set; }

        /// <summary>
        /// 领导ID
        /// </summary>
        public long LeaderId { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        [StringLength(20)]
        public string? Phone { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(200)]
        public string? Remark { get; set; }
    }
}
