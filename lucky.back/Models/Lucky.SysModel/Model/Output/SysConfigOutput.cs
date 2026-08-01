namespace Lucky.SysModel.Model.Output
{
    /// <summary>
    /// 配置出参
    /// </summary>
    public class SysConfigOutput
    {
        /// <summary>
        /// 配置Id
        /// </summary>
        public int Id { get; set; }


        /// <summary>
        /// 配置类型
        /// </summary>
        public string CfgType { get; set; } = null!;

        /// <summary>
        /// 配置类型名称
        /// </summary>
        public string? TypeName { get; set; }

        /// <summary>
        /// 选项名
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// 选项值
        /// </summary>
        public required string Value { get; set; }

        /// <summary>
        /// 选项编码
        /// </summary>
        public string? Code { get; set; }

        /// <summary>
        /// 状态 0=禁用 1=启用
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; } = 0;

        /// <summary>
        /// 是否系统内置 0=否 1=是
        /// </summary>
        public bool IsSystem { get; set; } = false;
    }
}
