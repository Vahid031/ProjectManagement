using DomainModels.Entities;
using DomainModels.Entities.ProjectDevision;
using Infrastracture.AppCode;
using System;
using System.Collections.Generic;
using ViewModels.ProjectDevision.ProjectSectionCeremonialViewModel;

namespace Services.Interfaces.ProjectDevision
{
    public interface IProjectSectionCeremonialService : IGenericService<ProjectSectionCeremonial>
    {
        IEnumerable<ListProjectSectionCeremonialViewModel> GetAll(ListProjectSectionCeremonialViewModel lpscvm, ref Paging pg);

        KeyValuePair<Int64, string> Save(CreateProjectSectionCeremonialViewModel cpscvm, Int64 MemberId, List<string> paths);
    }
}