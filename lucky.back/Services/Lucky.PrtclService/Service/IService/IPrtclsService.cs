using Lucky.PrtclModel.Model.Input;
using Lucky.PrtclModel.Model.Output;

namespace Lucky.PrtclService.Service.IService
{
    public interface IPrtclsService
    {
        Task<(int, List<PrtclOutput>?)> GetPageListAsync(PrtclQueryInput req);

        void CreateTables(string[] dllNames);
    }
}
