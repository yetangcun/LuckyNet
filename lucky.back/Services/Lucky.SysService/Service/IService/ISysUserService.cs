using Lucky.SysModel.Model.Input;
using Lucky.SysModel.Model.Output;

namespace Lucky.SysService.Service.IService
{
    /// <summary>
    /// 用户服务接口
    /// </summary>
    public interface ISysUserService
    {
        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="req"></param>
        Task<(int, List<SysUserOutput>)> GetList(SysUserQueryInput req);
    }
}
