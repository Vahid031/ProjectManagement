using DomainModels.Entities;
using DomainModels.Entities.ProjectDevision;
using Infrastracture.AppCode;
using System;
using System.Collections.Generic;
using ViewModels.ProjectDevision.ProjectSectionWinnerViewModel;

namespace Services.Interfaces.ProjectDevision
{
    public interface IProjectSectionWinnerService : IGenericService<ProjectSectionWinner>
    {
        IEnumerable<ListProjectSectionWinnerViewModel> GetAll(ListProjectSectionWinnerViewModel lpscvm, ref Paging pg);
        IEnumerable<ListProjectSection4648ViewModel> Get4648(long contractorId, ref Paging pg);
    }
}