using DomainModels.Entities;
using DomainModels.Entities.ProjectDevision;
using Infrastracture.AppCode;
using System;
using System.Collections.Generic;
using ViewModels.ProjectDevision.ProjectSectionTenderViewModel;

namespace Services.Interfaces.ProjectDevision
{
    public interface IProjectSectionTenderService : IGenericService<ProjectSectionTender>
    {
        IEnumerable<ListProjectSectionTenderViewModel> GetAll(ListProjectSectionTenderViewModel lpscvm, ref Paging pg);

        KeyValuePair<Int64, string> Save(CreateProjectSectionTenderViewModel cpscvm, Int64 MemberId, List<string> paths);
    }
}