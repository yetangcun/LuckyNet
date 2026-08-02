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
        public int id { get; set; }


        /// <summary>
        /// 配置类型
        /// </summary>
        public string? cfgType { get; set; } = null!;

        /// <summary>
        /// 配置类型名称
        /// </summary>
        public string? typeName { get; set; }

        /// <summary>
        /// 选项名
        /// </summary>
        public required string name { get; set; }

        /// <summary>
        /// 选项值
        /// </summary>
        public string? value { get; set; }

        /// <summary>
        /// 选项编码
        /// </summary>
        public string? code { get; set; }

        /// <summary>
        /// 状态 0=禁用 1=启用
        /// </summary>
        public int status { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int sort { get; set; } = 0;

        /// <summary>
        /// 是否系统内置 0=否 1=是
        /// </summary>
        public bool isSystem { get; set; } = false;


        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime createTime { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string? createUser { get; set; }
    }
}
