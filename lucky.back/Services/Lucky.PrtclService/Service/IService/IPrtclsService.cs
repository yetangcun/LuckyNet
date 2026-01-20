using Data.SqlSugar.Extension.Service;
using Lucky.PrtclModel.Model.Input;
using Lucky.PrtclModel.Model.Output;

namespace Lucky.PrtclService.Service.IService
{
    public interface IPrtclsService: ISugarBaseService
    {
        Task<(int, List<PrtclOutput>?)> GetPageListAsync(PrtclQueryInput req);
    }
}
