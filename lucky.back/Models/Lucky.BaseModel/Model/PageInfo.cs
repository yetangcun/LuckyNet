namespace Lucky.BaseModel.Model
{
    /// <summary>
    /// 分页请求入参基类
    /// </summary>
    public class PageInfo
    {
        private int _pageIndex = 1;
        private int _pageSize = 20;

        /// <summary>
        /// 页码
        /// </summary>
        public int PageIndex
        {
            get => _pageIndex;
            set => _pageIndex = Math.Max(1, value);
        }

        /// <summary>
        /// 每页数量
        /// </summary>
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = Math.Max(1, value);
        }

        /// <summary>
        /// 排序字段
        /// </summary>
        public string? Sort { get; set; } = "Id";

        /// <summary>
        /// 排序方式
        /// </summary>
        public string? SortType { get; set; } = "desc";

        /// <summary>
        /// 跳过数量
        /// 此字段不用传，后台会自动计算
        /// </summary>
        public int Skips => (PageIndex - 1) * PageSize;
    }
}
