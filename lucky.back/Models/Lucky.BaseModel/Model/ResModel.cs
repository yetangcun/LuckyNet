namespace Lucky.BaseModel.Model
{
    /// <summary>
    /// 统一响应模型
    /// </summary>
    public class ResModel<T>
    {
        /// <summary>
        /// 无参构造
        /// </summary>
        public ResModel() { }

        /// <summary>
        /// 有参构造
        /// </summary>
        public ResModel(int code, string msg, bool IsSucc, T data)
        {
            this.Msg = msg;
            this.Code = code;
            this.Data = data;
        }

        /// <summary>
        /// 状态码
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// 响应信息
        /// </summary>
        public string? Msg { get; set; }

        /// <summary>
        /// 响应结果数据
        /// </summary>
        public T? Data { get; set; }

        /// <summary>
        /// 成功
        /// </summary>
        public static ResModel<T> Success(T? data, string? msg = "success", int code = 200)
        {
            return new ResModel<T> { Code = code, Msg = msg, Data = data };
        }

        /// <summary>
        /// 失败
        /// </summary>
        public static ResModel<T> Failed(T? data, string? msg="failed", int code = 500)
        {
            return new ResModel<T> { Code = code, Msg = msg, Data = data };
        }
    }
}
