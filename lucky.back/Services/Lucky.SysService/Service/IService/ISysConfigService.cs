using Lucky.SysModel.Model.Input;
using Lucky.SysModel.Model.Output;

namespace Lucky.SysService.Service.IService
{
    public interface ISysConfigService
    {
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="req"></param>
        Task<(int, List<SysConfigOutput>)> GetPages(SysConfigQueryInput req);

        /// <summary>
        /// 根据id获取
        /// </summary>
        /// <param name="id"></param>
        Task<SysConfigOutput?> GetById(int id);

        /// <summary>
        /// 新增或修改
        /// </summary>
        /// <param name="req"></param>
        Task<bool> Opt(SysConfigOptInput req);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        Task<bool> Del(int id);
    }
}
