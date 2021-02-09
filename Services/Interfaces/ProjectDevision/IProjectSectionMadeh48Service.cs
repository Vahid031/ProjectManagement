using DomainModels.Entities;
using DomainModels.Entities.ProjectDevision;
using Infrastracture.AppCode;
using System;
using System.Collections.Generic;
using ViewModels.ProjectDevision.ProjectSectionMadeh48ViewModel;

namespace Services.Interfaces.ProjectDevision
{
    public interface IProjectSectionMadeh48Service : IGenericService<ProjectSectionMadeh48>
    {
        KeyValuePair<Int64, string> Save(CreateProjectSectionMadeh48ViewModel cpscvm, Int64 MemberId, List<string> paths);
        IEnumerable<ReportProjectSectionMadeh48ViewModel> GetAllReport(ReportParameterProjectSectionMadeh48ViewModel parameters);
    }
}