using DomainModels.Entities;
using DomainModels.Entities.ProjectDevision;
using Infrastracture.AppCode;
using System;
using System.Collections.Generic;
using ViewModels.ProjectDevision.ProjectSectionDepositeLiberalizationViewModel;

namespace Services.Interfaces.ProjectDevision
{
    public interface IProjectSectionDepositeLiberalizationService : IGenericService<ProjectSectionDepositeLiberalization>
    {
        IEnumerable<ListProjectSectionDepositeLiberalizationViewModel> GetAll(ListProjectSectionDepositeLiberalizationViewModel lpscvm, ref Paging pg);

        KeyValuePair<Int64, string> Save(CreateProjectSectionDepositeLiberalizationViewModel cpscvm, Int64 MemberId, List<string> paths);

        IEnumerable<ReportProjectSectionDepositeLiberalizationsViewModel> GetAllReport(ReportParameterProjectSectionDepositeLiberalizationsViewModel parameters);
    }
}