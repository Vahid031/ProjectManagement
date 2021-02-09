using DomainModels.Entities;
using DomainModels.Entities.ProjectDevision;
using Infrastracture.AppCode;
using System;
using System.Collections.Generic;
using ViewModels.ProjectDevision.ProjectSectionDraftViewModel;
using ViewModels.ProjectDevision.ProjectSectionStatementConfirmViewModel;

namespace Services.Interfaces.ProjectDevision
{
    public interface IProjectSectionStatementConfirmService : IGenericService<ProjectSectionStatementConfirm>
    {
        IEnumerable<ListProjectSectionStatementConfirmViewModel> GetAll(ListProjectSectionStatementConfirmViewModel lpscvm, ref Paging pg);
        IEnumerable<ListProjectSectionDraftListViewModel> GetConfirmDraft(ListProjectSectionDraftListViewModel lpscvm, ref Paging pg);
        KeyValuePair<Int64, string> Save(CreateProjectSectionStatementConfirmViewModel cpsivm, Int64 MemberId, List<string> paths);
    }
}