using Lucky.BaseModel.Enum;
using System.Text.Json.Serialization;

namespace Lucky.SysModel.Model.Output
{
    /// <summary>
    /// 角色输出
    /// </summary>
    public class SysRoleOutput
    {
        /// <summary>
        /// 角色Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 角色名称
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// 角色类型
        /// </summary>
        public RoleType RoleType { get; set; }

        /// <summary>
        ///  角色描述
        /// </summary>
        public string? Remark { get; set; }
    }
}
