using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Prtcl.MqttServr;

namespace Lucky.MqttServr.Controllers
{
    /// <summary>
    /// Mqtt服务管理
    /// </summary>
    [Route("mqtt/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "mqttservr")]
    public class MqttServrController : ControllerBase
    {
        /// <summary>
        /// 启动服务
        /// </summary>
        [HttpPost("start")]
        [AllowAnonymous]
        public async Task<IActionResult> StartAsync([FromServices] IMqttServr mqttServer)
        {
            await mqttServer.StartAsync();
            return new JsonResult(0);
        }

        /// <summary>
        /// 停止服务
        /// </summary>
        [HttpPost("stop")]
        [AllowAnonymous]
        public async Task<IActionResult> StopAsync([FromServices] IMqttServr mqttServer)
        {
            await mqttServer.StopAsync();
            return new JsonResult(0);
        }

        /// <summary>
        /// 获取连接客户端数量
        /// </summary>
        [HttpGet("getConnectedClientsCount")]
        public IActionResult GetConnectedClientsCountAsync([FromServices] IMqttServr mqttServer)
        {
            var count = mqttServer.GetConnectedClientsCountAsync();
            return new JsonResult(count);
        }

        /// <summary>
        /// 获取客户端信息
        /// </summary>
        [HttpGet("getClientInfo")]
        public IActionResult GetClientInfoAsync([FromServices] IMqttServr mqttServer, string clientId)
        {
            var info = mqttServer.GetClientsInfoAsync(clientId);
            return new JsonResult(info);
        }

        /// <summary>
        /// 获取客户端订阅
        /// </summary>
        [HttpGet("getClientSubscribes")]
        public IActionResult GetClientSubscribesAsync([FromServices] IMqttServr mqttServer, string clientId)
        {
            var list = mqttServer.GetClientSubscribesAsync(clientId);
            return new JsonResult(list);
        }
    }
}
