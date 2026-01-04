using Lucky.BaseModel.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace lucky.admin.Controllers.prtcl
{
    /// <summary>
    /// websocket
    /// </summary>
    public class WebsocController : PrtclBaseController
    {
        /// <summary>
        /// 列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("list")]
        public async Task<ResModel<string>> GetList()
        {
            return ResModel<string>.Success(string.Empty);
        }
    }
}
