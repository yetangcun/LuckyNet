using Lucky.SysModel.Model.Input;
using Lucky.SysModel.Model.Output;

namespace Lucky.SysService.Service.IService
{
    public interface ISysTskService
    {
        /// <summary>
        /// 获取分页数据
        /// </summary>
        Task<(int, List<SysTskOutput>)> GetPages(SysTskQueryInput req);

        /// <summary>
        /// 获取详情
        /// </summary>
        Task<SysTskOutput?> Get(int id);

        /// <summary>
        /// 操作: 新增/修改
        /// </summary>
        Task<(bool, string)> Opt(SysTskOptInput req, long uid);

        /// <summary>
        /// 删除
        /// </summary>
        Task<(bool, string)> Del(int id, long uid);

        /// <summary>
        /// 启用or禁用
        /// </summary>
        Task<(bool, string)> Set(int id, long uid);


        /// <summary>
        /// 获取分页数据
        /// </summary>
        Task<(int, List<SysTskRecordOutput>)> GetRecordPages(SysTskRecordInput req);

    }
}
