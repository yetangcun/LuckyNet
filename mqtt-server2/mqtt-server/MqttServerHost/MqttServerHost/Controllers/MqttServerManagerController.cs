using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MqttServerLib;

namespace MqttServerHost.Controllers
{
    /// <summary>
    /// Mqtt服务管理
    /// </summary>
    [Route("mqttServer/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "mqttservr")]
    public class MqttServerManagerController : ControllerBase
    {
        /// <summary>
        /// 启动服务
        /// </summary>
        [HttpPost("start")]
        [AllowAnonymous]
        public async Task<IActionResult> StartAsync([FromServices] IMqttServerExt mqttServer)
        {
            await mqttServer.StartAsync();
            return new JsonResult(0);
        }

        /// <summary>
        /// 停止服务
        /// </summary>
        [HttpPost("stop")]
        [AllowAnonymous]
        public async Task<IActionResult> StopAsync([FromServices] IMqttServerExt mqttServer)
        {
            await mqttServer.StopAsync();
            return new JsonResult(0);
        }

        /// <summary>
        /// 获取连接客户端数量
        /// </summary>
        [HttpGet("getConnectedClientsCount")]
        public IActionResult GetConnectedClientsCountAsync([FromServices] IMqttServerExt mqttServer)
        {
            var count = mqttServer.GetConnectedClientsCountAsync();
            return new JsonResult(count);
        }

        /// <summary>
        /// 获取客户端信息
        /// </summary>
        [HttpGet("getClientInfo")]
        public IActionResult GetClientInfoAsync([FromServices] IMqttServerExt mqttServer, string clientId)
        {
            var info = mqttServer.GetClientsInfoAsync(clientId);
            return new JsonResult(info);
        }

        /// <summary>
        /// 获取客户端订阅
        /// </summary>
        [HttpGet("getClientSubscribes")]
        public IActionResult GetClientSubscribesAsync([FromServices] IMqttServerExt mqttServer, string clientId)
        {
            var list = mqttServer.GetClientSubscribesAsync(clientId);
            return new JsonResult(list);
        }
    }
}
