namespace Lucky.BaseModel.Model
{
    /// <summary>
    /// 下拉框数据
    /// </summary>
    public class SelectKV
    {
        /// <summary>
        /// 显示文本
        /// </summary>
        public string? label { get; set; }

        /// <summary>
        /// 背地值
        /// </summary>
        public string? value { get; set; }

        /// <summary>
        /// 扩展字段
        /// </summary>
        public string? ext { get; set; }
    }

    /// <summary>
    /// 多级下拉框数据
    /// </summary>
    public class MulSelectKV
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string? name { get; set; }

        /// <summary>
        /// 数据列表
        /// </summary>
        public List<SelectKV>? lst { get; set; }
    }
}
