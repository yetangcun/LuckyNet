using Prtcl.Grpc.IService;
using System.Net.Sockets;
using System.Text;
using System.Net;
using Common.CoreLib.Model.Option;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Common.CoreLib.Extension.Common;

namespace Prtcl.Grpc
{
    public class UdpUtil : IUdpUtil
    {
        private readonly UdpOption _option;
        private readonly UdpClient _udpClient;
        private readonly ILogger<UdpUtil> _logger;

        /// <summary>
        /// 构造函数
        /// </summary>
        public UdpUtil(IOptions<UdpOption> options, ILogger<UdpUtil> logger)
        {
            _logger = logger;
            _option = options.Value;

            var lclEp = new IPEndPoint(IPAddress.Any, _option.Port);
            _udpClient = new UdpClient(lclEp);

            _udpClient.Client.ReceiveBufferSize = 1024 * 1024 * 10;
            _udpClient.EnableBroadcast = true;
        }

        private bool _istop = false;
        private bool _listening = false;
        private Func<byte[], Task<string>>? _handler; // 数据处理函数

        public void Start(Func<byte[], Task<string>> handler)
        {
            if (_listening) return;

            _istop = false;
            _handler = handler;
            _udpClient.BeginReceive(RecvCallback, null);
            _logger.LogInformation($"UDP服务开始监听【0.0.0.0:{_option.Port}】..."); _listening = true;
        }

        private void RecvCallback(IAsyncResult ar)
        {
            var remoteEp = new IPEndPoint(IPAddress.Any, 0);
            try
            {
                var bytes = _udpClient.EndReceive(ar, ref remoteEp); _logger.LogDebug($"收到 {remoteEp} 的数据，长度：{bytes.Length} 字节");
                if (_handler != null)
                {
                    var res = _handler(bytes);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Udp ReceiveCallback error");
            }

            if (!_istop) _udpClient.BeginReceive(RecvCallback, null);
        }

        /// <summary>
        /// 发送数据: 字节数组
        /// </summary>
        public async Task<bool> SendDataAsync(byte[] bytes, string host, int port)
        {
            try
            {
                var res = await _udpClient.SendAsync(bytes, host, port);
                return res > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Udp SendDataAsync error");
                return false;
            }
        }

        /// <summary>
        /// 发送数据: 字符串
        /// </summary>
        public async Task<bool> SendDataAsync(string data, string host, int port)
        {
            try
            {
                var bytes = Encoding.UTF8.GetBytes(data);
                var res = await _udpClient.SendAsync(bytes, host, port);
                return res > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Udp SendDataAsync error");
                return false;
            }
        }

        /// <summary>
        /// 发送数据: 对象
        /// </summary>
        public async Task<bool> SendDataAsync<T>(T data, string host, int port)
        {
            try
            {
                var json = data.ToJson();
                var bytes = Encoding.UTF8.GetBytes(json);
                var sendEp = new IPEndPoint(IPAddress.Parse(host), port);
                var res = await _udpClient.SendAsync(bytes, sendEp);
                return res > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Udp SendDataAsync error");
                return false;
            }
        }

        /// <summary>
        /// 广播数据
        /// </summary>
        public async Task<bool> SendBroadcastAsync(byte[] bytes, int port)
        {
            if (!_udpClient.EnableBroadcast)
            {
                _logger.LogError("请在配置中设置 EnableBroadcast = true");
                return false;
            }

            if (bytes == null || bytes.Length == 0)
            {
                _logger.LogWarning("尝试发送空广播数据");
                return false;
            }

            try
            {
                // 使用广播地址
                var broadcastEndPoint = new IPEndPoint(IPAddress.Broadcast, port);
                int sentBytes = await _udpClient.SendAsync(bytes, bytes.Length, broadcastEndPoint);
                bool success = (sentBytes == bytes.Length);
                if (success)
                {
                    _logger.LogInformation($"已广播 {sentBytes} 字节到端口 {port}");
                }

                return success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"发送广播到端口 {port} 时发生错误");
                return false;
            }
        }

        public void Dispose()
        {
            _istop = true;
            _handler = null;
            _udpClient.Close();
            _listening = false;
            _udpClient.Dispose();
        }
    }
}
