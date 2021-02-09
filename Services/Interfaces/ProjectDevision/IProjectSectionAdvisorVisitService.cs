using DomainModels.Entities;
using DomainModels.Entities.ProjectDevision;
using Infrastracture.AppCode;
using System;
using System.Collections.Generic;
using ViewModels.ProjectDevision.ProjectSectionAdvisorVisitViewModel;

namespace Services.Interfaces.ProjectDevision
{
    public interface IProjectSectionAdvisorVisitService : IGenericService<ProjectSectionAdvisorVisit>
    {
        IEnumerable<ListProjectSectionAdvisorVisitViewModel> GetAll(ListProjectSectionAdvisorVisitViewModel lpscvm, ref Paging pg, Int64 MemberId);

        KeyValuePair<Int64, string> Save(CreateProjectSectionAdvisorVisitViewModel cpscvm, Int64 MemberId, List<string> paths);
    }
}