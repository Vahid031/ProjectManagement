using DomainModels.Entities;
using DomainModels.Entities.ProjectDevision;
using Infrastracture.AppCode;
using System;
using System.Collections.Generic;
using ViewModels.ProjectDefine.ProjectSectionContractAmendmentDateReport;
using ViewModels.ProjectDevision.ProjectSectionComplementarityDateViewModel;

namespace Services.Interfaces.ProjectDevision
{
    public interface IProjectSectionComplementarityDateService : IGenericService<ProjectSectionComplementarityDate>
    {
        IEnumerable<ListProjectSectionComplementarityDateViewModel> GetAll(ListProjectSectionComplementarityDateViewModel lpscvm, ref Paging pg);

        KeyValuePair<Int64, string> Save(CreateProjectSectionComplementarityDateViewModel cpscvm, Int64 MemberId, List<string> paths);
        IEnumerable<ReportProjectSectionContractAmendmentDateViewModel> GetAllReport(ReportParameterProjectSectionContractAmendmentDateViewModel parameters);

    }
}