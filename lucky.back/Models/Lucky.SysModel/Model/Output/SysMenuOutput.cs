using Lucky.BaseModel.Enum;

namespace Lucky.SysModel.Model.Output
{
    /// <summary>
    /// 菜单输出
    /// </summary>
    public class SysMenuOutput
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public int id {  get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string? name { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string? code { get; set; }

        /// <summary>
        /// 上级id
        /// </summary>
        public int? parentId { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        public string? icon { get; set; }

        /// <summary>
        /// 图标大小
        /// </summary>
        public string? iconSize { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string? url { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int? sort { get; set; }

        /// <summary>
        /// 状态 
        /// </summary>
        public int? status { get; set; }

        /// <summary>
        /// 菜单类型
        /// </summary>
        public MenuType menuType { get; set; }

        /// <summary>
        /// 子菜单列表
        /// </summary>
        public List<SysMenuOutput>? children { get; set; }
    }
}
