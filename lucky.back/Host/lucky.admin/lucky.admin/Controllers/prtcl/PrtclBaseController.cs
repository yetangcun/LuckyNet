using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace lucky.admin.Controllers.prtcl
{
    /// <summary>
    /// mq 基类
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "mq")]
    public abstract class PrtclBaseController : ControllerBase
    {
    }
}
