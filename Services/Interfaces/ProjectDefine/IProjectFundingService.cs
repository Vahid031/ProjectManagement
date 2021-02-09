using DomainModels.Entities;
using DomainModels.Entities.BaseInformation;
using Infrastracture.AppCode;
using System;
using System.Collections.Generic;
using Services.Interfaces.ProjectDefine;
using DomainModels.Entities.ProjectDefine;
using ViewModels.ProjectDefine.ProjectFundingViewModel;

namespace Services.Interfaces.ProjectDefine
{
    public interface IProjectFundingService : IGenericService<ProjectFunding>
    {
        IEnumerable<ListProjectFundingViewModel> GetAll(ListProjectFundingViewModel lpfvm, ref Paging pg);

        KeyValuePair<Int64, string> Save(CreateProjectFundingViewModel cpavm, Int64 memberId, List<string> paths);
    }
}