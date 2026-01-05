namespace Lucky.BaseModel.Model
{
    /// <summary>
    /// 分页请求入参基类
    /// </summary>
    public class PageInfo
    {
        /// <summary>
        /// 页面查询数
        /// </summary>
        public int PageSize { get; set; } = 20;

        /// <summary>
        /// 当前页码
        /// </summary>
        public int PageIndex
        {
            get;
            set;
        } = 1;

        /// <summary>
        /// 排序字段
        /// </summary>
        public string? Sort { get; set; } = "id";

        /// <summary>
        /// 排序方式
        /// </summary>
        public string? SortType { get; set; } = "desc";

    }
}
