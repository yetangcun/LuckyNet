using Lucky.SysModel.Model.Input;
using Lucky.SysModel.Model.Output;
using Lucky.BaseModel.Model;

namespace Lucky.SysService.Service.IService
{
    public interface ISysOrgService
    {
        /// <summary>
        /// 获取组织机构树形数据
        /// </summary>
        /// <param name="req"></param>
        Task<List<SysOrgOutputTree>> GetOrgTree(SysOrgQueryInput req);

        /// <summary>
        /// 获取组织机构树形下拉框数据
        /// </summary>
        /// <param name="req"></param>
        Task<List<TreeSelectKV>> GetOrgTreeSel(SysOrgQueryInput req);

        /// <summary>
        /// 获取组织机构详情
        /// </summary>
        /// <param name="id"></param>
        Task<SysOrgOutput> Get(int id);

        /// <summary>
        /// 新增/编辑组织机构
        /// </summary>
        /// <param name="input"></param>
        /// <param name="uid"></param>
        Task<bool> Opt(SysOrgOptInput input, long uid);

        /// <summary>
        /// 删除组织机构
        /// </summary>
        /// <param name="id"></param>
        /// <param name="uid"></param>
        Task<bool> Del(int id, long uid);
    }
}
