using DomainModels.Entities;
using DomainModels.Entities.ProjectDefine;
using Infrastracture.AppCode;
using System;
using System.Collections.Generic;
using ViewModels.ProjectDefine.ProjectAllocateViewModel;

namespace Services.Interfaces.ProjectDefine
{
    public interface IProjectAllocateService : IGenericService<ProjectAllocate>
    {
        IEnumerable<ListProjectAllocateViewModel> GetAll(ListProjectAllocateViewModel latvm, ref Paging pg);

        KeyValuePair<Int64, string> Save(CreateProjectAllocateViewModel cpavm, Int64 memberId, List<string> paths);

        IEnumerable<ReportProjectAllocateViewModel> GetAllReport(ReportParameterProjectAllocateViewModel parameters);
    }
}