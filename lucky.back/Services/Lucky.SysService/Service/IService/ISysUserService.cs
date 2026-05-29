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

        Task<(int, List<SysUserInfoOutput>)> GetPages(SysUserQueryInput req);

        /// <summary>
        /// 获取用户权限
        /// </summary>
        /// <param name="uid"></param>

        Task<UsrData> GetPermissions(long uid);


        /// <summary>
        /// 登录操作
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        Task<SysUserOutput?> Dologin(string account);
    }
}
