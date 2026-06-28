using Lucky.BaseModel.Enum;

namespace Lucky.SysModel.Model.Output
{
    /// <summary>
    /// 输出结果模型
    /// </summary>
    public class SysOrgOutput
    {
        /// <summary>
        /// 组织机构ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 父级ID
        /// </summary>
        public int? Pid { get; set; }

        /// <summary>
        /// 组织机构名称
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        ///  编码
        /// </summary>
        public string? Code { get; set; }

        /// <summary>
        ///  描述
        /// </summary>
        public string? Remark { get; set; }

        /// <summary>
        /// 组织机构类型
        /// </summary>
        public OrganizationType OrgType { get; set; }

        /// <summary>
        /// 领导
        /// </summary>
        public string? Leader { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        public string? Phone { get; set; }
    }

    /// <summary>
    /// 树结构
    /// </summary>
    public class SysOrgOutputTree : SysOrgOutput
    {
        /// <summary>
        /// 子级
        /// </summary>
        public List<SysOrgOutputTree>? Childs { get; set; }
    }
}
