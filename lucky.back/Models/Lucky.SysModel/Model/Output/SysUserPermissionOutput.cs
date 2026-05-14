using Lucky.BaseModel.Enum;

namespace Lucky.SysModel.Model.Output
{
    /// <summary>
    /// 权限信息
    /// </summary>
    public class SysUserPermissionOutput
    {
        /// <summary>
        /// id
        /// </summary>
        public string id { get; set; }

        /// <summary>
        /// 父级id
        /// </summary>
        public string parent_id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 唯一编码
        /// </summary>
        public string code { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string path { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        public string icon { get; set; }

        /// <summary>
        /// 图标大小
        /// </summary>
        public string icon_size { get; set; }

        /// <summary>
        /// 菜单类型
        /// </summary>
        public MenuType menu_type { get; set; }

        /// <summary>
        /// 子级
        /// </summary>
        public List<SysUserPermissionOutput>? childs { get; set; }
    }
}
