namespace Prtcl.Tcudp.IService
{
    /// <summary>
    /// udp接口
    /// </summary>
    public interface IUdpUtil
    {
        /// <summary>
        /// 启动
        /// </summary>
        /// <param name="handler"></param>
        void Start(Func<byte[], Task<string>> handler);

        /// <summary>
        /// 发送数据: 字节数组
        /// </summary>
        Task<bool> SendDataAsync(byte[] bytes, string host, int port);

        /// <summary>
        /// 发送数据: 字符串
        /// </summary>
        Task<bool> SendDataAsync(string data, string host, int port);

        /// <summary>
        /// 泛型发送数据
        /// </summary>
        Task<bool> SendDataAsync<T>(T data, string host, int port);

        /// <summary>
        /// 广播发送数据: 字节数组
        /// </summary>
        Task<bool> SendBroadcastAsync(byte[] bytes, int port);
    }
}
