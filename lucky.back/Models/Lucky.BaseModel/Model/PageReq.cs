namespace Lucky.BaseModel.Model
{
    /// <summary>
    /// 分页请求入参基类
    /// </summary>
    public class PageReq<T>
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
        /// 参数对象
        /// </summary>
        public T? ReqParam { get; set; }
    }
}
