using Lucky.BaseModel.Model;
using Lucky.SysModel.Model.Input;
using Lucky.SysModel.Model.Output;

namespace Lucky.SysService.Service.IService
{
    public interface ISysLogService
    {
        /// <summary>
        /// 获取日志分页
        /// </summary>
        /// <param name="req"></param>
        Task<(int, List<SysLogOutput>)> GetPages(SysLogQueryInput req);
    }
}
