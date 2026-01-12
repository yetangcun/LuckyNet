using Lucky.BaseModel.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace Lucky.IOT.Controllers
{
    [Route("lot/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "IoT")]
    public class IoTManagerController : ControllerBase
    {
        public IoTManagerController()
        {
        }

        [HttpGet("list")]
        public async Task<ResModel<List<string>>> GetList()
        {
            return ResModel<List<string>>.Success(new List<string>() { "1", "2", "3" });
        }
    }
}
