using Common.CoreLib.Model.Option;
using Data.SqlSugar.Extension.Service;
using Microsoft.Extensions.Options;
using System;
using System.Text;
using Lucky.PrtclModel.Entity;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Lucky.PrtclService.Service.IService;

namespace Lucky.PrtclService.Rpsty
{
    public class PrtclService : SugarBaseService<Prtcl, PrtclDbOption>, IPrtclService
    {
        private readonly ILogger<PrtclService> _logger;

        public PrtclService(IOptions<PrtclDbOption> option, ILogger<PrtclService> logger) : base(option.Value, logger)
        {
            _logger = logger;
        }
    }
}
