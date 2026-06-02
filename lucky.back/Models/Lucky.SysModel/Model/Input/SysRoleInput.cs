using Lucky.BaseModel.Enum;
using Lucky.BaseModel.Model;

namespace Lucky.SysModel.Model.Input
{
    /// <summary>
    /// 角色查询入参
    /// </summary>
    public class SysRoleQueryInput : PageInfo
    {
        /// <summary>
        /// 关键字
        /// </summary>
        public string? txt { get; set; }

        /// <summary>
        /// 角色类型
        /// </summary>
        public RoleType? roleType { get; set; }
    }

    /// <summary>
    /// 操作入参
    /// </summary>
    public class SysRoleOptInput
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 角色名
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// 角色类型
        /// </summary>
        public RoleType? RoleType { get; set; }


        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }
    }
}
