using DomainModels.Entities;
using DomainModels.Entities.ProjectDevision;
using Infrastracture.AppCode;
using System;
using System.Collections.Generic;
using ViewModels.ProjectDevision.ProjectSectionOperationTitleViewModel;

namespace Services.Interfaces.ProjectDevision
{
    public interface IProjectSectionOperationTitleService : IGenericService<ProjectSectionOperationTitle>
    {
        IEnumerable<ListProjectSectionOperationTitleViewModel> GetAll(ListProjectSectionOperationTitleViewModel lpscvm, ref Paging pg);
    }
}