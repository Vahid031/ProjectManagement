using DomainModels.Entities;
using DomainModels.Entities.ProjectDevision;
using Infrastracture.AppCode;
using System;
using System.Collections.Generic;
using ViewModels.ProjectDefine.ProjectSectionContractAmendmentReport;
using ViewModels.ProjectDevision.ProjectSectionComplementarityPriceViewModel;

namespace Services.Interfaces.ProjectDevision
{
    public interface IProjectSectionComplementarityPriceService : IGenericService<ProjectSectionComplementarityPrice>
    {
        IEnumerable<ListProjectSectionComplementarityPriceViewModel> GetAll(ListProjectSectionComplementarityPriceViewModel lpscvm, ref Paging pg);
        CreateProjectSectionComplementarityPriceViewModel GetContractPrice(long ProjectSectionId);

        KeyValuePair<Int64, string> Save(CreateProjectSectionComplementarityPriceViewModel cpscvm, Int64 MemberId, List<string> paths);
        IEnumerable<ReportProjectSectionContractAmendmentViewModel> GetAllReport(ReportParameterProjectSectionContractAmendmentViewModel parameters);
    }
}