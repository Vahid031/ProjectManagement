using DomainModels.Entities;
using DomainModels.Entities.ProjectDevision;
using Infrastracture.AppCode;
using System;
using System.Collections.Generic;
using ViewModels.ProjectDevision.ProjectSectionCorrespondenceViewModel;

namespace Services.Interfaces.ProjectDevision
{
    public interface IProjectSectionCorrespondenceService : IGenericService<ProjectSectionCorrespondence>
    {
        IEnumerable<ListProjectSectionCorrespondenceViewModel> GetAll(ListProjectSectionCorrespondenceViewModel lpscvm, ref Paging pg);

        KeyValuePair<Int64, string> Save(CreateProjectSectionCorrespondenceViewModel cpscvm, Int64 MemberId, List<string> paths);
    }
}