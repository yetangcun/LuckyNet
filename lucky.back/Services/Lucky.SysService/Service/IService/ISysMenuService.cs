using Lucky.SysModel.Model.Input;
using Lucky.SysModel.Model.Output;

namespace Lucky.SysService.Service.IService
{
    public interface ISysMenuService
    {
        /// <summary>
        /// 查询菜单树
        /// </summary>
        /// <param name="input"></param>
        /// <param name="isAdmin"></param>
        Task<List<SysMenuOutput>> GetMenuTree(SysMenuQueryInput input, bool isAdmin = false);

        /// <summary>
        /// 根据id查询
        /// </summary>
        /// <param name="id"></param>
        Task<SysMenuOutput?> Get(int id);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="input"></param>
        /// <param name="uid"></param>
        Task<bool> Add(SysMenuOptInput input, long uid);

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="input"></param>
        /// <param name="uid"></param>
        Task<bool> Edit(SysMenuOptInput input, long uid);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        Task<bool> Del(int id, long uid);
    }
}
