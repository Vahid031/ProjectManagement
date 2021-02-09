using DomainModels.Entities;
using DomainModels.Entities.ProjectDevision;
using Infrastracture.AppCode;
using System;
using System.Collections.Generic;
using ViewModels.ProjectDevision.ProjectSectionMadeh46ViewModel;

namespace Services.Interfaces.ProjectDevision
{
    public interface IProjectSectionMadeh46Service : IGenericService<ProjectSectionMadeh46>
    {
        KeyValuePair<Int64, string> Save(CreateProjectSectionMadeh46ViewModel cpscvm, Int64 MemberId, List<string> paths);

        IEnumerable<ReportProjectSectionMadeh46ViewModel> GetAllReport(ReportParameterProjectSectionMadeh46ViewModel parameters);
    }
}