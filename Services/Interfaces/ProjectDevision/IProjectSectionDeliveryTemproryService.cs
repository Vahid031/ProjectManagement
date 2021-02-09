using DomainModels.Entities;
using DomainModels.Entities.ProjectDevision;
using Infrastracture.AppCode;
using System;
using System.Collections.Generic;
using ViewModels.ProjectDevision.ProjectSectionDeliveryTemproryViewModel;

namespace Services.Interfaces.ProjectDevision
{
    public interface IProjectSectionDeliveryTemproryService : IGenericService<ProjectSectionDeliveryTemprory>
    {
        IEnumerable<ListProjectSectionDeliveryTemproryViewModel> GetAll(ListProjectSectionDeliveryTemproryViewModel lpscvm, ref Paging pg);

        KeyValuePair<Int64, string> Save(CreateProjectSectionDeliveryTemproryViewModel cpscvm, Int64 MemberId, List<string> paths);

        IEnumerable<ReportProjectSectionDeliveryTemproryViewModel> GetAllReport(ReportParameterProjectSectionDeliveryTemproryViewModel parameters);
    }
}