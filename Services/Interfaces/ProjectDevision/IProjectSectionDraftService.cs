using DomainModels.Entities;
using DomainModels.Entities.ProjectDevision;
using Infrastracture.AppCode;
using System;
using System.Collections.Generic;
using ViewModels.ProjectDevision.ProjectSectionDraftViewModel;

namespace Services.Interfaces.ProjectDevision
{
    public interface IProjectSectionDraftService : IGenericService<ProjectSectionDraft>
    {
        KeyValuePair<Int64, string> Save(CreateProjectSectionDraftViewModel cpscvm, Int64 MemberId, List<string> paths);
        IEnumerable<ListProjectSectionDraftViewModel> GetAll(ListProjectSectionDraftViewModel ldtvm, ref Paging pg);
        
    }
}