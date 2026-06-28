using Lucky.BaseModel.Enum;

namespace Lucky.SysModel.Model.Input
{
    /// <summary>
    /// 查询入参
    /// </summary>
    public class SysMenuQueryInput
    {
        /// <summary>
        /// 父id
        /// </summary>
        public int ParentId { get; set; }

        /// <summary>
        /// 菜单名
        /// </summary>
        public string? Name { get; set; }
    }

    /// <summary>
    /// 操作入参
    /// </summary>
    public class SysMenuOptInput
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 父id
        /// </summary>
        public int ParentId { get; set; }

        /// <summary>
        /// 菜单类型
        /// </summary>
        public MenuType MenuType { get; set; }

        /// <summary>
        /// 菜单名
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// 唯一编码
        /// </summary>
        public string? Code { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        public string? Icon { get; set; }

        /// <summary>
        /// 图标大小
        /// </summary>
        public int IconSize { get; set; }

        /// <summary>
        /// 路径
        /// </summary>
        public string? Path { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }
    }
}
