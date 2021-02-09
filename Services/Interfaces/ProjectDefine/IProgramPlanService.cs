using DomainModels.Entities;
using DomainModels.Entities.ProjectDefine;
using Infrastracture.AppCode;
using System;
using System.Collections.Generic;
using ViewModels.ProjectDefine.ProgramPlanViewModel;

namespace Services.Interfaces.BaseInformation
{
    public interface IProgramPlanService : IGenericService<ProgramPlan>
    {
        IEnumerable<ListProgramPlanViewModel> GetAll(ListProgramPlanViewModel lppvm, ref Paging pg);

        IEnumerable<ReportProgramPlanViewModel> GetAllReport(ReportParameterProgramPlanViewModel parameters);
    }
}