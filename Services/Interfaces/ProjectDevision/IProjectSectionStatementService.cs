using DomainModels.Entities;
using DomainModels.Entities.ProjectDevision;
using Infrastracture.AppCode;
using System;
using System.Collections.Generic;
using ViewModels.ProjectDefine.ProjectStatementViewModel;
using ViewModels.ProjectDevision.ProjectSectionStatementViewModel;

namespace Services.Interfaces.ProjectDevision
{
    public interface IProjectSectionStatementService : IGenericService<ProjectSectionStatement>
    {
        IEnumerable<ListProjectSectionStatementViewModel> GetAll(ListProjectSectionStatementViewModel lpscvm, ref Paging pg);

        KeyValuePair<Int64, string> Save(CreateProjectSectionStatementViewModel cpscvm, Int64 MemberId, List<string> paths);
        IEnumerable<ReportProjectStatementViewModel> GetAllReport(ReportParameterProjectStatementViewModel parameters);
    }
}