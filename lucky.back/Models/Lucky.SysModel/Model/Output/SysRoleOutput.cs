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
        public int id { get; set; }

        /// <summary>
        /// 角色名称
        /// </summary>
        public string? name { get; set; }

        /// <summary>
        /// 角色标识
        /// </summary>
        public string? word { get; set; }

        /// <summary>
        /// 角色类型
        /// </summary>
        public RoleType roleType { get; set; }

        /// <summary>
        ///  角色描述
        /// </summary>
        public string? remark { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int? status { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int? sort { get; set; }
    }
}
