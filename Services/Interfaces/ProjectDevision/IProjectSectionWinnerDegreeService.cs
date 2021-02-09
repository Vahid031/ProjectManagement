using DomainModels.Entities;
using DomainModels.Entities.ProjectDevision;
using Infrastracture.AppCode;
using System;
using System.Collections.Generic;
using ViewModels.ProjectDevision.ProjectSectionWinnerDegreeViewModel;

namespace Services.Interfaces.ProjectDevision
{
    public interface IProjectSectionWinnerDegreeService : IGenericService<ProjectSectionWinnerDegree>
    {
        IEnumerable<ListProjectSectionWinnerDegreeViewModel> GetAll(ListProjectSectionWinnerDegreeViewModel lpscvm, ref Paging pg);

        KeyValuePair<Int64, string> Save(CreateProjectSectionWinnerDegreeViewModel cpscvm, Int64 MemberId, List<string> paths);
    }
}