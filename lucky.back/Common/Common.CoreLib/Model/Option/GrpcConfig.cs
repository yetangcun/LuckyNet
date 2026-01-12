

namespace Common.CoreLib.Model.Option
{
    /// <summary>
    /// grpc配置
    /// </summary>
    public class GrpcConfig
    {
        /// <summary>
        /// 监听端口
        /// </summary>
        public int ListenPort { get; set; }

        /// <summary>
        /// 远程服务端口
        /// </summary>
        public int RemotePort { get; set; }

        /// <summary>
        /// 远程服务地址
        /// </summary>
        public string? RemoteHost { get; set; }
    }
}
