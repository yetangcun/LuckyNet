using Lucky.PrtclModel.Model.Input;
using System;
using System.Text;
using System.Collections.Generic;
using Lucky.PrtclModel.Model.Output;

namespace Lucky.PrtclService.Service.IService
{
    public interface IPrtclsService
    {
        Task<(int, List<PrtclOutput>?)> GetPageListAsync(PrtclQueryInput req);
    }
}
