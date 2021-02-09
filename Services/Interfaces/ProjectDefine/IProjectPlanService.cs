using DomainModels.Entities;
using DomainModels.Entities.ProjectDefine;
using Infrastracture.AppCode;
using System;
using System.Collections.Generic;
using ViewModels.ProjectDefine.ProjectPlanViewModel;

namespace Services.Interfaces.BaseInformation
{
    public interface IProjectPlanService : IGenericService<ProjectPlan>
    {
        IEnumerable<ReportProjectPlanViewModel> GetAllReport(ReportParameterProjectPlanViewModel parameters);
    }
}