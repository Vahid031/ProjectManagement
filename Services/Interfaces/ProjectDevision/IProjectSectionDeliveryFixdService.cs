using DomainModels.Entities;
using DomainModels.Entities.ProjectDevision;
using Infrastracture.AppCode;
using System;
using System.Collections.Generic;
using ViewModels.ProjectDevision.ProjectSectionDeliveryFixdViewModel;

namespace Services.Interfaces.ProjectDevision
{
    public interface IProjectSectionDeliveryFixdService : IGenericService<ProjectSectionDeliveryFixd>
    {
        IEnumerable<ListProjectSectionDeliveryFixdViewModel> GetAll(ListProjectSectionDeliveryFixdViewModel lpscvm, ref Paging pg);

        KeyValuePair<Int64, string> Save(CreateProjectSectionDeliveryFixdViewModel cpscvm, Int64 MemberId, List<string> paths);

        IEnumerable<ReportProjectSectionDeliveryFixdViewModel> GetAllReport(ReportParameterProjectSectionDeliveryFixdViewModel parameters);
    }
}