using Lucky.BaseModel.Model;

namespace Lucky.SysModel.Model.Input
{
    /// <summary>
    /// 配置查询入参
    /// </summary>
    public class SysConfigQueryInput : PageInfo
    {
        /// <summary>
        /// 配置类型
        /// </summary>
        public string? CfgType { get; set; }

        /// <summary>
        /// 关键字 
        /// </summary>
        public string? Txt { get; set; }
    }

    /// <summary>
    /// 配置操作入参
    /// </summary>
    public class SysConfigOptInput
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// 选项名
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// 选项值
        /// </summary>
        public string? Value { get; set; }

        /// <summary>
        ///  配置类型
        /// </summary>
        public string? CfgType { get; set; }

        /// <summary>
        /// 类型名称
        /// </summary>
        public string? TypeName { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 配置编码
        /// </summary>
        public string? Code { get; set; }
    }
}
