using DomainModels.Entities;
using DomainModels.Entities.ProjectDevision;
using Infrastracture.AppCode;
using System;
using System.Collections.Generic;
using ViewModels.ProjectDevision.ProjectSectionContractViewModel;

namespace Services.Interfaces.ProjectDevision
{
    public interface IProjectSectionContractService : IGenericService<ProjectSectionContract>
    {
        KeyValuePair<Int64, string> Save(CreateProjectSectionContractViewModel cpscvm, Int64 MemberId, List<string> paths);

        IEnumerable<ReportProjectSectionContractViewModel> GetAllReport(ReportParameterProjectSectionContractViewModel parameters);
    }
}