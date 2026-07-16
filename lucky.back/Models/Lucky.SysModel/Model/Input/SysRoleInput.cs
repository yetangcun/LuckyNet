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
        /// 角色标识
        /// </summary>
        public string? Word { get; set; }

        /// <summary>
        /// 角色类型
        /// </summary>
        public RoleType? RoleType { get; set; }


        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int? Status { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int? Sort { get; set; }
    }

    /// <summary>
    /// 设置角色菜单入参
    /// </summary>
    public class SetRoleMenusInput
    {
        /// <summary>
        /// 角色id
        /// </summary>
        public int roleId { get; set; }

        /// <summary>
        /// 菜单id
        /// </summary>
        public List<int>? menuIds { get; set; }
    }
}
