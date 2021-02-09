using DomainModels.Entities;
using DomainModels.Entities.ProjectDevision;
using Infrastracture.AppCode;
using System;
using System.Collections.Generic;
using ViewModels.ProjectDevision.ProjectSectionRecoupmentViewModel;

namespace Services.Interfaces.ProjectDevision
{
    public interface IProjectSectionRecoupmentService : IGenericService<ProjectSectionRecoupment>
    {
        IEnumerable<ListProjectSectionRecoupmentViewModel> GetAll(ListProjectSectionRecoupmentViewModel lpscvm, ref Paging pg);

        KeyValuePair<Int64, string> Save(CreateProjectSectionRecoupmentViewModel cpscvm, Int64 MemberId, List<string> paths);

        IEnumerable<ReportProjectSectionRecoupmentViewModel> GetAllReport(ReportParameterProjectSectionRecoupmentViewModel parameters);
    }
}