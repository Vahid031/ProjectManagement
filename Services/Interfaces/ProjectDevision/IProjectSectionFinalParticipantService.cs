using DomainModels.Entities;
using DomainModels.Entities.ProjectDevision;
using Infrastracture.AppCode;
using System;
using System.Collections.Generic;
using ViewModels.ProjectDevision.ProjectSectionFinalParticipantViewModel;

namespace Services.Interfaces.ProjectDevision
{
    public interface IProjectSectionFinalParticipantService : IGenericService<ProjectSectionFinalParticipant>
    {
        IEnumerable<ListProjectSectionFinalParticipantViewModel> GetAll(ListProjectSectionFinalParticipantViewModel lpscvm, ref Paging pg);

        KeyValuePair<Int64, string> Save(CreateProjectSectionFinalParticipantViewModel cpscvm, Int64 MemberId, List<string> paths);

        IEnumerable<ReportProjectSectionFinalParticipantViewModel> GetAllReport(ReportParameterProjectSectionFinalParticipantViewModel parameters);
    }
}