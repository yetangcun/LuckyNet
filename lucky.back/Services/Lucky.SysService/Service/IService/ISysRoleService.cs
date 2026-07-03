using Lucky.BaseModel.Model;
using Lucky.SysModel.Model.Input;
using Lucky.SysModel.Model.Output;

namespace Lucky.SysService.Service.IService
{
    public interface ISysRoleService
    {
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="input"></param>
        Task<(int, List<SysRoleOutput>)> GetPages(SysRoleQueryInput input, bool isAdmin = false);

        /// <summary>
        /// 根据id查询
        /// </summary>
        /// <param name="id"></param>
        Task<SysRoleOutput?> Get(int id);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="input"></param>
        /// <param name="uid"></param>
        Task<bool> Add(SysRoleOptInput input, long uid);

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="input"></param>
        /// <param name="uid"></param>
        Task<bool> Edit(SysRoleOptInput input, long uid);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        Task<bool> Del(int id, long uid);

        /// <summary>
        /// 获取下拉列表
        /// </summary>
        /// <param name="uid"></param>
        Task<List<SelectKV>?> GetSelList(long uid);
    }
}
