using DomainModels.Entities;
using DomainModels.Entities.ProjectDevision;
using Infrastracture.AppCode;
using System;
using System.Collections.Generic;
using ViewModels.ProjectDefine.ProjectSectionProjectProgressReportViewModel;
using ViewModels.ProjectDevision.ProjectSectionSupervisorVisitViewModel;

namespace Services.Interfaces.ProjectDevision
{
    public interface IProjectSectionSupervisorVisitService : IGenericService<ProjectSectionSupervisorVisit>
    {
        IEnumerable<ListProjectSectionSupervisorVisitViewModel> GetAll(ListProjectSectionSupervisorVisitViewModel lpscvm, ref Paging pg, Int64 MemberId);
        KeyValuePair<Int64, string> Save(CreateProjectSectionSupervisorVisitViewModel cpscvm, Int64 MemberId, List<string> paths);
        IEnumerable<ReportProjectSectionSupervisorVisitsViewModel> GetAllReport(ReportParameterProjectSectionSupervisorVisitsViewModel parameters);
        IEnumerable<ReportProjectSectionProjectProgressViewModel> GetAllReportb(ReportParameterProjectSectionProjectProgressViewModel parameters);

    }
}