using DomainModels.Entities;
using DomainModels.Entities.ProjectDevision;
using Infrastracture.AppCode;
using System;
using System.Collections.Generic;
using ViewModels.ProjectDevision.ProjectSectionParticipantViewModel;

namespace Services.Interfaces.ProjectDevision
{
    public interface IProjectSectionParticipantService : IGenericService<ProjectSectionParticipant>
    {
        IEnumerable<ListProjectSectionParticipantViewModel> GetAll(ListProjectSectionParticipantViewModel lpscvm, ref Paging pg);
    }
}