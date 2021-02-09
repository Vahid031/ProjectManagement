using DomainModels.Entities;
using DomainModels.Entities.ProjectDevision;
using Infrastracture.AppCode;
using System;
using System.Collections.Generic;
using ViewModels.ProjectDevision.ProjectSectionScheduleViewModel;

namespace Services.Interfaces.ProjectDevision
{
    public interface IProjectSectionScheduleService : IGenericService<ProjectSectionSchedule>
    {
        IEnumerable<ListProjectSectionScheduleViewModel> GetAll(ListProjectSectionScheduleViewModel lpscvm, ref Paging pg);

        KeyValuePair<Int64, string> Save(CreateProjectSectionScheduleViewModel cpscvm, Int64 MemberId, List<string> paths);
    }
}