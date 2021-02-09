using DomainModels.Entities;
using DomainModels.Entities.ProjectDevision;
using Infrastracture.AppCode;
using System;
using System.Collections.Generic;
using ViewModels.ProjectDevision.ProjectSectionOpenDateViewModel;

namespace Services.Interfaces.ProjectDevision
{
    public interface IProjectSectionOpenDateService : IGenericService<ProjectSectionOpenDate>
    {
        IEnumerable<ListProjectSectionOpenDateViewModel> GetAll(ListProjectSectionOpenDateViewModel lpscvm, ref Paging pg);
    }
}