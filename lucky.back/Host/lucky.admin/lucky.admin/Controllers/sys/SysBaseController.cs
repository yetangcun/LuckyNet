using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace lucky.admin.Controllers.sys
{
    /// <summary>
    /// 系统管理
    /// </summary>
    [Route("api/sys/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "sys")]
    public abstract class SysBaseController : ControllerBase
    {
    }
}
