namespace Lucky.BaseModel.Model
{
    /// <summary>
    /// 分页响应结果基类
    /// </summary>
    public class PageRes<T>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public PageRes(int totals, int pages, T? data, string? msg, int code) // , bool isSucc
        {
            Total = totals;
            Page = pages;
            Data = data;
            //IsSucc = isSucc;
            Msg = msg;
            Code = code;
        }

        /// <summary>
        /// 总记录数
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        /// 总页数
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        /// 结果对象
        /// </summary>
        public T? Data { get; set; }

        /// <summary>
        /// 状态码
        /// </summary>
        public int Code { get; set; }

        ///// <summary>
        ///// 执行结果
        ///// </summary>
        // public bool IsSucc { get; set; }

        /// <summary>
        /// 结果信息
        /// 失败时为异常信息
        /// 成功时为空或其他信息
        /// </summary>
        public string? Msg { get; set; }

        /// <summary>
        /// 创建失败结果
        /// </summary>
        public static PageRes<T> Failed(int total, int page, T? data, string? msg = "failed", int code = 500)
        {
            return new PageRes<T>(total, page, data, msg, code);
        }

        /// <summary>
        /// 创建成功结果
        /// </summary>
        public static PageRes<T> Success(int total, int page, T data, string? msg = "success", int code = 200)
        {
            // var data = new { result = datas, totalNum = total, totalPage = page };
            return new PageRes<T>(total, page, data, msg, code);
        }
    }

    //public class ResponCommon<T>
    //{
    //    public int Code { get; set; }
    //    public string? Msg { get; set; }
    //    public T? Data { get; set; }
    //}
}
