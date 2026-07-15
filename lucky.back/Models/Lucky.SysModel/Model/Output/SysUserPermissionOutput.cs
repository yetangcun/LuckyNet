using Lucky.BaseModel.Enum;

namespace Lucky.SysModel.Model.Output
{
    /// <summary>
    /// 用户数据
    /// </summary>
    public class UsrData
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public string? uid { get; set; }

        /// <summary>
        /// 用户名|姓名
        /// </summary>
        public string? name { get; set; }

        /// <summary>
        /// 角色名称
        /// </summary>
        public string? roleName { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public string? avatar { get; set; }

        /// <summary>
        /// 布局类型 1左侧  2顶部+左侧
        /// 默认为1
        /// </summary>
        public int? layout { get; set; } = 1;

        /// <summary>
        /// 权限
        /// </summary>
        public List<SysUserPermissionOutput>? permissions { get; set; }
    }

    /// <summary>
    /// 权限信息
    /// </summary>
    public class SysUserPermissionOutput
    {
        /// <summary>
        /// id
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// 父级id
        /// </summary>
        public required string parent_id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public required string name { get; set; }

        /// <summary>
        /// 唯一编码
        /// </summary>
        public required string code { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string? url { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        public string? icon { get; set; }

        /// <summary>
        /// 图标大小
        /// </summary>
        public string? iconSize { get; set; }

        /// <summary>
        /// 菜单类型
        /// </summary>
        public MenuType menuType { get; set; }

        /// <summary>
        /// 是否选中
        /// </summary>
        public bool isSelect { get; set; } = false;

        /// <summary>
        /// 是否展开
        /// </summary>
        public bool isExpand { get; set; } = false;

        /// <summary>
        /// 子级
        /// </summary>
        public List<SysUserPermissionOutput>? children { get; set; }
    }
}
